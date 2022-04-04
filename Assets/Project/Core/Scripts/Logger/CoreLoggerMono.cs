using UnityEngine;

namespace Project.Core.Logger
{
    public class CoreLoggerMono : MonoBehaviour, ICoreLogger
    {
        public string prefix;
        public string separator = "::";
        public bool printCallingFunction;
        public bool verbose;
        public bool active;

        CoreLogger _logger;
        
        /// <summary>
        /// Instantiate Logger
        /// </summary>
        void Awake()
        {
            _logger = new CoreLogger(
                prefix, 
                separator,
                printCallingFunction, 
                verbose, 
                active, 
                2);
        }
        /// <summary>
        /// Wrappers for _logger methods
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message) => _logger.Info(message);
        public void Warning(object message) => _logger.Warning(message);
        public void Error(object message) => _logger.Error(message);
        public void Fatal(object message) => _logger.Fatal(message);
        public void PrintStatus()
        {
            Info("Mono Status Checkin");
        }
    }
}