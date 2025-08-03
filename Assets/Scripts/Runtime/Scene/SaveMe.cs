using UnityEngine;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class SaveMe : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
