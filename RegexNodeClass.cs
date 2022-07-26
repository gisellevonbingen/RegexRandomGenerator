using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public abstract class RegexNodeClass : RegexNode, ISetPattern
    {
        private readonly char[] CachedPool;

        public RegexNodeClass()
        {
            this.CachedPool = this.GetPool().ToArray();
        }

        public abstract char Char { get; }

        public IEnumerable<char> GetChoiceablePool() => this.CachedPool;

        protected abstract IEnumerable<char> GetPool();

        public override string ChoiceRandom(RegexRandomizer randomizer)
        {
            var c = this.CachedPool.GetRandom(randomizer.Random);
            return c.ToString();
        }

        public override string ToString() => $"{RegexReader.EscpaceChar}{this.Char}";
    }

    public class RegexNodeClassAnyDigit : RegexNodeClass
    {
        public static RegexNodeClassAnyDigit Instance { get; } = new RegexNodeClassAnyDigit();

        private RegexNodeClassAnyDigit()
        {

        }

        public override char Char => RegexReader.ClassAnyDigit;

        protected override IEnumerable<char> GetPool() => AnyDigits();

        public static IEnumerable<char> AnyDigits() => Enumerable2.FromToInclusive('0', '9');

    }

    public class RegexNodeClassNonDigit : RegexNodeClass
    {
        public static RegexNodeClassNonDigit Instance { get; } = new RegexNodeClassNonDigit();

        private RegexNodeClassNonDigit()
        {

        }

        public override char Char => RegexReader.ClassNonDigit;

        protected override IEnumerable<char> GetPool() => Enumerable2.InverseOnNormalChars(RegexNodeClassAnyDigit.AnyDigits());

    }

    public class RegexNodeClassAnyWord : RegexNodeClass
    {
        public static RegexNodeClassAnyWord Instance { get; } = new RegexNodeClassAnyWord();

        private RegexNodeClassAnyWord()
        {

        }

        public override char Char => RegexReader.ClassAnyWord;

        protected override IEnumerable<char> GetPool() => AnyWords();

        public static IEnumerable<char> AnyWords()
        {
            foreach (var c in RegexNodeClassAnyDigit.AnyDigits())
            {
                yield return c;
            }

            foreach (var c in Enumerable2.FromToInclusive('a', 'z'))
            {
                yield return c;
            }

            yield return '_';
        }

    }

    public class RegexNodeClassNonWord : RegexNodeClass
    {
        public static RegexNodeClassNonWord Instance { get; } = new RegexNodeClassNonWord();

        private RegexNodeClassNonWord()
        {

        }

        public override char Char => RegexReader.ClassNonWord;

        protected override IEnumerable<char> GetPool() => Enumerable2.InverseOnNormalChars(RegexNodeClassAnyWord.AnyWords());

    }

    public class RegexNodeClassAnyWhitespace : RegexNodeClass
    {
        public static RegexNodeClassAnyWhitespace Instance { get; } = new RegexNodeClassAnyWhitespace();

        private RegexNodeClassAnyWhitespace()
        {

        }

        public override char Char => RegexReader.ClassAnyWhitespace;

        protected override IEnumerable<char> GetPool() => AnyWhitespaces();

        public static IEnumerable<char> AnyWhitespaces()
        {
            yield return '\0';
            yield return ' ';
            yield return '\t';
        }

    }


    public class RegexNodeClassNonWhitespace : RegexNodeClass
    {
        public static RegexNodeClassNonWhitespace Instance { get; } = new RegexNodeClassNonWhitespace();

        private RegexNodeClassNonWhitespace()
        {

        }

        public override char Char => RegexReader.ClassNonWhitespace;

        protected override IEnumerable<char> GetPool() => Enumerable2.InverseOnNormalChars(RegexNodeClassAnyWhitespace.AnyWhitespaces());
    }


}
