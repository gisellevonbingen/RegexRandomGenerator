using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeGroup : RegexNode
    {
        public RegexNode Group { get; set; }

        public RegexNodeGroup()
        {

        }

        public RegexNodeGroup(RegexNode group) : this()
        {
            this.Group = group;
        }

        public override string ToString() => $"{RegexReader.GroupPrefix}{this.Group}{RegexReader.GroupSuffix}";

        public override string ChoiceRandom(RegexRandomizer randomizer) => this.Group.ChoiceRandom(randomizer);

    }

}
