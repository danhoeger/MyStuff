using System;

using Nevron.Nov;
using Nevron.Nov.Barcode;
using Nevron.Nov.Chart;
using Nevron.Nov.Diagram;
using Nevron.Nov.Grid;
using Nevron.Nov.Layout;
using Nevron.Nov.Schedule;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.Windows;

namespace BeyondBehaviorScheduleApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // TODO: Apply license for redistribution here. You can skip this code when evaluating NOV.
            NLicenseManager.Instance.SetLicense(new NLicense("LICENSE KEY"));

            // Install Nevron Open Vision for WPF
            NNovApplicationInstaller.Install(
                NTextModule.Instance,
                NChartModule.Instance,
                NDiagramModule.Instance,
                NScheduleModule.Instance,
                NGridModule.Instance,
                NBarcodeModule.Instance);

            // Initialize and run the application
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}