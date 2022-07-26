using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public static class Enumerable2
    {
        public static IEnumerable<T> Repeat<T>(Func<T> func, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return func();
            }

        }

        public static IEnumerable<char> NormalChars()
        {
            for (char c = '\0'; c < 256; c++)
            {
                if (char.IsControl(c) == false)
                {
                    yield return c;
                }

            }

        }

        public static IEnumerable<char> FromToInclusive(char from, char to)
        {
            for (var i = from; i <= to; i++)
            {
                yield return i;
            }

        }

        public static IEnumerable<char> InverseOnNormalChars(IEnumerable<char> pool) => NormalChars().Except(pool);

    }

}
