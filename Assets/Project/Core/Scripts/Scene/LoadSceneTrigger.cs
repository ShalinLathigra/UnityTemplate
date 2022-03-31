using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UnityEngine;

namespace Project.Core.Scene
{
    public class LoadSceneTrigger : MonoBehaviour
    {
        public AnchorSceneData toScene;

        /// <summary>
        /// Load in scene when enabled
        /// </summary>
        public void OnEnable()
        {
            if (ServiceLocator.Instance.TryGet(out ISceneLoader sceneLoader))
                sceneLoader.Load(toScene);
        }

        /// <summary>
        /// Will say to the game manager "transition to this scene please!".
        /// </summary>
        public void PlayScene()
        {
            // I want to be able to say to the game "Load this scene"
            // game state manager should handle everything from pausing to input redirection
            if (ServiceLocator.Instance.TryGet(out IGameStateManager gameState))
                gameState.MoveToScene(toScene);
        }
    }
}