namespace ProductLabeling.Models.Interfaces
{
    internal interface IModelName
    {
        public ModelName Model { get; }
    }

    public enum ModelName
    {
        HonestSign,
        RbExcises
    }
}
