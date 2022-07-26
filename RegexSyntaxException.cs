using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    [Serializable]
    public class RegexSyntaxException : Exception
    {
        public RegexSyntaxException() { }
        public RegexSyntaxException(string message) : base(message) { }
        public RegexSyntaxException(string message, Exception inner) : base(message, inner) { }
        protected RegexSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
