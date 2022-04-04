using System.Collections.Generic;
using System.Linq;
using Project.Core.Scene;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.ScriptableObjects
{
    /// <summary>
    /// Pretty much a pure data container with information about a scene, root of this all is stored in the SceneLib
    /// Child scenes are things that will be loaded
    /// </summary>
    [CreateAssetMenu(menuName = "SceneLoader/Create SceneData", fileName = "SceneData", order = 1)]
    public class SceneGroup : SerializedScriptableObject
    {
        public List<SceneField> childScenes;
        
        [HideInInspector] public UnityEngine.SceneManagement.Scene scene;

        // Tracking load state of the scene.
        public bool Loaded => childScenes.All(s => s.Loaded);
    }
}