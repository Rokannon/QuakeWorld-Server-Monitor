using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeWorld_Server_Monitor
{
    public class AppModel
    {
        public bool settingsOpened;
        public string serverHostname;
        public int serverPort;

        public void LoadSettings()
        {
            serverHostname = Properties.Settings.Default.serverHostname;
            serverPort = Properties.Settings.Default.serverPort;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.serverHostname = serverHostname;
            Properties.Settings.Default.serverPort = serverPort;
        }
    }
}
