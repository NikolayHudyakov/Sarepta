using ProductLabeling.Commands;
using ProductLabeling.Models;
using System.Windows;
using System.Windows.Input;

namespace ProductLabeling.ViewModels
{
    internal class AddProductWindowViewModel : ViewModel
    {
        private const int GtinLenght = 14;
        private const int GtinLenght_ = 13;

        #region Field
        private string _gtin = string.Empty;
        private string _error = string.Empty;
        #endregion

        #region Properties
        public string Name { get; set; } = string.Empty;
        public string Gtin
        {
            get => _gtin;
            set
            {
                if (value.Length == GtinLenght_)
                {
                    _gtin = $"0{value}";
                    return;
                }
                _gtin = value;
            }
        }
        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }
        #endregion

        #region Commands
        public ICommand AddCommand => new RelayCommand(Add);
        #endregion

        public Product? NewProduct { get; set; }

        private void Add(object? parametr)
        {
            var addProductWindow = parametr as Window;

            if (Name != string.Empty && _gtin.Length == GtinLenght)
            {
                NewProduct = new Product { Name = Name, Gtin = _gtin };
                addProductWindow!.DialogResult = true;
                return;
            }

            Error = $"Пустое название или длина gtin не равна {GtinLenght} или {GtinLenght_}";
        }

    }
}
