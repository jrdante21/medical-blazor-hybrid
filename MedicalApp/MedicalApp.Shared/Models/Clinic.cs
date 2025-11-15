#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MedicalApp.Shared.Data;

namespace MedicalApp.Shared.Models
{
    [Table("Clinics")]
    public class Clinic : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Clinic Name")]
        [Required]
        public string ClinicName { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Use this function to validate others using DB Context
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Get DB Context
            var dbContext = (AppDbContext)validationContext?.GetService(typeof(AppDbContext)) ?? throw new NotImplementedException();

            // Clinic name must be unique
            if (dbContext.Clinic.Any(c => c.Id != Id && c.ClinicName == ClinicName))
            {
                yield return new ValidationResult("The clinic name already used.", [nameof(ClinicName)]);
            }
        }
    }
}
