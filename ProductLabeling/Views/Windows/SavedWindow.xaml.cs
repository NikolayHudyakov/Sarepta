using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProductLabeling.Views.Windows
{
    public partial class SavedWindow : Window
    {
        private const int WindowLifiTime = 1000;

        private SynchronizationContext? _synchronizationContext = SynchronizationContext.Current;

        public SavedWindow() => InitializeComponent();

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(WindowLifiTime);
                _synchronizationContext?.Post(_ => Close(), null);
            });
        }
    }
}