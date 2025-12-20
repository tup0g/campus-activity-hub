using System.ComponentModel.DataAnnotations;

namespace CampusActivityHub.Validation
{
    public class EventDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext ctx)
        {
            if (value == null) return ValidationResult.Success;

            var dt = (DateTime)value;
            
            if (dt <= DateTime.Now) 
                return new ValidationResult("Дата має бути в майбутньому");
            
            if (dt.Hour < 7 || dt.Hour > 22) 
                return new ValidationResult("Подія не може бути вночі (дозволено 07:00 - 22:00)");

            return ValidationResult.Success;
        }
    }
}