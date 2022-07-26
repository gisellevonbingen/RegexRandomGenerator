using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeTimes : RegexNode
    {
        public RegexNode Times { get; set; } = null;
        public int? Minimum { get; set; } = null;
        public int? Maximum { get; set; } = null;

        public RegexNodeTimes()
        {

        }

        public RegexNodeTimes(RegexNode times)
        {
            this.Times = times;
        }

        public RegexNodeTimes(RegexNode times, int count) : this(times, count, count)
        {

        }

        public RegexNodeTimes(RegexNode times, int minimum, int maximum) : this(times)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public RegexNodeTimes(RegexNode times, int? minimum, int? maximum) : this(times)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public override string ToString()
        {
            var builder = new StringBuilder().Append(this.Times).Append(RegexReader.QuantificationMatchTimesPrefix);

            if (this.Minimum.HasValue == true && this.Minimum == this.Maximum)
            {
                builder.Append(this.Minimum.Value);
            }
            else
            {
                if (this.Minimum.HasValue == true)
                {
                    builder.Append(this.Minimum.Value);
                }

                if (this.Minimum.HasValue == true || this.Maximum.HasValue == true)
                {
                    builder.Append(RegexReader.QuantificationMatchTimesDelimiter);
                }

                if (this.Maximum.HasValue == true)
                {
                    builder.Append(this.Maximum.Value);
                }

            }

            return builder.Append(RegexReader.QuantificationMatchTimesSuffix).ToString();
        }

        public override string ChoiceRandom(RegexRandomizer randomizer)
        {
            var count = randomizer.NextTimes(this.Minimum ?? randomizer.TimesMinimumInsclusive, this.Maximum ?? randomizer.TimesMaximumExclusive);
            return string.Join("", Enumerable2.Repeat(() => this.Times.ChoiceRandom(randomizer), count));
        }

    }

}
