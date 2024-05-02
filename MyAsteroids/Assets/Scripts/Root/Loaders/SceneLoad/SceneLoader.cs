using System;
using Root.Core;
using Root.Loaders.SceneLoad.View;
using Tools.MyFramework;
using Tools.ResourceLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Root.Loaders.SceneLoad
{
	public class SceneLoader : BaseDisposable, ISceneLoader
	{
		public struct Ctx
		{
			public IResourceLoader resourceLoader;
		}

		private readonly Ctx _ctx;

		private bool _isLoading;
		private IDisposable _unloadingLevel;
		private IDisposable _loadingLevel;
		private SceneLoaderView _view;
		

		private static string LOAD_PREF = "LoadingScreen";

		public SceneLoader(Ctx ctx)
		{
			_ctx = ctx;
			_isLoading = false;
			_ctx.resourceLoader.LoadPrefab(LOAD_PREF, pref =>
			{
				GameObject loadScreen =AddComponent(GameObject.Instantiate(pref));
				Object.DontDestroyOnLoad(loadScreen);
				_view = loadScreen.GetComponent<SceneLoaderView>();
				_view.SetCtx(new SceneLoaderView.Ctx());
				_view.gameObject.SetActive(false);
			});
		}

		public void LoadScene(string sceneName, Action onComplete)
		{
			if (_isLoading)
			{
				Debug.LogError($"Can't start load {sceneName}. Level loader is busy");
				onComplete?.Invoke();
				return;
			}
			_isLoading = true;
			Debug.Log($"Trying to load scene {sceneName}");
			LoadSceneAsync(sceneName, ()=>
			{
				onComplete?.Invoke();
				_isLoading = false;
			});

		}

		protected override void OnDispose()
		{
			Reset();
			base.OnDispose();
		}

		private void Reset()
		{
			_isLoading = false;
		}

		private void LoadSceneAsync(string name, Action onComplete)
		{
			_view.gameObject.SetActive(true);
			_view.StartLoadScene(name,onComplete);
		}
	}
}