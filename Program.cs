using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SFTPAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            Build build = new Build();
            build.loadSystemProperties();
            build.createDirectories();
            build.createLog();
            build.createConnection();
            build.transferFiles();
            build.closeLog();

        }
    }
}
