using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace PlatformDemo.ModelValidations
{
    public class Ticket_EnsureDueDateForTicketOwner : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var x = validationContext.ObjectInstance as Ticket;

            if (x != null && !string.IsNullOrEmpty(x.Description))
            {
                if (x.ProjectId != 2)
                {
                    return new ValidationResult("Project id must be equal to 2");
                }
            }
            return ValidationResult.Success;
        }
    }
}
