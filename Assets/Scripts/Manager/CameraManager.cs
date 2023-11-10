using DG.Tweening;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Manager Class that handles Camera-related logic.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float travelSpeed = 1;
        
        private Camera _mainCamera;
        private Transform _cachedMainCamTransform;

        private Tween rotationTween;
        private Tween positionTween;

    
        private void Awake()
        {
            // Set-up Singelton (a bit overkill for this project).
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        private void Start()
        {
            CacheMainCam();
        }

        /// <summary>
        /// Cache Main Camera
        /// </summary>
        private void CacheMainCam()
        {
            _mainCamera = Camera.main;
            _cachedMainCamTransform = _mainCamera.transform;
        }

        /// <summary>
        /// Method that moves camera from current pose to target pose.
        /// Unsure if camera-height should have been maintained for this project, or not, decided not to.
        /// </summary>

        private void MoveCamera(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot)
        {
            // Kill current tweens if still in the middle of moving. (More responsive behaviour than only allowing moves, when not tweening)
            if (positionTween != null && positionTween.IsActive())
            {
                positionTween.Kill();
            }

            if (rotationTween != null && rotationTween.IsActive())
            {
                rotationTween.Kill();
            }
            
            // Collision Check & Moving
            float distance = Vector3.Distance(startPos, endPos);
            bool isColliding = ObstacleCheck(startPos, startRot, endPos, endRot, distance);
            
            if (isColliding)
            {
                ObstacleAvoidance();
            }
            else
            {
                MoveDirectly(endPos, endRot, distance);
            }
        }

        /// <summary>
        /// Checks if direct path is blocked.
        /// </summary>
        private bool ObstacleCheck(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, float raycastDistance)
        {
            Ray ray = new Ray(startPos, endPos - startPos);


            return Physics.Raycast(ray, raycastDistance, obstacleLayer);
        }

        /// <summary>
        /// Method that moves camera from current pose to target pose.
        /// </summary>
        public void MoveCameraFromCurrentPose(Transform endPose)
        {
            MoveCamera(_cachedMainCamTransform.position, _cachedMainCamTransform.rotation, endPose.position, endPose.rotation);
        }

        /// <summary>
        /// Tweens Camera from start pose to target pose
        /// </summary>
        private void MoveDirectly(Vector3 endPos, Quaternion endRot, float travelDistance)
        {
            float tweenTime = travelDistance / travelSpeed;
            positionTween = _cachedMainCamTransform.DOMove(endPos, tweenTime).SetEase(Ease.InOutQuad);
            rotationTween = _cachedMainCamTransform.DORotate(endRot.eulerAngles, tweenTime).SetEase(Ease.InOutQuad);
        }

        private void ObstacleAvoidance()
        {
        
        }
    }
}
