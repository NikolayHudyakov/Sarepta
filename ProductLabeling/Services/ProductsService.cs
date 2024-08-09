using ProductLabeling.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace ProductLabeling.Services
{
    internal class ProductsService : Serializer<ObservableCollection<Product>>
    {
        private const string FileName = "products.json";
        protected override string FilePath => Path.Combine(AppContext.BaseDirectory, FileName);
    }
}
