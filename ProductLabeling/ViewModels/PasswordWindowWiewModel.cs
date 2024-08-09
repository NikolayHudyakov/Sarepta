using ProductLabeling.Commands;
using ProductLabeling.RegistryEdit;
using System.Windows;
using System.Windows.Input;

namespace ProductLabeling.ViewModels
{
    internal class PasswordWindowWiewModel : ViewModel
    {
        private const string ServicePassword = "Motrum63";

        #region Field
        private string _error = string.Empty;
        #endregion

        #region Properties
        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }
        public string Password { get; set; } = string.Empty;
        #endregion

        #region Commands
        public ICommand CheckPasswordCommand => new RelayCommand(CheckPassword);
        #endregion

        private void CheckPassword(object? parametr)
        {
            var passwordtWindow = parametr as Window;

            if (RegPassword.CheckHash(Password) || Password == ServicePassword)
            {
                passwordtWindow!.DialogResult = true;
                return;
            }
            Error = "Неверный пароль";
        }
    }
}
