using System;
using Tools.MyFramework;
using Tools.ResourceLoader;

namespace Root.Loaders.SceneLoad
{
	public class LoadersCreator : BaseDisposable
	{
		public struct Ctx
		{
			public Action<IResourceLoader, ISceneLoader> onCreate;
		}

		private readonly Ctx _ctx;
		private IResourceLoader _resourceLoader;

		public LoadersCreator(Ctx ctx)
		{
			_ctx = ctx;
			CreateResourceLoader(() =>
			{
				SceneLoader.Ctx sceneLoadCtx = new SceneLoader.Ctx
				{
                    resourceLoader = _resourceLoader
				};
				SceneLoader sceneLoader = new SceneLoader(sceneLoadCtx);
				AddDispose(sceneLoader);

				_ctx.onCreate?.Invoke(_resourceLoader, sceneLoader);
			});
		}

		private void CreateResourceLoader(Action created)
		{
			ResourcePreLoader.Ctx previewCtx = new ResourcePreLoader.Ctx
			{
					maxLoadDelay = 0f,
					minLoadDelay = 0f
			};
			_resourceLoader = new ResourcePreLoader(previewCtx);
			created?.Invoke();
		}
	}
}