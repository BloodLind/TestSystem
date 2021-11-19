using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestSystem.WpfUI.Views.ValidationRules
{
    public class EmptyStringvalidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((string)value).Length <= 2)
                return new ValidationResult(false, $"{Properties.Resources.WrongTextBox}");
            return ValidationResult.ValidResult;
        }
    }
}
