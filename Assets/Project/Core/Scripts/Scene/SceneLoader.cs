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

/*
 
TODO: modify SceneLoader to use ^ to determine whether it is important to blow away a scene or not.

so the game starts, we say "CoreSceneManager, please play the game menu + load any immediate linked scenes."
CSM then goes, loads game menu if not already loaded, plays it, then background loads any other scenes using load async

Scenes are only unloaded when the current anchor no longer has use for them.
Scenes are loaded if needed and not part of the existing set
Scenes will register any available services with the service locator to expose api calls to whoever requires them

    i.e. Dialogue.

Main menu loads just itself, stores a sceneData ref under the play button keyed to level one (anchor scene)

    When play is pressed,
    ServLoc.LoadScene(new scene)
    Scenetransition.Hide(transition type) [This should also lock player input temporarily]
    Check if new scene already loaded, if not, load it in.
    Look through available scenes, for each one, if not in required scenes, remove it
    look through required scenes, if not in available, add it         
    When all complete, SceneTransition.Show(TransitionType) [This should also unlock player input]

    Now, new scene is loaded and activated, dialogue scene is present and registered, but not out of sight
    Next step is to say "ServLoc".Get(DialoguePlayer).play(Some dialogue scriptable object)

Other more interesting projects may need to preload scenes in some way so that you can seamlessly move between them
 
 TODO: revisit below
 have each sceneRef store a render texture + the scene controller, after a transition, slot this in, then go. 
 */