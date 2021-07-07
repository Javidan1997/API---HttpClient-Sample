using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.DTOs
{
    public class MemberUpdatePasswordDto
    {

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
    public class MemberUpdatePasswordDtoValidator : AbstractValidator<MemberUpdatePasswordDto>
    {
        public MemberUpdatePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword).MaximumLength(20).NotNull().NotEmpty();
            RuleFor(x => x.CurrentPassword).NotEqual(x => x.NewPassword).WithMessage("New Password and Current password can not be same!");
            RuleFor(x => x.NewPassword).MaximumLength(20).NotNull().NotEmpty();
            RuleFor(x => x.NewPassword).Equal(x => x.ConfirmNewPassword).WithMessage("Password and Confirm password are not same!");
        }
    }
}
