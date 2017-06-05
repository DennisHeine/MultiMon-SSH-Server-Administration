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
            public  Hashtable Sessions = new Hashtable();
            public  Hashtable Macros = new Hashtable();
            public  Hashtable Monitors = new Hashtable();

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
                }
            }
            catch { }
        }
    }
}
