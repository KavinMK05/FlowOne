using System.Configuration;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class settings_window : Window
    {   
        public MainWindow.UpdateTimerDelegate UpdateTimer { get;set; }
        public MainWindow.UpdateTotalSessionDelegate UpdateTotalSession { get; set; }
        public settings_window()
        {
            InitializeComponent();

            PomodoroTextBox.Text = Properties.Settings.Default.Pomodoro.ToString();
            ShortBreakTextBox.Text = Properties.Settings.Default.ShortBreak.ToString();
            SessionNumber.Text = Properties.Settings.Default.Sessions.ToString();

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Pomodoro = int.Parse(PomodoroTextBox.Text);
            Properties.Settings.Default.ShortBreak = int.Parse(ShortBreakTextBox.Text);
            Properties.Settings.Default.Sessions = int.Parse(SessionNumber.Text);
            Properties.Settings.Default.Save();

            UpdateTotalSession.Invoke();
            UpdateTimer?.Invoke();

            this.Close();
        }
    }
}
