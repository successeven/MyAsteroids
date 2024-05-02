﻿using System;
using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Root.Core;
using Root.Loaders.SceneLoad.View;
using Tools.MyFramework;

namespace Logic.Player.ProjectileWeapon
{
    public class ProjectilePm : BaseDisposable
    {
        public struct Ctx
        {
            public int ownewId;
            public ProjectileView view;
            public ProjectileModel projectileModel;
            public SceneContextView sceneContextView;
            public Action returnView;
            public IEntitiesController entitiesController;
        }

        private readonly Ctx _ctx;
        private ProjectileView _view;
        private ProjectileModel _projectileModel;

        public ProjectilePm(Ctx ctx)
        {
            _ctx = ctx;
            _view = _ctx.view;
            _projectileModel = _ctx.projectileModel;
            
            EntityMoverPm.Ctx entityMoverCtx = new EntityMoverPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.projectileModel,
                useAcceleration = false
            };
            AddDispose(new EntityMoverPm(entityMoverCtx));
            
            BorderControllerPm.Ctx borderCtx = new BorderControllerPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.projectileModel,
                entitiesController = _ctx.entitiesController
            };
            AddDispose(new BorderControllerPm(borderCtx));
            
            //_ctx.projectileModel.OnDestroy += DestroyMe;
            _view.Collided += OnCollided;
            _ctx.sceneContextView.OnUpdated += UpdateMe;
        }
        private void DestroyMe(int? killerId)
        {
            _ctx.entitiesController.TryDestroyEntity(_ctx.projectileModel.Id);
        }
        private void UpdateMe(float obj)
        {
            _view.transform.position = _projectileModel.Position.Value;
        }
        private void OnCollided(CollidedInfo collidedInfo)
        {
            if (!_ctx.entitiesController.TryGetEntityInfo(collidedInfo.defenderId, out var entityInfo))
                return;
            
            if (entityInfo.Model.EntityType is EntityType.Asteroid or EntityType.AsteroidPart or EntityType.UFO)
            {
                _ctx.entitiesController.TryDestroyEntity(entityInfo.Model.Id, _ctx.projectileModel.OwnerId);
            }
            
            if (entityInfo.Model.EntityType is EntityType.PlayerShip)
                return;
            DestroyMe(null);
        }
        
        protected override void OnDispose()
        {
            //_ctx.projectileModel.OnDestroy -= DestroyMe;
            _view.Collided -= OnCollided;
            _ctx.sceneContextView.OnUpdated -= UpdateMe;
            _ctx.returnView?.Invoke();
            base.OnDispose();
        }
    }
}