using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.Core.Services.UI.View
{
    public class BaseUIBehaviour :UIBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransfrom
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = (RectTransform) transform;
                }
                return _rectTransform;
            }
        }

    }
}
