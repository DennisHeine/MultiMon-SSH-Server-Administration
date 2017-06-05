using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WindowsFormsApplication1
{
    [Serializable]
    public static class Globals
    {
        [NonSerialized]
        public static Form1 mainForm;
        [NonSerialized]
        public static SSHConnection sshconnection;
        [NonSerialized]
        public static String directory = "c:\\";

        public static settings Settings = new settings();
        public static String masterpass;
        [Serializable]
        public class settings{
            private  Hashtable _Sessions = new Hashtable();
            private  Hashtable _Macros = new Hashtable();
            private  Hashtable _Monitors = new Hashtable();
            private String _Token = "";

            public String Token
            {
                get
                {
                    return _Token;
                }
                set
                {
                    _Token = value;
                }
            }

            public Hashtable Sessions
            {
                get
                {
                    return _Sessions;
                }
                set
                {
                    _Sessions = value;
                }
            }

            public Hashtable Macros
            {
                get
                {
                    return _Macros;
                }
                set
                {
                    _Macros = value;
                }
            }

            public Hashtable Monitors
            {
                get
                {
                    return _Monitors;
                }
                set
                {
                    _Monitors = value;
                }
            }

        }

        public static void save_data()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fsout = new FileStream("settings.dat", FileMode.Create, FileAccess.Write, FileShare.None);
            try
            {
                using (fsout)
                {
                    bf.Serialize(fsout, Settings);               
                }
            }
            catch
            {                
            }  
        }

        public static void load_data()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fsin = new FileStream("settings.dat", FileMode.Open, FileAccess.Read, FileShare.None);
                try
                {
                    using (fsin)
                    {
                        Settings = (settings)bf.Deserialize(fsin);
                    }
                }
                catch
                {
                    Globals.Settings.Token = SessionData.StringCipher.Encrypt("B33F", Globals.masterpass);
                }
            }
            catch { }
        }
    }
}
