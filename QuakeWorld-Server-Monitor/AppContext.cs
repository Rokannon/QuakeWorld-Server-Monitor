using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuakeWorld_Server_Monitor
{
    public class AppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Form1 form;
        private AppModel model;
        private AppController controller;

        public AppContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.Quake_icon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Settings...", onSettings),
                    new MenuItem("Exit", onExit)
                }),
                Visible = true
            };
            form = new Form1();
        }

        public NotifyIcon GetTrayIcon()
        {
            return trayIcon;
        }

        public Form1 GetForm()
        {
            return form;
        }

        public void Connect(AppModel model, AppController controller)
        {
            this.model = model;
            this.controller = controller;
            form.Connect(model, controller);
        }

        private void onSettings(object sender, EventArgs e)
        {
            controller.OpenSettings();
        }

        private void onExit(object sender, EventArgs e)
        {
            controller.DoExit();
        }
    }
}
