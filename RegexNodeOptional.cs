using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeOptional : RegexNode
    {
        public RegexNode Optional { get; set; }

        public RegexNodeOptional()
        {

        }

        public RegexNodeOptional(RegexNode optional) : this()
        {
            this.Optional = optional;
        }

        public override string ToString() => $"{this.Optional}{RegexReader.QuantificationQuestionMark}";

        public override string ChoiceRandom(RegexRandomizer randomizer) => randomizer.NextOptional() ? this.Optional.ChoiceRandom(randomizer) : string.Empty;

    }

}
