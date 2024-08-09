using ProductLabeling.Models;
using ProductLabeling.ViewModels;
using ProductLabeling.Views.Windows;
using System;

namespace ProductLabeling.Commands
{
    class AddProductShowDialogCommand : Command
    {
        private readonly GetProductCallBack _getProductCallBack;
        private readonly Predicate<object?>? _canExecute;

        public delegate void GetProductCallBack(Product product);

        public AddProductShowDialogCommand(GetProductCallBack getProductCallBack, Predicate<object?>? canExecute = null)
        {
            _canExecute = canExecute;
            _getProductCallBack = getProductCallBack;
        }

        public override void Execute(object? parameter)
        {
            var addProductWindow = new AddProductWindow();

            var addProductWindowViewModel = (AddProductWindowViewModel)addProductWindow.DataContext;

            addProductWindow.ShowDialog();

            if (addProductWindow.DialogResult ?? false)
                _getProductCallBack(addProductWindowViewModel.NewProduct!);
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    }
}
