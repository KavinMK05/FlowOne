using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class PomodoroTimerSettings:ConfigurationSection
    {
        [ConfigurationProperty("pomodoro", DefaultValue = 25)]
        public int Pomodoro 
        { get { return (int)this["pomodoro"]; }
            set {this["pomodoro"] = value; } 
        }
        
        [ConfigurationProperty("longBreak", DefaultValue = 30)]
        public int LongBreak
        {
            get { return (int)this["longBreak"]; }
            set { this["longBreak"] = value; }
        }

        [ConfigurationProperty("shortBreak", DefaultValue = 5)]
        public int ShortBreak
        {
            get { return (int)this["shortBreak"]; }
            set { this["shortBreak"] = value; }
        }


    }
}
