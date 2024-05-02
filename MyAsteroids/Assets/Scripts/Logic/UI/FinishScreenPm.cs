using System;
using Logic.Entities;
using Logic.Scene;
using Root.Core;
using Tools.MyFramework;
using Tools.ResourceLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic.UI
{
    public class FinishScreenPm : BaseDisposable
    {
        public struct Ctx
        {
            public Action restartGame;
            public IResourceLoader resourceLoader;
            public IEntitiesController entitiesController;
            public MainSceneContextView mainSceneContextView;
        }

        private readonly Ctx _ctx;
        private FinishScreenView _view;
        private const string FINISH_UI_PREF = "FinishScreen";

        public FinishScreenPm(Ctx ctx)
        {
            _ctx = ctx;
            var player = _ctx.entitiesController.GetPlayerModel();
            
            _ctx.resourceLoader.LoadPrefab(FINISH_UI_PREF, prefab =>
            {
                GameObject objView = AddComponent(Object.Instantiate(prefab, _ctx.mainSceneContextView.UiParent, false));
                _view = objView.GetComponent<FinishScreenView>();
            });
            _view.SetCtx(new FinishScreenView.Ctx
            {
                reloadClicked = _ctx.restartGame
            });
            _view.ScoreLabel.text = $"YOUR SCORE: {player.Score.Value}";
            
            
        }
        
    }
}