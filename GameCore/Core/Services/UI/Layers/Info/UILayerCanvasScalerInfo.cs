using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Core.Services.UI.Layers.Info
{
    public struct UILayerCanvasScalerInfo
    {
        public CanvasScaler.ScaleMode ScaleMode;
        public float ScaleFactor;
        public Vector2 ReferenceResolution;
        public CanvasScaler.ScreenMatchMode ScreenMatchMode;
        public float Match;
        public CanvasScaler.Unit Unit;
        public float FallbackScreenDPI;
        public float DefaultSpriteDPI;
        public float ReferencePixelPerUnit;
    }
}