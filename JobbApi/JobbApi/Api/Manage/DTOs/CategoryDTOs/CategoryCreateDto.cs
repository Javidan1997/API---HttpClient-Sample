using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Manage.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Lenght can not be 50!")
                 .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Icon).MaximumLength(50).WithMessage("Lenght can not be 50!");
        }
    }
}
