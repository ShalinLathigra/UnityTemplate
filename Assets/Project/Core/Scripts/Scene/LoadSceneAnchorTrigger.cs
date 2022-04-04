using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UniRx;
using UnityEngine;

namespace Project.Core.Scene
{
    public class LoadSceneAnchorTrigger : MonoBehaviour
    {
        public AnchorSceneGroup toScene;

        /// <summary>
        /// Will say to the game manager "transition to this scene please!".
        /// </summary>
        public void PlayScene()
        {
            if (ServiceLocator.Instance.TryGet(out IGameStateManager gameState))
                gameState.MoveToScene(toScene);
        }
    }
}