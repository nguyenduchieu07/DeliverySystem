using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.Helpers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var currentValue = value as DateTime?;
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            return new ValidationResult($"Không tìm thấy thuộc tính {_comparisonProperty}");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (currentValue.HasValue && comparisonValue.HasValue)
        {
            if (currentValue.Value <= comparisonValue.Value)
            {
                return new ValidationResult(ErrorMessage ??
                                            $"{validationContext.DisplayName} phải lớn hơn {_comparisonProperty}");
            }
        }

        return ValidationResult.Success;
    }
}