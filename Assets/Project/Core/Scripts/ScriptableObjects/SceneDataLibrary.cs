using System.Collections.Generic;
using System.Linq;
using Project.Core.Service;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Core.ScriptableObjects
{
    public interface ISceneLibrary : ICoreService
    {
        public bool GetGroup(string targetGroupName, out SceneGroup scene);
    }
    
    /// <summary>
    /// Actual object used to store all the available sceneData SOs in one convenient place.
    /// Will want to create an EditorWindow for this eventually to ease navigation.
    /// Not used by actual game, just by me. 
    /// </summary>
    [CreateAssetMenu(menuName = "SceneLoader/Create SceneLibrary", fileName = "SceneLibrary", order = 0)]
    public class SceneDataLibrary : SerializedScriptableObject, ISceneLibrary
    {
        [SerializeField][ShowInInspector] List<SceneGroup> sceneList;

        public bool GetGroup(string targetGroupName, out SceneGroup scene)
        {
            foreach (SceneGroup s in sceneList.Where(s => s.name == targetGroupName))
            {
                scene = s;
                return true;
            }
            scene = default;
            return false;
        }

        public void PrintStatus()
        {
            Debug.Log("SceneLib containing: " + sceneList.Count);
        }
    }
}