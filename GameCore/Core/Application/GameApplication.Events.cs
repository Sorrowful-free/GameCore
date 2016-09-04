using System;

namespace GameCore.Core.Application
{
    public partial class GameApplication
    {
        public static event Action OnAppEnable;
        public static event Action OnAppAwake;
        public static event Action OnAppStart;
        public static event Action OnAppUpdate;
        public static event Action OnAppFixedUpdate;
        public static event Action OnAppLateUpdate;
        public static event Action OnAppDisable;
        public static event Action OnAppDestroy;
    }
}
