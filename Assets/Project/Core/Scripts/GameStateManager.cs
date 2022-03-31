using Project.Core.Logger;
using Project.Core.Scene;
using Project.Core.ScriptableObjects;
using Project.Core.Service;
using Project.Core.Tasks;
using UnityEngine;

namespace Project.Core
{
    public interface IGameStateManager : ICoreService
    {
        public void MoveToScene(AnchorSceneData sceneData);
    }
    
    
    public class GameStateManager : MonoBehaviour, IGameStateManager
    {
        [SerializeField] AnchorSceneData mainMenu;
        [SerializeField] CoreLoggerMono logger;
        [SerializeField] SceneDataLibrary library;
        ISceneLoader _sceneLoader;

        AnchorSceneData _currentScene;

        void Awake()
        {
            ServiceLocator.Instance.TryRegister(this as IGameStateManager);
            ServiceLocator.Instance.TryRegister(library as ISceneLibrary);
        }

        void Start()
        {
            if (!ServiceLocator.Instance.TryGet(out _sceneLoader))
                logger.Fatal("No SceneLoader Present");
            CoreTask loadSingleAsync = _sceneLoader.LoadSingleAsync(mainMenu);
            loadSingleAsync.Finished += manual =>
            {
                MoveToScene(mainMenu);
            }; 
            loadSingleAsync.Start();
        }

        public void PrintStatus()
        {
            logger.Info("Ready!");
        }

        // Node, this is just used to transition between larger game states. Stores and stuff will be handled within
        // the levels, will ask the world to handle shit though.
        public void MoveToScene(AnchorSceneData sceneData)
        {
            // in coroutine: start scene load, do outro, then play intro when ready.
            // should be in a coroutine
            if (_currentScene != default)
            {
                logger.Info("Playing Outro Probably");
                _currentScene.controller.PlayOutro();
            }

            _currentScene = sceneData;
            bool hasAnim = false;
            foreach (GameObject obj in sceneData.scene.GetRootGameObjects())
            {
                hasAnim = obj.TryGetComponent(out SceneController controller);
                if (!hasAnim) continue;
                _currentScene.controller = controller;
                break;
            }
            if (hasAnim)
                _currentScene.controller.PlayIntro();
        }
    }
}
