using System.ComponentModel.DataAnnotations;

namespace WebSite.ViewModel
{
    public class Login
    {
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A confirmação da senha não confere com a senha digitada")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Lembre-me")]
        public bool IsPersistent { get; set; }
    }
}
