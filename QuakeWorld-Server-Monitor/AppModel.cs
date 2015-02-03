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
        public ServerQueryResponse prevQueryResponse = new ServerQueryResponse();
        public ServerQueryResponse currQueryResponse = new ServerQueryResponse();
        public bool enableNotifications;

        public void LoadSettings()
        {
            serverHostname = Properties.Settings.Default.serverHostname;
            serverPort = Properties.Settings.Default.serverPort;
            enableNotifications = Properties.Settings.Default.enableNotifications;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.serverHostname = serverHostname;
            Properties.Settings.Default.serverPort = serverPort;
            Properties.Settings.Default.enableNotifications = enableNotifications;
            Properties.Settings.Default.Save();
        }

        public void SwapResponses()
        {
            ServerQueryResponse temp;
            temp = currQueryResponse;
            currQueryResponse = prevQueryResponse;
            prevQueryResponse = temp;
        }
    }
}
