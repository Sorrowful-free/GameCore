using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore.Core.Base;
using GameCore.Core.Extentions;

namespace GameCore.Core.Application
{
    public partial class GameApplication : BaseMonoBehaviour
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            OnAppEnable.SafeInvoke();
        }

        protected override void Awake()
        {
            base.Awake();
            OnAppAwake.SafeInvoke();
        }

        protected override void Start()
        {
            base.Start();
            OnAppStart.SafeInvoke();
        }

        protected override void Update()
        {
            base.Update();
            OnAppUpdate.SafeInvoke();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            OnAppFixedUpdate.SafeInvoke();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            OnAppLateUpdate.SafeInvoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnAppDisable.SafeInvoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnAppDestroy.SafeInvoke();
            OnAppEnable.ClearAllHandlers();
            OnAppAwake.ClearAllHandlers();
            OnAppStart.ClearAllHandlers();
            OnAppUpdate.ClearAllHandlers();
            OnAppFixedUpdate.ClearAllHandlers();
            OnAppLateUpdate.ClearAllHandlers();
            OnAppDisable.ClearAllHandlers();
            OnAppDestroy.ClearAllHandlers();
        }
    }
}
