using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuakeWorld_Server_Monitor
{
    public partial class Form1 : Form
    {
        private AppModel model;
        private AppController controller;

        public Form1()
        {
            InitializeComponent();
            Icon = Properties.Resources.Quake_icon;
            Activated += Form1_Activated;
        }

        public void Connect(AppModel model, AppController controller)
        {
            this.model = model;
            this.controller = controller;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            textBox1.Text = model.serverHostname.ToString();
            textBox2.Text = model.serverPort.ToString();
            checkBox1.Checked = model.enableNotifications;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.serverHostname = textBox1.Text;
            model.serverPort = int.Parse(textBox2.Text);
            model.enableNotifications = checkBox1.Checked;
            model.SaveSettings();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            controller.QueryServer();
            MessageBox.Show(controller.GetServerStatusText(), "Server Status");
        }
    }
}
