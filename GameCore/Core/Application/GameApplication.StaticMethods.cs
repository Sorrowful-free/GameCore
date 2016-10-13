using UnityEngine;

namespace GameCore.Core.Application
{
    public partial class GameApplication
    {
        public static bool IsPause { get; private set; }

        public static void Pause(bool isPause)
        {
            IsPause = isPause;
            Time.timeScale = isPause ? 0 : 1;
            PauseServices(isPause);
        }
    }
}
