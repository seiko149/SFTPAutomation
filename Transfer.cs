using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPAutomation
{
    class Transfer
    {
        SftpClient client;
        String directory;
        string remoteDirectory = "get/";
        String sendFiles;
        String getFiles;
        FileStream fileStream;
        Boolean successfulConnection;
        String archiveDirectory;
        Log log;
        String rejectDirectory;

        private Boolean connected;

        public Transfer(SftpClient client, String directory, String sendFiles, String getFiles, String archiveDirectory, Log log, String rejectDirectory)
        {
            this.client = client;
            this.directory = directory;
            this.sendFiles = sendFiles;
            this.getFiles = getFiles;
            successfulConnection = true;
            this.archiveDirectory = archiveDirectory;
            this.log = log;
            this.rejectDirectory = rejectDirectory;
        }

        public Boolean isConnected()
        {
            return successfulConnection;
        }


        public void send()
        {
            log.append("Loading files to send to SFTP");
            String[] files = System.IO.Directory.GetFiles(sendFiles, "*.csv");

            try
            {
                client.Connect();
                client.ChangeDirectory("put");
                log.append("Sucessfully opened put folder");
                
            }
            catch (Exception e)
            {
                successfulConnection = false;
                log.append("Unable to upload files to FTP Server: error connection to FTP server");
            }


            if (successfulConnection)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    fileStream = new FileStream(files[i], FileMode.Open);
                    client.UploadFile(fileStream, Path.GetFileName(files[i]));
                    log.append("Coppied file " + Path.GetFileName(files[i]) + "to SFTP server");
                    fileStream.Close();
                }
                client.Disconnect();

                for (int i = 0; i < files.Length; i++)
                {
                    if (System.IO.File.Exists(archiveDirectory + Path.GetFileName(files[i])))
                    {
                        System.IO.File.Delete(archiveDirectory + Path.GetFileName(files[i]));
                    }
                    File.Move(files[i], archiveDirectory + Path.GetFileName(files[i]));
                    log.append("Moved file " + Path.GetFileName(files[i]) + " to archive directory");
                }
            }

        }

        public void get()
        {

            try
            {
                client.Connect();
            }
            catch (Exception e)
            {
                successfulConnection = false;
                log.append("Unable to download files from FTP Server: Error connecting to FTP site");
            }

            if (successfulConnection)
            {

                var files = client.ListDirectory(remoteDirectory);

                foreach (var file in files)
                {

                    if ((file.Name.EndsWith(".ASC")))
                    {
                        string remoteFileName = file.Name;
                        using (Stream file1 = File.OpenWrite(getFiles + remoteFileName))
                        {
                            client.DownloadFile(remoteDirectory + remoteFileName, file1);
                            log.append("Downloaded file " + remoteFileName);
                            client.DeleteFile(remoteDirectory + remoteFileName);
                        }
                    }

                    else if ((file.Name.EndsWith(".csv")))
                    {
                        string remoteFileName = file.Name;


                        using (Stream file1 = File.OpenWrite(rejectDirectory + remoteFileName))
                        {
                            System.Diagnostics.Debug.WriteLine("testccc " + rejectDirectory + remoteFileName);
                            client.DownloadFile(remoteDirectory + remoteFileName, file1);
                            log.append("Downloaded file " + remoteFileName);
                            client.DeleteFile(remoteDirectory + remoteFileName);
                        }
                    }
                }
                //test
                client.Disconnect();

            }
        }
    }
}
