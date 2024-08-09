using ProductLabeling.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductLabeling.Services
{
    internal class RejecterService : IRejecterService
    {
        private const int MaxQuantityProduct = 1000;
        private const int VelocityCalcPeriod = 1;

        private readonly object _lockObject = new();

        private readonly List<Item> _list;

        public RejecterService() => _list = new List<Item>() { Capacity = MaxQuantityProduct };

        public double Velocity { get; set; }

        public async Task RejectByDelayAsync(long id, int delay, bool isEnable, Action rejectCallBack)
        {
            lock (_lockObject) _list.Add(new Item() { Id = id, Flag = true, IsEnable = isEnable });

            await Task.Delay(delay);

            lock (_lockObject)
            {
                Item? item = _list.Find(item => item.Id == id);

                if (item!.Flag && item.IsEnable) Task.Run(rejectCallBack);

                _list.Remove(item);
            }
        }

        public async Task RejectByDistanceAsync(long id, double distance, bool isEnable, Action rejectCallBack)
        {
            lock (_lockObject) _list.Add(new Item() { Id = id, Flag = true, IsEnable = isEnable });

            await Task.Run(async () =>
            {
                DateTime previosTime = DateTime.Now;
                double previosVelocity = Velocity;

                while (true)
                {
                    var time = DateTime.Now;
                    double velocity = Velocity;

                    var deltaS = 0.5 * (previosVelocity + velocity) * (time - previosTime).TotalSeconds;

                    previosVelocity = velocity;
                    previosTime = time;

                    distance -= deltaS;
                    if (distance < 0) break;

                    await Task.Delay(VelocityCalcPeriod);
                }
            });

            lock (_lockObject)
            {
                Item? item = _list.Find(item => item.Id == id);

                if (item!.Flag && item.IsEnable) Task.Run(rejectCallBack);

                _list.Remove(item);
            }
        }

        public async Task UpdateAsync(long id)
        {
            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    Item? item = _list.Find(item => item.Id == id);

                    if (item != null) item.Flag = false;
                }
            });
        }

        private class Item
        {
            public long Id { get; set; }
            public bool Flag { get; set; }
            public bool IsEnable { get; set; }
        }
    }
}
