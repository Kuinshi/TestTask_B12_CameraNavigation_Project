using System.Collections.Generic;
using System.IO;
using ScriptableObjects;
using UnityEngine;

namespace Manager
{
    public class JsonManager : MonoBehaviour
    {
        public static JsonManager Instance;

        [SerializeField] private List<ASettings> settings;
        

        private void Awake()
        {
            // Set-up Singleton (a bit overkill for this project).
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        public void SaveSettings()
        {
            foreach (var setting in settings)
            {
                setting.SaveSetting(Application.streamingAssetsPath + "/" + setting.name + ".json");
            }
        }

        public void LoadSettings()
        {
            foreach (var setting in settings)
            {
                string path = Application.streamingAssetsPath + "/" + setting.name + ".json";
                
                if (!File.Exists(path))
                {
                    continue;
                }

                string json = File.ReadAllText(path);
                setting.LoadAndApplySetting(json);
            }
        }
    }
}
