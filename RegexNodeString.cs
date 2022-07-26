using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public class RegexNodeString : RegexNode
    {
        public string Text { get; set; } = string.Empty;

        public RegexNodeString()
        {

        }

        public RegexNodeString(string text)
        {
            this.Text = text;
        }

        public override string ToString() => this.Text;

        public override string ChoiceRandom(RegexRandomizer randomizer) => this.Text;

    }

}
