using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Service;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Core.ScriptableObjects
{
    public interface ISceneLibrary : ICoreService
    {
        public void GetScene(string sceneName, out SceneData scene);
    }
    // Not used at runtime, only in editor. Need to create an editorWindow for these that will let you search names
    [CreateAssetMenu(menuName = "SceneLoader/Create SceneLibrary", fileName = "SceneLibrary", order = 0)]
    public class SceneDataLibrary : SerializedScriptableObject, ISceneLibrary
    {
        [SerializeField][ShowInInspector] List<SceneData> sceneList;

        public void GetScene(string sceneName, out SceneData scene)
        {
            foreach (SceneData s in sceneList.Where(s => s.SceneName == sceneName))
            {
                scene = s;
                return;
            }
            scene = default;
        }

        public void PrintStatus()
        {
            Debug.Log("SceneLib containing: " + sceneList.Count);
        }
    }
}