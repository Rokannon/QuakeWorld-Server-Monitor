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

        public void Connect(AppContext context, AppModel model)
        {
            this.context = context;
            this.model = model;
        }

        public void DoExit()
        {
            context.GetTrayIcon().Visible = false;
            Application.Exit();
        }

        public void OpenSettings()
        {
            if (model.settingsOpened)
            {
                return;
            }
            model.settingsOpened = true;
            context.GetForm().ShowDialog();
            model.settingsOpened = false;
        }

        public XElement QueryServer()
        {
            Process qstatProcess = new Process();
            qstatProcess.StartInfo.FileName = "qstat.exe";
            qstatProcess.StartInfo.Arguments = String.Format("-qws {0}:{1} -xml -P",
                model.serverHostname.ToString(),
                model.serverPort.ToString());
            qstatProcess.StartInfo.UseShellExecute = false;
            qstatProcess.StartInfo.RedirectStandardOutput = true;
            qstatProcess.StartInfo.CreateNoWindow = true;
            qstatProcess.Start();
            XElement xml = XElement.Parse(qstatProcess.StandardOutput.ReadToEnd());
            Console.WriteLine(xml.Element("server"));
            return xml;
        }

        public void ShowBalloonTip(string text, int timeout)
        {
            context.GetTrayIcon().BalloonTipTitle = "Header";
            context.GetTrayIcon().BalloonTipText = text;
            context.GetTrayIcon().ShowBalloonTip(timeout);
        }
    }
}
