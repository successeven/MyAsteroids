using System;
using Root.Core;
using Tools.MyFramework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Root.Loaders.SceneLoad.View
{
	public class SceneContextView : BaseMonoBehaviour
	{
		
		[FormerlySerializedAs("camera")]
		[SerializeField]
		private Camera _camera;
		public Camera Camera => _camera;
		
		public event Action<float> OnUpdated;
		public event Action<float>  OnFixedUpdated;
		[SerializeField]
		private Canvas mainCanvas;
		[SerializeField]
		private Transform uiParent;

		public Transform UiParent
			=> uiParent;
		public Canvas MainCanvas
			=> mainCanvas;
		
		
		private void Update()
		{
			OnUpdated?.Invoke(Time.deltaTime);
		}

		private void FixedUpdate()
		{
			OnFixedUpdated?.Invoke(Time.fixedDeltaTime);
		}
	}
}