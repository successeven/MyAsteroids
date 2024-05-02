using System;

namespace Root.Loaders.SceneLoad
{
	public interface ISceneReloader
	{
		void ReloadFirstScene(Action onComplete);
	}
}