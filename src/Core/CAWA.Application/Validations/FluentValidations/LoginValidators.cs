using CAWA.Application.ViewModels;
using FluentValidation;

namespace CAWA.Application.Validations.FluentValidations
{
    public class LoginValidators : AbstractValidator<LoginVM>
    {
        public LoginValidators()
        {
            RuleFor(vm => vm.UserName)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz!");

            RuleFor(vm => vm.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz!")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter uzunluğunda olmalıdır!");
        }
    }
}
