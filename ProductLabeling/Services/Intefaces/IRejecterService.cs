using System;
using System.Threading.Tasks;

namespace ProductLabeling.Services.Intefaces
{
    internal interface IRejecterService
    {
        public double Velocity { get; set; }
        public Task RejectByDelayAsync(long id, int delay, bool isEnable, Action rejectCallBack);
        public Task RejectByDistanceAsync(long id, double distance, bool isEnable, Action rejectCallBack);
        public Task UpdateAsync(long id);
    }
}
