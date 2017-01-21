using GameCore.Core.Base.Factory;

namespace GameCore.Core.Services.Tutorial.Actions
{
    public interface ITutorialActionAttribute<TActionType> : IFactoryElementAttribute<TActionType>
        where TActionType : struct 
    {
    }
}
