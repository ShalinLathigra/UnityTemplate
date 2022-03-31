using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Logger;
using Project.Core.ScriptableObjects;
using Project.Core.Service;
using Project.Core.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.Scene
{
    public interface ISceneLoader : ICoreService
    {

        public void Load(SceneData scene);
        public CoreTask LoadSingleAsync(SceneData scene);
        public CoreTask UnloadSingle(SceneData scene);
    }
    
    public class SceneLoader : ISceneLoader
    {
        readonly ICoreLogger _logger;
        readonly Dictionary<SceneData, CoreTask> _taskDict;

        public SceneLoader(ICoreLogger logger)
        {
            _logger = logger;
            _taskDict = new Dictionary<SceneData, CoreTask>();
        }

        /// <summary>
        /// Load in the scene and childScenes sequentially
        /// Load in asyncChildScenes after
        /// </summary>
        /// <param name="sceneData"> SceneData to Load </param>
        public void Load(SceneData sceneData)
        {
            LoadSingleAsync(sceneData).Start();
            foreach (SceneData s in sceneData.childScenes) LoadSingleAsync(s).Start();
        }

        /// <summary>
        /// Load in scene + asynchronously. This will only process ONE scene.
        /// </summary>
        /// <param name="sceneData"> Data to load </param>
        /// <param name="autostart"> start loading immediately? </param>
        public CoreTask LoadSingleAsync(SceneData sceneData)
        {
            if (_taskDict.ContainsKey(sceneData)) return _taskDict[sceneData];
            CoreTask task = new CoreTask(LoadSceneAsync(sceneData), false);
            _taskDict.Add(sceneData, task);
            task.Finished += manual => _taskDict.Remove(sceneData);
            return task;
            // for this and all direct children
        }

        /// <summary>
        /// Will unload exactly one scene. to do multiple, must call it once for each.
        /// </summary>
        /// <param name="sceneData"> Data to unload </param>
        public CoreTask UnloadSingle(SceneData sceneData)
        {
            return new CoreTask(UnloadSceneAsync(sceneData));
        }

        /// <summary>
        /// Coroutines actually used to do the async loading
        /// </summary>
        IEnumerator LoadSceneAsync(SceneData sceneData)
        {
            _logger.Info("Loading Async: " + sceneData.SceneName);
            if (sceneData.Loaded)
                yield break;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneData.SceneName, LoadSceneMode.Additive);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            sceneData.scene = SceneManager.GetSceneByName(sceneData.SceneName);
            _logger.Info("Loaded Async: " + sceneData.SceneName);
        }
        
        IEnumerator UnloadSceneAsync(SceneData sceneData)
        {
            _logger.Info("Unloading Scene: " + sceneData.SceneName);
            if (!sceneData.Loaded) yield break;
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneData.SceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            _logger.Info("Unloaded Scene: " + sceneData.SceneName);
        }
        
        /// <summary>
        /// Print any state information of the SceneLoader
        /// </summary>
        public void PrintStatus()
        {
            _logger.Info("SceneLoader Active");
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