using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConsoleFramework;
using Microsoft.EnterpriseManagement.UI.DataModel;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Cireson.Timer.Activity.WPF
{
    public class AdminSettingsCommand : ConsoleCommand
    {
        public AdminSettingsCommand()
        {

        }

        public override void ExecuteCommand(IList<NavigationModelNodeBase> nodes, NavigationModelNodeTask task, ICollection<string> parameters)
        {
            TimerActivitySettingsWindow timerActivitySettingsWindow = new TimerActivitySettingsWindow();
            timerActivitySettingsWindow.ShowDialog(); //Make it modal, because we can. Also prevents two instances from opening.
        }
    }

}
