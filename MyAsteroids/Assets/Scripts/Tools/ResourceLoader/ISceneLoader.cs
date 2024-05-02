using System;

namespace Tools.ResourceLoader
{
	public interface ISceneLoader: IDisposable
	{
		void LoadScene(string sceneName, Action onComplete);
	}
}