using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser;
using DotNetBrowser.WinForms;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace URL_Screen_Saver
{
    public partial class Form1 : Form
    {
        public bool IsPreviewMode { get; private set; }
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        private WinFormsBrowserView browserView = new WinFormsBrowserView() { BrowserType = BrowserType.LIGHTWEIGHT };

        #endregion
        public Form1(Rectangle Bounds)
        {
            //LoggerProvider.Instance.LoggingEnabled = true;
            //LoggerProvider.Instance.ConsoleLoggingEnabled = true;

            InitializeComponent();
            this.Bounds = Bounds;
            IsPreviewMode = false;

            Controls.Add((Control)browserView);

            BrowserPreferences prefs = browserView.Browser.Preferences;

            prefs.FireKeyboardEventsEnabled = true;
            prefs.FireMouseEventsEnabled = true;

            browserView.Browser.Preferences = prefs;

            if(Properties.Settings.Default.UseBingWallpaper)
            {
                string urlBase = GetBackgroundUrlBase();
                urlBase = urlBase + GetResolutionExtension(urlBase, Bounds);
                browserView.Browser.LoadURL(urlBase);
            }
            else
            {
                browserView.Browser.LoadURL(Properties.Settings.Default.DisplayURL);
            }

            browserView.MouseMove += MouseMoveHandler;
            browserView.KeyDown += KeyDownHandler;
        }

        public Form1(IntPtr PreviewHandle)
        {
            InitializeComponent();

            //set the preview window as the parent of this window
            SetParent(this.Handle, PreviewHandle);

            //make this a child window, so when the select screensaver 
            //dialog closes, this will also close
            SetWindowLong(this.Handle, -16,
                  new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            //set our window's size to the size of our window's new parent
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;

            //set our location at (0, 0)
            this.Location = new Point(0, 0);
            IsPreviewMode = true;

            Controls.Add((Control)browserView);

            BrowserPreferences prefs = browserView.Browser.Preferences;

            prefs.FireKeyboardEventsEnabled = true;
            prefs.FireMouseEventsEnabled = true;

            browserView.Browser.Preferences = prefs;

            if (Properties.Settings.Default.UseBingWallpaper)
            {
                string urlBase = GetBackgroundUrlBase();
                urlBase = urlBase + GetResolutionExtension(urlBase, ParentRect);
                browserView.Browser.LoadURL(urlBase);
            }
            else
            {
                browserView.Browser.LoadURL(Properties.Settings.Default.DisplayURL);
            }
            browserView.MouseMove += MouseMoveHandler;
            browserView.KeyDown += KeyDownHandler;
            browserView.Click += Form1_Click;
        }

        private static dynamic DownloadJson()
        {
            using (WebClient webClient = new WebClient())
            {
                Console.WriteLine("Downloading JSON...");
                string jsonString = webClient.DownloadString("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US");
                return JsonConvert.DeserializeObject<dynamic>(jsonString);
            }
        }

        private static string GetBackgroundUrlBase()
        {
            dynamic jsonObject = DownloadJson();
            return "https://www.bing.com" + jsonObject.images[0].urlbase;
        }

        private static bool WebsiteExists(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "HEAD";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        private static string GetResolutionExtension(string url, Rectangle Bounds)
        {
            Rectangle resolution = Bounds;
            string widthByHeight = resolution.Width + "x" + resolution.Height;
            string potentialExtension = "_" + widthByHeight + ".jpg";
            if (WebsiteExists(url + potentialExtension))
            {
                Console.WriteLine("Background for " + widthByHeight + " found.");
                return potentialExtension;
            }
            else
            {
                Console.WriteLine("No background for " + widthByHeight + " was found.");
                Console.WriteLine("Using 1920x1080 instead.");
                return "_1920x1080.jpg";
            }
        }

        #region User Input

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        //start off OriginalLoction with an X and Y of int.MaxValue, because
        //it is impossible for the cursor to be at that position. That way, we
        //know if this variable has been set yet.
        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                //see if originallocation has been set
                if (OriginalLocation.X == int.MaxValue &
                    OriginalLocation.Y == int.MaxValue)
                {
                    OriginalLocation = e.Location;
                }
                //see if the mouse has moved more than 20 pixels 
                //in any direction. If it has, close the application.
                if (Math.Abs(e.X - OriginalLocation.X) > 20 |
                    Math.Abs(e.Y - OriginalLocation.Y) > 20)
                {
                    Application.Exit();
                }
            }
        }
        #endregion
    }
}
