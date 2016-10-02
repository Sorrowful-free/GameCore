using System.Threading.Tasks;
using GameCore.Core.Services.UI.Layers.Info;
using GameCore.Core.UnityThreading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.Core.Services.UI.Layers
{
    public static class UILayerFactory
    {
        public static async Task<UILayer> CreateLayer(UILayerInfo layerInfo, GameObject root)
        {
            var task = new Task<UILayer>(() =>
            {
                var uiLayerGameObject = new GameObject("TempUiLayer");
                uiLayerGameObject.transform.SetParent(root.transform);
                return ApplySettings(uiLayerGameObject, layerInfo);
            });
            task.Start(UnityTaskScheduler.Instance);
            return await task;
        }

        private static UILayer ApplySettings(GameObject gameObject, UILayerInfo info)
        {
            var uiLayer = gameObject.AddComponent<UILayer>();
            ApplyCameraSettings(gameObject, info);
            ApplyCanvasSettings(gameObject, info);
            ApplyCanvasScalerSettings(gameObject,info);
            ApplyRaycasterSettings(gameObject,info);
            return uiLayer;
        }

        private static void ApplyCanvasSettings(GameObject gameObject, UILayerInfo info)
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = info.RenderMode;
            canvas.pixelPerfect = info.PixelPerfect;
            canvas.sortingOrder = info.SortOrder;
            canvas.sortingLayerName = info.SortLayer;
            canvas.targetDisplay = info.TargetDisplay;
            canvas.worldCamera = gameObject.GetComponent<Camera>();
        }

        private static void ApplyCameraSettings(GameObject gameObject, UILayerInfo info)
        {
            if (info.RenderMode != RenderMode.ScreenSpaceOverlay ||
                info.RaycasterInfo.RaycasterType != UILayerRaycasterType.Graphic)
            {
                var camInfo = info.CameraInfo;
                var camera = gameObject.AddComponent<Camera>();
                camera.clearFlags = camInfo.ClearFlags;
                camera.backgroundColor = camInfo.Background;
                camera.cullingMask = camInfo.CullingMask.value;
                camera.nearClipPlane = camInfo.NearClippingPlain;
                camera.farClipPlane = camInfo.FarClippingPlain;
                camera.depth = camInfo.Depth;
                camera.projectionMatrix = camInfo.ProjectionMatrix;
                camera.orthographic = camInfo.CameraProjectionType == UILayerCameraProjectionType.Ortographic;
                camera.fieldOfView = camInfo.FieldOfView;
                camera.orthographicSize= camInfo.OrtographicSize;
            }
        }
        
        private static void ApplyCanvasScalerSettings(GameObject gameObject, UILayerInfo info)
        {
            var scalerInfo = info.CanvasScalerInfo;
            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = scalerInfo.ScaleMode;
            scaler.scaleFactor = scalerInfo.ScaleFactor;
            scaler.referenceResolution = scalerInfo.ReferenceResolution;
            scaler.screenMatchMode = scalerInfo.ScreenMatchMode;
            scaler.matchWidthOrHeight = scalerInfo.Match;
            scaler.physicalUnit = scalerInfo.Unit;
            scaler.fallbackScreenDPI = scalerInfo.FallbackScreenDPI;
            scaler.defaultSpriteDPI = scalerInfo.DefaultSpriteDPI;
            scaler.referencePixelsPerUnit = scalerInfo.ReferencePixelPerUnit;
        }

        private static void ApplyRaycasterSettings(GameObject gameObject, UILayerInfo info)
        {
            var raycasterInfo = info.RaycasterInfo;
            switch (raycasterInfo.RaycasterType)
            {
                default:
                case UILayerRaycasterType.Graphic:
                    var gRaycaster = gameObject.AddComponent<GraphicRaycaster>();
                    gRaycaster.blockingObjects = raycasterInfo.BlockingObjects;
                    gRaycaster.ignoreReversedGraphics = raycasterInfo.IgnoreReversedGraphic;
                    if(gRaycaster.eventCamera != null)
                        gRaycaster.eventCamera.eventMask = raycasterInfo.EventMask.value;
                    break;
                case UILayerRaycasterType.Physics2D:
                    var p2Raycaster = gameObject.AddComponent<Physics2DRaycaster>();
                    p2Raycaster.eventMask = raycasterInfo.EventMask;
                    break;
                case UILayerRaycasterType.Physics:
                    var pRaycaster = gameObject.AddComponent<PhysicsRaycaster>();
                    pRaycaster.eventMask = raycasterInfo.EventMask;
                    break;
            }
        }
    }
}