using System.Globalization;
using System.Windows.Controls;

namespace Main_Program.Validators
{
	class NotEmptyValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return string.IsNullOrWhiteSpace((value ?? "").ToString())
				? new ValidationResult(false, "Строка пустая")
				: ValidationResult.ValidResult;
		}
	}
}
