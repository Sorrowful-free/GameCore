namespace GameCore.Core.Base.Factory
{
    public interface IFactoryElementAttribute<TType> where TType : struct 
    {
        TType Type { get; }
    }
}