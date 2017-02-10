using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Core.Services.Tutorial.Actions;
using GameCore.Core.Services.Tutorial.Conditions;
using GameCore.Core.Services.Tutorial.Steps.Data;

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
            await Task.WhenAll(Actions.Select(e => e.Begin()));
            if (WaitingType == ConditionWaitingType.WaitAll)
                await Task.WhenAll(Conditions.Select(e => e.Wait()));
            else
                await Task.WhenAny(Conditions.Select(e => e.Wait()));
            await Task.WhenAll(Actions.Select(e => e.End()));
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
