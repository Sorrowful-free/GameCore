using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCore.Core.Services.Tutorial.Data
{
    public interface ITutorialActionData<TActionType> where TActionType : struct 
    {
        TActionType ActionType { get; }
    }
}
