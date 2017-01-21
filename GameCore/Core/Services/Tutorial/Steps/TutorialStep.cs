using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore.Core.Services.Tutorial.Actions;
using GameCore.Core.Services.Tutorial.Conditions;

namespace GameCore.Core.Services.Tutorial.Steps
{
    public class TutorialStep
    {
        public IEnumerable<ITutorialCondition> Conditions { get; }
        public IEnumerable<ITutorialAction> Actions { get; }
        public TutorialStep(IEnumerable<ITutorialCondition> conditions, IEnumerable<ITutorialAction> actions)
        {
            Conditions = conditions;
            Actions = actions;
        }
    }
}
