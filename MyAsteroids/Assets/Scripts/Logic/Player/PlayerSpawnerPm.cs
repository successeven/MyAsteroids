using System;
using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Root.Core;
using Root.Inputs;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;

namespace Logic.Player
{
    public class PlayerSpawnerPm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public MainSceneContextView sceneContextView;
            public IEntitiesController entitiesController;
            public Action playerDead;
        }

        private readonly Ctx _ctx;

        public PlayerSpawnerPm(Ctx ctx)
        {
            _ctx = ctx;
            var playerController = AddDispose(new PlayerCotroller(new PlayerCotroller.Ctx()));
            var playerSettings = _ctx.sceneContextView.PlayerSettings;
            var playerModel = new PlayerModel
            {
                Id = _ctx.entitiesController.GenerateId(),
                EntityType = EntityType.PlayerShip,
                Position = {Value = playerSettings.StartPosition},
                MaxSpeed = {Value = playerSettings.MaxSpeed},
                AccelerationSpeed = {Value = playerSettings.Acceleration},
                DecelerationSpeed = {Value = playerSettings.Deceleration},
                MaxRotateSpeed = {Value = playerSettings.MaxRotationSpeed},
                AccelerationRotateSpeed = {Value = playerSettings.RotationAcceleration},
                DecelerationRotateSpeed = {Value = playerSettings.RotationDeceleration},
            };
            PlayerPm.Ctx playerCtx = new PlayerPm.Ctx
            {
                playerModel = playerModel,
                poolManager = _ctx.poolManager,
                resourceLoader = _ctx.resourceLoader,
                playerCotroller = playerController,
                sceneContextView = _ctx.sceneContextView,
                entitiesController = _ctx.entitiesController,
                Dead = _ctx.playerDead
            };
            var player = new PlayerPm(playerCtx);
            
            _ctx.entitiesController.AddEntity(playerModel.Id, new EntityInfo
            {
                Logic = player,
                Model = playerModel,
            });
        }

    }
}