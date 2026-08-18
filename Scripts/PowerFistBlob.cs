using HarmonyLib;
using Kingmaker.Code.UI.MVVM.VM.ServiceWindows.Inventory;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.UI.MVVM;
using System;
using UniRx;
using UnityEngine;
using static StbDxtSharp.Error;


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


        //Patch that lock both sets slots for Power Fist, like vanilla Shields
        [HarmonyPatch(typeof(EquipSlotVM), nameof(EquipSlotVM.InitializeSecondSetSecondaryFakeItem))]
        public static class EquipSlotVM_InitializeSecondSetSecondaryFakeItem_Patch_Armoury
        {
            [HarmonyPostfix]
            static void AdditionalCheck(EquipSlotVM __instance, EquipSlotVM secondSetSecondarySlot)
            {
                try
                {
                    Main.log.Log($"Enter");
                    if (secondSetSecondarySlot.HasItem && //if powerfist
                        (secondSetSecondarySlot.Item.Value.Blueprint?.AssetGuid == "f03a8a4cd37b4b88820784419b5ecdfc"|| secondSetSecondarySlot.Item.Value.OriginalBlueprint?.AssetGuid == "f03a8a4cd37b4b88820784419b5ecdfc"))
                    {
                        __instance.SecondSetSecondarySlot = secondSetSecondarySlot;
                        Main.log.Log($"A");
                        var FakeItem = AccessTools.FieldRefAccess<EquipSlotVM, ReactiveProperty<ItemEntity>>("m_FakeItem");
                        FakeItem(__instance) = secondSetSecondarySlot.Item;
                        Main.log.Log($"B");

                        var addDisposable = AccessTools.Method(typeof(BaseDisposable), "AddDisposable");
                        var getIcon = AccessTools.Method(typeof(EquipSlotVM), "GetIcon");
                        Main.log.Log($"C");


                        //reflection fuckery that I refuse to aknowledge
                        addDisposable.Invoke(__instance,
                            new object[]
                            {
                                FakeItem(__instance).Subscribe(delegate
                                {
                                    __instance.Icon.Value = (Sprite)getIcon.Invoke(__instance, null);
                                })
                            }
                        );
                        Main.log.Log($"D");

                        addDisposable.Invoke(__instance,
                        new object[]
                        {
                            FakeItem(__instance).CombineLatest(__instance.Item, (ItemEntity fake, ItemEntity item) => new { fake, item })
                            .Subscribe(value =>
                            {
                                __instance.CanBeFakeItem.Value =
                                    value.fake != null && value.item == null;
                            })});
                    }
                }
                catch (Exception)
                {
                    Main.log.Log($"[ArmouryOfTheExpanse][ERROR] bug duplicating {secondSetSecondarySlot.Item.Value.Name ?? " powerfist"} for second weapon slot");
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

