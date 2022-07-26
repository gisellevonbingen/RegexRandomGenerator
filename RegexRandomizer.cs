using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexRandomizer
    {
        public Random Random { get; set; } = new Random();
        public double OptionalChance { get; set; } = 0.5D;
        public int TimesMinimumInsclusive { get; set; } = 0;
        public int TimesMaximumExclusive { get; set; } = 2;

        public bool NextOptional() => this.Random.TestChance(this.OptionalChance);

        public int NextTimes(int min) => this.NextTimes(min, this.TimesMaximumExclusive);

        public int NextTimes(int min, int max)
        {
            min = Math.Max(this.TimesMinimumInsclusive, min);
            max = Math.Max(Math.Min(this.TimesMaximumExclusive, max), min);

            return this.Random.Next(min, max);
        }

    }

}
