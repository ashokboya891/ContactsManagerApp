using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        public static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> results = new List<ValidationResult>();
            bool isvalid = Validator.TryValidateObject(obj, validationContext, results, true);
            if (!isvalid)
            {
                throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);

            }
        }
    }
}
