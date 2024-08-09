using System;

namespace ProductLabeling.Models
{
    internal class Counter
    {
        private int _total;
        private int _read;
        private int _ok;
        private int _ng;

        public event Action<int>? TotalChange;
        public event Action<int>? ReadChange;
        public event Action<int>? OkChange;
        public event Action<int>? NgChange;

        public int Total
        {
            get => _total;
            set => Set(ref _total, value, TotalChange);
        }
        public int Read
        {
            get => _read;
            set => Set(ref _read, value, ReadChange);
        }
        public int Ok
        {
            get => _ok;
            set => Set(ref _ok, value, OkChange);
        }
        public int Ng
        {
            get => _ng;
            set => Set(ref _ng, value, NgChange);
        }

        public void Reset()
        {
            Total = 0;
            Read = 0;
            Ok = 0;
            Ng = 0;
        }

        private void Set<T>(ref T field, T value, Action<T>? action)
        {
            field = value;
            action?.Invoke(value);
        }
    }
}
