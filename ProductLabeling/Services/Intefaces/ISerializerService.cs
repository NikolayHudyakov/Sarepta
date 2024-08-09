namespace ProductLabeling.Services.Intefaces
{
    internal interface ISerializerService<T>
    {
        public void Serialize(T obj);
        public T Deserialize();
    }
}
