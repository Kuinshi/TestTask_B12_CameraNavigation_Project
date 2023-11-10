using Manager;
using UnityEngine;

namespace Wrapper
{
    /// <summary>
    /// Wrapper class to call JsonManager Singletons Save and Load functions from a button, without actual prefab to prefab dependency.
    /// </summary>
    public class SaveLoadButtonWrapper : MonoBehaviour
    {

        public void Save()
        {
            JsonManager.Instance.SaveSettings();
        }

        public void Load()
        {
            JsonManager.Instance.LoadSettings();
        }
    }
}