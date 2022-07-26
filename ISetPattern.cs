using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public interface ISetPattern
    {
        IEnumerable<char> GetChoiceablePool();
    }

}
