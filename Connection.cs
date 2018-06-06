using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPAutomation
{
    class Connection
    {
        String userName;
        String password;
        String host;
        Log log;

        public Connection(String userName, String password, String host, Log log)
        {
            this.userName = userName;
            this.password = password;
            this.host = host;
            this.log = log;
        }

        public SftpClient makeConnection()
        {
            SftpClient client = null;
            try
            {
                System.Diagnostics.Debug.WriteLine(host + "   " + userName + "    " + password);
                 client = new SftpClient(host, 22, userName, password);
            }
            catch(Exception e)
            {
                log.append(e.StackTrace);
            }
            return client;
        }
    }
}
