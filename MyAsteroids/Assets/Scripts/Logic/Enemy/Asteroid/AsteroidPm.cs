using System;
using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Root.Core;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;
using UnityEngine;

namespace Logic.Enemy.Asteroid
{
    public class AsteroidPm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public MainSceneContextView sceneContextView;
            public AsteroidModel asteroidModel;
            public Action<Vector3> requestSpawn;
            public IEntitiesController entitiesController;
        }

        public BaseView View
            => _view;

        private readonly Ctx _ctx;
        private GameObject _pref;
        private AsteroidView _view;
        private float _rotateSide;
        private const string ASTEROID_PREF_NAME = "Asteroid";

        public AsteroidPm(Ctx ctx)
        {
            _ctx = ctx;
            EntityMoverPm.Ctx entityMoverCtx = new EntityMoverPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.asteroidModel,
                useAcceleration = false
            };
            AddDispose(new EntityMoverPm(entityMoverCtx));
            BorderControllerPm.Ctx borderCtx = new BorderControllerPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.asteroidModel,
                entitiesController = _ctx.entitiesController
            };
            AddDispose(new BorderControllerPm(borderCtx));
            
            _ctx.resourceLoader.LoadPrefab(ASTEROID_PREF_NAME, pref =>
            {
                _pref = pref;
                var spawnPlayer = _ctx.poolManager.Get(pref, _ctx.asteroidModel.Position.Value);
                _view = spawnPlayer.GetComponent<AsteroidView>();
                _view.SetCtx(new AsteroidView.Ctx
                {
                    model = _ctx.asteroidModel
                });
            });
            _ctx.asteroidModel.OnDestroy += TryCollapse;
            _ctx.sceneContextView.OnUpdated += UpdateView;
        }
        
        private void TryCollapse(int? killerId)
        {
            if (killerId != null && _ctx.asteroidModel.CanCollapse.Value)
            {
                _ctx.requestSpawn?.Invoke(_ctx.asteroidModel.Position.Value);
            }
        }

        protected override void OnDispose()
        {
            
            _ctx.asteroidModel.OnDestroy -= TryCollapse;
            _ctx.sceneContextView.OnUpdated -= UpdateView;
            _ctx.poolManager.Return(_pref, _view.gameObject);
            base.OnDispose();
        }
        private void UpdateView(float deltaTime)
        {
            _view.transform.position = _ctx.asteroidModel.Position.Value;
            var curRotate = _view.Holder.rotation;
            curRotate = Quaternion.Euler(0, 0, curRotate.eulerAngles.z - _ctx.asteroidModel.MaxRotateSpeed.Value * deltaTime);
            _view.Holder.rotation = curRotate;
        }

    }
}