using System.Collections;

namespace Project.Core.Tasks
{
    public class TaskState
    {
        public bool Running { get; private set; }

        public bool Paused { get; private set; }

        public delegate void FinishedHandler(bool manual);
        public event FinishedHandler Finished;

        readonly IEnumerator _coroutine;
        bool _stopped;
	
        public TaskState(IEnumerator c)
        {
            _coroutine = c;
        }
	
        public void Pause()
        {
            Paused = true;
        }
	
        public void Unpause()
        {
            Paused = false;
        }
	
        public void Start()
        {
            Running = true;
            TaskManager.Instance.StartCoroutine(CallWrapper());
        }
	
        public void Stop()
        {
            _stopped = true;
            Running = false;
        }
	
        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = _coroutine;
            while(Running) {
                if(Paused)
                    yield return null;
                else {
                    if(e != null && e.MoveNext()) {
                        yield return e.Current;
                    }
                    else {
                        Running = false;
                    }
                }
            }
		
            FinishedHandler handler = Finished;
            handler?.Invoke(_stopped);
        }
    }
}