using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Core.Scene;
using UnityEngine;

namespace Project.Core.ScriptableObjects
{
    /// <summary>
    /// Pretty much a pure data container with information about a scene, root of this all is stored in the SceneLib
    /// When an "anchor" scene is loaded
    /// </summary>
    [CreateAssetMenu(menuName = "SceneLoader/Create AnchorSceneData", fileName = "AnchorSceneData", order = 1)]
    public class AnchorSceneGroup : SceneGroup
    {
        SceneController _controller;
        bool _controllerFound;
        
        /// <summary>
        /// Lazily get the sceneController. Every scene has to have one
        /// </summary>
        public SceneController Controller
        {
            get => _controllerFound ? _controller : null;
            set
            {
                _controllerFound = value != null;
                if (_controllerFound) _controller = value;
            }
        }

        public async Task PlayOutro()
        {
            if (_controllerFound) await Controller.PlayOutro();
        }

        public async Task PlayIntro()
        {
            if (_controllerFound) await Controller.PlayIntro();
        }
    }
}