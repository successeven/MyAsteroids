using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Logic.Settings;
using Root.Core;
using Root.Inputs;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;
using UnityEngine;

namespace Logic.Player.ProjectileWeapon
{
    public class ProjectileWeaponPm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public PlayerModel playerModel;
            public PlayerView playerView;
            public PlayerCotroller playerCotroller;
            public MainSceneContextView sceneContextView;
            public ProjectileSettings projectileSettings;
            public IEntitiesController entitiesController;
        }

        private const string PROJECTILE_PREF = "Projectile";
        private readonly Ctx _ctx;
        private bool _inputFire;
        private Transform _spawnPosition;
        private int _indexator;
        private ProjectileSettings _projectileSettings;
        private GameObject _projectilePref;
        public ProjectileWeaponPm(Ctx ctx)
        {
            _ctx = ctx;
            _spawnPosition = _ctx.playerView.ShootPoint;
            _projectileSettings = _ctx.projectileSettings;
            LoadPref();
            _ctx.playerCotroller.Fire1 += Fire;
            _ctx.sceneContextView.OnFixedUpdated += FixedUpdate;
        }
        
        private void Fire()
        {
            CreateProjectile();
        }
        private void CreateProjectile()
        { 
            var position = _spawnPosition.position;
            var model = new ProjectileModel
            {
                EntityType = EntityType.Projectile,
                Id = _ctx.entitiesController.GenerateId(),
                Position = {Value = position},
                CurrentAngle = {Value = _ctx.playerModel.CurrentAngle.Value},
                MaxSpeed = {Value = _projectileSettings.ProjectileMaxSpeed},
            };

            var view = _ctx.poolManager.Get(_projectilePref, position, _ctx.playerModel.CurrentAngle.Value);
            var projectileView = view.GetComponent<ProjectileView>();
            projectileView.SetCtx(new BaseView.Ctx
            {
                model = model
            });
           
            ProjectilePm.Ctx projectileCtx = new ProjectilePm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                view = projectileView,
                ownewId = _ctx.playerModel.Id,
                projectileModel = model,
                returnView = () => { _ctx.poolManager.Return(_projectilePref, view); },
                entitiesController = _ctx.entitiesController
            };

            var projectile = new ProjectilePm(projectileCtx);
            _ctx.entitiesController.AddEntity(model.Id, new EntityInfo
            {
                Logic = projectile,
                Model = model
            } );
        }
        
        private void LoadPref()
        {
            _ctx.resourceLoader.LoadPrefab(PROJECTILE_PREF, pref =>
            {
                _projectilePref = pref;
            });
        }

        private void FixedUpdate(float deltaTime)
        {
            
        }

        protected override void OnDispose()
        {
            _ctx.playerCotroller.Fire1 -= Fire;
            _ctx.sceneContextView.OnFixedUpdated -= FixedUpdate;
            base.OnDispose();
        }
        
    }
}