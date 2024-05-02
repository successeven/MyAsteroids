﻿using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Root.Core;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;
using UnityEngine;

namespace Logic.Enemy.UFO
{
    public class UFOPm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public MainSceneContextView sceneContextView;
            public UFOModel ufoModel;
            public PlayerModel playerModel;
            public IEntitiesController entitiesController;
        }

        private readonly Ctx _ctx;
        private GameObject _pref;
        private UFOView _view;
        private const string UFO_PREF_NAME = "UFO";

        public UFOPm(Ctx ctx)
        {
            _ctx = ctx;
            
            var baseCtx =  new EntityMoverPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.ufoModel,
                useAcceleration = true
            };
            
            var UFOMoverCtx = new UFOMoverPm.UFOMoverCtx()
            {
                playerModel = _ctx.playerModel
            };
            AddDispose(new UFOMoverPm(UFOMoverCtx, baseCtx));
            
            BorderControllerPm.Ctx borderCtx = new BorderControllerPm.Ctx
            {
                sceneContextView = _ctx.sceneContextView,
                model = _ctx.ufoModel,
                entitiesController = _ctx.entitiesController
            };
            AddDispose(new BorderControllerPm(borderCtx));
            
            _ctx.resourceLoader.LoadPrefab(UFO_PREF_NAME, pref =>
            {
                _pref = pref;
                var spawnPlayer = _ctx.poolManager.Get(pref, _ctx.ufoModel.Position.Value);
                _view = spawnPlayer.GetComponent<UFOView>();
                _view.SetCtx(new UFOView.Ctx
                {
                    model = _ctx.ufoModel
                });
            });
            //_ctx.ufoModel.OnDestroy += DestroyMe;
            _ctx.sceneContextView.OnUpdated += UpdateView;
        }

        protected override void OnDispose()
        {
            //_ctx.ufoModel.OnDestroy -= DestroyMe;
            _ctx.sceneContextView.OnUpdated -= UpdateView;
            _ctx.poolManager.Return(_pref, _view.gameObject);
            base.OnDispose();
        }
        private void UpdateView(float deltaTime)
        {
            _view.transform.position = _ctx.ufoModel.Position.Value;
            _view.transform.rotation = Quaternion.Euler(0, 0, _ctx.ufoModel.CurrentAngle.Value);
        }

    }
}