using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cireson.Timer.Activity.WPF
{
    public class TimerActivitySettingsViewModel : INotifyPropertyChanged
    {



        #region Variables

        //TODO: Create our two variables
        //TODO: On the WPF form, create bindings in our two fields that match the same variable names.
        //TODO: On the WPF form, add an OK and cancel button. Hook them up to the view model somehow. 

        #endregion

        #region Command Methods

        public ICommand CancelCommand
        {
            get { return new RelayCommand(CancelMethod); }
        }

        public ICommand OKCommand
        {
            get { return new RelayCommand(OKMethod, CanPressOk); }
        }

        public bool CanPressOk()
        {
            return false; //Do some logic here to determine if I can press OK?
        }

        /// <summary>
        /// The user pressed the cancel button on the form.
        /// </summary>
        public void CancelMethod()
        {
            
        }

        public void OKMethod()
        {
            try
            {
                SaveFormData();
            }
            catch (Exception) { throw; }

        }


        /// <summary>
        /// This only gets called in test mode, as the SCSM window would normally just save the idataitem.
        /// </summary>
        public void SaveFormData()
        {


        }

        #endregion

        #region PropertyChanged Method
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
