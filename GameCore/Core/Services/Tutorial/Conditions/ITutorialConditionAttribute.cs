using GameCore.Core.Base.Factory;

namespace GameCore.Core.Services.Tutorial.Conditions
{
    public interface ITutorialConditionAttribute<TConditionType> :IFactoryElementAttribute<TConditionType>
        where TConditionType :struct
    {
    }
}
