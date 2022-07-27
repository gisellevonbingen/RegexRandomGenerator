using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexReader : IDisposable
    {
        public const char Or = '|';
        public const char Begin = '^';
        public const char End = '$';

        public const char GroupPrefix = '(';
        public const char GroupSuffix = ')';

        public const char QuantificationQuestionMark = '?';
        public const char QuantificationAsterisk = '*';
        public const char QuantificationPlusSign = '+';

        public const char QuantificationMatchTimesPrefix = '{';
        public const char QuantificationMatchTimesSuffix = '}';
        public const char QuantificationMatchTimesDelimiter = ',';

        public const char Wildcard = '.';

        public const char SetPrefix = '[';
        public const char SetSuffix = ']';
        public const char SetNegative = '^';
        public const char SetRangeDelimiter = '-';

        public const char EscpaceChar = '\\';
        public const char ClassAnyDigit = 'd';
        public const char ClassNonDigit = 'D';
        public const char ClassAnyWord = 'w';
        public const char ClassNonWord = 'W';
        public const char ClassAnyWhitespace = 's';
        public const char ClassNonWhitespace = 'S';

        public static bool IsControl(char c)
        {
            if (c == Or)
            {
                return true;
            }
            else if (c == GroupPrefix || c == GroupSuffix)
            {
                return true;
            }
            else if (c == QuantificationQuestionMark || c == QuantificationAsterisk || c == QuantificationPlusSign)
            {
                return true;
            }
            else if (c == QuantificationMatchTimesPrefix || c == QuantificationMatchTimesSuffix || c == QuantificationMatchTimesDelimiter)
            {
                return true;
            }
            else if (c == Wildcard)
            {
                return true;
            }
            else if (c == SetPrefix || c == SetSuffix || c == SetNegative || c == SetRangeDelimiter)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected TextReader Reader { get; private set; }
        protected bool LeaveOpen { get; private set; }

        public RegexReader(string input) : this(new StringReader(input))
        {

        }

        public RegexReader(TextReader reader) : this(reader, false)
        {

        }

        public RegexReader(TextReader reader, bool leaveOpen)
        {
            this.Reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.LeaveOpen = leaveOpen;
        }

        protected bool TryPeek(out char value)
        {
            var c = this.Reader.Peek();
            value = (char)c;
            return c > -1;
        }

        protected bool TryRead(out char value)
        {
            var c = this.Reader.Read();
            value = (char)c;
            return c > -1;
        }

        protected char Read(char require)
        {
            var c = this.Reader.Read();

            if (c == -1)
            {
                throw new IOException();
            }
            else if (c != require)
            {
                throw new RegexSyntaxException($"Unexpected char '{c}'");
            }
            else
            {
                return (char)c;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.LeaveOpen == false)
            {
                this.Reader.Dispose();
            }

        }

        public RegexNode Next() => this.Next(false);

        protected RegexNode Next(bool isGroup)
        {
            RegexNode preceding = null;

            while (this.TryPeek(out var c) == true)
            {
                if (c == Or)
                {
                    this.Read(c);
                    var trailing = this.Next(isGroup);
                    return new RegexNodeOr() { Left = preceding, Right = trailing };
                }
                else if (c == GroupSuffix)
                {
                    if (isGroup == true)
                    {
                        break;
                    }
                    else
                    {
                        throw new RegexSyntaxException($"Unexpected input: {c}");
                    }

                }
                else
                {
                    var one = this.NextOne();

                    if (one == null)
                    {
                        break;
                    }
                    else if (preceding is RegexNodeString str1 && one is RegexNodeString str2)
                    {
                        str1.Text += str2.Text;
                    }
                    else
                    {
                        preceding = this.Concat(preceding, one);
                    }

                }

            }

            return preceding ?? throw new RegexSyntaxException("Unexpected input");
        }

        protected RegexNode Concat(RegexNode left, RegexNode right)
        {
            if (left == null)
            {
                return right;
            }
            else if (right == null)
            {
                return null;
            }
            else
            {
                return new RegexNodeConcat() { Left = left, Right = right };
            }

        }

        protected RegexNode NextOne()
        {
            var preceding = this.NextOneInternal();

            if (this.TryPeek(out var c) == true)
            {
                if (c == QuantificationQuestionMark)
                {
                    this.Read(c);
                    return new RegexNodeOptional() { Optional = preceding };
                }
                else if (c == QuantificationAsterisk)
                {
                    this.Read(c);
                    return new RegexNodeMultiple0() { Multiple0 = preceding };
                }
                else if (c == QuantificationPlusSign)
                {
                    this.Read(c);
                    return new RegexNodeMultiple1() { Multiple1 = preceding };
                }
                else if (c == QuantificationMatchTimesPrefix)
                {
                    return this.NextTimes(preceding);
                }

            }

            return preceding;
        }

        protected RegexNodeTimes NextTimes(RegexNode preceding)
        {
            if (this.TryRead(out var prefix) == true && prefix != QuantificationMatchTimesPrefix)
            {
                throw new RegexSyntaxException($"Unexpected input : {prefix}");
            }

            var complete = false;
            var timesBuilder = new StringBuilder();

            while (this.TryPeek(out var c) == true)
            {
                if (c == QuantificationMatchTimesSuffix)
                {
                    this.Read(c);
                    complete = true;
                    break;
                }
                else if (c != QuantificationMatchTimesDelimiter && IsControl(c) == true)
                {
                    throw new RegexSyntaxException($"Unexpected input : {c}");
                }
                else
                {
                    this.Read(c);
                    timesBuilder.Append(c);
                }

            }

            if (complete == false)
            {
                throw new RegexSyntaxException($"Unexpected input : {timesBuilder}");
            }

            var timesToString = timesBuilder.ToString();
            var delimiterIndex = timesToString.IndexOf(QuantificationMatchTimesDelimiter);
            int? min;
            int? max;

            if (delimiterIndex > -1)
            {
                var minText = timesToString.Substring(0, delimiterIndex);
                var maxText = timesToString.Substring(delimiterIndex + 1);
                min = string.IsNullOrEmpty(minText) ? new int?() : int.Parse(minText);
                max = string.IsNullOrEmpty(maxText) ? new int?() : int.Parse(maxText);
            }
            else
            {
                min = max = string.IsNullOrEmpty(timesToString) ? new int?() : int.Parse(timesToString);
            }

            return new RegexNodeTimes() { Times = preceding, Minimum = min, Maximum = max };
        }

        protected RegexNode NextOneInternal()
        {
            if (this.TryPeek(out var c) == true)
            {
                if (c == GroupPrefix)
                {
                    var group = this.NextGroup();
                    return group;
                }
                else if (c == Wildcard)
                {
                    this.Read(c);
                    return new RegexNodeWildcard();
                }
                else if (c == SetPrefix)
                {
                    var set = this.NextSet();
                    return set;
                }
                else
                {
                    var ch = this.NextChar();
                    return ch;
                }

            }
            else
            {
                throw new RegexSyntaxException("Unexpected input");
            }

        }

        protected RegexNodeSet NextSet()
        {
            if (this.TryRead(out var prefix) == true && prefix != SetPrefix)
            {
                throw new RegexSyntaxException($"Unexpected input : {prefix}");
            }

            var set = new RegexNodeSet();

            if (this.TryPeek(out var first) == true && first == SetNegative)
            {
                this.Read(first);
                set.Negative = true;
            }

            var list = new List<RegexNode>();

            while (this.TryPeek(out var c) == true)
            {
                if (c == SetSuffix)
                {
                    this.Read(c);
                    break;
                }
                else
                {
                    var ch = this.NextChar();
                    list.Add(ch);
                }

            }

            if (list.Count < 3)
            {
                set.Patterns.AddRange(list.Select(n => this.ToSetPattern(n)));
            }
            else
            {
                var count = list.Count;

                for (var i = 0; i < count; i++)
                {
                    var l = list[i + 0];
                    var m = (i + 1 < count) ? list[i + 1] : null;
                    var r = (i + 2 < count) ? list[i + 2] : null;
                    var foundRange = false;

                    if (l is RegexNodeString str1 && m is RegexNodeString str2 && r is RegexNodeString str3)
                    {
                        var start = str1.Text[0];
                        var delimiter = str2.Text[0];
                        var end = str3.Text[0];

                        if (delimiter == SetRangeDelimiter)
                        {
                            foundRange = true;

                            if (start <= end)
                            {
                                set.Patterns.Add(new SetPattern(start, end));
                            }
                            else
                            {
                                throw new RegexSyntaxException($"SetPattern range is reversed : {start}{SetRangeDelimiter}{end}");
                            }

                            i += 2;
                        }

                    }

                    if (foundRange == false)
                    {
                        set.Patterns.Add(this.ToSetPattern(l));

                        if (i + 3 >= count)
                        {
                            if (m != null)
                            {
                                set.Patterns.Add(this.ToSetPattern(m));
                            }

                            if (r != null)
                            {
                                set.Patterns.Add(this.ToSetPattern(r));
                            }

                            break;
                        }

                    }

                }

            }

            return set;
        }

        private ISetPattern ToSetPattern(RegexNode node)
        {
            if (node is ISetPattern pattern)
            {
                return pattern;
            }
            else if (node is RegexNodeString str)
            {
                return new SetPattern(str.Text[0]);
            }
            else
            {
                throw new ArgumentException();
            }

        }

        protected RegexNodeGroup NextGroup()
        {
            if (this.TryRead(out var prefix) == true && prefix != GroupPrefix)
            {
                throw new RegexSyntaxException($"Unexpected input : {prefix}");
            }

            var content = this.Next(true);

            if (this.TryRead(out var suffix) == true && suffix != GroupSuffix)
            {
                throw new RegexSyntaxException($"Unexpected input : {suffix}");
            }

            return new RegexNodeGroup() { Group = content };
        }

        protected RegexNode NextChar()
        {
            var builder = new StringBuilder();
            var escaping = false;
            StringBuilder octalBuilder = null;

            while (this.TryPeek(out var c) == true)
            {
                if (escaping == true)
                {
                    if (octalBuilder != null)
                    {
                        bool finalizing;
                        this.Read(c);

                        if ('0' <= c && c <= '9')
                        {
                            octalBuilder.Append(c);
                            finalizing = octalBuilder.Length >= 3;
                        }
                        else
                        {
                            finalizing = true;
                        }

                        if (finalizing == true)
                        {
                            escaping = false;
                            break;
                        }

                    }
                    else
                    {
                        if (c == 'd')
                        {
                            this.Read(c);
                            return RegexNodeClassAnyDigit.Instance;
                        }
                        else if (c == 'D')
                        {
                            this.Read(c);
                            return RegexNodeClassNonDigit.Instance;
                        }
                        else if (c == 'w')
                        {
                            this.Read(c);
                            return RegexNodeClassAnyWord.Instance;
                        }
                        else if (c == 'W')
                        {
                            this.Read(c);
                            return RegexNodeClassNonWord.Instance;
                        }
                        else if (c == 's')
                        {
                            this.Read(c);
                            return RegexNodeClassAnyWhitespace.Instance;
                        }
                        else if (c == 'S')
                        {
                            this.Read(c);
                            return RegexNodeClassNonWhitespace.Instance;
                        }
                        else if ('0' <= c && c <= '9')
                        {
                            if (builder.Length == 0)
                            {
                                this.Read(c);
                                octalBuilder = new StringBuilder();
                                octalBuilder.Append(c);
                            }
                            else
                            {
                                throw new RegexSyntaxException($"Unexpected input : {c}");
                            }

                        }
                        else if (c == 'r')
                        {
                            this.Read(c);
                            escaping = false;
                            builder.Append('\r');
                            break;
                        }
                        else if (c == 'n')
                        {
                            this.Read(c);
                            escaping = false;
                            builder.Append('\n');
                            break;
                        }
                        else if (c == 't')
                        {
                            this.Read(c);
                            escaping = false;
                            builder.Append('\t');
                            break;
                        }
                        else
                        {
                            this.Read(c);
                            escaping = false;
                            builder.Append(c);
                            break;
                        }

                    }

                }
                else if (c == EscpaceChar)
                {
                    if (builder.Length == 0)
                    {
                        this.Read(c);
                        escaping = true;
                    }
                    else
                    {
                        throw new RegexSyntaxException($"Unexpected input : {c}");
                    }

                }
                else
                {
                    this.Read(c);
                    builder.Append(c);
                    break;
                }

            }

            if (octalBuilder != null)
            {
                builder.Append((char)int.Parse(octalBuilder.ToString()));
            }
            else if (escaping == true)
            {
                throw new RegexSyntaxException($"Unfinished reading escape char");
            }

            return new RegexNodeString() { Text = builder.ToString() };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        ~RegexReader()
        {
            this.Dispose(false);
        }

    }

}
