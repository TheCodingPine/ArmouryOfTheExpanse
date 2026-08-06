using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.Modding;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityModManagerNet;

namespace ArmouryOfTheExpanse
{
    public static partial class Main
    {
        /*
         The Powerfist is an EE with enlarged bones for L_ForeArm and L_Hand
            - Limited to Left Hand 
                            -> HandSlot.IsItemSupported patch that disable the right hand slot
            - Always visible over every other EEs (big gauntlet go brrrr)
                            -> AugmentationBodyPartReplacer.CutVerticesByBonePrefix patch that exclude it from the augment cut
         */


        //Patch that limit the powerfist to the Secondary Hand
        [HarmonyPatch(typeof(HandSlot), nameof(HandSlot.IsItemSupported))]
        public static class ItemEntityWeapon_HoldInTwoHands_Patch_Armoury
        {
            [HarmonyPostfix]
            static void AdditionalCheck(ref bool __result, HandSlot __instance, ItemEntityWeapon item)
            {
                try
                {
                    //already locked, not weapon or other slot
                    if (__result == false ||
                        item == null ||
                        !(item is ItemEntityWeapon) ||
                        !__instance.IsPrimaryHand)
                    {
                        return;
                    }

                    if (item.Blueprint?.AssetGuid == "f03a8a4cd37b4b88820784419b5ecdfc"
                        || item.OriginalBlueprint?.AssetGuid == "f03a8a4cd37b4b88820784419b5ecdfc")
                    {
                        __result = false;
                    }

                }
                catch (Exception)
                {
                    Main.log.Log($"[ArmouryOfTheExpanse][ERROR] bug loading {item.Name ?? " powerfist"} while disabling right handslot");
                }

            }
        }

        //ADDB's "don't cut my PowerFist if the character have an Augment"
        [HarmonyPatch(typeof(AugmentationBodyPartReplacer), nameof(AugmentationBodyPartReplacer.CutVerticesByBonePrefix))]
        private static class PreservePowerFistPatch
        {
            [HarmonyPrefix]
            private static bool Prefix(EquipmentEntity equipmentEntity, string bonePrefix, ref GameObject __result)
            {
                if ((equipmentEntity?.name != "EE_PowerFist_M_HM_AotE"  //male
                  && equipmentEntity?.name != "EE_PowerFist_F_HM_AotE") //female
                    || bonePrefix != "L_")
                {
                    return true;
                }

                __result = null;
                return false;
            }
        }
    }
}

