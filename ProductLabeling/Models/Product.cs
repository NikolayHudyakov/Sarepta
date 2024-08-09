namespace ProductLabeling.Models
{
    internal class Product
    {
        private const int GtinLenght = 14;
        private const int GtinLenght_ = 13;
        private const string DefaultName = "Нет названия";
        private const string DefaultGtin = "00000000000000";

        private string _name = DefaultName;
        private string _gtin = DefaultGtin;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                _name = value;
            }
        }
        public string Gtin
        {
            get => _gtin;
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                if (value.Length == GtinLenght)
                    _gtin = value;

                if (value.Length == GtinLenght_)
                    _gtin = $"0{value}";
            }
        }
    }
}
