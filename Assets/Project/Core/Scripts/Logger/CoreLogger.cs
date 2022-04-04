using System;
using System.Diagnostics;
using Project.Core.Service;

namespace Project.Core.Logger
{
    public interface ICoreLogger : ICoreService
    {
        void Info(object message);
        void Warning(object message);
        void Error(object message);
        void Fatal(object message);
    }
    
    /// <summary>
    /// Class used in some places to define logging parameters in one convenient object
    /// Allows me to 
    /// </summary>
    public class CoreLogger : ICoreLogger
    {
        readonly string _prefix;
        readonly string _separator;
        readonly bool _printCallingFunction;
        readonly bool _verbose;
        readonly bool _active;
        readonly int _separation;
        
        public CoreLogger(
            string prefix, 
            string separator="::", 
            bool printCallingFunction=false, 
            bool verbose=false, 
            bool active=false, 
            int separation=1)
        {
            _prefix = prefix;
            _separator = separator;
            _printCallingFunction = printCallingFunction;
            _verbose = verbose;
            _active = active;
            _separation = separation;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Wrapper for debug.log, will modify message based on other logger parameters
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            if (!_active) return;
            UnityEngine.Debug.Log(ComposeMessage(message));
        }

        /// <summary>
        /// Wrapper for debug.warning, will modify message based on other logger parameters
        /// </summary>
        /// <param name="message"></param>
        public void Warning(object message)
        {
            if (!_active) return;
            UnityEngine.Debug.LogWarning(ComposeMessage(message));
        }

        /// <summary>
        /// Wrapper for debug.error, will modify message based on other logger parameters
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            if (!_active) return;
            UnityEngine.Debug.LogError(ComposeMessage(message));
        }

        public void Fatal(object message)
        {
            UnityEngine.Debug.LogError(ComposeMessage(message));
            UnityEngine.Debug.Break();
        }


        /// <summary>
        /// Composes a message tailored to the logger's parameters
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Composed string</returns>
        object ComposeMessage(object message)
        {
            if (!GetClassAndMethod(out string className, out string funcName))
            {
                className = "NoClass";
                funcName = "NoFunC";
            }

            object ret = _prefix + _separator +
                         (_verbose ? className + _separator : "") +
                         (_printCallingFunction ? funcName + _separator : "") +
                         message;

            return ret;
        }
        /// <summary>
        /// Retrieves the class and function of the caller if possible
        /// </summary>
        /// <param name="className"> output parameter </param>
        /// <param name="funcName"> output parameter </param>
        /// <returns>true if class exists, false otherwise</returns>
        bool GetClassAndMethod(out string className, out string funcName)
        {
            StackTrace stackTrace = new StackTrace();
            Type type = stackTrace.GetFrame(index: _separation + 2).GetMethod().DeclaringType;
            className = type is { } ? type.Name : "";
            funcName = stackTrace.GetFrame(index: _separation + 2).GetMethod().Name;
            return type is { };
        }

        public void PrintStatus()
        {
            Info("Status check");
        }
    }
}