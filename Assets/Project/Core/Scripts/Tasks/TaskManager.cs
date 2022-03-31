using System.Collections;
using Project.Core.Logger;
using Project.Core.Service;
using UnityEngine;

namespace Project.Core.Tasks
{
    public class TaskManager : MonoBehaviour, ITaskManager
    {
        public CoreLoggerMono logger;
        public static TaskManager Instance;

        void Awake()
        {
            Instance = this;
            ServiceLocator.Instance.TryRegister(this as ITaskManager);
        }

        public TaskState CreateTask(IEnumerator coroutine)
        {
            return new TaskState(coroutine);
        }

        public void PrintStatus()
        {
            logger.Info("Active!");
        }
    }

    public interface ITaskManager: ICoreService
    {
        public TaskState CreateTask(IEnumerator coroutine);
    }
}
/*
 TaskManager.cs

 This is a convenient coroutine API for Unity.

 Example usage:
   IEnumerator MyAwesomeTask()
   {
       while(true) {
           // ...
           yield return null;
/      }
   }

   IEnumerator TaskKiller(float delay, Task t)
   {
       yield return new WaitForSeconds(delay);
       t.Stop();
   }

   // From anywhere
   Task my_task = new Task(MyAwesomeTask());
   new Task(TaskKiller(5, my_task));

 The code above will schedule MyAwesomeTask() and keep it running
 concurrently until either it terminates on its own, or 5 seconds elapses
 and triggers the TaskKiller Task that was created.

 Note that to facilitate this API's behavior, a "TaskManager" GameObject is
 created lazily on first use of the Task API and placed in the scene root
 with the internal TaskManager component attached. All coroutine dispatch
 for Tasks is done through this component.
*/