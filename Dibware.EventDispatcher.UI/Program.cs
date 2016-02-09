using Dibware.EventDispatcher.Core;
using System;
using System.Windows.Forms;

namespace Dibware.EventDispatcher.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationEventDispatcher applicationEventDispatcher = null;
            MainProcess mainProcess = null;
            try
            {
                applicationEventDispatcher = new ApplicationEventDispatcher();
                mainProcess = new MainProcess(applicationEventDispatcher);

                Application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error!");
            }
            finally
            {
                if (mainProcess != null) mainProcess.Dispose();
                if (applicationEventDispatcher != null) applicationEventDispatcher.Dispose();
            }
        }
    }
}