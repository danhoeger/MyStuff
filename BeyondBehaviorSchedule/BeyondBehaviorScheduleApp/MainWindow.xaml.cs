using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Nevron.Nov.Schedule;
using Nevron.Nov.UI;
using Nevron.Nov.Windows;

namespace BeyondBehaviorScheduleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set window size
            this.Width = 800;
            this.Height = 600;

            // Create NOV content
            NNovWidgetHost<NScheduleViewWithRibbon> novContent = new NNovWidgetHost<NScheduleViewWithRibbon>();
            Content = novContent;

            // Add appointments
            AddAppointments(novContent.Widget.View);
        }

        private void AddAppointments(NScheduleView scheduleView)
        {
            // Pause history service to prevent creating undo actions for the added content
            scheduleView.Document.PauseHistoryService();

            // Get the schedule
            NSchedule schedule = scheduleView.Content;

            // TODO: add appointments
            DateTime today = DateTime.Today;
            schedule.Appointments.Add(new NAppointment("Meeting with John", today.AddHours(9), today.AddHours(10)));
            schedule.Appointments.Add(new NAppointment("Lunch", today.AddHours(12), today.AddHours(13)));
            schedule.Appointments.Add(new NAppointment("Video Presentation", today.AddHours(14), today.AddHours(16)));

            // Resume history service
            scheduleView.Document.ResumeHistoryService();
        }
    }
}
