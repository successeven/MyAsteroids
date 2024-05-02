using System;
using System.Collections;
using System.Collections.Generic;
using Root.Core;
using Tools.MyFramework;
using UnityEngine.SceneManagement;

namespace Root.Loaders.SceneLoad
{
    public class SceneReloader : BaseDisposable, ISceneReloader
    {
        public struct Ctx
        {
            public BaseMonoBehaviour monoBehaviour;
        }

        private readonly Ctx _ctx;

        private const string INTRO_SCENE_NAME = "Intro";

        public SceneReloader(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void ReloadFirstScene(Action onComplete)
        {
            if (SceneManager.GetActiveScene().name == INTRO_SCENE_NAME)
            {
                onComplete?.Invoke();
                return;
            }

            SceneManager.LoadScene(INTRO_SCENE_NAME);
            _ctx.monoBehaviour.StartCoroutine(WaitNexFrame(() =>
            {
                onComplete?.Invoke();
            }));
        }

        IEnumerator WaitNexFrame(Action OnCompleted)
        {
            yield return null;
            OnCompleted?.Invoke();
        }
    }
}