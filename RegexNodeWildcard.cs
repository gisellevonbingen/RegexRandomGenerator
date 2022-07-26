using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeWildcard : RegexNode
    {
        public RegexNodeWildcard()
        {

        }

        public override string ToString() => $"{RegexReader.Wildcard}";

        public override string ChoiceRandom(RegexRandomizer randomizer)
        {
            var pool = Enumerable2.NormalChars().ToArray();
            var c = pool.GetRandom(randomizer.Random);
            return c.ToString();
        }
    }

}
