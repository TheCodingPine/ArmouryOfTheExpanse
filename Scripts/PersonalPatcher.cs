using Epic.OnlineServices.Ecom;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Attributes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence.Versioning;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Modding;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Progression.Features;
using Kingmaker.View;
using Kingmaker.View.Animation;
using Kingmaker.View.Mechanics.Entities;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.Core.Logging;
using Owlcat.Runtime.Visual.FogOfWar;
using StateHasher.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using static ak.wwise.core;
using static Kingmaker.Blueprints.Area.FactHolder;
using static UnityModManagerNet.UnityModManager;

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