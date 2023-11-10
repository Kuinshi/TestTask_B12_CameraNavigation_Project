using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// Abstract class for saveable settings as scriptable object.
    /// </summary>
    public abstract class ASettings : ScriptableObject
    {
        public abstract void SaveSetting(string savePath);
        public abstract void LoadAndApplySetting(string json);
    }
}