using Kingmaker.Blueprints;
using Kingmaker.Modding;
using System.Collections.Generic;
using System.Linq;
using UnityModManagerNet;

namespace ArmouryOfTheExpanse
{


    /// <summary> Manager for third parts mods used as soft dependancy </summary>
    public static class ModsDependanciesManager
    {
        public static List<SupportedMod> modList = new List<SupportedMod>();

        public static void Init()
        {
            if (modList.Any()) { return; }
            modList.Add(new SupportedMod("PsykersOfTheExpanse", ModType.OwlMod));
            modList.Add(new SupportedMod("OriginsOfTheExpanse", ModType.OwlMod));
            modList.Add(new SupportedMod("DPWeaponAssetPack", ModType.OwlMod));
            modList.Add(new SupportedMod("TestNotThereFakeMod", ModType.OwlMod));


            foreach (var mod in modList)
            {
                mod.isLoaded = IsModEnabled(mod.Name, mod.type);
            }
        }

        #region CascadingDragon's dependency finder 
        public static bool IsModEnabled(string modName, ModType modtype = ModType.UMM)
        {
            Main.log.Log($"Checking for {modName}");
            bool found = false;
            if (modtype == ModType.UMM)
                found = UnityModManager.ModEntries.Where(mod => mod.Info.Id.Equals(modName) && mod.Enabled && !mod.ErrorOnLoading).Any();
            if (modtype == ModType.OwlMod)
                found = OwlcatModificationsManager.Instance.AppliedModifications.Any(x => x.Manifest.UniqueName == modName);
            LogModState(found, modName);
            return found;
        }

        public static void LogModState(bool found, string modName)
        {
            if (!found)
            { Main.log.Log($"Armoury of the Expanse - {modName} not found"); }
            else
            { Main.log.Log($"Armoury of the Expanse - {modName} found"); }
        }
        #endregion

        public class SupportedMod
        {
            public string Name { get; set; }
            public bool? isLoaded { get; set; }
            public ModType type { get; set; }
            public SupportedMod(string name, ModType type = ModType.UMM) { this.Name = name; this.type = type; }
        }

        public enum ModType
        {
            UMM,
            OwlMod
        }

    }






    
}