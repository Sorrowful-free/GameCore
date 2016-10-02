using UnityEngine;

namespace GameCore.Core.Services.UI.Layers.Info
{
    public struct UILayerCameraInfo
    {
        public CameraClearFlags ClearFlags;
        public Color Background;
        public LayerMask CullingMask;
        public float OrtographicSize;
        public float FieldOfView;
        public UILayerCameraProjectionType CameraProjectionType;
        public float NearClippingPlain;
        public float FarClippingPlain;
        public Rect ViewPortRect;
        public float Depth;

        public Matrix4x4 ProjectionMatrix
        {
            get
            {
                if (CameraProjectionType == UILayerCameraProjectionType.Ortographic)
                    return Matrix4x4.Ortho(ViewPortRect.xMin, ViewPortRect.xMax, ViewPortRect.yMin, ViewPortRect.yMax, NearClippingPlain, FarClippingPlain);
                return Matrix4x4.Perspective(FieldOfView, (float) Screen.width/(float) Screen.height, NearClippingPlain, FarClippingPlain);
            }
        }
    }
}