using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private TimeSpan _remainingTime;
        private bool _isPomodoro = true; //True for pomodoro , and false for break

        
        private PomodoroTimerSettings PomodoroSettingSection;
        public MainWindow()
        {
            this.InitializeComponent();

            //Menu button animation
            menu_icon.MouseEnter += Menu_MouseEnter;
            menu_icon.MouseLeave += Menu_MouseLeave;

            
            //Load Settings
            loadSettings();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); //Sets Interval
            _timer.Tick += Timer_tick;


            //Pomodoro timer 
            int PomodoroTime = PomodoroSettingSection.Pomodoro;
            _remainingTime = TimeSpan.FromMinutes(PomodoroTime); //Sets Timer 
            UpdateTimerDisplay();
            


        }

        //Load the Settings 
        private void loadSettings()
        {
            Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            

            PomodoroSettingSection = (PomodoroTimerSettings)AppConfig.GetSection("PomodoroTimerSettings");
            this.DataContext = PomodoroSettingSection;
        }

        //To make the window draggable 
        private void DragRegion_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        //Animation to make the menu button hide and unhide
        private void Menu_MouseEnter(object sender,MouseEventArgs e)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
            menu_icon.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            menu_icon.BeginAnimation(UIElement.OpacityProperty,fadeOut);
        }

        private void StartButton_Click(object sender , MouseEventArgs e)
        {
            
            
            _timer.Start();
            playButton.Visibility = Visibility.Hidden;
            pauseButton.Visibility = Visibility.Visible;
            //Moves the position of the playButton 
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = -45;
            playButton.RenderTransform = translateTransform;
            

            //Slide in transition for the pause button
            Storyboard sb = new Storyboard();

            DoubleAnimation slideAnimation = new();
            slideAnimation.From = 0;
            slideAnimation.To = -45;
            slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            slideAnimation.EasingFunction = new CubicEase { EasingMode=EasingMode.EaseOut};

            Storyboard.SetTargetName(slideAnimation, "pauseBorderTranslateTransform");
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(TranslateTransform.XProperty));

            sb.Children.Add(slideAnimation);
            sb.Begin(this);

            if (stopButton.Visibility == Visibility.Hidden)
            {
                stopButton.Visibility = Visibility.Visible;
                DoubleAnimation fadeInAnimation = new DoubleAnimation();
                fadeInAnimation.From = 0;
                fadeInAnimation.To = 1;
                fadeInAnimation.Duration=new Duration(TimeSpan.FromSeconds(0.5));
                Storyboard storyboard2 = new Storyboard();
                storyboard2.Children.Add(fadeInAnimation);
                Storyboard.SetTarget(fadeInAnimation, stopButton);
                Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
                storyboard2.Begin();
            }
            

        }

        private void PauseButton_Click(object sender , MouseEventArgs e)
        {
            _timer.Stop();
            pauseButton.Visibility = Visibility.Hidden;
            playButton.Visibility = Visibility.Visible;

            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = -45;
            pauseButton.RenderTransform = translateTransform;



        }


        //Every time a second is gone by
        private void Timer_tick(object sender , EventArgs e)
        {
            if(_remainingTime> TimeSpan.Zero)
            {
                _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerDisplay();
            }
            else
            {
                int PomodoroTime = (int)PomodoroSettingSection.Pomodoro;
                _timer.Stop();
                _isPomodoro = !_isPomodoro;
                _remainingTime = TimeSpan.FromMinutes(PomodoroTime);
                UpdateTimerDisplay();
            }
        }

        //Stop Button Click functionality 
        private void StopButton_Click(object sender,RoutedEventArgs e) {
            int PomodoroTime = (int)PomodoroSettingSection.Pomodoro;
            _timer.Stop();
            _isPomodoro = !_isPomodoro;
            _remainingTime = TimeSpan.FromMinutes(PomodoroTime);
            UpdateTimerDisplay();

            pauseButton.Visibility = Visibility.Hidden;
            playButton.Visibility = Visibility.Hidden;
            stopButton.Visibility = Visibility.Hidden;

            plusButton.Visibility = Visibility.Visible;
            skipButton.Visibility = Visibility.Visible;
        }

        private void SkipButton_Click(object sender,RoutedEventArgs e)
        {
            pauseButton.Visibility = Visibility.Hidden;
            playButton.Visibility = Visibility.Visible;
            stopButton.Visibility = Visibility.Hidden;

            plusButton.Visibility = Visibility.Hidden;
            skipButton.Visibility = Visibility.Hidden;

            playButton.RenderTransform = null;
        }

        //Updating the Timer text box
        private void UpdateTimerDisplay()
        {
            Focus_timer.Text = $"{(int)_remainingTime.TotalMinutes} : {_remainingTime.Seconds:00}";
            
        }
        

        
        //Show the settings page 
        private void showSettings(object sender,RoutedEventArgs e)
        {
            settings_window settings_Window = new();
            settings_Window.ShowDialog();
        }

        
        
    }
}