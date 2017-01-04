using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using GameCore.Core.Services.UI;
using GameCore.Core.Services.UI.Layers;
using UnityEditor;
using UnityEngine;

namespace GameCore.Editor.Core.Services.UI
{
    [CustomEditor(typeof(BaseUIService),true)]
    public class BaseUIServiceEditor : UnityEditor.Editor
    {
        private BaseUIService Target
        {
            get
            {
                return (BaseUIService)target;
            }
        }

        private Type LayerEnumType {
            get { return Target == null?null: Target.GetType().BaseType.GetGenericArguments()[0]; }
        }
        private int[] LayersValues
        {
            get { return Enum.GetValues(LayerEnumType).Cast<int>().ToArray(); }
        }

        private string[] LayersNames
        {
            get { return Enum.GetNames(LayerEnumType); }
        }

        private IList TargetList
        {
            get
            {
                return Target?.LayerInfos;
            }
        }

        private void UpdateList()
        {
            if (TargetList == null)
                return;
            if (TargetList.Count != LayersNames.Length)
            {
                TargetList.Clear();
                foreach (var layersValue in LayersValues)
                {
                    TargetList.Add(new UILayerInfo {LayerType = layersValue});
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UpdateList();
            if (TargetList == null)
                return;
            foreach (UILayerInfo layerInfo in TargetList)
            {
                GUI.Box(EditorGUILayout.BeginVertical(),"");
                GUI.enabled = false;
                layerInfo.LayerType =
                    LayersValues[EditorGUILayout.Popup("Layer Type",Array.IndexOf(LayersValues, layerInfo.LayerType), LayersNames)];
                GUI.enabled = true;
                layerInfo.Layer = (UILayer)EditorGUILayout.ObjectField(new GUIContent("Layer"), layerInfo.Layer,typeof(UILayer));
                EditorGUILayout.EndVertical();
                
            }

        }
    }
}
