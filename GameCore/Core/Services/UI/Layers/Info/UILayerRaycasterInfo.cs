using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Core.Services.UI.Layers.Info
{
    public struct UILayerRaycasterInfo
    {
        public UILayerRaycasterType RaycasterType;
        public bool IgnoreReversedGraphic;
        public GraphicRaycaster.BlockingObjects BlockingObjects;
        public LayerMask BlockingMask;
        public LayerMask EventMask;
    }
}