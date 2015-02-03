using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QuakeWorld_Server_Monitor
{
    public class AppController
    {
        private AppContext context;
        private AppModel model;
        private Timer timer;

        public AppController()
        {
            timer = new Timer();
            timer.Interval = 30000;
            timer.Tick += timer_Tick;
        }

        public void Connect(AppContext context, AppModel model)
        {
            this.context = context;
            this.model = model;
        }

        public void StartApp()
        {
            QueryServer();
            ShowBalloonTip(GetServerStatusText());
        }

        public void UpdateNotificationsState()
        {
            timer.Enabled = model.enableNotifications && !model.settingsOpened;
            UpdateNotification();
        }

        public void DoExit()
        {
            context.GetTrayIcon().Visible = false;
            Application.Exit();
        }

        public void OpenSettings()
        {
            if (model.settingsOpened)
                return;
            model.settingsOpened = true;
            timer.Enabled = false;
            context.GetForm().ShowDialog();
            model.currQueryResponse.Clear();
            UpdateNotificationsState();
            model.settingsOpened = false;
        }

        public XElement QueryServer()
        {
            model.swapResponses();
            model.currQueryResponse.Clear();
            Process qstatProcess = new Process();
            qstatProcess.StartInfo.FileName = "qstat.exe";
            qstatProcess.StartInfo.Arguments = String.Format("-qws {0}:{1} -xml",
                model.serverHostname.ToString(),
                model.serverPort.ToString());
            qstatProcess.StartInfo.UseShellExecute = false;
            qstatProcess.StartInfo.RedirectStandardOutput = true;
            qstatProcess.StartInfo.CreateNoWindow = true;
            qstatProcess.Start();
            XElement xml = XElement.Parse(qstatProcess.StandardOutput.ReadToEnd());
            model.currQueryResponse.Parse(xml);
            return xml;
        }

        public void ShowBalloonTip(string text)
        {
            context.GetTrayIcon().BalloonTipTitle = "QuakeWorld Server Monitor";
            context.GetTrayIcon().BalloonTipText = text;
            context.GetTrayIcon().ShowBalloonTip(1000);
        }

        public string GetServerStatusText()
        {
            string text = "";
            if (model.currQueryResponse.serverStatus == ServerQueryStatus.UP)
            {
                text += string.Format("Server \"{0}\" is up.\n", model.currQueryResponse.serverName);
                if (model.currQueryResponse.numPlayers == 0)
                    text += "No active players.";
                else
                    text += string.Format("Active players: {0}/{1}", model.currQueryResponse.numPlayers, model.currQueryResponse.maxPlayers);
            }
            else
            {
                text += string.Format("Server \"{0}\" is down.\n", model.currQueryResponse.serverHostname);
                if (model.currQueryResponse.serverStatus == ServerQueryStatus.TIMEOUT)
                    text += "Server query timed out.";
                else
                    text += string.Format("Error: {0}", model.currQueryResponse.serverError);
            }
            return text;
        }

        public void UpdateNotification()
        {
            if (!model.enableNotifications)
                return;
            QueryServer();
            if (model.currQueryResponse.serverStatus == ServerQueryStatus.UP &&
                model.currQueryResponse.numPlayers > 0 && model.prevQueryResponse.numPlayers == 0)
            {
                ShowBalloonTip(GetServerStatusText());
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateNotification();
        }
    }
}
