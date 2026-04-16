using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Weapons;
using System.Collections.Generic;
using System.Linq;

namespace ArmouryOfTheExpanse
{
    public static class WeaponsOptions
    {



        public static void Lascannon_Patch(bool val)
        {
            if (val)
            {
                Main.log.Log($"Lore Accurate Lascannon - patching");
                var lascannonList = new List<BlueprintItemWeapon>();
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("40d8a98cd786488c87103c97cbdb8631")); //Lascannon Act1
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("857411fca50b4d33a3953bf87abd3c97")); //Lascannon Act2
                lascannonList.Add(ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("e879f2d1d6634aac8326e24a55d49a58")); //Lascannon Act3
                Main.log.Log($" {lascannonList.Count()} lascannon found");

                foreach (var item in lascannonList)
                {
                    EquipmentRestrictionStat strRequisite = item.ComponentsArray.OfType<EquipmentRestrictionStat>().First();
                    strRequisite.MinValue = 80;
                }

                Main.log.Log($"Lascannon's STR requisite is now 80");

            }
        }

    }

}