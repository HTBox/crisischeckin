using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectTests.Infrastructure
{
    public class ModelValidation
    {
        //Credit goes to: http://www.jondavis.net/techblog/post/2010/12/01/Testing-Basic-ASPNET-MVC-View-Model-Validation-With-Brevity.aspx
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
