using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace KubbAdminAPI.Utils;

[System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidJSON : ValidationAttribute {
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
        var json = value?.ToString();
        
        if (string.IsNullOrWhiteSpace(json)) return new ValidationResult("invalid json format");
        try {
            JsonDocument.Parse(json);
            return ValidationResult.Success;
        } catch (JsonException) {
            return new ValidationResult("invalid json format");
        }
    }
}