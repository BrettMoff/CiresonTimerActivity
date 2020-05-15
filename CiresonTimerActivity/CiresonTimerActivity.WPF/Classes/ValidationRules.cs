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

    public class CustomerObjectiveOrReasonValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            BindingGroup bindingGroup = value as BindingGroup;
            if (bindingGroup == null)
                return new ValidationResult(false,
                  "CustomerObjectiveOrReasonValidationRule should only be used with a BindingGroup");

            if (bindingGroup.Items.Count == 1)
            {
                object item = bindingGroup.Items[0];

                CiresonTimerActivityViewModel viewModel = item as CiresonTimerActivityViewModel;
                if (viewModel != null && viewModel.HasValidDateInfoEntered() == false)
                    return new ValidationResult(false,
                      "You must enter one of Customer, Objective, or Reason to a valid entry");
            }
            return ValidationResult.ValidResult;
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

                if (value is BindingExpression) //Since our XAML has ValidationStep="UpdatedValue" />, then our value is now of type BindingExpression, not the normal string that we expected.
                {
                    BindingExpression be = value as BindingExpression;
                    return new ValidationResult(false, "Was not expecting a BindingExpression here...");
                }
                else if (value is string)
                {
                    strValue = (string)value;
                    if (strValue == null || (string)strValue == "" || ((string)strValue).Trim() == "")
                        return new ValidationResult(false, "You must enter a valid value");
                }
                else if (value is DateTime)
                {
                    if (((DateTime)value) == DateTime.MinValue)
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
}
