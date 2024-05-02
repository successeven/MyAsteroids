using System;
using Root.Core.RX;
using Root.Loaders.SceneLoad;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;

namespace Logic.Entities.Core
{
	public class CorePm : BaseDisposable
	{
		public struct Ctx
		{
			public IResourceLoader resourceLoader;
			public ISceneLoader sceneLoader;
		}

		private readonly Ctx _ctx;
		private readonly ReactiveProperty<SceneName> _currentScene;
		private IDisposable _scene;
		private IPoolManager _pool;

		public CorePm(Ctx ctx)
		{
			_ctx = ctx;
			_currentScene = new ReactiveProperty<SceneName>(SceneName.MainScene);
            
			// create pool
			PoolManager.Ctx poolCtx = new PoolManager.Ctx();
			_pool = AddDispose(new PoolManager(poolCtx));
			_currentScene.OnChanged += CurrentSceneChanged;
			_currentScene.Invoke();
		}
		private void CurrentSceneChanged(SceneName newSceneName)
		{
			string sceneName = newSceneName.ToString();
				
			_ctx.sceneLoader.LoadScene(sceneName, () =>
			{
				_scene?.Dispose();
				// create scene context
				ScenePm.Ctx sceneCtx = new ScenePm.Ctx
				{
						resourceLoader = _ctx.resourceLoader,
						poolManager = _pool,
						sceneName = newSceneName,
						requstChangeScene = RequstChangeScene
				};
				_scene = new ScenePm(sceneCtx);
			});
		}
		private void RequstChangeScene(SceneName sceneName)
		{
			_currentScene.ForceSet(sceneName);
		}

		protected override void OnDispose()
		{
			_currentScene.OnChanged -= CurrentSceneChanged;
			_scene?.Dispose();
			base.OnDispose();
		}
	}
}