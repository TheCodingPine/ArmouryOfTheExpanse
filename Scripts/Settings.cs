using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using ArmouryOfTheExpanse;


namespace ArmouryOfTheExpanse
{
    public class MyModSettings
    {
        public bool loreAccurateLascannon { get; set; } = false;
    }

    public static class ModDataManager
    {
        private static readonly string path = Path.Combine(
            Application.persistentDataPath,
            "Modifications", "ArmouryOTE_settings.json"
        );

        public static MyModSettings Load()
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!File.Exists(path))
                    return new MyModSettings();

                var json = File.ReadAllText(path);
                var settings = JsonConvert.DeserializeObject<MyModSettings>(json);
                return settings; /*?? new MyModSettings();*/ // Fallback to defaults
            }
            catch (Exception ex)
            {
                Main.log.Log($"[ModDataManager] Failed to load settings");
                Debug.LogError($"[ModDataManager] Failed to load settings: {ex.Message}");
                return new MyModSettings();
            }
        }

        public static void Save(MyModSettings settings)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Main.log.Log($"[ModDataManager] ArmouryOTE_settings.json not found, creating setting file");
                }


                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Main.log.Log($"[ModDataManager] Failed to load settings");
                Debug.LogError($"[ModDataManager] Failed to save settings: {ex.Message}");
            }
        }
    }
}