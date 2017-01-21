using System.Collections.Generic;
using GameCore.Core.Services.Tutorial.Steps.Data;

namespace GameCore.Core.Services.Tutorial.Data
{
    public interface ITutorialData<TConditionType, TActionType>
        where TConditionType : struct
        where TActionType : struct
    {
        IEnumerable<ITutorialStepData<TConditionType, TActionType>> Steps { get; }
    }
}
