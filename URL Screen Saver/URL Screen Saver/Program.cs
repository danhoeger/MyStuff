using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace URL_Screen_Saver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //            MessageBox.Show("ScreenSaverStarted...");
            if (args.Length > 0)
            {
                string commandLineSwitch = args[0];
                string secondArg = String.Empty;
                if (args[0].Contains(":"))
                {
                    string[] commandInfo = args[0].Split(':');
                    commandLineSwitch = commandInfo[0];
                    secondArg = commandInfo[1];
                }
                else if (args.Length > 1)
                {
                    secondArg = args[1];
                }
                switch (commandLineSwitch.ToLower())
                {
                    case "/s":
                    case "-s":
                        // show the screen saver
                        ShowScreenSaver();
                        break;
                    case "/p":
                    case "-p":
                    case "/l":
                    case "-l":
                        // preview the screen saver
                        if (secondArg != String.Empty)
                        {
                            ShowPreview((new IntPtr(long.Parse(secondArg))));
                        }
                        else
                        {
                            ShowScreenSaver();
                        }
                        break;
                    case "/c":
                    case "-c":
                        // configure the screen saver
                        if (secondArg != string.Empty)
                        {
                            ShowConfigure(new IntPtr(long.Parse(secondArg)));
                        }
                        else
                        {
                            ShowConfigure();
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // no args passed just configure with no parent
                ShowConfigure();
            }
        }

        static void ShowConfigure()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form configForm = new ConfigureForm();
            configForm.ShowDialog();
//            Application.Run(new ConfigureForm());
        }

        static void ShowConfigure(IntPtr ParentWindow)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form configForm = new ConfigureForm();
            configForm.ShowDialog(new WindowWrapper(ParentWindow));
            //            Application.Run(new ConfigureForm(ParentWindow));
        }

        static void ShowPreview(IntPtr ParentWindow)
        {
            // preview the screen saver
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //args[1] is the handle to the preview window
            Application.Run(new Form1(ParentWindow));
        }

        static void ShowScreenSaver()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            int screenCount = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (++screenCount > 1)
                {
                    // wait for the next timeout value so that we can have a different value on each screen.
                    System.Threading.Thread.Sleep((int)Properties.Settings.Default.DelayValue * 1000);
                }
                Form1 screensaver = new Form1(screen.Bounds);
                screensaver.Show();
                screensaver.SetDesktopLocation(screen.Bounds.X, screen.Bounds.Y);
                screensaver.BringToFront();
            }
            Application.Run();
        }

    }
}
