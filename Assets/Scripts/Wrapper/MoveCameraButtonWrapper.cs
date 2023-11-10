using Manager;
using UnityEngine;

namespace Wrapper
{
    /// <summary>
    /// Wrapper class to call Camera Manager's MoveCameraFromCurrentPose function from a Button.
    /// </summary>
    public class MoveCameraButtonWrapper : MonoBehaviour
    {
        public void MoveCameraTo(Transform targetPose)
        {
            CameraManager.Instance.MoveCameraFromCurrentPose(targetPose);
        }
    }
}
