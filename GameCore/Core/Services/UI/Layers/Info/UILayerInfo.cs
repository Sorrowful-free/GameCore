using UnityEngine;

namespace GameCore.Core.Services.UI.Layers.Info
{
    public struct UILayerInfo
    {
        public RenderMode RenderMode;
        public bool PixelPerfect;
        public int SortOrder;
        public string SortLayer;
        public int TargetDisplay;
        public UILayerCameraInfo CameraInfo;
        public UILayerCanvasScalerInfo CanvasScalerInfo;
        public UILayerRaycasterInfo RaycasterInfo;
    }
}