using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeMultiple0 : RegexNode
    {
        public RegexNode Multiple0 { get; set; } = null;

        public RegexNodeMultiple0()
        {

        }

        public RegexNodeMultiple0(RegexNode multiple0) : this()
        {
            this.Multiple0 = multiple0;
        }

        public override string ToString() => $"{this.Multiple0}{RegexReader.QuantificationAsterisk}";

        public override string ChoiceRandom(RegexRandomizer randomizer)
        {
            var count = randomizer.NextTimes(0);
            return string.Join("", Enumerable2.Repeat(() => this.Multiple0.ChoiceRandom(randomizer), count));
        }

    }

}
