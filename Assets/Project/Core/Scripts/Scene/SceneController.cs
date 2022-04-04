using System.Threading.Tasks;
using Project.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Core.Scene
{
    /// <summary>
    /// Optional sceneController, handles playing transitions if assigned.
    /// Can define an intro, an outro, or none.
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class SceneController : MonoBehaviour
    {
        [SerializeField] AnchorSceneGroup origin;
        public PlayableDirector director;
        public PlayableAsset intro;
        public PlayableAsset outro;
        bool _isIntroNotNull;
        bool _isOutroNotNull;

        public bool animating;

        void Start()
        {
            _isIntroNotNull = intro != null;
            _isOutroNotNull = intro != null;
            animating = false;

            origin.Controller = this;
        }

        public async Task PlayIntro()
        {
            if (!_isIntroNotNull) return;
            animating = true;
            director.Play(intro);
            while (director.state != PlayState.Playing)
            {
                await Task.Yield();
            }
            animating = false;
        }

        public async Task PlayOutro()
        {
            if (!_isOutroNotNull) return;
            animating = true;
            director.Play(outro);
            while (director.state != PlayState.Playing)
            {
                await Task.Yield();
            }
            animating = false;
        }
    }
}
