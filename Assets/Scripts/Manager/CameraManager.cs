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
        
        public Camera MainCamera => _mainCamera;
        private Camera _mainCamera;
        
        private Transform _cachedMainCamTransform;

        private Tween _rotationTween;
        private Tween _positionTween;

    
        private void Awake()
        {
            // Set-up Singleton (a bit overkill for this project).
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

        private void MoveCamera(Vector3 startPos, Vector3 endPos, Quaternion endRot)
        {
            // Kill current tweens if still in the middle of moving. (More responsive behaviour than only allowing moves, when not tweening)
            if (_positionTween != null && _positionTween.IsActive())
            {
                _positionTween.Kill();
            }

            if (_rotationTween != null && _rotationTween.IsActive())
            {
                _rotationTween.Kill();
            }
            
            // Collision Check & Moving
            Vector3 rayDirection = endPos - startPos;
            float distance = Vector3.Distance(startPos, endPos);

            bool isColliding = ObstacleCheck(startPos, rayDirection, distance, out var hitColliderBounds);
            
            if (isColliding)
            {
                ObstacleAvoidance(startPos, endPos, endRot, hitColliderBounds, rayDirection);
            }
            else
            {
                MoveDirectly(endPos, endRot, distance);
            }
        }

        /// <summary>
        /// Checks if direct path is blocked.
        /// </summary>
        private bool ObstacleCheck(Vector3 startPos, Vector3 rayDirection, float raycastDistance, out Bounds hitColliderBounds)
        {
            Ray ray = new Ray(startPos, rayDirection);
            RaycastHit hit;
            bool blocked = Physics.Raycast(ray, out hit, raycastDistance, obstacleLayer);

            if (blocked)
            {
                hitColliderBounds = hit.collider.bounds;
            }
            else
            {
                hitColliderBounds = new Bounds();
            }

            return blocked;
        }

        /// <summary>
        /// Method that moves camera from current pose to target pose.
        /// </summary>
        public void MoveCameraFromCurrentPose(Transform endPose)
        {
            MoveCamera(_cachedMainCamTransform.position, endPose.position, endPose.rotation);
        }

        /// <summary>
        /// Tweens Camera from start pose to target pose.
        /// </summary>
        private void MoveDirectly(Vector3 endPos, Quaternion endRot, float travelDistance)
        {
            float tweenTime = travelDistance / travelSpeed;
            _positionTween = _cachedMainCamTransform.DOMove(endPos, tweenTime).SetEase(Ease.InOutQuad);
            _rotationTween = _cachedMainCamTransform.DORotate(endRot.eulerAngles, tweenTime).SetEase(Ease.InOutQuad);
        }

        /// <summary>
        /// Calculates a curved camera path around the hit obstacle and moves tweens camera along that path.
        /// </summary>
        private void ObstacleAvoidance(Vector3 startPos, Vector3 endPos, Quaternion endRot, Bounds hitColliderBounds, Vector3 rayDirection)
        {
            // Calculate a third point for the curve, based on the collider center and bounds
            float safeDistance = hitColliderBounds.extents.x;
            
            if (hitColliderBounds.extents.y > safeDistance)
            {
                safeDistance = hitColliderBounds.extents.y;
            }
            
            if (hitColliderBounds.extents.z > safeDistance)
            {
                safeDistance = hitColliderBounds.extents.z;
            }

            safeDistance *= 2;
            Vector3 perpendicularVector = new Vector3(-rayDirection.z, 0, rayDirection.x).normalized; // Vector that is perpendicular to ray on the xz-plane
            Vector3 thirdPoint = hitColliderBounds.center + perpendicularVector * safeDistance;

            Vector3[] path = {startPos, thirdPoint, endPos};
            // NOTE!!! At this point I would use the Tweening or Spline Libraries path-length calculation. However, DOTweenPath is part of DOTween Pro, which I do not own.
            // I do not want to rework this skill demo project with another tweening library from scratch now, instead I chose to approximate the distance naively. I would obviously not do so in a proper project.
            float naiveDistance = Vector3.Distance(startPos, thirdPoint) + Vector3.Distance(thirdPoint, endPos);
            float tweenTime = naiveDistance / travelSpeed;
            
            _positionTween = _cachedMainCamTransform.DOPath(path, tweenTime, PathType.CatmullRom).SetEase(Ease.InOutQuad);
            _rotationTween = _cachedMainCamTransform.DORotate(endRot.eulerAngles, tweenTime).SetEase(Ease.InOutQuad);
        }
    }
}
