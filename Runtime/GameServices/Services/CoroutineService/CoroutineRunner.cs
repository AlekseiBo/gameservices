using System.Collections;
using UnityEditor;
using UnityEngine;

namespace GameServices
{
    public class CoroutineRunner
    {
        public static bool IsInitialized => runner != null;

        private static MonoBehaviour runner;

        public CoroutineRunner(MonoBehaviour behaviour) => runner = behaviour;

        public static Coroutine Start(IEnumerator coroutine) => runner.StartCoroutine(coroutine);

        public static void Stop(Coroutine coroutine) => runner.StopCoroutine(coroutine);

        public static void StopAll() => runner.StopAllCoroutines();

    }
}