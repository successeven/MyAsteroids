using System;
using Logic.Entities;
using Logic.Entities.Core;
using Logic.Player.LaserWeapon;
using Logic.Player.ProjectileWeapon;
using Logic.Scene;
using Root.Core;
using Root.Inputs;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;
using UnityEngine;

namespace Logic.Player
{
    public class PlayerPm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public MainSceneContextView sceneContextView;
            public PlayerModel playerModel;
            public PlayerCotroller playerCotroller;
            public Action Dead;
            public IEntitiesController entitiesController;
        }

        public PlayerView View => _view;
        private readonly Ctx _ctx;
        private const string PLAYER_PREF_NAME = "Player";
        private GameObject _pref;
        private PlayerView _view;

        public PlayerPm(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.resourceLoader.LoadPrefab(PLAYER_PREF_NAME, pref =>
            {
                _pref = pref;
                var spawnPlayer = _ctx.poolManager.Get(pref);
                _view = spawnPlayer.GetComponent<PlayerView>();
                _view.SetCtx(new BaseView.Ctx
                {
                    model = _ctx.playerModel,
                });
                OnViewLoaded();
            });
        }
        private void OnViewLoaded()
        {
            InitLogic();
            _view.Collided += Collided;
            _ctx.sceneContextView.OnUpdated += UpdateView;
        }
        private void InitLogic()
        {
            EntityMoverPm.Ctx entityMoverCtx = new EntityMoverPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.playerModel,
                inputManager = _ctx.playerCotroller.InputManager,
                useAcceleration = true
            };
            AddDispose(new EntityMoverPm(entityMoverCtx));
            
            ScreenWraperPm.Ctx screenWraperCtx = new ScreenWraperPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                playerModel = _ctx.playerModel
            };
            AddDispose(new ScreenWraperPm(screenWraperCtx));

            ProjectileWeaponPm.Ctx projectileWeaponCtx = new ProjectileWeaponPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                playerModel = _ctx.playerModel,
                playerCotroller = _ctx.playerCotroller,
                playerView = _view,
                poolManager = _ctx.poolManager,
                resourceLoader = _ctx.resourceLoader,
                projectileSettings = _ctx.sceneContextView.ProjectileSettings,
                entitiesController = _ctx.entitiesController
            };
            AddDispose(new ProjectileWeaponPm(projectileWeaponCtx));

            LasetWeaponPm.Ctx laserWeaponCtx = new LasetWeaponPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                playerModel = _ctx.playerModel,
                playerCotroller = _ctx.playerCotroller,
                playerView = _view,
                poolManager = _ctx.poolManager,
                resourceLoader = _ctx.resourceLoader,
                entitiesController = _ctx.entitiesController,
                laserSettings = _ctx.sceneContextView.laserSettings,
            };
            AddDispose(new LasetWeaponPm(laserWeaponCtx));
        }

        private void Collided(CollidedInfo collidedInfo)
        {
            if (!_ctx.entitiesController.TryGetEntityInfo(collidedInfo.defenderId, out var entityInfo))
             return;
            
            if (entityInfo.Model.EntityType == EntityType.Projectile)
            {
                if (((ProjectileModel)entityInfo.Model).OwnerId == _ctx.playerModel.Id)
                    return;
            }
            
            _ctx.Dead?.Invoke();
        }

        protected override void OnDispose()
        {
            _view.Collided -= Collided;
            _ctx.sceneContextView.OnUpdated -= UpdateView;
            _ctx.poolManager.Return(_pref, _view.gameObject);
            base.OnDispose();
        }
        private void UpdateView(float deltaTime)
        {
            _view.transform.position = _ctx.playerModel.Position.Value;
            _view.transform.rotation = Quaternion.Euler(0, 0, _ctx.playerModel.CurrentAngle.Value);
        }
    }
}