using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;

namespace WindowsFormsApplication1
{
    public class SSHConnection
    {
        ConnectionInfo ConnNfo;
        SshClient sshclient;
        SshClient forwardedssh;

        SshClient sshclient1;
        ShellStream stream;

        public ShellStream gstream()
        {
            Random rnd = new Random();
            int month = rnd.Next(1, 999999999);

            ShellStream mstream = sshclient.CreateShellStream(month.ToString(), 80, 24, 800, 600, 4096);
            return mstream;
        }
        public SSHConnection(String host, int Port, String Username, String Password, String name)
        {
            try
            {
                ConnNfo = new ConnectionInfo(host, Port, Username, new AuthenticationMethod[] { new PasswordAuthenticationMethod(Username, Password) });
                sshclient = new SshClient(ConnNfo);
                sshclient.Connect();
                stream = gstream();               
                Globals.sshconnection = this;
                updatePath();
                //SessionData d = new SessionData(name, host, Port, Username, Password);
                //Globals.Settings.Sessions.Add(name, this);
            }
            catch (Exception e) { System.Windows.Forms.MessageBox.Show("Error connecting to server."); }
        }

        public void getSyslog()
        {
            if (sshclient.IsConnected)
            {
                
                SshClient sshclient1 = new SshClient(ConnNfo);
                sshclient1.Connect();
                SshCommand c = sshclient1.RunCommand("cat /var/log/syslog");
                Globals.mainForm.setSyslog(c.Result);
            }
        }

        public void getPS()
        {
            try
            {
                SshCommand c = sshclient.RunCommand("ps -aux");
                Globals.mainForm.setPS(c.Result);
            }catch(Exception ex) { }
        }

        private String doCommand(String cmd)
        {
            SshClient sshclient1 = new SshClient(ConnNfo);
            sshclient1.Connect();
            ShellStream mstream = sshclient1.CreateShellStream("dsaf", 80, 24, 800, 600, 4096);

            ShellStream stream = mstream;
            stream.WriteLine(cmd);
            stream.Expect(cmd);
            String results = "";
            String result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
            while (result != null)
            {
                result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                if (result != null)
                    results += "\r\n" + result;
            }
            return results;
        }

        public void getNetstat()
        {
            try
            {
                SshCommand c = sshclient.RunCommand("netstat -a -p --tcp");
                Globals.mainForm.setNetstat(c.Result);
            }
            catch (Exception ex) { }
        }

        public void getCPUPercentage()
        {
         //  stream.WriteLine("top -bn1 | grep \"Cpu(s)\" | sed \"s/.*, *\\([0-9.]*\\)%* id.*/\\1/\" | awk '{print 100 - $1}'");
           // stream.Expect("top -bn1 | grep \"Cpu(s)\" | sed \"s/.*, *\\([0-9.]*\\)%* id.*/\\1/\" | awk '{print 100 - $1}'");
            
         //   String results = "";
         //   String result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
        //    while (result != null)
       //     {
       //         result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
        //        if (result != null)
        //            results += "\r\n" + result;
        //    }

            //String percentage=doCommand("top -bn1 | grep \"Cpu(s)\" | sed \"s/.*, *\\([0-9.]*\\)%* id.*/\\1/\" | awk '{print 100 - $1}'");
           // Globals.mainForm.setCPUPercentage(double.Parse(results));
        }

        public void getRAMPercentage()
        {
            /*
            String totalram = doCommand("free -m | awk '{print $2}'| head -2 | tail -1");
            String freeram = doCommand("free -m | awk '{print $4}'| head -3| tail -1");
            int freepercent = 100 * int.Parse(freeram) / int.Parse(totalram);
            int usedpercent = 100 - freepercent;
            
            Globals.mainForm.setRAMPercentage(usedpercent);
             * */
        }

    

        public void upOneDir()
        {         
            sshclient.RunCommand("cd ..");
            updatePath();
        }

        public void chdir(String dir)
        {
            sshclient.RunCommand("cd "+dir);            
            updatePath();
        }


        public void updatePath()
        {         
            try
            {
              
                stream.WriteLine("pwd");
                stream.Expect("pwd");
                
                String result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                while (result == "")
                    result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                Globals.mainForm.setTerminalPathText(result);
                Globals.mainForm.setReomtePath(result);

                Globals.mainForm.clearServers();
                stream.WriteLine("ls -1ad */ 2>/dev/null");
                stream.Expect("ls -1ad */ 2>/dev/null");                
                result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                while (result != null)
                {
                    result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                    if(result!=null)
                        Globals.mainForm.addServerFolderPath(result);
                }

                Globals.mainForm.addServerFolderFile(".");
                Globals.mainForm.addServerFolderFile("..");                    
                stream.WriteLine("ls -p | grep -v / 2>/dev/null");
                stream.Expect("ls -p | grep -v / 2>/dev/null");
                result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                while (result != null)
                {
                    result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                    Globals.mainForm.addServerFolderFile(result);
                }



            }
            catch (Exception e1) { }                                   
        }

        public void sendCommand(String command)
        {
            try
           {                
                
                stream.WriteLine(command);
                stream.Expect(command);
                String result = "";
                do
                    result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                while (result != "");
                while (result != null)
                {
                    if (result.Length > 1)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                    if (result != "")
                        Globals.mainForm.setTerminalText(result);
                }
                updatePath();
            }
            catch (Exception e2) { }
        }
    }
}
