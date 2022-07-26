using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public struct SetPattern : ISetPattern
    {
        public char Start { get; set; }
        public char? End { get; set; }

        public SetPattern(char start) : this(start, null)
        {

        }

        public SetPattern(char start, char? end) : this()
        {
            this.Start = start;
            this.End = end;
        }

        public override string ToString()
        {
            var builder = new StringBuilder().Append(this.Start);

            if (this.End.HasValue == true)
            {
                builder.Append(RegexReader.SetRangeDelimiter);
                builder.Append(this.End);
            }

            return builder.ToString();
        }

        public IEnumerable<char> GetChoiceablePool()
        {
            if (this.End.HasValue == true)
            {
                foreach (var c in Enumerable2.FromToInclusive(this.Start, this.End.Value))
                {
                    yield return c;
                }

            }
            else
            {
                yield return this.Start;
            }

        }

    }

}
