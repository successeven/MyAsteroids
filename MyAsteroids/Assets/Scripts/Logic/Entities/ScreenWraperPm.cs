﻿using Logic.Entities.Core;
using Logic.Scene;
using Root.Core;
using Tools.MyFramework;
using UnityEngine;

namespace Logic.Entities
{
	public class ScreenWraperPm : BaseDisposable
	{
		public struct Ctx
		{
			public MainSceneContextView sceneContextView;
			public PlayerModel playerModel;
		}

		private readonly Ctx _ctx;
		private Camera _camera;

		public ScreenWraperPm(Ctx ctx)
		{
			_ctx = ctx;
			_camera = _ctx.sceneContextView.Camera;
			_ctx.sceneContextView.OnFixedUpdated += CheckScreenPos;
		}

		protected override void OnDispose()
		{
			base.OnDispose(); 
			_ctx.sceneContextView.OnFixedUpdated -= CheckScreenPos;
		}

		private void CheckScreenPos(float deltaTime)
		{
			var playerPos = _ctx.playerModel.Position.Value;
			Vector3 viewPosition = _camera.WorldToViewportPoint(playerPos);

            if (viewPosition.x is < 0 or > 1)
	            playerPos.x = -playerPos.x;
            
            if (viewPosition.y is < 0 or > 1)
	            playerPos.y = -playerPos.y;
            
            _ctx.playerModel.Position.Value = playerPos;
		}
		
	}
}