using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace atnet_service
{
    public partial class Service1 : ServiceBase
    {
        private const string _logFileLocation = @"C:\temp\servicelog.txt";
        private static System.Timers.Timer aTimer;

        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
        }

        protected override void OnStart(string[] args)
        {
            Log("Starting");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Log("Stopping");
            base.OnStop();
        }

        protected override void OnPause()
        {
            Log("Pausing");
            base.OnPause();
        }

        protected static void SetTimer()
        {
            // časovač na 3600 vteřin => 1 hodina
            aTimer = new Timer(60 * 60 * 1000);
            // událost zavolá metodu OnTimedEvent 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                e.SignalTime);
        }
    }
}
