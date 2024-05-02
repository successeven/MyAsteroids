using Root.Load;
using Tools.MyFramework;
using UnityEngine;

namespace Logic.Entities.Core
{
	public class RootUndestructable : BaseMonoBehaviour
	{
		public struct Ctx
		{
			public bool debugMode;
		}

		[SerializeField]
		private GameObject debugObjects;

		private Ctx _ctx;
		private Root _root;

		public void SetCtx(Ctx ctx)
		{
			_ctx = ctx;
			
			
			DebugView debugView = debugObjects.GetComponent<DebugView>();
			debugView.SetCtx(new DebugView.Ctx());
			debugObjects.SetActive(_ctx.debugMode);

			// start root
			Root.Ctx rootCtx = new Root.Ctx
			{
				debugView = debugView,
				debugMode = _ctx.debugMode
			};
			_root = new Root(rootCtx);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_root?.Dispose();
		}
	}
}