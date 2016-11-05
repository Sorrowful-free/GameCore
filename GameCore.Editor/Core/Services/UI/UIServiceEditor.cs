using System;
using System.Collections.Generic;
using GameCore.Core.Services.UI;
using UnityEditor;

namespace GameCore.Editor.Core.Services.UI
{
    [CustomEditor(typeof(BaseUIService<>),true)]
    public class UIServiceEditor : UnityEditor.Editor
    {
        private List<UILayerInfo> TargetList { get}
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }
    }
}
