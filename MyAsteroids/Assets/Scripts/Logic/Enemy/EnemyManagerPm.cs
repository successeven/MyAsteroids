using Logic.Entities;
using Logic.Scene;
using Root.Core;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;

namespace Logic.Enemy
{
    public class EnemyManagerPm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public MainSceneContextView sceneContextView;
            public IEntitiesController entitiesController;
        }

        private readonly Ctx _ctx;
        
        public EnemyManagerPm(Ctx ctx)
        {
            _ctx = ctx;

            EnemyCoutControllerPm.Ctx enemyCoutControllerCtx = new EnemyCoutControllerPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                entitiesController = _ctx.entitiesController
            };
            var enemyController = new EnemyCoutControllerPm(enemyCoutControllerCtx);
            AddDispose(enemyController);

            EnemySpawnerPm.Ctx enemySpawnerCtx = new EnemySpawnerPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                poolManager = _ctx.poolManager,
                resourceLoader = _ctx.resourceLoader,
                enemyCoutController = enemyController,
                entitiesController = _ctx.entitiesController
            };
            AddDispose(new EnemySpawnerPm(enemySpawnerCtx));
        }
    }
}