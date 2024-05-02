using System;
using Root.Core;
using Tools.MyFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Load
{
	public class DebugView : BaseMonoBehaviour
	{
		public struct Ctx
		{
		}
		
		[SerializeField] private GameObject _holder;
		[SerializeField] private Button _reloadButton;

		[SerializeField] private Toggle _generalToggle;
		
		public event Action OnReloaded;

		private Ctx _ctx;

		public void SetCtx(Ctx ctx)
		{
			_ctx = ctx;
			_reloadButton.onClick.AddListener(RelodeClicked);
			_generalToggle.onValueChanged.AddListener(SwichDebugElements);
			_generalToggle.isOn = true;
        }
		
		protected override void OnDestroy()
		{
			_generalToggle.onValueChanged.RemoveListener(SwichDebugElements);
			_reloadButton.onClick.RemoveListener(RelodeClicked);
			base.OnDestroy();
		}

		private void SwichDebugElements(bool value)
		{
			_holder.SetActive(value);
		}
		
		private void RelodeClicked()
		{
			_generalToggle.isOn = false;
			OnReloaded?.Invoke();
		}

	}
}