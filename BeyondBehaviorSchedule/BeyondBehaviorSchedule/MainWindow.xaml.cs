using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeyondBehaviorSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            XmlDataProvider ClientDefault = new XmlDataProvider();
            ClientDefault.Document.Load("ClientDefault.xml");
        }

        private void WeekStarting_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(this.WeekStarting.SelectedDate.Value.DayOfWeek > DayOfWeek.Monday)
            {
                this.WeekStarting.SelectedDate = this.WeekStarting.SelectedDate.Value.AddDays((double)(-1 * ((int)this.WeekStarting.SelectedDate.Value.DayOfWeek - 1)));
            }
            else if(this.WeekStarting.SelectedDate.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                this.WeekStarting.SelectedDate = this.WeekStarting.SelectedDate.Value.AddDays(1);
            }
        }
    }
}
