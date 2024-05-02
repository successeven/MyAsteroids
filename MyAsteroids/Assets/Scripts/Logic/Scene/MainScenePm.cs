using System;
using System.Collections.Generic;
using Logic.Enemy;
using Logic.Player;
using Logic.UI;
using Root.Core;
using Root.Core.RX;
using Root.Loaders.SceneLoad;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;

namespace Logic.Scene
{
	public class MainScenePm : BaseDisposable
	{
		public struct Ctx
		{
			public IPoolManager poolManager;
			public IResourceLoader resourceLoader;
			public MainSceneContextView sceneContextView;
			public Action<SceneName> requstChangeScene;
		}

		private readonly Ctx _ctx;
		private EntitiesControllerPm _entitiesController;
		private IDisposable _enemyManager;
		private IDisposable _mainScreen;

		public MainScenePm(Ctx ctx)
		{
			_ctx = ctx;
			_entitiesController = new EntitiesControllerPm(new EntitiesControllerPm.Ctx());
			AddDispose(new EntitiesControllerPm(new EntitiesControllerPm.Ctx()));
			
			PlayerSpawnerPm.Ctx playerSpawnerCtx = new PlayerSpawnerPm.Ctx
			{
					resourceLoader = _ctx.resourceLoader,
					poolManager = _ctx.poolManager,
					sceneContextView = _ctx.sceneContextView,
					entitiesController = _entitiesController,
					playerDead = ShowFinishScreen
			};
			AddDispose(new PlayerSpawnerPm(playerSpawnerCtx));

			EnemyManagerPm.Ctx enemyManagerCtx = new EnemyManagerPm.Ctx
			{
				resourceLoader = _ctx.resourceLoader,
				poolManager = _ctx.poolManager,
				sceneContextView = _ctx.sceneContextView,
				entitiesController = _entitiesController
			};
			_enemyManager = new EnemyManagerPm(enemyManagerCtx);
			AddDispose(_enemyManager);

			MainScreenPm.Ctx mainScreenCtx = new MainScreenPm.Ctx
			{
				resourceLoader = _ctx.resourceLoader,
				entitiesController = _entitiesController,
				mainSceneContextView = _ctx.sceneContextView
			};
			_mainScreen = new MainScreenPm(mainScreenCtx);
			AddDispose(_mainScreen);
		}

		private void ShowFinishScreen()
		{
			_enemyManager?.Dispose();
			_mainScreen?.Dispose();
			FinishScreenPm.Ctx FinishScreenCtx = new FinishScreenPm.Ctx
			{
				resourceLoader = _ctx.resourceLoader,
				entitiesController = _entitiesController,
				mainSceneContextView = _ctx.sceneContextView,
				restartGame = () => {_ctx.requstChangeScene.Invoke(SceneName.MainScene);}
			};
			AddDispose(new FinishScreenPm(FinishScreenCtx));
			_entitiesController?.Dispose();
		}
	}
}