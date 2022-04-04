using System.Collections.Generic;
using Project.Core.Logger;

namespace Project.Core.Service
{
    public interface ICoreService
    {
    }


    public class ServiceLocator
    {
        // custom logger to make logging a bit less annoying
        readonly ICoreLogger _logger;

        ServiceLocator(ICoreLogger logger) => _logger = logger;
     
        /// <summary>
        /// Stores registered services
        /// </summary>
        readonly Dictionary<string, ICoreService> _services = new Dictionary<string, ICoreService>();
        
        /// <summary>
        /// Singleton instance of the Service Locator
        /// </summary>
        public static ServiceLocator Instance { get; private set; }

        /// <summary>
        /// Initialize Instance, only called by bootstrapper.
        /// </summary>
        public static void Init(ICoreLogger logger) => Instance = new ServiceLocator(logger);

        /// <summary>
        /// Tries to get the service of this type.
        /// Use when object initialized, or if only rarely used in lifetime. AFTER AWAKE
        /// avoid using this in update, cause dang.
        /// </summary>
        /// <param name="tService"> Secondary return value for method. Only use if return is true. </param>
        /// <typeparam name="T"> Type of the service to retrieve. </typeparam>
        /// <returns> success of function </returns>
        public bool TryGet<T>(out T tService) where T : ICoreService
        {
            string key = typeof(T).Name;
            if (Instance._services.TryGetValue(key, out ICoreService coreService))
            {
                tService = (T) coreService;
                return true;
            }
            _logger.Error("Key " + key + " Does not exist");
            tService = default;
            return false;
        }

        /// <summary>
        /// Attempt to register a new service to the Service Locator. Only one of each service is allowed to exist.
        /// Generally, services should be registered in the AWAKE method if attached to unity Monobehaviour
        /// </summary>
        /// <param name="service"> Instance of the service to add in </param>
        /// <typeparam name="T"> Type of service being added </typeparam>
        /// <returns> success of function </returns>
        public bool TryRegister<T>(T service) where T : ICoreService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                _logger.Error("Key " + key + " Already present");
                return false;
            }
            _services.Add(key, service);
            return true;
        }

        /// <summary>
        /// Attempt to remove service.
        /// </summary>
        /// <typeparam name="T"> type of function to remove </typeparam>
        /// <returns> success of function </returns>
        public bool TryRemove<T>() where T : ICoreService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key)) return _services.Remove(key);
            _logger.Error("Key " + key + " Does not exist");
            return false;

        }
    }
}