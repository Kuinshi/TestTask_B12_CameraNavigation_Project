using System.IO;
using Manager;
using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// Implemented saveable Camera Setting class
    /// </summary>
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "SOSettings/CameraSettings", order = 0)]
    public class CameraSettings : ASettings
    {
        public Vector3 worldPos;
        public Vector3 localPos;
        public Vector3 worldRot;
        public Vector3 localRot;
        public Vector3 localScale;

        public float fieldOfView;
        public float nearClippingPLane;
        public float farClippingPlane;
        public int iso;
        public float shutterSpeed;
        public float aperture;
        public float focusDistance;
        public int bladeCount;
        public Vector2 curvature;
        public float barrelClipping;
        public float anamorphism;
        public Vector2 sensorSize;
        public Vector2 lensShift;
        
        /// <summary>
        /// Saves current camera values to scriptable object and json
        /// </summary>
        public override void SaveSetting(string savePath)
        {
            // Save to this Scriptable Object
            Camera camera = CameraManager.Instance.MainCamera;
            Transform cameraTransform = camera.transform;

            worldPos = cameraTransform.position;
            localPos = cameraTransform.localPosition;
            worldRot = cameraTransform.eulerAngles;
            localRot = cameraTransform.localEulerAngles;
            localScale = cameraTransform.localScale;

            fieldOfView = camera.fieldOfView;
            nearClippingPLane = camera.nearClipPlane;
            farClippingPlane = camera.farClipPlane;
            iso = camera.iso;
            shutterSpeed = camera.shutterSpeed;
            aperture = camera.aperture;
            focusDistance = camera.focusDistance;
            bladeCount = camera.bladeCount;
            curvature = camera.curvature;
            barrelClipping = camera.barrelClipping;
            anamorphism = camera.anamorphism;
            sensorSize = camera.sensorSize;
            lensShift = camera.lensShift;
            
            // Save to Json, could delay this to a better point in time, if neccessary.
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(savePath, json);
        }
        
        /// <summary>
        /// Loads saved settings from json into this scriptable object and the applys the loaded values to the camera.
        /// </summary>
        public override void LoadAndApplySetting(string json)
        {
            // Load Setting from json to this Scriptable Object
            JsonUtility.FromJsonOverwrite(json, this);
            
            // Apply new values to actual camera.
            Camera camera = CameraManager.Instance.MainCamera;
            Transform cameraTransform = camera.transform;
            
             cameraTransform.position = worldPos;
             cameraTransform.localPosition = localPos;
             cameraTransform.eulerAngles = worldRot;
             cameraTransform.localEulerAngles = localRot;
             cameraTransform.localScale = localScale;

             camera.fieldOfView = fieldOfView;
             camera.nearClipPlane = nearClippingPLane;
             camera.farClipPlane = farClippingPlane;
             camera.iso = iso;
             camera.shutterSpeed = shutterSpeed;
             camera.aperture = aperture;
             camera.focusDistance = focusDistance;
             camera.bladeCount = bladeCount;
             camera.curvature = curvature;
             camera.barrelClipping = barrelClipping;
             camera.anamorphism = anamorphism;
             camera.sensorSize = sensorSize;
             camera.lensShift = lensShift;
        }
    }
    
}