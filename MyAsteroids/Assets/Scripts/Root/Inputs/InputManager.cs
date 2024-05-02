using Root.Core;
using Root.Input;
using Tools.MyFramework;

namespace Root.Inputs
{
	public class InputManager : BaseDisposable, IInputManager
	{
		public struct Ctx
		{
		}

		public Controls.PlayerActions Player
			=> _controls.Player;
		private readonly Ctx _ctx;
		private readonly Controls _controls;

		public InputManager(Ctx ctx)
		{
			_ctx = ctx;
			_controls = new Controls();
			_controls.Player.Enable();
		}

	}
}