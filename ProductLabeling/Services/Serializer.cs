using Newtonsoft.Json;
using ProductLabeling.Services.Intefaces;
using System;
using System.IO;

namespace ProductLabeling.Services
{
    internal abstract class Serializer<T> : ISerializerService<T>
    {
        private T? _obj;
        protected virtual string FilePath => throw new NotImplementedException();

        public T Deserialize() => _obj ??= GetObj();

        private T GetObj()
        {
            string json = string.Empty;

            if (File.Exists(FilePath))
                json = File.ReadAllText(FilePath);

            return JsonConvert.DeserializeObject<T>(json) ?? (T)Activator.CreateInstance(typeof(T))!;
        }

        public void Serialize(T obj) => File.WriteAllText(FilePath, JsonConvert.SerializeObject(obj, Formatting.Indented));
    }
}
