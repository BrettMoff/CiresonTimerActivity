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

        public TimerActivitySettingsViewModel()
        {
            LoadFormData();
        }

        #region Variables

        //TODO: Create our two variables

        private bool _LogEnable;
        public bool LogEnable
        {
            get
            {
                return this._LogEnable;
            }
            set
            {
                if (this._LogEnable != value)
                {
                    this._LogEnable = value;
                    string strPropertyName = "LogEnable";
                    RaisePropertyChanged(strPropertyName);
                }
            }
        }

        private string _LogPath;

        public string LogPath
        {
            get
            {
                return this._LogPath;
            }
            set
            {
                if (this._LogPath != value)
                {
                    this._LogPath = value;
                    string strPropertyName = "LogPath";
                    RaisePropertyChanged(strPropertyName);
                }
            }
        }
        //TODO: On the WPF form, create bindings in our two fields that match the same variable names. - DONE (I think)
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
        //Do some logic here to determine if I can press OK?
        {
            if ((this._LogEnable != LogEnable) || (this._LogPath != LogPath)) //If the form value does not match the saved value, then the form has changed and can be saved.
            { 
                return true;
            }
            else
            { 
                return false;
            } 
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
            //Get and set the values for our settings object

            throw new NotImplementedException("not yet...");
            
            var emoSettingsObject = Common.GetSettingsObject(Constants.TimerActivityClassName); //Cireson.Timer.Activity.Settings
            emoSettingsObject.Commit();
        }

        public void LoadFormData()
        {
            var emoSettingsObject = Common.GetSettingsObject(Constants.TimerActivityClassName); //Cireson.Timer.Activity.Settings
            bool isLogEnabledFromSettings = false;
            bool.TryParse((string)emoSettingsObject[null, "LogEnable"].Value, out isLogEnabledFromSettings);
            
            string isLogPathFromSettings = "";
            string.((string)emoSettingsObject[null, "LogPath"].Value, out isLogPathFromSettings);  //????? This does not need to be converted to a Bool so not sure how to pass a string from the data set

            //Set our values on the form.
            this.LogEnable = isLogEnabledFromSettings;
            this.LogPath = isLogPathFromSettings;
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
