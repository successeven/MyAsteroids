using System;
using System.Linq;
using Logic.Scene;
using Root.Loaders.SceneLoad;
using Root.Loaders.SceneLoad.View;
using Tools.MyFramework;
using Tools.Pool;
using Tools.ResourceLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Logic.Entities.Core
{
	public class ScenePm : BaseDisposable
    {
        public struct Ctx
        {
            public IPoolManager poolManager;
            public IResourceLoader resourceLoader;
            public SceneName sceneName;
            public Action<SceneName> requstChangeScene;
        }

        private readonly Ctx _ctx;

        public ScenePm(Ctx ctx)
        {
            _ctx = ctx;

            // choose logic depends on scene
            SceneContextView sceneContext = FindContext(_ctx.sceneName);
            switch (_ctx.sceneName)
            {
                case SceneName.MainScene:
                    CreateMainScene();
                    break;
            }

            void CreateMainScene()
            {
                if (sceneContext is not MainSceneContextView sceneContextView)
                {
                    Debug.LogError("sceneContextView was null");
                    return;
                }

                MainScenePm.Ctx mainSceneCtx = new MainScenePm.Ctx
                {
                    resourceLoader = _ctx.resourceLoader,
                    poolManager = _ctx.poolManager,
                    sceneContextView = sceneContextView,
                    requstChangeScene = _ctx.requstChangeScene
                };
                MainScenePm mainScenePm = new MainScenePm(mainSceneCtx);
                AddDispose(mainScenePm);
            }
        }
        private static SceneContextView FindContext(SceneName sceneName)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
            SceneContextView[] sceneContexts = Object.FindObjectsOfType<SceneContextView>();
            SceneContextView sceneContext = sceneContexts.FirstOrDefault(ctx =>
            {
                return sceneName switch
                {
                    SceneName.MainScene => ctx is MainSceneContextView,
                    _ => false
                };
            });
            return sceneContext;
        }

    }
}