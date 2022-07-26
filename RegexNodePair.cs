using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public abstract class RegexNodePair : RegexNode
    {
        public RegexNode Left { get; set; } = null;
        public RegexNode Right { get; set; } = null;

        public RegexNodePair()
        {

        }

        public RegexNodePair(RegexNode left, RegexNode right)
        {
            this.Left = left;
            this.Right = right;
        }

    }

}
