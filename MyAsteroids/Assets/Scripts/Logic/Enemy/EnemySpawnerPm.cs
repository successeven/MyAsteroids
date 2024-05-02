using System;
using Logic.Enemy.Asteroid;
using Logic.Enemy.UFO;
using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Logic.Settings;
using Root.Core;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;
using Random = UnityEngine.Random;

namespace Logic.Enemy
{
	public class EnemySpawnerPm : BaseDisposable
	{
		public struct Ctx
		{
			public IPoolManager poolManager;
			public IResourceLoader resourceLoader;
			public MainSceneContextView sceneContextView;
			public EnemyCoutControllerPm enemyCoutController;
			public IEntitiesController entitiesController;
		}

		private readonly Ctx _ctx;
		private AsteroidSettings _asteroidSettings;
		private RewardSettings _rewardSettings;
		private UFOSettings _ufoSettings;

		public EnemySpawnerPm(Ctx ctx)
		{
			_ctx = ctx;
			_asteroidSettings = _ctx.sceneContextView.AsteroidSettings;
			_rewardSettings = _ctx.sceneContextView.RewardSettings;
			_ufoSettings = _ctx.sceneContextView.UFOSettings;
			_ctx.enemyCoutController.SpawnEnemy += SpawnEnemy;
		}

		protected override void OnDispose()
		{
			_ctx.enemyCoutController.SpawnEnemy -= SpawnEnemy;
			base.OnDispose();
		}

		private void SpawnEnemy(EnemySpawnInfo spawninfo)
		{
			switch (spawninfo.entityType)
			{
				case EntityType.Asteroid:
					SpawnBigAsteroid(spawninfo);
					break;
				case EntityType.AsteroidPart:
					var count = Random.Range(2, 5);
					for (int i = 0; i < count; i++)
						SpawnSmallAsteroid(spawninfo);
						
					break;
				case EntityType.UFO:
					SpawnUFO(spawninfo);
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}
		private void SpawnUFO(EnemySpawnInfo spawninfo)
		{
			var model = new UFOModel()
			{
				Id = _ctx.entitiesController.GenerateId(),
				Position = {Value = spawninfo.SpawnPosition},
				EntityType = EntityType.UFO,
				CurrentAngle = {Value = spawninfo.Angle},
				MaxSpeed = {Value = _ufoSettings.MaxSpeed},
				AccelerationSpeed = {Value = _ufoSettings.Acceleration},
				Reward = _rewardSettings.UFOReward
			};
			UFOPm.Ctx ufoCtx = new UFOPm.Ctx
			{
				poolManager = _ctx.poolManager,
				sceneContextView = _ctx.sceneContextView,
				resourceLoader = _ctx.resourceLoader,
				ufoModel = model,
				playerModel = _ctx.entitiesController.GetPlayerModel() ,
				entitiesController = _ctx.entitiesController
			};

			var ufo = new UFOPm(ufoCtx);
			_ctx.entitiesController.AddEntity(model.Id, new EntityInfo
			{
				Logic = ufo,
				Model = model,
			});
		}
		private void SpawnBigAsteroid(EnemySpawnInfo spawninfo)
		{
			var rotateSide = Random.Range(0, 2) == 0 ? -1 : 1;
			var model = new AsteroidModel
			{
				Id = _ctx.entitiesController.GenerateId(),
				Position = {Value = spawninfo.SpawnPosition},
				EntityType = EntityType.Asteroid,
				CurrentAngle = {Value = spawninfo.Angle},
				CanCollapse = {Value = true},
				MaxSpeed = {Value = Random.Range(_asteroidSettings.MinSpeed, _asteroidSettings.MaxSpeed)},
				MaxRotateSpeed = {Value = rotateSide * Random.Range(_asteroidSettings.MinRotateSpeed, _asteroidSettings.MaxRotateSpeed)},
				Reward = _rewardSettings.AsteroidBigReward
			};
			AsteroidPm.Ctx asteroidCtx = new AsteroidPm.Ctx
			{
				poolManager = _ctx.poolManager,
				sceneContextView = _ctx.sceneContextView,
				resourceLoader = _ctx.resourceLoader,
				asteroidModel = model,
				entitiesController = _ctx.entitiesController,
				requestSpawn = spawnPos =>
				{
					SpawnEnemy(new EnemySpawnInfo
					{
						SpawnPosition = spawnPos,
						entityType = EntityType.AsteroidPart
					});
				}
			};
			
			var asteroid = new AsteroidPm(asteroidCtx);
			_ctx.entitiesController.AddEntity(model.Id, new EntityInfo
			{
				Logic = asteroid,
				Model = model,
			});
		}
		
		private void SpawnSmallAsteroid(EnemySpawnInfo spawninfo)
		{
			var rotateSide = Random.Range(0, 2) == 0 ? -1 : 1;
			var model = new AsteroidModel
			{
				Id = _ctx.entitiesController.GenerateId(),
				Position = {Value = spawninfo.SpawnPosition},
				EntityType = EntityType.AsteroidPart,
				CurrentAngle = {Value =  Random.Range(0, 360)},
				CanCollapse = {Value = false},
				MaxSpeed = {Value = Random.Range(_asteroidSettings.MinSpeedSmall ,_asteroidSettings.MaxSpeedSmall)},
				MaxRotateSpeed = {Value = rotateSide * Random.Range(_asteroidSettings.MinRotateSpeedSmall, _asteroidSettings.MaxRotateSpeedSmall)},
				Reward = _rewardSettings.AsteroidSmallReward
			};
			AsteroidPm.Ctx asteroidCtx = new AsteroidPm.Ctx
			{
				poolManager = _ctx.poolManager,
				sceneContextView = _ctx.sceneContextView,
				resourceLoader = _ctx.resourceLoader,
				asteroidModel = model,
				entitiesController = _ctx.entitiesController,
				requestSpawn = spawnPos =>
				{
					SpawnEnemy(new EnemySpawnInfo
					{
						SpawnPosition = spawnPos,
						entityType = EntityType.AsteroidPart
					});
				} 
			};

			var asteroid = new AsteroidPm(asteroidCtx);
			_ctx.entitiesController.AddEntity(model.Id, new EntityInfo
			{
				Logic = asteroid,
				Model = model,
			});
			
		}
	}
}