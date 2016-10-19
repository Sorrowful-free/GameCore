using System.Threading.Tasks;
using GameCore.Core.Services.UI;
using GameCore.Core.Services.UI.Layers.Info;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public enum TestUILayers
    {
        First,
        Second
    }
    public class TestUIService : BaseUIService<TestUILayers>
    {
        public override async Task Initialize()
        {
            await AddLayer(TestUILayers.First, new UILayerInfo
            {
                RenderMode = RenderMode.ScreenSpaceOverlay,
                SortOrder = 0,
                CanvasScalerInfo = new UILayerCanvasScalerInfo
                {
                    ScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize,
                    ScreenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
                    FallbackScreenDPI = 96,
                    Match = 1,
                    DefaultSpriteDPI = 96,
                    ReferenceResolution = new Vector2(800,600),
                    ScaleFactor = 1,
                    Unit = CanvasScaler.Unit.Inches,
                    ReferencePixelPerUnit = 100
                },
                RaycasterInfo = new UILayerRaycasterInfo
                {
                    BlockingObjects = GraphicRaycaster.BlockingObjects.TwoD,
                    RaycasterType = UILayerRaycasterType.Graphic
                }
            });
            await AddLayer(TestUILayers.Second, new UILayerInfo
            {
                RenderMode = RenderMode.ScreenSpaceOverlay,
                SortOrder = 2,
                CanvasScalerInfo = new UILayerCanvasScalerInfo
                {
                    ScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
                    ScreenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
                    FallbackScreenDPI = 96,
                    Match = 1,
                    DefaultSpriteDPI = 96,
                    ReferenceResolution = new Vector2(800, 600),
                    ScaleFactor = 1,
                    Unit = CanvasScaler.Unit.Inches,
                    ReferencePixelPerUnit = 100
                },
                RaycasterInfo = new UILayerRaycasterInfo
                {
                    BlockingObjects = GraphicRaycaster.BlockingObjects.TwoD,
                    RaycasterType = UILayerRaycasterType.Graphic
                }
            });



        }

        public override async Task Deinitialize()
        {
            
        }
    }
}
