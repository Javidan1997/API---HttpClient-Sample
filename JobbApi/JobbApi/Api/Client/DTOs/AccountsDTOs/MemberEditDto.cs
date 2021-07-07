using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.DTOs
{
    public class MemberEditDto
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string Desc { get; set; }
        public string Photo { get; set; }
        public IFormFile File { get; set; }

    }
    public class MemberEditDtoValidator : AbstractValidator<MemberEditDto>
    {
        public MemberEditDtoValidator()
        {
            RuleFor(x => x.UserName).MaximumLength(100)
                .WithMessage("Lenght can not be 100!")
                 .NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Email).MaximumLength(100)
                .WithMessage("Lenght can not be 100!")
                 .NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.PhoneNumber).MaximumLength(50)
                 .WithMessage("Lenght can not be 50!");
            RuleFor(x => x.Occupation).MaximumLength(50)
               .WithMessage("Lenght can not be 50!")
                 .NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Address).MaximumLength(100)
                .WithMessage("Lenght can not be 100!")
                 .NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Desc).MaximumLength(1500)
               .WithMessage("Lenght can not be 1500!");
            RuleFor(x => x.Photo).MaximumLength(100)
              .WithMessage("Lenght can not be 100!");
        }
    }
}
