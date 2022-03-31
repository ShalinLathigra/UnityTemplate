using System.Collections;
using Project.Core.Logger;

namespace Project.Core.Tasks
{
	/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
	/// It is an error to attempt to start a task that has been stopped or which has
	/// naturally terminated.
	public class CoreTask
	{
		/// Returns true if and only if the coroutine is running.  Paused tasks
		/// are considered to be running.
		public bool Running => _task.Running;

		/// Returns true if and only if the coroutine is currently paused.
		public bool Paused => _task.Paused;

		/// Delegate for termination subscribers.  manual is true if and only if
		/// the coroutine was stopped with an explicit call to Stop().
		public delegate void FinishedHandler(bool manual);
	
		/// Termination event.  Triggered when the coroutine completes execution.
		public event FinishedHandler Finished;

		/// Creates a new Task object for the given coroutine.
		///
		/// If autoStart is true (default) the task is automatically started
		/// upon construction.
		public CoreTask(IEnumerator c, bool autoStart = true)
		{
			_task = TaskManager.Instance.CreateTask(c);
			_task.Finished += TaskFinished;
			if(autoStart)
				Start();
		}
	
		/// Begins execution of the coroutine
		public void Start()
		{
			_task.Start();
		}

		/// Discontinues execution of the coroutine at its next yield.
		public void Stop()
		{
			_task.Stop();
		}
	
		public void Pause()
		{
			_task.Pause();
		}
	
		public void Unpause()
		{
			_task.Unpause();
		}
	
		void TaskFinished(bool manual)
		{
			FinishedHandler handler = Finished;
			handler?.Invoke(manual);
		}

		readonly TaskState _task;
	}
}