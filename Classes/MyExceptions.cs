using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    [Serializable()]
    public class exceptBadType : Exception
    {
        public exceptBadType() : base() { }
        public exceptBadType(string message) : base(message) { }
        public exceptBadType(string message, System.Exception inner) : base(message, inner) { }

        protected exceptBadType(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
