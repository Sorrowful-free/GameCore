using System.Threading.Tasks;

namespace GameCore.Core.Services.UI.View
{
    public abstract class BaseUIItemRenderer<TData> : BaseUIBehaviour
    {
        public abstract Task SetData(TData data);
    }
}
