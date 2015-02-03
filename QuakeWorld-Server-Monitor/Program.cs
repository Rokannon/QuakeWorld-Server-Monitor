using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuakeWorld_Server_Monitor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppModel model = new AppModel();
            model.LoadSettings();
            AppContext context = new AppContext();
            AppController controller = new AppController();
            controller.Connect(context, model);
            context.Connect(model, controller);
            controller.StartApp();
            Application.Run(context);
        }
    }
}
