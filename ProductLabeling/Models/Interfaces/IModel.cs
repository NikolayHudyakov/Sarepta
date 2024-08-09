using System.Threading.Tasks;
using ProductLabeling.Models.Interfaces.FirstStage;

namespace ProductLabeling.Models.Interfaces
{
    internal interface IModel
    {
        public string Name { get; }

        public IFirstStage FirstStage { get; }

        public IDevices Devices { get; }

        public Task OnAsync();

        public Task OffAsync();
    }
}
