﻿using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cireson.Timer.Activity.WPF;

namespace CiresonTimerActivity.TesterApp
{
    class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {


            Console.WriteLine("Type in the Work Item ID (e.g. 'AC1234'). Or just press enter to grab the first found Timer Activity. Tot test settings type 'settings'");
            string strID = Console.ReadLine();



            EnterpriseManagementGroup emg = new EnterpriseManagementGroup("localhost");


            ManagementPackClass mpcTimerActivity = Common.GetManagementPackClassByName("Cireson.Timer.Activity", emg);
            ManagementPackClass mpcWorkItem = Common.GetManagementPackClassByName("System.WorkItem", emg);


            EnterpriseManagementObject emoActivity = null;
            Window defaultWindow = null;

            if (String.IsNullOrEmpty(strID))
            {
                var emoReader = emg.EntityObjects.GetObjectReader<EnterpriseManagementObject>(mpcTimerActivity, ObjectQueryOptions.Default);

                if (emoReader.Count == 0)
                    throw new Exception("Found zero objects of class " + mpcTimerActivity.Name);
                emoActivity = emoReader.First();
            }
            else if (strID == "settings")
            {
                
                defaultWindow = new TimerActivitySettingsWindow();

                defaultWindow.Width = 400;
                defaultWindow.Height = 300;

            }
            else
            {
                EnterpriseManagementObjectGenericCriteria cmogc = new EnterpriseManagementObjectGenericCriteria("Name = '" + strID + "'");
                var emoReader = emg.EntityObjects.GetObjectReader<EnterpriseManagementObject>(cmogc, ObjectQueryOptions.Default);

                if (emoReader.Count == 0)
                    throw new Exception("Found zero objects with id '" + strID + "'");
                emoActivity = emoReader.First();
                var taUserControl = new CiresonTimerActivityUserControl(); //This manually triggers the user control dataContext changed event

                defaultWindow = new Window();


                defaultWindow.Width = 800;
                defaultWindow.Height = 1000;
                defaultWindow.Content = taUserControl;
                taUserControl.DataContext = emoActivity; //This simulates when SCSM sets the datacontext as an idataitem on Window initialization.


            }



            defaultWindow.ShowDialog(); //Make it modal, because we can. Also prevents the window from closing instantly. 
            //defaultWindow.Show();



        }
    }
}
