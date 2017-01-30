using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore.Core.Services.Tutorial.Data;
using JetBrains.Annotations;

namespace GameCore.Core.Services.Tutorial.Steps.Data
{
    public enum ConditionWaitingType
    {
        WaitAll,
        WaitAny
    }

    public interface ITutorialStepData<TConditionType,TActionType> 
        where TConditionType : struct 
        where TActionType : struct
    {
        ConditionWaitingType WaitingType { get; }
        IEnumerable<ITutorialConditionData<TConditionType>> Conditions { get; }
        IEnumerable<ITutorialActionData<TActionType>> Actions { get; }
    }
}
