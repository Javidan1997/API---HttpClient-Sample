using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.DTOs
{
    public class CompanyCreateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Desc { get; set; }
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }

    public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50)
                .WithMessage("Lenght can not be 50!")
                 .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Email).MaximumLength(100)
               .WithMessage("Lenght can not be 100!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Phone).MaximumLength(50)
               .WithMessage("Lenght can not be 50!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Category).MaximumLength(100)
               .WithMessage("Lenght can not be 100!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Address).MaximumLength(100)
               .WithMessage("Lenght can not be 100!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Desc).MaximumLength(1500)
               .WithMessage("Lenght can not be 1500!");
            RuleFor(x => x.Photo).MaximumLength(100)
              .WithMessage("Lenght can not be 100!");
        }
    }
}
