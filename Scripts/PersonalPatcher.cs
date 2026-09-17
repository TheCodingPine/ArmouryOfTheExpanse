using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Loot;
using System;

namespace ArmouryOfTheExpanse
{
    public static class PersonalPatcher
    {
        static PersonalPatcher() { }

        internal static string AddWeaponToContainer(WeaponLocation instance)
        {
            if (string.IsNullOrEmpty(instance.guidWeapon) || string.IsNullOrEmpty(instance.guidContainer))
            {
                return "[ERROR] Not enough data for weapon to be placed";
            }
            //get container blueprint
            try
            {
                var weapon = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(instance.guidWeapon);
                var container = ResourcesLibrary.TryGetBlueprint<BlueprintLoot>(instance.guidContainer);
                //append weapon
                AddItem(weapon, ref container.Items);
                return weapon.name + " {" + instance.guidWeapon+ "} is now inside " + container.ContainerName + ", a "+ container.Setting +" crate in area " + container.Area.Name;
            }
            catch (Exception)
            {
                return "[ERROR] An error occurred placing " + instance.guidWeapon + " in container "+ instance.guidContainer;
            }

        }

        internal static void AddItem(BlueprintItemWeapon item, ref LootEntry[] Items)
        {
            Array.Resize(ref Items, Items.Length + 1);
            LootEntry newEntry = new LootEntry() {
                Item = item.ToReference<BlueprintItemReference>(),
                Diversity = 0,
                Count = 1,
                ReputationPointsToUnlock = 0, };
            Items[^1] = newEntry;

        }
    }
}