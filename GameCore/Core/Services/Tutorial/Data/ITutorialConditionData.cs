namespace GameCore.Core.Services.Tutorial.Data
{
    public interface ITutorialConditionData<TConditionType> where TConditionType : struct 
    {
        TConditionType ConditionType { get; }
    }
}
