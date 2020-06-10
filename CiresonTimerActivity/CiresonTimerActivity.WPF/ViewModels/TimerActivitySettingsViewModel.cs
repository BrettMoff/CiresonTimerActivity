using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private bool _LogEnableDBValue; //Variable to hold the value read from the DB when form loaded.
        private string _LogPathDBValue; //Variable to hold the path value read from the DB when form loaded.

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
       
        public Action CloseAction { get; set; }
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
            if ((this._LogEnable != _LogEnableDBValue) || (this._LogPath != _LogPathDBValue)) //If the form value does not match the saved value, then the form has changed and can be saved.
            { 
                return true;
            }
            else
            { 
                return false;
            } 
        }

        /// The user pressed the cancel button on the form.
        public void CancelMethod()
        {
            CloseAction();
        }

        /// The user pressed the OK button on the form.
        public void OKMethod()
        {
            try
            {
                SaveFormData();
                CloseAction();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw; 
            }
        }

        /// <summary>
        /// This only gets called in test mode, as the SCSM window would normally just save the idataitem.
        /// </summary>
        public void SaveFormData()
        {
            //Get and set the values for our settings object
            var emoSettingsObject = Common.GetSettingsObject(Constants.TimerActivityClassName); //Cireson.Timer.Activity.Settings
            emoSettingsObject[null, "LogEnable"].Value = this.LogEnable;
            emoSettingsObject[null, "LogPath"].Value = this.LogPath.Trim();
            emoSettingsObject.Commit();
        }

        public void LoadFormData()
        {
            var emoSettingsObject = Common.GetSettingsObject(Constants.TimerActivityClassName); //Cireson.Timer.Activity.Settings
            bool isLogEnabledFromSettings = false;
            bool.TryParse((string)emoSettingsObject[null, "LogEnable"].Value, out isLogEnabledFromSettings);
            _LogEnableDBValue = isLogEnabledFromSettings;

            string strLogPathFromSettings = "";
            if (emoSettingsObject[null, "LogPath"].Value != null)
            {
                strLogPathFromSettings = (string)emoSettingsObject[null, "LogPath"].Value;
            }
            _LogPathDBValue = strLogPathFromSettings;

            //Set our values on the form.
            this.LogEnable = isLogEnabledFromSettings;
            this.LogPath = strLogPathFromSettings;
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
