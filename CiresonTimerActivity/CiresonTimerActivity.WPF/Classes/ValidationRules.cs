using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CiresonTimerActivity.WPF
{
    public class TextNotNullOrEmptyRule : ValidationRule
    {
        public TextNotNullOrEmptyRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null)
                    return new ValidationResult(false, "You must enter a non-empty value");

                string strValue = null;

                if (value is BindingExpression) //Since our XAML has ValidationStep="UpdatedValue" />, then our value is now of type BindingExpression, not the normal string that we expected.
                {
                    BindingExpression be = value as BindingExpression;
                    return new ValidationResult(false, "Was not expecting a BindingExpression here...");
                }
                else if (value is string) {
                    strValue = (string)value;
                    if (strValue == null || (string)strValue == "" || ((string)strValue).Trim() == "")
                        return new ValidationResult(false, "You must enter a valid value");
                }
                else if (value is DateTime)
                {
                    if ( ((DateTime)value) == DateTime.MinValue)
                    {
                        return new ValidationResult(false, "You must enter a valid DateTime");
                    }
                }
                

                return ValidationResult.ValidResult;

            }
            catch
            {
                return new ValidationResult(false, "You must enter a value");
            }

        }
    }

    public class ValidDateTimeSelection : ValidationRule
    {
        public ValidDateTimeSelection()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null)
                    return new ValidationResult(false, "You must enter a non-empty value");

                string strValue = null;

                
                if (value is BindingGroup) //Since our XAML has ValidationStep="UpdatedValue" />, then our value is now of type BindingExpression, not the normal string that we expected.
                {
                    BindingGroup bg = value as BindingGroup;
                    //CiresonTimerActivity ta = bg.Owner.
                    StackPanel sp = bg.Owner as StackPanel;
                    CiresonTimerActivityViewModel taViewModel = sp.DataContext as CiresonTimerActivityViewModel;
                    string strError = null;
                    if (taViewModel.HasValidDateInfoEntered(out strError))
                    {
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        return new ValidationResult(false, strError);
                    }
                }
                /*
                else if (value is BindingExpression) //Since our XAML has ValidationStep="UpdatedValue" />, then our value is now of type BindingExpression, not the normal string that we expected.
                {
                    //BindingExpression be = value as BindingExpression;
                    return new ValidationResult(false, "Was not expecting a BindingExpression here...");
                }
                else if (value is string)
                {
                    return new ValidationResult(false, "Was not expecting a string here...");
                }
                else if (value is DateTime)
                {
                    return new ValidationResult(false, "Was not expecting a DateTime here...");
                }
                */
                else
                {
                    return new ValidationResult(false, "Was not expecting this obejct here...");
                }


                return ValidationResult.ValidResult;

            }
            catch
            {
                return new ValidationResult(false, "You must make a selection and enter a value");
            }

        }
    }
}
