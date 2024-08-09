using Microsoft.Extensions.Logging;
using ProductLabeling.Commands;
using ProductLabeling.Logging;
using ProductLabeling.Models;
using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;
using ProductLabeling.RegistryEdit;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using ProductLabeling.Views.Windows;
using ProductLabeling.WindowsOS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProductLabeling.ViewModels
{
    internal class MainWindowViewModel : ViewModelAppSettings
    {
        private ILogger<MainWindowViewModel> _logger;
        private readonly IModel _model;
        private readonly ISerializerService<ObservableCollection<Product>> _productsService;
        private ObservableCollection<Product>? _products;

        #region Fields
        private bool _status;
        private bool _statusHand;
        private bool _ioModuleStatus;
        private bool _codeReaderStatus;
        private bool _handCodeReaderStatus;
        private bool _dataBaseStatus;
        private string? _error;
        private bool _isEnable;

        private int _totalStage1;
        private int _readStage1;

        private int _okStage1;
        private int _ngStage1;

        private int _totalHand;
        private int _readHand;
        private int _okHand;
        private int _ngHand;
        private int _allOK;
        private long _allDB;

        private string? _codeReaderData;
        private string? _handCodeReaderData;
        
        private string? _messageSatge1;
        private string? _messageHand;

        private double _velosity;

        private double _timeDI0;

        private double _timeCodeReader;

        private double _firstStageHandTimeDb;
        private double _firstStageAutoTimeDb;
        #endregion

        public MainWindowViewModel(ISerializerService<AppSettingsDto> appSettings, IEnumerable<IModel> models,
            ISerializerService<ObservableCollection<Product>> productsService, ILogger<MainWindowViewModel> logger) : base(appSettings)
        {
            _model = models.First();
            _productsService = productsService;

            _logger = logger;
        }

        #region Properties
        public ObservableCollection<Product> Products => _products ??= _productsService.Deserialize();
        public Product? SelectedProduct 
        {
            set
            {
                _model.FirstStage.AutoReading.SelectedProduct = value;
                _model.FirstStage.HandReading.SelectedProduct = value;
            }
            get => _model.FirstStage.AutoReading.SelectedProduct; 
        }

        public int HandCodeReaderSelectedMode 
        {
            set
            {
                _model.FirstStage.HandReading.HandCodeReaderSelectedMode = value;
                OnPropertyChanget(nameof(HandCodeReaderSelectedMode));
            }
            get => _model.FirstStage.HandReading.HandCodeReaderSelectedMode!; 
        }

        public bool Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        public bool StatusHand
        {
            get => _statusHand;
            set => Set(ref _statusHand, value);
        }
        public bool IoModuleStatus
        {
            get => _ioModuleStatus;
            set => Set(ref _ioModuleStatus, value);
        }
        public bool CodeReaderStatus
        {
            get => _codeReaderStatus; 
            set => Set(ref _codeReaderStatus, value);
        }
        
        public bool HandCodeReaderStatus
        {
            get => _handCodeReaderStatus;
            set => Set(ref _handCodeReaderStatus, value);
        }
        public bool DataBaseStatus
        {
            get => _dataBaseStatus;
            set => Set(ref _dataBaseStatus, value);
        }
        public string? Error
        {
            get => _error;
            set
            {
                Set(ref _error, value);
                _logger.Error(value);
            }
        }
        public bool IsEnable
        {
            get => _isEnable;
            set => Set(ref _isEnable, value);
        }

        public int TotalStage1 
        { 
            get => _totalStage1;
            set
            {
                Set(ref _totalStage1, value);
                AllOK = OkStage1 + OkHand - NgHand;
            }
        }
        public int ReadStage1 
        { 
            get => _readStage1; 
            set => Set(ref _readStage1, value); 
        }
        public int OkStage1
        { 
            get => _okStage1;
            set => Set(ref _okStage1, value); 
        }
        public int NgStage1
        { 
            get => _ngStage1; 
            set => Set(ref _ngStage1, value);
        }
        public int TotalHand
        { 
            get => _totalHand;
            set
            {
                Set(ref _totalHand, value);
                AllOK = OkStage1 + OkHand - NgHand;
            }
        }
        public int ReadHand 
        { 
            get => _readHand;
            set => Set(ref _readHand, value);
        }
        public int OkHand 
        {
            get => _okHand; 
            set 
            {
                Set(ref _okHand, value);
                AllOK = OkStage1 + OkHand - NgHand;
            } 
        }
        public int NgHand
        { 
            get => _ngHand; 
            set 
            {
                Set(ref _ngHand, value);
                AllOK = OkStage1 + OkHand - NgHand;
            }
        }
        
        public string? CodeReaderData
        {
            get => _codeReaderData;
            set => Set(ref _codeReaderData, value);
        }
        public string? HandCodeReaderData
        {
            get => _handCodeReaderData;
            set => Set(ref _handCodeReaderData, value);
        }

        public string? MessageStage1
        {
            get => _messageSatge1;
            set => Set(ref _messageSatge1, value);
        }
        public string? MessageHand
        {
            get => _messageHand;
            set => Set(ref _messageHand, value);
        }
        
        public double Velosity
        {
            get => Math.Round(_velosity, 2);
            set => Set(ref _velosity, value);
        }

        public int AllOK
        {
            get => _allOK;
            set => Set(ref _allOK, value);
        }

        public long AllDB
        {
            get => _allDB;
            set => Set(ref _allDB, value);
        }

        public double TimeDI0
        {
            get => Math.Round(_timeDI0, 2);
            set => Set(ref _timeDI0, value);
        }

        public double TimeCodeReader
        {
            get => Math.Round(_timeCodeReader, 2);
            set => Set(ref _timeCodeReader, value);
        }
        public double FirstStageAutoTimeDb
        {
            get => Math.Round(_firstStageAutoTimeDb, 2);
            set => Set(ref _firstStageAutoTimeDb, value);
        }
        public double FirstStageHandTimeDb
        {
            get => Math.Round(_firstStageHandTimeDb, 2);
            set => Set(ref _firstStageHandTimeDb, value);
        }
        #endregion

        #region Commands
        public ICommand OnOffSystemCommand => new RelayWithConfirmCommand(OnOffSystemAsync);
        public ICommand ExitAndStartExplorerCommand => new RelayCommand(ExitAndStartExplorerAsync);
        public ICommand RebootCommand => new RelayCommand(RebootAsync);
        public ICommand ShutdownCommand => new RelayCommand(ShutdownAsync);
        public ICommand ResetStage1Command => new RelayCommand(ResetCounts);
        public ICommand ResetHandCommand => new RelayCommand(ResetCounts);
        public ICommand AddProductCommand => new AddProductShowDialogCommand(AddProduct);
        public ICommand RemoveProductCommand => new RelayCommand(RemoveProduct);
        public ICommand SaveProductCommand => new RelayCommand(SaveProduct);
        public ICommand HandCodeReaderSwitchingModeCommand => new RelayCommand(HandCodeReaderSwitchingMode);
        public ICommand ClearErrorCommand => new RelayCommand(ClearError);
        public ICommand GetAllCountsFromDBCmd => new RelayCommand(() => AllDB = _model.FirstStage.AutoReading.GetAllCountsFromDB());
        #endregion

        private void HandCodeReaderSwitchingMode()
        {
            HandCodeReaderSelectedMode++;

            if (HandCodeReaderSelectedMode > 1)
                HandCodeReaderSelectedMode = 0;
        }

        private void AddProduct(Product product) => _products?.Add(product);
        private void RemoveProduct(object? index) => _products?.RemoveAt(Convert.ToInt32(index));
        private void SaveProduct()
        {
            try
            {
                _productsService.Serialize(_products ?? new ());
                new SavedWindow().Show();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        private async Task OnOffSystemAsync()
        {
            var loadWindow = new WaitWindow();
            loadWindow.Show();

            if (RegIDProcessor.CheckHash())
            {
                if (!IsEnable) await OnSystemAsync();
                else await OffSystemAsync();
                IsEnable = !IsEnable;
            }
            else
                Error = "Проблема активации";

            loadWindow.Close();
        }
        private async Task OnSystemAsync()
        {
            SubDevices();
            SubFirstStage();
            SubHandStage();

            await _model.OnAsync();
        }
        private async Task OffSystemAsync()
        {
            await _model.OffAsync();

            UnSubDevices();
            UnSubFirstStage();
            UnSubHandStage();
        }

        #region Devices
        private void SubDevices()
        {
            IDevices devices = _model.Devices;

            devices.IoModule.Status += GetEventHandler(IoModuleStatus);
            devices.CodeReader.Status += GetEventHandler(CodeReaderStatus);
            devices.HandCodeReader.Status += GetEventHandler(HandCodeReaderStatus);
            devices.DataBase.Status += GetEventHandler(DataBaseStatus);

            Action<string> eventHandler = GetEventHandler(Error);

            devices.IoModule.Error += eventHandler;
            devices.CodeReader.Error += eventHandler;
            devices.HandCodeReader.Error += eventHandler;
            devices.DataBase.Error += eventHandler;

            devices.Velosity += GetEventHandler(Velosity);
        }

        private void UnSubDevices()
        {
            IDevices devices = _model.Devices;

            devices.IoModule.Status -= GetEventHandler(IoModuleStatus);
            devices.CodeReader.Status -= GetEventHandler(CodeReaderStatus);
            devices.HandCodeReader.Status -= GetEventHandler(HandCodeReaderStatus);
            devices.DataBase.Status -= GetEventHandler(DataBaseStatus);

            Action<string> eventHandler = GetEventHandler(Error);

            devices.IoModule.Error -= eventHandler;
            devices.CodeReader.Error -= eventHandler;
            devices.HandCodeReader.Error -= eventHandler;
            devices.DataBase.Error -= eventHandler;

            devices.Velosity -= GetEventHandler(Velosity);
        }
        #endregion

        #region FirstAuto
        private void SubFirstStage()
        {
            IFirstStageAutoReading auto = _model.FirstStage.AutoReading;

            auto.Status += GetEventHandler(Status);
            auto.CodeReaderData += GetEventHandler(CodeReaderData);
            auto.Message += GetEventHandler(MessageStage1);
            auto.TimeDI += GetEventHandler(TimeDI0);
            auto.TimeCodeReader += GetEventHandler(TimeCodeReader);
            auto.TimeDb += GetEventHandler(FirstStageAutoTimeDb);

            Counter cnt = auto.Counter;

            cnt.TotalChange += GetEventHandler(TotalStage1);
            cnt.ReadChange += GetEventHandler(ReadStage1);
            cnt.OkChange += GetEventHandler(OkStage1);
            cnt.NgChange += GetEventHandler(NgStage1);
        }

        private void UnSubFirstStage()
        {
            IFirstStageAutoReading auto = _model.FirstStage.AutoReading;

            auto.Status -= GetEventHandler(Status);
            auto.CodeReaderData -= GetEventHandler(CodeReaderData);
            auto.Message -= GetEventHandler(MessageStage1);
            auto.TimeDI -= GetEventHandler(TimeDI0);
            auto.TimeCodeReader -= GetEventHandler(TimeCodeReader);
            auto.TimeDb -= GetEventHandler(FirstStageAutoTimeDb);

            Counter cnt = auto.Counter;

            cnt.TotalChange -= GetEventHandler(TotalStage1);
            cnt.ReadChange -= GetEventHandler(ReadStage1);
            cnt.OkChange -= GetEventHandler(OkStage1);
            cnt.NgChange -= GetEventHandler(NgStage1);
        }
        #endregion

        #region FirstHand
        private void SubHandStage()
        {
            IFirstStageHandReading hand = _model.FirstStage.HandReading;

            hand.Status += GetEventHandler(StatusHand);
            hand.CodeReaderData += GetEventHandler(HandCodeReaderData);
            hand.Message += GetEventHandler(MessageHand);
            hand.TimeDb += GetEventHandler(FirstStageHandTimeDb);

            Counter cnt = hand.Counter;

            cnt.TotalChange += GetEventHandler(TotalHand);
            cnt.ReadChange += GetEventHandler(ReadHand);
            cnt.OkChange += GetEventHandler(OkHand);
            cnt.NgChange += GetEventHandler(NgHand);
        }

        private void UnSubHandStage()
        {
            IFirstStageHandReading hand = _model.FirstStage.HandReading;

            hand.Status -= GetEventHandler(StatusHand);
            hand.CodeReaderData -= GetEventHandler(HandCodeReaderData);
            hand.Message -= GetEventHandler(MessageHand);
            hand.TimeDb -= GetEventHandler(FirstStageHandTimeDb);

            Counter cnt = hand.Counter;

            cnt.TotalChange -= GetEventHandler(TotalHand);
            cnt.ReadChange -= GetEventHandler(ReadHand);
            cnt.OkChange -= GetEventHandler(OkHand);
            cnt.NgChange -= GetEventHandler(NgHand);
        }
        #endregion

        private async Task ExitAndStartExplorerAsync(object? window) => await CloseWindowAsync(window, Explorer.StartProcess);

        private async Task RebootAsync(object? window) => await CloseWindowAsync(window, Reboot.StartProcess);

        private async Task ShutdownAsync(object? window) => await CloseWindowAsync(window, Shutdown.StartProcess);

        private async Task CloseWindowAsync(object? window, Action startProcessCallBack)
        {
            if (!new PasswordWindow().ShowDialog() ?? true) return;

            var mainWindow = window as Window;

            if (IsEnable) await OnOffSystemAsync();

            mainWindow?.Close();

            startProcessCallBack();
        }

        private void ResetCounts()
        {
           _logger.Info(String.Format("Pressed button Reset: Total={0} Read={1} OK={2} Brak={3} HAdd={4} HDel={5}",
                _model.FirstStage.AutoReading.Counter.Total.ToString(),
                _model.FirstStage.AutoReading.Counter.Read.ToString(),
                _model.FirstStage.AutoReading.Counter.Ok.ToString(),
                _model.FirstStage.AutoReading.Counter.Ng.ToString(),
                _model.FirstStage.HandReading.Counter.Ok.ToString(),
                _model.FirstStage.HandReading.Counter.Ng.ToString()));
            _model.FirstStage.AutoReading.Counter.Reset();
            _model.FirstStage.HandReading.Counter.Reset();
            CodeReaderData = string.Empty;
            MessageStage1 = string.Empty;
            HandCodeReaderData = string.Empty;
            MessageHand = string.Empty;
            AllOK = 0;
            AllDB = 0;
            
        }

        private void ClearError()
        {
            Error = string.Empty;
        }
    }
}
