using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore.Core.Services.Tutorial.Data;

namespace GameCore.Core.Services.Tutorial.Steps.Data
{
    public interface ITutorialStepData<TConditionType,TActionType> 
        where TConditionType : struct 
        where TActionType : struct
    {
        IEnumerable<ITutorialConditionData<TConditionType>> Conditions { get; }
        IEnumerable<ITutorialActionData<TActionType>> Actions { get; }
    }
}
