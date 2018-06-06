using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPAutomation
{
    class Log
    {
        StreamWriter w;
        public Log(StreamWriter w)
        {
            this.w = w;
        }

        public void append(String message)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", message);
            w.WriteLine("-----------------------------------------------------------");
        }
    }
}
