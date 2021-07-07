using FluentValidation;
using JobbApi.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.DTOs
{
    public class JobCreateDto
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public decimal Experience { get; set; }
        public string Desc { get; set; }
        public bool IsActive { get; set; }
        public DateTime Deadline { get; set; }
        public Gender Gender { get; set; }
        public JobType JobType { get; set; }
        public Qualification Qualification { get; set; }

        public int CategoryId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int CompanyId { get; set; }
    }

    public class JobCreateDtoValidator : AbstractValidator<JobCreateDto>
    {
        public JobCreateDtoValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100)
                .WithMessage("Lenght can not be 100!")
                 .NotEmpty().NotNull().WithMessage("Can not be empty");

            RuleFor(x => x.Address).MaximumLength(100)
               .WithMessage("Lenght can not be 100!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");

            RuleFor(x => x.Desc).MaximumLength(2000)
               .WithMessage("Lenght can not be 2000!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");

            RuleFor(x => x.Salary).GreaterThan(0).WithMessage("Can not be less and zero!");
            RuleFor(x => x.Experience).GreaterThan(0).WithMessage("Can not be less and zero!");

            RuleFor(x => x.CategoryId).NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.CityId).NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.CountryId).NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.CompanyId).NotNull().WithMessage("Can not be empty");

            RuleFor(x => x.Gender).NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.JobType).NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Qualification).NotNull().WithMessage("Can not be empty");

        }
    }
}
