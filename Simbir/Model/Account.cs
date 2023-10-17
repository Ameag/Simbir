using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Simbir.Model
{
    public class Account
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public bool is_admin { get; set; }
        public int balance { get; set; }
    }

    public class AccountOutput
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Логин должен быть от 5 до 20 символов")]
        public string login { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Пароль должен быть от 2 до 20 символов")]
        public string password { get; set; }
    }


    public class AccountInput
    {
        public string login { get; set; }
        public string password { get; set; }
    }
}
