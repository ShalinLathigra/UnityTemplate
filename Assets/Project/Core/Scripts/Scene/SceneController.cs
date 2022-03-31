using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Core.Scene
{
    public class SceneController : MonoBehaviour
    {
        public PlayableDirector director;
        public PlayableAsset intro;
        public PlayableAsset outro;
        
        public void PlayIntro()
        {
            director.Play(intro);
        }

        public void PlayOutro()
        {
            director.Play(outro);
        }
    }
}
