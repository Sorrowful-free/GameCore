using System.Diagnostics;
using System.Threading.Tasks;
using GameCore.Core.Application;
using Debug = UnityEngine.Debug;

namespace GameCore.Core
{
    public class TestService : ITestService
    {
        public async Task Initialize()
        {
            
        }

        public async Task Deinitialize()
        {
        }

        public void Hello()
        {
            Debug.Log("asds");
        }
    }
}
