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

namespace Cireson.Timer.Activity.WPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TimerActivitySettingsWindow : Window
    {
        TimerActivitySettingsViewModel settingsViewModel;
        public TimerActivitySettingsWindow()
        {
            InitializeComponent();
            settingsViewModel = new TimerActivitySettingsViewModel();
            this.DataContext = settingsViewModel;

            if (settingsViewModel.CloseAction == null)
                settingsViewModel.CloseAction = new Action(this.Close);
        }
    }
}
