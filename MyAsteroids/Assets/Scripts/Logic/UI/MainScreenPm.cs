using Logic.Entities;
using Logic.Entities.Core;
using Logic.Scene;
using Root.Core;
using Tools.MyFramework;
using Tools.ResourceLoader;
using UnityEngine;

namespace Logic.UI
{
    public class MainScreenPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public IEntitiesController entitiesController;
            public MainSceneContextView mainSceneContextView;
        }

        private readonly Ctx _ctx;
        private const string MAIN_UI_PREF = "MainScreen";
        private const string LASER_BATTARY_UI_PREF = "LaserChargeUI";
        private PlayerModel _playerModel;
        private MainScreenView _view;
        private LaserChargeUiView[] _battaries;

        public MainScreenPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.resourceLoader.LoadPrefab(MAIN_UI_PREF, prefab =>
            {
                GameObject objView = AddComponent(Object.Instantiate(prefab, _ctx.mainSceneContextView.UiParent, false));
                _view = objView.GetComponent<MainScreenView>();
            });
            
            _playerModel = _ctx.entitiesController.GetPlayerModel();
            _battaries = new LaserChargeUiView[_playerModel.Charges.Length];
            
            _ctx.resourceLoader.LoadPrefab(LASER_BATTARY_UI_PREF, prefab =>
            {
                for (int i = 0; i < _battaries.Length; i++)
                {
                    GameObject objView = AddComponent(Object.Instantiate(prefab, _view.LaserChargeHolder, false));
                    _battaries[i] = objView.GetComponent<LaserChargeUiView>();
                }
            });
            
            _ctx.mainSceneContextView.OnUpdated += OnUpdated;
            _playerModel.Position.OnChanged += UpdatePos;
            _playerModel.Score.OnChanged += ScoreOnChanged;
            _playerModel.CurrentSpeed.OnChanged += UpdateCurSpeed;
            _playerModel.CurrentAngle.OnChanged += UpdateCurAngle;
        }
        private void ScoreOnChanged(int score)
        {
            _view.Score.text = $"Score: {score}";
        }
        private void OnUpdated(float deltaTime)
        {
            for (int i = 0; i < _battaries.Length; i++)
            {
                var valueCharge = _playerModel.Charges[i].Charge.Value;
                _battaries[i].Slider.value = valueCharge;
                _battaries[i].FillImage.color = valueCharge < 1f ? Color.yellow : Color.green;
            }
        }
        
        private void UpdateCurAngle(float angle)
        {
            _view.Angle.text = $"Angle: {Mathf.Abs(Mathf.Floor(angle))}";
        }
        private void UpdateCurSpeed(float speed)
        {
            _view.Speed.text = $"Speed: {Mathf.Floor(speed)}";
        }
        private void UpdatePos(Vector2 position)
        {
            _view.PosX.text = $"X: {position.x:N0}";
            _view.PosY.text = $"X: {position.y:N0}";
        }

        protected override void OnDispose()
        {
            _ctx.mainSceneContextView.OnUpdated -= OnUpdated;
            _playerModel.CurrentAngle.OnChanged -= UpdateCurAngle;
            _playerModel.CurrentSpeed.OnChanged -= UpdateCurSpeed;
            _playerModel.Position.OnChanged -= UpdatePos;
            base.OnDispose();
        }

    }
}