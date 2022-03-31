using Project.Core.Scene;
using UnityEngine;

namespace Project.Core.Service
{
    public static class ServiceBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            ServiceLocator.Init(
                new Logger.CoreLogger(
                    "SrvLoc",
                    printCallingFunction:true,
                    verbose:true, 
                    active:true));

            ServiceLocator.Instance.TryRegister(
                new SceneLoader(
                    new Logger.CoreLogger(
                        "ScnLod",
                        printCallingFunction:false,
                        verbose:false, 
                        active:true)) as ISceneLoader);
        }
    }
}