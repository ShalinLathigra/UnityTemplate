using System.Collections.Generic;
using Project.Core.Scene;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.ScriptableObjects
{
    public enum SceneType
    {
        Basic,
        Anchor,
    }
    
    /// <summary>
    /// Pretty much a pure data container with information about a scene, root of this all is stored in the SceneLib
    /// When an "anchor" scene is loaded
    /// </summary>
    [CreateAssetMenu(menuName = "SceneLoader/Create SceneData", fileName = "SceneData", order = 1)]
    public class SceneData : SerializedScriptableObject
    {
        public SceneField sceneField;
        public List<SceneData> childScenes;
        public string SceneName => sceneField.SceneName;
        
        [HideInInspector] public UnityEngine.SceneManagement.Scene scene;

        // Tracking load state of the scene.
        public bool Loaded => SceneManager.GetSceneByName(SceneName).isLoaded;
        
        public override string ToString()
        {
            return sceneField.SceneName;
        }
    }
}