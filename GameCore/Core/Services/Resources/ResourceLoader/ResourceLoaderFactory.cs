using GameCore.Core.Base.Dependency;
using GameCore.Core.Services.Resources.ResourceLoader.Bundle;
using GameCore.Core.Services.Resources.ResourceLoader.Local;

namespace GameCore.Core.Services.Resources.ResourceLoader
{
    public class ResourceLoaderFactory
    {
        private readonly ResourceTree _resourceTree;

        public ResourceLoaderFactory(ResourceTree resourceTree)
        {
            _resourceTree = resourceTree;
        }

        public IResourceLoader CreateInstance(int id)
        {
            if (id <= 0)
            {
                return new LocalResourceLoader();
            }
            var bundleInfo = _resourceTree.GetBundleInfo(id);
            var bundlePath = _resourceTree.GetBundlePath(id);
            if(!UnityEngine.Application.isEditor)
                return new BundleResourceLoader(bundlePath,bundleInfo);
            return DependencyInjector.GetDependency<IResourceLoader>(bundlePath,bundleInfo);
        }
    }
}
