using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore.Core.Services.UI.Utils
{
    public static class UIUtils
    {
        public static Vector2 GetPhysicalScreenSize()
        {
            return new Vector2(Screen.width,Screen.height)/Screen.dpi;
        }

        public static float GetPhysicalScreenDiagonal()
        {
            return GetPhysicalScreenSize().magnitude;
        }
    }
}
