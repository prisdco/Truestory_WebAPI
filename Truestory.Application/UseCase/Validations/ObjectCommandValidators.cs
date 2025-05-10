using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Application.UseCase.Commands;
using Truestory.Domain.Models;

namespace Truestory.Application.UseCase.Validations
{

    public class ObjectCommandValidators : AbstractValidator<AddDevice>
    {

        public ObjectCommandValidators()
        {
            //Validation for name
            this.RuleFor(x => x.name).NotNull().NotEmpty().WithMessage("Please specify a name");

            //Validation for data
            this.RuleFor(x => x.data).NotNull().NotEmpty().WithMessage("Please specify property data");

        }
    }
}
