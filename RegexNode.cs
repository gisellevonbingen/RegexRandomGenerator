using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public abstract class RegexNode
    {
        public static RegexNode Parse(string input)
        {
            using (var regexReader = new RegexReader(input))
            {
                return regexReader.Next();
            }

        }

        public static RegexNode Parse(TextReader reader)
        {
            using (var regexReader = new RegexReader(reader))
            {
                return regexReader.Next();
            }

        }

        public static RegexNode Parse(TextReader reader, bool leaveOpen)
        {
            using (var regexReader = new RegexReader(reader, leaveOpen))
            {
                return regexReader.Next();
            }

        }

        public abstract string ChoiceRandom(RegexRandomizer randomizer);
    }

}
