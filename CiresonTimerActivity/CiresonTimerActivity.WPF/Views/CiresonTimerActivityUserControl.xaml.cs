using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.GenericForm;
using Microsoft.EnterpriseManagement.ServiceManager.Application.Common;
using Microsoft.EnterpriseManagement.UI.DataModel;
using Microsoft.EnterpriseManagement.UI.WpfControls;
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
using Xceed.Wpf.Toolkit;

namespace Cireson.Timer.Activity.WPF
{
    /// <summary>
    /// Interaction logic for CiresonTimerActivityUserControl.xaml
    /// </summary>
    public partial class CiresonTimerActivityUserControl : UserControl
    {

        public CiresonTimerActivityViewModel activityViewModel; //null to start

        private RelatedItemsPane relatedItemsPane;
        public bool _isTemplateMode = false; //This form was actually opened as a template, not a real work item

        public CiresonTimerActivityUserControl()
        {
            InitializeComponent();

            AddEventHanders();

            AddFormControls();

        }

        private void AddEventHanders()
        {
            this.lblTimeFrameTypeDuration.PreviewMouseDown += lblTimeFrameTypeDuration_PreviewMouseDown;
            this.lblTimeFrameTypeSpecificDateTime.PreviewMouseDown += lblTimeFrameTypeSpecificDateTime_PreviewMouseDown;

            this.panelTimeControls.LostFocus += TimeControls_LostFocus;

            //For loaded and data context changed events
            this.DataContextChanged += UserControl_DataContextChanged;

            //Only allow numbers to be entered into the Duration Units textbox
            this.txtDurationUnits.PreviewTextInput += NumbersOnly_PreviewTextInput;
            this.txtDurationUnits.TextChanged += TriggerTimeValidation_TextChanged;
            this.txtDurationUnits.GotFocus += TriggerTimeValidation_GotFocus;
            this.dateScheduledEndDate.GotFocus += TriggerTimeValidation_GotFocus;
            this.rdbTimeFrameTypeDuration.Checked += TriggerTimeValidation_GotFocus;
            this.rdbTimeFrameTypeSpecificDateTime.Checked += TriggerTimeValidation_GotFocus;

        }

        private void TriggerTimeValidation_GotFocus(object sender, RoutedEventArgs e)
        {
            bindingGroupTimeFrame.CommitEdit();
        }

        private void TriggerTimeValidation_TextChanged(object sender, TextChangedEventArgs e)
        {
            bindingGroupTimeFrame.CommitEdit();
        }

        private void NumbersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Constants.regex_NumericOnly.IsMatch(e.Text);
        }
        

        private void TimeControls_LostFocus(object sender, RoutedEventArgs e)
        {
            if (rdbTimeFrameTypeSpecificDateTime.IsChecked == true)
            {
                //The radio button changed. See if any of the child controls contain the access
                if (rdbTimeFrameTypeSpecificDateTime.IsFocused || panelTimeFrameSpecificDateTime.IsFocused || lblTimeFrameTypeSpecificDateTime.IsFocused || borderTimeFrameSpecificDateTime.IsFocused || dateScheduledEndDate.IsFocused)
                    return;
            }
            else if (rdbTimeFrameTypeDuration.IsChecked == true)
            {
                //The parent radio button control prolly lost focus to one of the child controls
                if (rdbTimeFrameTypeDuration.IsFocused || panelTimeFrameDuration.IsFocused || lblTimeFrameTypeDuration.IsFocused || txtDurationUnits.IsFocused || cbxDurationEnum.IsFocused)
                    return;
            }
            
            bindingGroupTimeFrame.CommitEdit();

        }


        private void AddFormControls()
        {
            this.relatedItemsPane = new RelatedItemsPane(new WorkItemRelatedItemsConfiguration("RelatedWorkItem", "RelatedWorkItemSource", "RelatedConfigItem", "RelatedKnowledge", "FileAttachment"));
            this.tabRelatedItems.Content = this.relatedItemsPane;

            this.tabHistory.Content = new HistoryTab(); //Put this here instead of the WPF form because the WPF form wigs out half the time when debugging.
        }


