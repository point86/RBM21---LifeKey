using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using System.IO;

/*
 * 
 * By default RBM21_core will only sync database "users" and local CAME file.
 * To sync with RBM21 hardware, this program must be called with args[1] = "hardwaresync"
 */
namespace RBM21_core
{
    public partial class CoreForm : Form
    {
        static System.Windows.Forms.Timer exitTimer = new System.Windows.Forms.Timer();
        static int secondsToExit = 20;
        SettingsManager sm;

        public CoreForm()
        {            
            InitializeComponent();
            sm = new SettingsManager();
            Shown += PerformOperations;
        }

        private void PerformOperations(object sender, EventArgs e)
        {
              Tools.LogMessageToFile("CoreForm --- START OPERATION ---");
              Tools.LogSizeManager(); //if size of log file is too much (bigger than 100000 lines), cut file in half.
              System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;                          
              LogLabel.Text += "SQLiteDatabasePath: " + sm.SQLiteDB+ "\r\n";

              HardwareSync();

              LogLabel.Text += "\r\n - OPERATIONS COMPLETED -";
              button1.Enabled = true;
              exitTimer.Tick += new EventHandler(TimerEventProcessor);          
              exitTimer.Interval = 1000;
              exitTimer.Start();

              Tools.LogMessageToFile("CoreForm - FINISH OPERATIONS -");             
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Tools.LogMessageToFile("CoreForm - Exception Occurred: "+ e.ExceptionObject.ToString());
            MessageBox.Show(e.ExceptionObject.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            //  Environment.Exit(1);
        }

        /* if secondsToExit == 0 -> Close application, otherwise decrement counter */
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (secondsToExit == 0)
                Application.Exit();
            button1.Text = "Close (" + secondsToExit + ")";
            secondsToExit--;
            exitTimer.Start();
        }

            private void HardwareSync()
        {
            LogLabel.Text += "Performing sync with RBM21 on "+ sm.SerialPort + "\r\n";
            Tools.LogMessageToFile("CoreForm - Performing sync with RBM21 on Port: " + sm.SerialPort + " SQLite file: " + sm.SQLiteDB);
            
            Rbm21Polling rbm21 = new Rbm21Polling(sm.SerialPort); //se non va e si pianta? che fffamo?
            DBmanager dbm = new DBmanager(sm.SQLiteDB);
            
            List<User> rbm21List = new List<User>();
            rbm21List = rbm21.ReadAllUsers();            
            rbm21.Close();
                        
            foreach (User usr in rbm21List)
            {              
                DateTime rbm21Time = usr.Time; //from rbm1 acquired data
                DateTime dbTime = dbm.GetLastEntrance(usr.Key);
             
                int result1 = DateTime.Compare(dbTime, rbm21Time);
                if (result1 >= 0)
                    continue;
                //else we have to upgrade "Entrances" table.
                int r1 = dbm.AddEntrance(usr.Key, rbm21Time);                
                LogLabel.Text += "Aggiunto ingresso: \"" + usr.Key + "\" Time:" + rbm21Time.ToString() + "\r\n";
            }
            dbm.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // Tools.LogMessageToFile("CoreForm - button1_Click(), Application.Exit()");
            Application.Exit();
        }


        

    }
}
