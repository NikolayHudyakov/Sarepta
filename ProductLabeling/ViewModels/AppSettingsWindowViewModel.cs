using ProductLabeling.Commands;
using ProductLabeling.RegistryEdit;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using ProductLabeling.Views.Windows;
using System;
using System.IO.Ports;
using System.Windows.Input;

namespace ProductLabeling.ViewModels
{
    internal class AppSettingsWindowViewModel : ViewModelAppSettings
    {
        #region Field
        private string _error = string.Empty;
        #endregion

        public AppSettingsWindowViewModel(ISerializerService<AppSettingsDto> appSettings) : base(appSettings) { }

        #region Properties
        public AppSettingsDto Dto => dto;
        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }
        public string[] PortNames => SerialPort.GetPortNames();
        public int[] BaudRates => new int[] { 9600, 14400, 19200, 38400, 56000, 57600, 115200 };
        public string Password { get; set; } = string.Empty;

        #endregion

        #region Commands
        public ICommand SaveAppSetingsCommand => new RelayCommand(SaveAppSettings);
        public ICommand ChangePasswordCommand => new RelayWithConfirmCommand(password => RegPassword.CreateHash(password as string ?? string.Empty));
        #endregion

        private void SaveAppSettings()
        {
            try
            {
                SetAppSettings();
                new SavedWindow().Show();
            }
            catch(Exception ex)
            {
                Error = ex.Message;
            }
        }

    }
    
}