        private void lblTimeFrameTypeSpecificDateTime_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.rdbTimeFrameTypeSpecificDateTime.IsChecked = true;
        }

        private void lblTimeFrameTypeDuration_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.rdbTimeFrameTypeDuration.IsChecked = true;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null && this.DataContext is IDataItem)
            {
                var dataItem = this.DataContext as IDataItem;
                FormUtilities.Instance.OpenParentWI(dataItem);
            }
            else
            {
                System.Windows.MessageBox.Show("Cannot open parent work item for non-IDataItem", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        public void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Our datacontext likes to change after the form has been loaded. Refresh some stuff.
            Console.WriteLine("UserControl_DataContextChanged"); //TODO: delete me.

            if (this.DataContext == null)
                return;

            if (this.DataContext is IDataItem == false && this.DataContext is EnterpriseManagementObject == false && this.DataContext is EnterpriseManagementObjectProjection == false)
                return;

            this.IsEnabled = false;

            //SCSM will eventually load the DataContext as an IDataItem. This can't really be changed, since SCSM requires an IDataItem when pressing the OK or Apply buttons.
            //Set the dataitem from the DataContext

            if (this.DataContext is IDataItem)
            {
                var idataItem = this.DataContext as IDataItem;

                //If this is a new item, then double check and make sure that our workitem ID has a prefix.
                if ((bool)idataItem["$IsNew$"] == true)
                {
                    var strPrefix = Microsoft.EnterpriseManagement.UI.Extensions.Shared.ConsoleContextHelper.Instance.GetActivityPrefix(idataItem);
                    if (strPrefix != null && strPrefix.Length > 0 && idataItem["Id"].ToString().StartsWith(strPrefix) == false)
                    {
                        //it's a brand new WI? Add hte prefix.
                        idataItem["Id"] = strPrefix + idataItem["Id"];
                    }
                }

                //Merge this form with the instance, using our full type projection
                //ConsoleContextHelper.Instance.FetchAndMergeSubProjection(idataItem, guid_notificationActivityFullProjection); //Obsolete
                Microsoft.EnterpriseManagement.UI.Extensions.Shared.ConsoleContextHelper.Instance.FetchAndMergeSubProjection(idataItem, Constants.guid_Projection_CiresonTimerActivity_FullProjection);



                //We also have to load our viewmodel with other properties.
                activityViewModel = new CiresonTimerActivityViewModel(idataItem, IsTemplateMode());

                paneltestconfirmation.Visibility = Visibility.Hidden;
            }
            else if (this.DataContext is EnterpriseManagementObject)
            {
                var emoActivity = this.DataContext as EnterpriseManagementObject; //Set this globally here. 

                activityViewModel = new CiresonTimerActivityViewModel(emoActivity, IsTemplateMode()); //Also set this globally here.

                paneltestconfirmation.Visibility = Visibility.Visible;
            }
            else if (this.DataContext is EnterpriseManagementObjectProjection)
            {
                throw new NotImplementedException("Code for EnterpriseManagementObject Projection not implemented yet");
            }


            //Update the datacontext with our new viewmodel, but in a different thread. 
            Task.Factory.StartNew(UpdateDataContextFromViewModel);  //UpdateViewModelFromScsmObject(naViewModel); //Calling this here without a task would not be from a different thread, and we would see no change.
            //this.DataContext = naViewModel; //Removes the iDataItem or EMO or EMOP and replaces it with our viewModel. Doesn't work within this method.
        }

        private void UpdateDataContextFromViewModel()
        {
            if (activityViewModel == null)
                throw new NullReferenceException("The ViewModel cannot be null when setting the DataContext.");

            //System.Threading.Thread.Sleep(2000);
            var thisWindowDispatcher = this.Dispatcher;

            thisWindowDispatcher.BeginInvoke(new Action(() =>
            {

                this.maingrid.DataContext = activityViewModel; //So that we can access our ActivityViewModelHelper natively from the UI for an actual MVVM experience.
                this.tabHistory.DataContext = this.DataContext; //So that the history tab simply works
                this.tabRelatedItems.DataContext = this.DataContext; //So that the related item tab simply works
                this.textParentWorkItem.DataContext = this.DataContext; //So that we can show the link to the real parent work item

                //We also need to know if this is in template mode or not.
                _isTemplateMode = IsTemplateMode();

                //Trigger our form validation for our binding group.
                bindingGroupTimeFrame.CommitEdit();

                this.IsEnabled = true;
                Console.WriteLine("Completed setting DataContexts");

            }), null);


        }

        private bool IsTemplateMode()
        {
            if (this.DataContext is IDataItem)
            {
                FrameworkElement parentForm = this;
                return FormUtilities.Instance.IsFormInTemplateMode(parentForm);
            }
            return false;
        }

    }
}
