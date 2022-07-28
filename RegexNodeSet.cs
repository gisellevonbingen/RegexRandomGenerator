using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeSet : RegexNode
    {
        public bool Negative { get; set; } = false;
        public List<ISetPattern> Patterns { get; } = new List<ISetPattern>();

        public RegexNodeSet()
        {

        }

        public override string ToString()
        {
            var builder = new StringBuilder().Append(RegexReader.SetPrefix);

            if (this.Negative == true)
            {
                builder.Append(RegexReader.SetNegative);
            }

            foreach (var pattern in this.Patterns)
            {
                builder.Append(pattern.ToString());
            }

            return builder.Append(RegexReader.SetSuffix).ToString();
        }

        public override string ChoiceRandom(RegexRandomizer randomizer)
        {
            var pool = this.Patterns.SelectMany(p => p.GetChoiceablePool()).Distinct();

            if (this.Negative == true)
            {
                pool = Enumerable2.InverseOnNormalChars(pool);
            }

            var computedPool = pool.ToArray();
            var c = computedPool.GetRandom(randomizer.Random);
            return c.ToString();
        }

    }

}
