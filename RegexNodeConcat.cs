using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeConcat : RegexNodePair
    {
        public RegexNodeConcat()
        {

        }

        public RegexNodeConcat(RegexNode left, RegexNode right) : base(left, right)
        {

        }

        public override string ToString() => $"{this.Left}{this.Right}";

        public override string ChoiceRandom(RegexRandomizer randomizer) => $"{this.Left.ChoiceRandom(randomizer)}{this.Right.ChoiceRandom(randomizer)}";

    }

}
