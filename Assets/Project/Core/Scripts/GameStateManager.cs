using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Core.Logger;
using Project.Core.Scene;
using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UniRx;
using UnityEngine;

namespace Project.Core
{
    public interface IGameStateManager : ICoreService
    {
        public void MoveToScene(AnchorSceneGroup sceneGroup);
    }
    
    
    public class GameStateManager : MonoBehaviour, IGameStateManager
    {
        [SerializeField] AnchorSceneGroup mainMenu;
        [SerializeField] CoreLoggerMono logger;
        [SerializeField] SceneDataLibrary library;
        ISceneLoader _sceneLoader;

        AnchorSceneGroup _currentScene;

        void Awake()
        {
            ServiceLocator.Instance.TryRegister(this as IGameStateManager);
            ServiceLocator.Instance.TryRegister(library as ISceneLibrary);
        }

        void Start()
        {
            if (!ServiceLocator.Instance.TryGet(out _sceneLoader))
                logger.Fatal("No SceneLoader Present");
            MoveToScene(mainMenu);
        }
        
        public async void MoveToScene(AnchorSceneGroup sceneGroup)
        {
            // Load new scene immediately
            IObservable<Unit> loadObservable = _sceneLoader.Load(sceneGroup);
            
            // Play outro optionally
            if (_currentScene != default)
            {
                logger.Info("Playing Outro Probably");
                // delay unt
                await _currentScene.PlayOutro();
            }
            
            // Ensure new scene is loaded
            await loadObservable;
            
            // begin unloading of old scenes
            // first, get scenes that are in old list and not old list
            if (_currentScene != default)
            {
                List<SceneField> newScenes = sceneGroup.childScenes;
                List<SceneField> oldScenes = _currentScene.childScenes;
                IEnumerable<SceneField> targetList = newScenes.Where(s => !oldScenes.Contains(s));
                
                // don't need to track unloading yet
                _sceneLoader.Unload(targetList);
            }

            // update current scene
            _currentScene = sceneGroup;
            await _currentScene.PlayIntro();
        }
    }
}
