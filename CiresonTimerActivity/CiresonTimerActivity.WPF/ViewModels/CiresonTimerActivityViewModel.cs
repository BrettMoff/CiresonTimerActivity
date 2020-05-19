using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.EnterpriseManagement.UI.DataModel;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess.DataAdapters;
using Microsoft.EnterpriseManagement.Configuration;

namespace CiresonTimerActivity.WPF
{
    public class CiresonTimerActivityViewModel : INotifyPropertyChanged
    {

        public IDataItem _dataItem;
        public EnterpriseManagementObject _emoActivity;
        private bool _isTemplateMode; //Disables validation

        public CiresonTimerActivityViewModel(IDataItem dataItem, bool isTemplateMode)
        {
            //Set this idataitem equal to what SCSM set it to. 
            _dataItem = dataItem;
            this._isTemplateMode = isTemplateMode;
            LoadFormData(dataItem);
            
        }

        public CiresonTimerActivityViewModel(EnterpriseManagementObject emoActivity, bool isTemplateMode)
        {

            _emoActivity = emoActivity;
            this._isTemplateMode = isTemplateMode; 
            LoadFormDataAsync(emoActivity);
        }



        #region Variables

        private bool _isGuiTestMode; //Enables the OK and Cancel buttons on the main form.
        public bool isGuiTestMode
        {
            get
            {
                return this._isGuiTestMode;
            }
            set
            {
                if (this._isGuiTestMode != value)
                    this._isGuiTestMode = value;
            }
        }


        private string _lblisGuiTestMode;
        public string lblisGuiTestMode
        {
            get
            {
                return this._lblisGuiTestMode;
            }
            set
            {
                if (this._lblisGuiTestMode != value)
                    this._lblisGuiTestMode = value;
            }
        }

