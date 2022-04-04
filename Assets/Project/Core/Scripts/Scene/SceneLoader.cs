using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Logger;
using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.Scene
{
    public interface ISceneLoader : ICoreService
    {

        public IObservable<Unit> Unload(IEnumerable<SceneField> sceneGroup);
        public IObservable<Unit> Load(SceneGroup sceneGroup);
    }

    public class SceneLoader : ISceneLoader
    {
        readonly ICoreLogger _logger;

        public SceneLoader(ICoreLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Load in scene and any children. No nesting.
        /// </summary>
        /// <param name="sceneGroup">Contains a set of scenes (not necessarily unique)</param>
        /// <returns></returns>
        public IObservable<Unit> Load(SceneGroup sceneGroup)
        { 
            return Observable.FromCoroutine(() => LoadSceneGroup(sceneGroup));
        }

        /// <summary>
        /// Will unload exactly one scene. to do multiple, must call it once for each.
        /// </summary>
        /// <param name="sceneGroup"> Data to unload </param>
        /// <param name="oldScenes">Scenes to remove</param>
        public IObservable<Unit> Unload(IEnumerable<SceneField> oldScenes)
        {
            return Observable.FromCoroutine(() => UnloadScenes(oldScenes));
        }

        /// <summary>
        /// Coroutines actually used to do the async loading
        /// </summary>
        IEnumerator LoadSceneGroup(SceneGroup sceneGroup)
        {
            IEnumerable<AsyncOperation> asyncOperations = 
                sceneGroup.childScenes.Where(s => !s.Loaded)
                    .Select( s => SceneManager.LoadSceneAsync(s.SceneName, LoadSceneMode.Additive));

            IEnumerable<AsyncOperation> asyncOperationsArray =
                asyncOperations as AsyncOperation[] ?? asyncOperations.ToArray();
            yield return new WaitUntil(() => asyncOperationsArray.Sum(s => s.isDone ? 0 : 1) <= 0);
        }
        
        IEnumerator UnloadScenes(IEnumerable<SceneField> oldScenes)
        {
            _logger.Info(oldScenes.Count());
            IEnumerable<AsyncOperation> asyncOperations = 
                oldScenes.Where(s => s.Loaded)
                    .Select(s => SceneManager.UnloadSceneAsync(s.SceneName));
            
            IEnumerable<AsyncOperation> asyncOperationsArray =
                asyncOperations as AsyncOperation[] ?? asyncOperations.ToArray();
            yield return new WaitUntil(() => asyncOperationsArray.Sum(s => s.isDone ? 0 : 1) <= 0);
            
        }
    }
}