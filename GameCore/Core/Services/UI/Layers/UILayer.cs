using System.Collections.Generic;
using GameCore.Core.Services.UI.View;

namespace GameCore.Core.Services.UI.Layers
{

    public class UILayer : BaseUIBehaviour
    {
        private List<BaseUIView> _views = new List<BaseUIView>();
        public bool ContainChild(BaseUIView view)
        {
            return _views.Contains(view);
        }

        public void AddChild(BaseUIView view,int index = 0)
        {
            view.RectTransfrom.SetParent(RectTransfrom,false);
            view.RectTransfrom.SetSiblingIndex(index);
            _views.Add(view);
        }

        public void RemoveChild(BaseUIView view)
        {
            view.RectTransfrom.SetParent(null, false);
            _views.Remove(view);
        }
    }
}
