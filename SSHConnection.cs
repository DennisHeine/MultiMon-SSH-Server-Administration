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
        ShellStream stream;
        public SSHConnection(String host, int Port, String Username, String Password, String name)
        {
            try
            {
                ConnNfo = new ConnectionInfo(host, Port, Username, new AuthenticationMethod[] { new PasswordAuthenticationMethod(Username, Password) });
                sshclient = new SshClient(ConnNfo);
                sshclient.Connect();
                stream = sshclient.CreateShellStream(name, 80, 24, 800, 600, 4096);
                ForwardedPortLocal forwarding = new ForwardedPortLocal("127.0.0.1", (uint)12345, host, (uint)Port);
                sshclient.AddForwardedPort(forwarding);
                forwarding.Start();
                forwardedssh = new SshClient("127.0.0.1", 12345, Username, Password);
                forwardedssh.Connect();
                Globals.sshconnection = this;
                updatePath();
                SessionData d = new SessionData(name, host, Port, Username, Password);
                Globals.Settings.Sessions.Add(name, this);
            }
            catch (Exception e) { System.Windows.Forms.MessageBox.Show("Error connecting to server."); }
        }

        public void getSyslog()
        {
            if (sshclient.IsConnected)
            {
                stream.WriteLine("cat /var/log/syslog");
                stream.Expect("cat /var/log/syslog");
                String results = "";
                String result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                while (result != null)
                {
                    result = stream.ReadLine(TimeSpan.FromMilliseconds(500));
                    if(result!=null)
                        results += "\r\n" + result;
                }
                Globals.mainForm.setSyslog(results);
            }
        }

        public void upOneDir()
        {
            stream.WriteLine("cd ..");
            stream.Expect("cd ..");
            updatePath();
        }

        public void chdir(String dir)
        {
            stream.WriteLine("cd "+dir);
            stream.Expect("cd "+dir);
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
