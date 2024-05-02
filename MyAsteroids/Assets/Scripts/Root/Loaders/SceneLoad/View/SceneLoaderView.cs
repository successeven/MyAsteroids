using System;
using System.Collections;
using TMPro;
using Tools.MyFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Root.Loaders.SceneLoad.View
{
	public class SceneLoaderView : BaseMonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _progressTextBlock;
		
		public struct Ctx
		{
		}

		private Ctx _ctx;
		
		public void SetCtx(Ctx ctx)
		{
			_ctx = ctx;
        }

		public void StartLoadScene(string sceneName, Action OnCompleted)
		{
			StartCoroutine(LoadScene(sceneName, OnCompleted));
		}

		IEnumerator LoadScene(string sceneName, Action OnCompleted)
		{
			float loadingProgress;
			// var currentScene = SceneManager.GetActiveScene();
			// var loadMode = currentScene.name != sceneName ? LoadSceneMode.Additive
			AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(sceneName);
			while (!loadingSceneOp.isDone)
			{
				loadingProgress = Mathf.Clamp01(loadingSceneOp.progress / .9f);
				_progressTextBlock.text = $"Loading {loadingProgress:0}%";
				yield return null;
			}
			OnCompleted?.Invoke();
			gameObject.SetActive(false);
		}
		
		IEnumerator UnLoadScene(Scene scene, Action OnCompleted)
		{
			AsyncOperation unloadingSceneOp = SceneManager.UnloadSceneAsync(scene);
			while (!unloadingSceneOp.isDone)
			{
				yield return null;
			}
			OnCompleted?.Invoke();
			gameObject.SetActive(false);

		}
	}
}