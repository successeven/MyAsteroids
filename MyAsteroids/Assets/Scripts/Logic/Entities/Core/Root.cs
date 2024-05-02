using System;
using Root.Load;
using Root.Loaders.SceneLoad;
using Tools.MyFramework;
using Tools.ResourceLoader;

namespace Logic.Entities.Core
{
	public class Root : BaseDisposable
	{
		public struct Ctx
		{
			public DebugView debugView;
			public bool debugMode;
		}

		private readonly Ctx _ctx;
		private IResourceLoader _resourceLoader;
		private ISceneLoader _sceneLoader;
		private IDisposable _gameCore;
		private SceneReloader _sceneReloader;

		public Root(Ctx ctx)
		{
			_ctx = ctx;
			SceneReloader.Ctx reloaderCtx = new SceneReloader.Ctx
			{
				monoBehaviour = _ctx.debugView
			};
			_sceneReloader = new SceneReloader(reloaderCtx);
			AddDispose(_sceneReloader);
			LoadersCreator.Ctx resCtx = new LoadersCreator.Ctx
			{
				onCreate = (resLoader, sceneLoader) =>
				{
					_resourceLoader = resLoader;
					_sceneLoader = sceneLoader;
					if (_ctx.debugMode)
						RestartSubscribe();
					else
						ReStart();
				}
			};
			LoadersCreator loadersCreator = new LoadersCreator(resCtx);
			AddDispose(loadersCreator);

			void RestartSubscribe()
			{
				_ctx.debugView.OnReloaded += ReStart;
			}
		}

		// create game cycle
		void ReStart()
		{
			_gameCore?.Dispose();
			_sceneReloader.ReloadFirstScene(() =>
			{
				CorePm.Ctx coreCtx = new CorePm.Ctx
				{
						resourceLoader = _resourceLoader,
						sceneLoader = _sceneLoader,
				};
				_gameCore = new CorePm(coreCtx);
			});
		}

		protected override void OnDispose()
		{
			_ctx.debugView.OnReloaded -= ReStart;
			_gameCore?.Dispose();
			base.OnDispose();
		}
	}
}