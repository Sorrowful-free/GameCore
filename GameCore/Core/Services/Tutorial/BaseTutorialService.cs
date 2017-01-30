using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;
using GameCore.Core.Base.Async;
using GameCore.Core.Base.Factory;
using GameCore.Core.Extentions;
using GameCore.Core.Services.Tutorial.Actions;
using GameCore.Core.Services.Tutorial.Conditions;
using GameCore.Core.Services.Tutorial.Data;
using GameCore.Core.Services.Tutorial.Steps;
using GameCore.Core.Services.Tutorial.Steps.Data;
using GameCore.Core.UnityThreading;

namespace GameCore.Core.Services.Tutorial
{
    public class BaseTutorialService<TConditionType, TActionType> : IService
        where TConditionType : struct
        where TActionType : struct
    {
        private BaseFactory<TConditionType,ITutorialCondition> _conditionsFactory = new BaseFactory<TConditionType, ITutorialCondition>((type,args)=>(ITutorialCondition) Activator.CreateInstance(type,args));
        private BaseFactory<TActionType, ITutorialAction> _actionFactory = new BaseFactory<TActionType, ITutorialAction>((type, args) => (ITutorialAction)Activator.CreateInstance(type, args));
        private Queue<TutorialStep> _tutorialSteps;
        public async Task Initialize()
        {
        }

        public async Task StartTutorial(ITutorialData<TConditionType, TActionType> tutorialData)
        {
            SkipTutorial();
            await UnityTask.ThreadPoolFactory.StartNew(() =>
            {
                foreach (var stepData in tutorialData.Steps)
                {
                    var conditions =
                        stepData.Conditions.Select(e => _conditionsFactory.CreateInstance(e.ConditionType, e));
                    var actions = stepData.Actions.Select(e => _actionFactory.CreateInstance(e.ActionType, e));
                    _tutorialSteps.Enqueue(new TutorialStep(conditions, actions, stepData.WaitingType));
                }
            });
            NextStep();
        }

        public void SkipStep()
        {
            _tutorialSteps.Peek().SkipStep();
        }

        public void SkipTutorial()
        {
            SkipStep();
            _tutorialSteps.Clear();
        }

        private async Task NextStep()
        {
            await _tutorialSteps.Dequeue().ProcessStep();
            if(_tutorialSteps.Count > 0)
                await NextStep();
        }

        public async Task Deinitialize()
        {
            SkipTutorial();
        }
    }
}
