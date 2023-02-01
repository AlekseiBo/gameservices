using System.Collections;
using UnityEngine;
using Framework;
using GameServices;

namespace Scripts
{
    public class SampleServiceReader : MonoBehaviour
    {
        private ISampleService sampleService;

        private IEnumerator Start()
        {
            sampleService = Services.All.Single<ISampleService>();

            yield return Utilities.WaitFor(2f);
            Debug.Log(sampleService.Message);

        }
    }
}