using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace _3DSceneEditorCS.Classes
{
    public class ReaderVS : Reader
    {
        protected StreamReader stream;

        public ReaderVS(StreamReader nStream)
        {
            this.stream = nStream; 
        }

        public override bool EndOf()
        {
            return stream.EndOfStream;
        }

        public override string ReadLine()
        {
            return stream.ReadLine();
        }
    }
}
