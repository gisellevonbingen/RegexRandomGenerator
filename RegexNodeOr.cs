using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeOr : RegexNodePair
    {
        public RegexNodeOr()
        {

        }

        public RegexNodeOr(RegexNode left, RegexNode right) : base(left, right)
        {

        }

        public override string ToString() => $"{this.Left}{RegexReader.Or}{this.Right}";

        public override string ChoiceRandom(RegexRandomizer randomizer) => (randomizer.Random.NextBool() ? this.Left : this.Right).ChoiceRandom(randomizer);

    }

}
