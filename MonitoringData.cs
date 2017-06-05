using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    [Serializable]
    class MonitoringData
    {

        private string _name;
        private string _ip;
        private int _port;
        private string _bannertext;
        private bool _ignorebanner;

        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public String ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
            }
        }
        public int port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }
        public String bannertext
        {
            get
            {
                return _bannertext;
            }
            set
            {
                _bannertext = value;
            }
        }

        public bool ignorebanner
        {
            get
            {
                return _ignorebanner;
            }
            set
            {
                _ignorebanner = value;
            }
        }
    }
}
