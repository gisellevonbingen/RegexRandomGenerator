using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeMultiple1 : RegexNode
    {
        public RegexNode Multiple1 { get; set; } = null;

        public RegexNodeMultiple1()
        {

        }

        public RegexNodeMultiple1(RegexNode multiple1) : this()
        {
            this.Multiple1 = multiple1;
        }

        public override string ToString() => $"{this.Multiple1}{RegexReader.QuantificationPlusSign}";

        public override string ChoiceRandom(RegexRandomizer randomizer)
        {
            var count = randomizer.NextTimes(1);
            return string.Join("", Enumerable2.Repeat(() => this.Multiple1.ChoiceRandom(randomizer), count));
        }

    }

}
