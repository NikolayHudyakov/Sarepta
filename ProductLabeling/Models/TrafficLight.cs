using System.Threading.Tasks;

namespace ProductLabeling.Models
{
    internal class TrafficLight
    {
        public delegate void ExecuteCallBack(bool value);

        public ExecuteCallBack? Green { get; set; }

        public ExecuteCallBack? Red { get; set; }

        public ExecuteCallBack? Yellow { get; set; }

        public ExecuteCallBack? Buzzer { get; set; }

        public void LightGreen(int duration) => ExecutePulseAsync(Green, duration);

        public void LightRed(int duration) => ExecutePulseAsync(Red, duration);

        public void LightYellow(int duration) => ExecutePulseAsync(Yellow, duration);

        public void OnBuzzer(int duration) => ExecutePulseAsync(Buzzer, duration);

        public void LightGreen(bool value) => ExecuteAsync(Green, value);

        public void LightRed(bool value) => ExecuteAsync(Red, value);

        public void LightYellow(bool value) => ExecuteAsync(Yellow, value);

        public void OnBuzzer(bool value) => ExecuteAsync(Buzzer, value);

        private async void ExecutePulseAsync(ExecuteCallBack? execute, int duration)
        {
            if (execute is null) return;

            execute(true);
            await Task.Delay(duration);
            execute(false);
        }

        private async void ExecuteAsync(ExecuteCallBack? execute, bool value)
        {
            await Task.Run(() =>
            {
                if (execute is null) return;
                execute(value);
            });
        }
    }
}
