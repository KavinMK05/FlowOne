using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
       



        public MainWindow()
        {
            this.InitializeComponent();

            //Menu button animation
            menu_icon.MouseEnter += Menu_MouseEnter;
            menu_icon.MouseLeave += Menu_MouseLeave;

            int pomodoroTime = Properties.Settings.Default.Pomodoro;
            int shortBreakTime = Properties.Settings.Default.ShortBreak;

            //New Dispatcher Timer

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); //Sets Interval
            _timer.Tick += Timer_tick;



            //Pomodoro timer   
            _remainingTime = TimeSpan.FromMinutes(pomodoroTime); //Sets Timer 
            UpdateTimerDisplay();

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
        private void Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
            menu_icon.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            menu_icon.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }



        private void StartButton_Click(object sender, MouseEventArgs e)
        {

            _timer.Start();
            playButton.Visibility = Visibility.Hidden;
            pauseButton.Visibility = Visibility.Visible;
            
            //Moves the position of the playButton 
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = -45;
            playButton.RenderTransform = translateTransform;




            
            if (stopButton.Visibility == Visibility.Hidden)
            {
                stopButton.Visibility = Visibility.Visible;
                DoubleAnimation fadeInAnimation = new DoubleAnimation();
                fadeInAnimation.From = 0;
                fadeInAnimation.To = 1;
                fadeInAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                Storyboard storyboard2 = new Storyboard();
                storyboard2.Children.Add(fadeInAnimation);
                Storyboard.SetTarget(fadeInAnimation, stopButton);
                Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
                storyboard2.Begin();

                //Slide in transition for the pause button
                Storyboard sb = new Storyboard();

                DoubleAnimation slideAnimation = new();
                slideAnimation.From = 0;
                slideAnimation.To = -45;
                slideAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                slideAnimation.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };

                Storyboard.SetTargetName(slideAnimation, "pauseBorderTranslateTransform");
                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(TranslateTransform.XProperty));

                sb.Children.Add(slideAnimation);
                sb.Begin(this);

            }





        }


        private void PauseButton_Click(object sender, MouseEventArgs e)
        {
            _timer.Stop();
            pauseButton.Visibility = Visibility.Hidden;
            playButton.Visibility = Visibility.Visible;


            if (!(pauseButton.RenderTransform is TranslateTransform))
            {
                TranslateTransform translateTransform = new TranslateTransform();
                translateTransform.X = -45;
                pauseButton.RenderTransform = translateTransform;

            }



        }


        //Every time a second is gone by
        private void Timer_tick(object? sender, EventArgs e)
        {
            if (_remainingTime > TimeSpan.Zero)
            {
                _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerDisplay();
            }
            else
            {
                _timer.Stop();
                _isPomodoro = !_isPomodoro;
                
                    
                    _remainingTime = TimeSpan.FromMinutes(5);
                    UpdateTimerDisplay();
                    _timer.Start();
                
                
            }
        }

        //Stop Button Click functionality 
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            
                
                _timer.Stop();
                _isPomodoro = !_isPomodoro;
                _remainingTime = TimeSpan.FromMinutes(5);
                UpdateTimerDisplay();
                _timer.Start();

                pauseButton.Visibility = Visibility.Hidden;
                playButton.Visibility = Visibility.Hidden;
                stopButton.Visibility = Visibility.Hidden;

                plusButton.Visibility = Visibility.Visible;
                skipButton.Visibility = Visibility.Visible;
            
        }


        //Skip Button Functionality 
        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            
                pauseButton.Visibility = Visibility.Hidden;
                stopButton.Visibility = Visibility.Hidden;

                plusButton.Visibility = Visibility.Hidden;
                skipButton.Visibility = Visibility.Hidden;

                playButton.RenderTransform = Transform.Identity;
                playButton.Visibility = Visibility.Visible;

                
                _timer.Stop();
                _isPomodoro = !_isPomodoro;
                _remainingTime = TimeSpan.FromMinutes(50);
                UpdateTimerDisplay();
                //_timer.Start();  
            
        }
        private void plusOneButton_Click(object sender, RoutedEventArgs e)
        {

            _remainingTime += TimeSpan.FromMinutes(1);
            UpdateTimerDisplay();

        }

        //Updating the Timer text box
        private void UpdateTimerDisplay()
        {
            Focus_timer.Text = $"{(int)_remainingTime.TotalMinutes} : {_remainingTime.Seconds:00}";

        }



        //Show the settings page 
        private void showSettings(object sender, RoutedEventArgs e)
        {
            settings_window settings_Window = new();
            settings_Window.ShowDialog();

            int pomodoroTime = Properties.Settings.Default.Pomodoro;
            int shortBreakTime = Properties.Settings.Default.ShortBreak;
        }



    }
}