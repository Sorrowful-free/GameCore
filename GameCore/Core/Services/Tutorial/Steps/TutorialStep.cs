using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.Extentions;
using GameCore.Core.Services.Tutorial.Actions;
using GameCore.Core.Services.Tutorial.Conditions;
using GameCore.Core.Services.Tutorial.Steps.Data;
using GameCore.Core.UnityThreading;
using UnityEngine;

namespace GameCore.Core.Services.Tutorial.Steps
{
    public class TutorialStep
    {
        public ConditionWaitingType WaitingType { get; }
        public IEnumerable<ITutorialCondition> Conditions { get; }
        public IEnumerable<ITutorialAction> Actions { get; }

        public bool IsSkiped { get; private set; }

        public TutorialStep(IEnumerable<ITutorialCondition> conditions, IEnumerable<ITutorialAction> actions, ConditionWaitingType waitingType)
        {
            Conditions = conditions;
            Actions = actions;
            WaitingType = waitingType;
            IsSkiped = false;
        }

        public async Task ProcessStep()
        {
            await new AwaitableOperation((c) => AsyncProcessStep(c).StartAsCoroutine());
        }

        private IEnumerator AsyncProcessStep(Action callback)
        {
            yield return new WaitWhile(() => !Actions.All(e => e.Begin().IsCompleted));
            yield return new WaitWhile(() => (WaitingType == ConditionWaitingType.WaitAll 
            ? !Conditions.All(e => e.Wait().IsCompleted)
            : !Conditions.Any(e => e.Wait().IsCompleted)));
            yield return new WaitWhile(() => !Actions.All(e => e.End().IsCompleted));
            callback.SafeInvoke();
        }

        public void SkipStep()
        {
            IsSkiped = true;
            foreach (var condition in Conditions)
            {
                condition.Cancel();
            }
        }
    }
}
