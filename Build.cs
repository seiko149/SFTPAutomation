using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.IO;
using System.Reflection;

namespace SFTPAutomation
{
    class Build
    {
        private SftpClient client;
        private String directory;
        private String sendFiles;
        private String getFiles;
        private String archiveDirectory;
        private String rejectDirectory;
        private String host;
        private String userName;
        private String password;
        private String port;
        private String fullName;
        private String myName;
        private String systemProperties;
        private String logDirectory;
        private String logFile;
        private StreamWriter w;
        private Connection connection;
        Log log;
        List<String> properties;
        StreamReader reader;
        Encryption encryption;
        public Build()
        {
            fullName = Assembly.GetEntryAssembly().Location;
            myName = Path.GetDirectoryName(fullName);
            systemProperties = myName + @"\system.properties";
            host = "sfile.cegid.com";
            
        }

        public void closeLog()
        {
            w.Close();
        }

        public void transferFiles()
        {
            Transfer transfer = new Transfer(client, directory, sendFiles, getFiles, archiveDirectory, log, rejectDirectory);
            transfer.send();

            if (!transfer.isConnected())
            {
                log.append("error connecting to SFTP exiting program");
                w.Close();
                Environment.Exit(0);
            }

            else
            {
                transfer.get();
                if (!transfer.isConnected())
                {
                    w.Close();
                    Environment.Exit(1);
                }
                w.Close();
            }
        }

        public void createConnection()
        {
            connection = new Connection(userName, password, host, log);
            client = connection.makeConnection();
        }

        public void createLog()
        {
            if (directory != null && logFile != null)
            {
                w = File.AppendText(logFile);
                log = new Log(w);

            }
        }

        public void loadSystemProperties()
        {
            properties = new List<String>();

            System.Diagnostics.Debug.WriteLine(systemProperties);
            reader = new StreamReader(systemProperties);
            String line;

            while((line = reader.ReadLine()) != null)
            {
                properties.Add(line);
            }
            reader.Close();

            encryption = new Encryption();

            for (int i = 0; i < properties.Count; i++)
            {
                String[] temp = new String[2];
                temp = properties[i].Split('=');
                if (temp[0].Equals("host"))
                {
                    host = temp[1];
                }
                else if (temp[0].Equals("username"))
                {
                    String s = (temp[1] + @"==");
                    userName = encryption.Decrypted(s);

                }
                else if (temp[0].Equals("password"))
                {
                    String s = (temp[1] + @"==");
                    password = encryption.Decrypted(s);
                }
                else if (temp[0].Equals("directory"))
                {
                    directory = temp[1];
                }
                else if (temp[0].Equals("WRtoY2Folder"))
                {
                    sendFiles = directory + @"\" + temp[1];
                }
                else if (temp[0].Equals("Y2toWRFolder"))
                {
                    getFiles = directory + @"\" + temp[1] + @"\";
                }

            }

            archiveDirectory = sendFiles + @"\Archive\";
            rejectDirectory = sendFiles + @"\Rejects\";
            logDirectory = directory + @"\log\";
            logFile = logDirectory + "log.txt";          
        }

        public void createDirectories()
        {

            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating base directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(sendFiles))
                    Directory.CreateDirectory(sendFiles);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating Send Folder directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(getFiles))
                    Directory.CreateDirectory(getFiles);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating get Folder directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(archiveDirectory))
                    Directory.CreateDirectory(archiveDirectory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating Archive directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating logging directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(rejectDirectory))
                    Directory.CreateDirectory(rejectDirectory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating reject directory!");
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
