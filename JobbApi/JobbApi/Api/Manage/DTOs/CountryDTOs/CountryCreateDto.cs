using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Manage.DTOs
{
    public class CountryCreateDto
    {
        public string Name { get; set; }
    }
    public class CountryCreateDtoValidator: AbstractValidator<CountryCreateDto>
    {
        public CountryCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Lenght can not be 50!")
                 .NotEmpty().NotNull().WithMessage("Can not be empty");
        }
    }
}
