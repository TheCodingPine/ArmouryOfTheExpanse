using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace ArmouryOfTheExpanse
{
    public static class WeaponsOptions
    {



        public static void Lascannon_Patch(bool val)
        {
            if (val)
            {
                Main.log.Log($"Lore Inaccurate Lascannon - patching");
                var lascannonList = new List<BlueprintItemWeapon>();
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("40d8a98cd786488c87103c97cbdb8631")); //Lascannon Act1
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("857411fca50b4d33a3953bf87abd3c97")); //Lascannon Act2
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("10c9c8cf23d24c67af3b8365c0d026d6")); //Lascannon Act2 Hullbreacher
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("e879f2d1d6634aac8326e24a55d49a58")); //Lascannon Act3 Hammer of Mankind
                Main.log.Log($" {lascannonList.Count()} lascannon found");

                foreach (var item in lascannonList)
                {
                    EquipmentRestrictionStat strRequisite = item.ComponentsArray.OfType<EquipmentRestrictionStat>().First();
                    strRequisite.MinValue = 80;
                }

                Main.log.Log($"Lascannon's STR requisite is now 60");

            }
        }

        public static void TauBurst_Patch(bool val)
        {
            //if (val)
            //{
            //    Main.log.Log($"Remove T'au Propaganda - patching");
            //    BlueprintItemWeapon rifle1 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("68cf5d05a72844859ec3ed459927669a"); //Xeno Pulse Rifle | 12-20 Rof2
            //    TweakStats(rifle1, 7, 9);
            //    BlueprintItemWeapon rifle2 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("040ada7657ca4bd9ac2b5ed49fd28f97"); //Pulse Rifle | 22-28 RoF3
            //    TweakStats(rifle2, 9, 11);
            //    BlueprintItemWeapon rifle3 = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("a68df8a0f14542a59aeea2e009e80c0b"); //Shas'ui Pride | 24-34 RoF3 
            //    TweakStats(rifle3, 10, 13);



            //    Main.log.Log($"Nerfed damage and RoF on T'au burst weapons");

            //}
        }

        //private static void TweakStats(BlueprintItemWeapon item, int minDmg, int maxDmg, int? RoF = null)
        //{
        //    EquipmentRestrictionStat strRequisite = item.ComponentsArray.OfType<EquipmentRestrictionStat>().First();
        //    item.WarhammerDamage = minDmg;
        //    item.WarhammerMaxDamage = maxDmg;
        //    if (RoF != null) item.RateOfFire = RoF.Value;
        //}
        

    }

}