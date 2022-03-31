using Project.Core.Scene;
using UnityEngine;

namespace Project.Core.ScriptableObjects
{
    /// <summary>
    /// Pretty much a pure data container with information about a scene, root of this all is stored in the SceneLib
    /// When an "anchor" scene is loaded
    /// </summary>
    [CreateAssetMenu(menuName = "SceneLoader/Create AnchorSceneData", fileName = "AnchorSceneData", order = 1)]
    public class AnchorSceneData : SceneData
    {
        [HideInInspector] public SceneController controller;
    }
}