        private string _Id;
        public string Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if (this._Id != value)
                {
                    this._Id = value;
                    string strPropertyName = "Id";
                    RaisePropertyChanged(strPropertyName);
                    //UpdateScsmObject(strPropertyName, value); //how about we don't update the ID key field...
                }
            }
        }

        private ManagementPackEnumeration _Status;
        public ManagementPackEnumeration Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if (this._Status != value)
                {
                    this._Status = value;
                    string strPropertyName = "Status";
                    RaisePropertyChanged(strPropertyName);
                    UpdateScsmObject(strPropertyName, (ManagementPackEnumeration)value);
                }
            }
        }


        private string _Title;
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if (this._Title != value)
                {
                    this._Title = value;
                    string strPropertyName = "Title";
                    RaisePropertyChanged(strPropertyName);
                    UpdateScsmObject(strPropertyName, value);
                }
            }
        }


        private string _Description;
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
                string strPropertyName = "Description";
                RaisePropertyChanged(strPropertyName);
                UpdateScsmObject(strPropertyName, value);
            }
        }

        private DateTime? _ScheduledEndDate;
        public DateTime? ScheduledEndDate
        {
            get
            {
                return this._ScheduledEndDate;
            }
            set
            {
                this._ScheduledEndDate = value;
                string strPropertyName = "ScheduledEndDate";
                RaisePropertyChanged(strPropertyName);
                UpdateScsmObject(strPropertyName, value); 
            }
        }


        private DateTime _TimeAdded;
        public DateTime TimeAdded
        {
            get
            {
                return this._TimeAdded;
            }
            set
            {
                if (this._TimeAdded != value)
                {
                    this._TimeAdded = value;
                    string strPropertyName = "TimeAdded";
                    RaisePropertyChanged(strPropertyName);
                    //UpdateScsmObject(strPropertyName, value); //This TimeAdded property is read-only.
                }
            }
        }

        private bool _rdbTimeFrameTypeDuration;
        public bool rdbTimeFrameTypeDuration
        {
            get
            {
                return this._rdbTimeFrameTypeDuration;
            }
            set
            {
                this._rdbTimeFrameTypeDuration = value;
                string strPropertyName = "rdbTimeFrameTypeDuration";
                RaisePropertyChanged(strPropertyName);
                if (_isTemplateMode && rdbTimeFrameTypeDuration == true)
                    ScheduledEndDate = null;
                //UpdateScsmObject(strPropertyName, intTimeDelay); //not a real SCSM property. Form only.
            }
        }

        private bool _rdbTimeFrameTypeSpecificDateTime;
        public bool rdbTimeFrameTypeSpecificDateTime
        {
            get
            {
                return this._rdbTimeFrameTypeSpecificDateTime;
            }
            set
            {
                this._rdbTimeFrameTypeSpecificDateTime = value;
                string strPropertyName = "rdbTimeFrameTypeSpecificDateTime";
                RaisePropertyChanged(strPropertyName);
                if (_isTemplateMode && rdbTimeFrameTypeSpecificDateTime == true)
                {
                    TimeDelay = null;
                    selectedDurationType = null;
                }
                //UpdateScsmObject(strPropertyName, intTimeDelay); //not a real SCSM property. Form only.
            }
        }


        private string _TimeDelay; //it's actually an integer.
        public string TimeDelay
        {
            get
            {
                return this._TimeDelay;
            }
            set
            {
                this._TimeDelay = value;
                string strPropertyName = "TimeDelay";
                RaisePropertyChanged(strPropertyName);
                int intTimeDelay = 0;
                if (int.TryParse(TimeDelay, out intTimeDelay))
                    UpdateScsmObject(strPropertyName, intTimeDelay);
            }
        }

        private ObservableCollection<ManagementPackEnumeration> _timeDurationTypeSource = new ObservableCollection<ManagementPackEnumeration>();
        public ObservableCollection<ManagementPackEnumeration> timeDurationTypeSource
        {
            //Just the initial creation of the combo box.
            get
            {
                //return this._notificationTemplateSource;
                return _timeDurationTypeSource ?? (_timeDurationTypeSource = new ObservableCollection<ManagementPackEnumeration>());
            }
            set
            {
                var newObsColVal = (ObservableCollection<ManagementPackEnumeration>)value;
                if (newObsColVal != _timeDurationTypeSource)
                {
                    foreach (var thisTimerTypeEnum in newObsColVal)
                    {
                        //Add it to our collection.
                        if (_timeDurationTypeSource.Contains(thisTimerTypeEnum) == false)
                        {
                            this._timeDurationTypeSource.Add(thisTimerTypeEnum); //do not worry about deleting
                        }
                    }
                    //this._notificationTemplateSource = value; //kinda bad for binding. do not use this.
                    RaisePropertyChanged("notificationTemplateSource");
                }
            }
        }


        private ManagementPackEnumeration _selectedDurationType;
        public ManagementPackEnumeration selectedDurationType
        {
            get
            {
                return this._selectedDurationType;
            }
            set
            {
                if (value != null && _selectedDurationType != value)
                {
                    _selectedDurationType = value;
                    string strPropertyName = "TimeDelayType_Enum";
                    RaisePropertyChanged(strPropertyName);
                    //The SCSM property name for the enumeration is TimeDelayType_Enum
                    strPropertyName = "TimeDelayType_Enum";
                    UpdateScsmObject(strPropertyName, selectedDurationType);
                }
            }
        }




        private void UpdateScsmObject(string strPropertyName, object value)
        {
            if (_dataItem == null && _emoActivity == null)
                throw new NullReferenceException("Both the idataitem and EnterpriseManagementObject cannot be null");

            
            if (value == null || value is string || value is int || value is Guid || value is DateTime)
            {
                if (_dataItem != null)
                    _dataItem[strPropertyName] = value;

                if (_emoActivity != null)
                    _emoActivity[null, strPropertyName].Value = value;
            }
            else if (value is ManagementPackEnumeration)
            {
                ManagementPackEnumeration enumValue = value as ManagementPackEnumeration;
                if (_dataItem != null)
                    _dataItem[strPropertyName] = value;

                if (_emoActivity != null)
                    _emoActivity[null, strPropertyName].Value = enumValue.Id;
            }
            else
            {
                throw new InvalidCastException("Unknown value type to update");
            }
        }

        #endregion



        private void LoadFormData(IDataItem dataItem)
        {
            var emo = Common.ConvertIdataitemToEmo(dataItem);
            LoadFormDataAsync(emo);
        }



        private async void LoadFormDataAsync(EnterpriseManagementObject emoActivity)
        {
            //Load our property values from this object.
            this.Id = GetValueFromEmoProperty(emoActivity, "Id");
            this.Title = GetValueFromEmoProperty(emoActivity, "Title");
            this.Description = GetValueFromEmoProperty(emoActivity, "Description");

            this.Status = GetEnumerationFromEmoProperty(emoActivity, "Status");
            if (this.Status == null && _isTemplateMode == false)
                this.Status = Common.GetEnumerationFromGuid(Constants.guidEnum_ActivityStatusEnum_Ready);

            this.TimeAdded = emoActivity.TimeAdded.ToLocalTime();

            this.ScheduledEndDate = GetDateTimeFromEmoProperty(emoActivity, "ScheduledEndDate"); //converts a date to a string, if the date exists.
            this.TimeDelay = GetValueFromEmoProperty(emoActivity, "TimeDelay");

            //Show the time information, prioritizing scheduled start date over duration.
            if (this.ScheduledEndDate == null) {
                rdbTimeFrameTypeSpecificDateTime = false;
                if (TimeDelay == null)
                    rdbTimeFrameTypeDuration = false;
                else
                    rdbTimeFrameTypeDuration = true;
            }
            else
            {
                this.TimeDelay = null; //blank out the time delay.
                rdbTimeFrameTypeSpecificDateTime = true;
            }

            await Task.Run(() => LoadFormDataTimerTypeEnums(emoActivity) ); //loads the enums and sets the saved value.
            
        }


        private string GetValueFromEmoProperty(EnterpriseManagementObject emoObject, string strPropertyName)
        {
            if (emoObject[null, strPropertyName] == null || emoObject[null, strPropertyName].Value == null || emoObject[null, strPropertyName].Value.ToString() == null)
            {
                return null;
            }

            DateTime parsedDateTime;
            if ((strPropertyName.StartsWith("Date") || strPropertyName.EndsWith("Date")) && DateTime.TryParse(strPropertyName, out parsedDateTime))
            {
                parsedDateTime = parsedDateTime.ToLocalTime();
                return parsedDateTime.ToLongDateString() + " " + parsedDateTime.ToLongTimeString();
            }

            return emoObject[null, strPropertyName].Value.ToString().Trim(); //it could be a GUID or something. Surprise us!
        }

        private DateTime? GetDateTimeFromEmoProperty(EnterpriseManagementObject emoObject, string strPropertyName)
        {
            if (emoObject[null, strPropertyName] == null || emoObject[null, strPropertyName].Value == null)
                return null;

            DateTime? parsedDateTime = emoObject[null, strPropertyName].Value as DateTime?;
            if (parsedDateTime != null)
            {
                parsedDateTime = ((DateTime)parsedDateTime).ToLocalTime();
                return parsedDateTime;
            }

            //The direct assignment failed? Try a parse.
            DateTime parsedDateTimeTemp;
            if (DateTime.TryParse(strPropertyName, out parsedDateTimeTemp))
            {
                parsedDateTime = (DateTime)parsedDateTimeTemp.ToLocalTime();
                return parsedDateTime;
            }
            
            return null;
        }

        public ManagementPackEnumeration GetEnumerationFromEmoProperty(EnterpriseManagementObject emoObject, string strPropertyName)
        {
            if (emoObject[null, strPropertyName] == null || emoObject[null, strPropertyName].Value == null)
            {
                return null;
            }

            if (emoObject[null, strPropertyName].Value is ManagementPackEnumeration)
            {
                try
                {
                    return (ManagementPackEnumeration)emoObject[null, strPropertyName].Value;
                }
                catch (Exception) { /* do nothing */  }
            }

            return null;
        }

        private void LoadFormDataTimerTypeEnums(EnterpriseManagementObject emoActivity)
        {
            //Load our list of Timer Type enumerations
            var emg = emoActivity.ManagementGroup;

            var mpcTimerActivity = emoActivity.GetLeastDerivedNonAbstractClass().GetManagementPack();

            var enumTimerDelayTypeRootEnum = emg.EntityTypes.GetEnumeration("TimeDelayTypeEnum", mpcTimerActivity); //The room enum.
            var ilistEnumTimerTypes = (emg.EntityTypes.GetChildEnumerations(enumTimerDelayTypeRootEnum.Id, TraversalDepth.Recursive)).OrderBy(x => x.Ordinal);
            
            ObservableCollection<ManagementPackEnumeration> ocEnumerations = new ObservableCollection<ManagementPackEnumeration>(ilistEnumTimerTypes);

            this.timeDurationTypeSource = ocEnumerations;

            //Set the selected value that was previously saved, if it exists.
            if (ocEnumerations.Count > 0)
            {

                //And also select the template that was previously saved on the activity.
                ManagementPackEnumeration mpeSavedEnum = GetEnumerationFromEmoProperty(emoActivity, "TimeDelayType_Enum"); //might be null
                ManagementPackEnumeration matchingEnum = null;

                if (mpeSavedEnum != null)
                    matchingEnum = this.timeDurationTypeSource.Where(x => x.Name == mpeSavedEnum.Name).FirstOrDefault();
                if (matchingEnum != null)
                {
                    this.selectedDurationType = matchingEnum; //And select our current value that is saved on the form.
                    //this.SelectedNotificationTemplateDescription = matchingNotificationTemplate.templateDescription;
                }
            }
        }

        internal bool HasValidDateInfoEntered()
        {
            string tempstr;
            return HasValidDateInfoEntered(out tempstr);
        }

        internal bool HasValidDateInfoEntered(out string stringError)
        {

            if (_isTemplateMode)
            {
                stringError = null;
                return true;
            }
            //bool blnCalendarDateIsValid = true, blnTimerDateIsValid = true;

            if (this.Status == null || this.Status.Id == Constants.guidEnum_ActivityStatusEnum_Ready)
            {
                //it's either a new activity, or pending activity. Make sure the date doesn't occur in the past.
                if (rdbTimeFrameTypeSpecificDateTime == true)
                {
                    if (this.ScheduledEndDate == null)
                    {
                        stringError = "The scheduled date must have a value.";
                        return false;
                    }
                    else if (this.ScheduledEndDate < DateTime.Now)
                    {
                        stringError = "The scheduled date should occur in the future.";
                        return false;
                    }
                    this.TimeDelay = null;
                }
                else if (rdbTimeFrameTypeDuration == true)
                {
                    if (String.IsNullOrEmpty(this.TimeDelay) == true || Constants.regex_NumericOnly.IsMatch(this.TimeDelay) == false)
                    {
                        stringError = "You must enter a number for the time duration.";
                        return false;
                    }
                    else if (int.Parse(this.TimeDelay) < 1)
                    {
                        stringError = "The time duration must be 1 or greater.";
                        return false;
                    }
                    else if (this.selectedDurationType == null || this.selectedDurationType.DisplayName == null) {
                        stringError = "The time duration units must be selected.";
                        return false;   
                    }
                    this.ScheduledEndDate = null;
                }
                else
                {
                    stringError = "A date or durattion must be selected";
                    return false;
                }
            }
            stringError = null;
            return true;
        }


        #region Command Methods

        public ICommand CancelCommand
        {
            get { return new RelayCommand(CancelMethod); }
        }

        public ICommand OKCommand
        {
            get { return new RelayCommand(OKMethod, HasValidDateInfoEntered); }
        }

        /// <summary>
        /// The user pressed the cancel button on the form.
        /// </summary>
        public void CancelMethod()
        {
            lblisGuiTestMode = "Cancel pressed!";
        }

        public void OKMethod()
        {
            lblisGuiTestMode = "OK pressed!";
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

            this._emoActivity[null, "Title"].Value = this.Title;
            this._emoActivity[null, "Description"].Value = this.Description;

            if (this.Status == null)
                this._emoActivity[null, "Status"].Value = Constants.guidEnum_ActivityStatusEnum_Ready; //This is usually possible if something created an orphaned activity and didn't set a status.
            else
                this._emoActivity[null, "Status"].Value = this.Status.Id;

            if (ScheduledEndDate != null)
                this._emoActivity[null, "ScheduledEndDate"].Value = this.ScheduledEndDate;

            if (TimeDelay != null && selectedDurationType.DisplayName != null)
            {
                this._emoActivity[null, "TimeDelay"].Value = this.TimeDelay;
                this._emoActivity[null, "TimeDelayType_Enum"].Value = this.selectedDurationType.Id;
            }

            this._emoActivity.Commit();

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
