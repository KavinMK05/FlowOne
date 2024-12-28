using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class settings_window : Window
    {
        Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public settings_window()
        {
            InitializeComponent();

            if (AppConfig.Sections["PomodoroTimerSettings"] is null)
            {
                AppConfig.Sections.Add("PomodoroTimerSettings", new PomodoroTimerSettings());

            }

            var PomodoroSettingSection = AppConfig.GetSection("PomodoroTimerSettings");
            this.DataContext = PomodoroSettingSection;
        }
        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            AppConfig.Save();
        }
    }
}
