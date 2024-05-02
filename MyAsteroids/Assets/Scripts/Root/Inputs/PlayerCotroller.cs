using System;
using Root.Core;
using Tools.MyFramework;
using UnityEngine.InputSystem;

namespace Root.Inputs
{
	public class PlayerCotroller : BaseDisposable
	{
		public struct Ctx
		{
		}

		private readonly Ctx _ctx;
		public IInputManager InputManager { get; }
		public event Action Fire1;
		public event Action Fire2;

		public PlayerCotroller(Ctx ctx)
		{
			_ctx = ctx;
			InputManager = AddDispose(new InputManager(new InputManager.Ctx()));
			InputManager.Player.Fire1.performed += Fireleft;
			InputManager.Player.Fire2.performed += FireRight;
		}
		private void Fireleft(InputAction.CallbackContext callbackContext)
		{
			Fire1?.Invoke();
		}
		private void FireRight(InputAction.CallbackContext callbackContext)
		{
			Fire2?.Invoke();
		}

		protected override void OnDispose()
		{
			InputManager.Player.Fire1.performed -= Fireleft;
			InputManager.Player.Fire2.performed -= FireRight;
			base.OnDispose();
		}

	}
}