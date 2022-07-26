using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public static class RandomExtensions
    {
        public static T GetRandom<T>(this IList<T> list, Random random) => GetRandom(random, list);

        public static T GetRandom<T>(this Random random, IList<T> list)
        {
            var index = random.Next(list.Count);
            return list[index];
        }

        public static bool TestChance(this Random random, double chance)
        {
            var n = random.NextDouble();
            return n < chance;
        }

        public static bool NextBool(this Random random) => TestChance(random, 0.5D);

    }

}
