using ArmouryOfTheExpanse;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using System.Collections.Generic;
using UnityEngine;




public static class DPAssetsManager
{
    private static readonly Dictionary<DPAsset, DPGuids> _guids =
        new()
        {
                { DPAsset.DarktideA,
                    new DPGuids(DPGuids.DarktideAAsset(), DPGuids.DarktideAIcon()) },
                { DPAsset.Aquila,
                    new DPGuids(DPGuids.AquilaAsset(), DPGuids.AquilaIcon()) },
                { DPAsset.Holy,
                    new DPGuids(DPGuids.HolyAsset(), DPGuids.HolyIcon()) },
                { DPAsset.Sunblade,
                    new DPGuids(DPGuids.SunbladeAsset(), DPGuids.SunbladeIcon()) },
                { DPAsset.Runeblade,
                    new DPGuids(DPGuids.RunebladeAsset(), DPGuids.RunebladeIcon()) },
                { DPAsset.Runeblade_On,
                    new DPGuids(DPGuids.Runeblade_OnAsset(), DPGuids.Runeblade_OnIcon()) }
        };

    /// <summary> both asset and icon  </summary>
    public static DPGuids GetGuids(DPAsset type)
        => _guids[type];

    public static void Apply()
    {
        Main.log.Log($"Armoury of the Expanse - DPAssetsManager.Apply() started switching assets");

        List<ArmouryElement> catalogue = ArmouryCatalogue.Get();

        foreach (var catalogueItem in catalogue)
        {
            try
            {
                var item = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(catalogueItem.Guid);
                if (item != null)
                {
                    AssetsSwitching(item, catalogueItem.NewVisual);
                    Main.log.Log($"Armoury of the Expanse - jbp {catalogueItem.Guid} is now a {catalogueItem.NewVisual}");
                }
                else
                {
                    Main.log.Log($"[ERROR] Armoury of the Expanse - jbp {catalogueItem.Guid} not found, can't switch to {catalogueItem.NewVisual}");
                }

            }
            catch (System.Exception)
            {
                Main.log.Log($"[ERROR] Armoury of the Expanse - jbp {catalogueItem.Guid} crashed while switching to {catalogueItem.NewVisual}");
                throw;
            }

        }
    }

    private static void AssetsSwitching(BlueprintItemWeapon item, DPAsset newVisual)
    {
        var sprite = AccessTools.FieldRefAccess<BlueprintItemWeapon, UnityEngine.Sprite>("m_Icon");
        sprite(item) = _guids[newVisual].icon;

        //access the private asset guid
        var visualParams = AccessTools.FieldRefAccess<BlueprintItemWeapon, WeaponVisualParameters>("m_VisualParameters");
        var model = AccessTools.FieldRefAccess<WeaponVisualParameters, GameObject>("m_WeaponModel");
        model(visualParams(item)) = _guids[newVisual].asset;
    }
}



public enum DPAsset
{
    //Powerswords
    //1Handed
    DarktideA,
    //Forceswords
    //1Handed
    Aquila,
    Holy,
    Sunblade,
    //2Handed
    Runeblade,
    Runeblade_On,

    //Ranged
    //Sniper
    BoltSniper_SM,
    //Lascannon
    Lascannon_Black,
    Lascannon_Blue,
    Lascannon_Green,
    Lascannon_Red,
    //Plasmacannon
    Plasmacannon_Black,
    Plasmacannon_Yellow,
    Plasmacannon_Green,
    Plasmacannon_Red,
}

/// <summary>holding model id and icon id
public class DPGuids
{
    public GameObject asset { get; set; }
    public UnityEngine.Sprite icon { get; set; }
    public DPGuids() { }

    public DPGuids(GameObject asset, UnityEngine.Sprite icon)
    { this.asset = asset; this.icon = icon; }

    #region do we need logging? time will tell
    //internal static UnityEngine.Sprite RunebladeIcon()
    //{
    //    try
    //    {
    //        return IconFetcher("84ecb45f00db4e8db38039e17efcf094");
    //    }
    //    catch (System.Exception)
    //    {
    //        Main.log.Log($"Armoury of the Expanse - RunebladeIcon() crashed");
    //        throw;
    //    }

    //}

    //internal static GameObject RunebladeAsset()
    //{
    //    try
    //    {
    //        return ModelFetcher("84ecb45f00db4e8db38039e17efcf094");
    //    }
    //    catch (System.Exception)
    //    {
    //        Main.log.Log($"Armoury of the Expanse - RunebladeAsset() crashed");
    //        throw;
    //    }
    //}
    #endregion

    internal static UnityEngine.Sprite RunebladeIcon() => IconFetcher("84ecb45f00db4e8db38039e17efcf094");
    internal static GameObject RunebladeAsset() => ModelFetcher("84ecb45f00db4e8db38039e17efcf094");

    internal static UnityEngine.Sprite Runeblade_OnIcon() => IconFetcher("78fcf37ac1bc40fbbe091246b334cdbb");
    internal static GameObject Runeblade_OnAsset() => ModelFetcher("78fcf37ac1bc40fbbe091246b334cdbb");

    internal static UnityEngine.Sprite DarktideAIcon() => IconFetcher("cb1d378f478e409f9b4ade82d2788f6c");
    internal static GameObject DarktideAAsset() => ModelFetcher("cb1d378f478e409f9b4ade82d2788f6c");

    internal static UnityEngine.Sprite AquilaIcon() => IconFetcher("785ad34b5b434a668fd7226fa617d79c");
    internal static GameObject AquilaAsset() => ModelFetcher("785ad34b5b434a668fd7226fa617d79c");

    internal static UnityEngine.Sprite HolyIcon() => IconFetcher("376e974a0587492ab96c2070572a207c");
    internal static GameObject HolyAsset() => ModelFetcher("376e974a0587492ab96c2070572a207c");

    internal static UnityEngine.Sprite SunbladeIcon() => IconFetcher("b2ac51880aa94818800129089eae2135");
    internal static GameObject SunbladeAsset() => ModelFetcher("b2ac51880aa94818800129089eae2135");



    private static GameObject ModelFetcher(string guid)
    {
        try
        {
            var runeblade = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(guid);
            //access the private asset guid
            var visualParams = AccessTools.FieldRefAccess<BlueprintItemWeapon, WeaponVisualParameters>("m_VisualParameters");
            var model = AccessTools.FieldRefAccess<WeaponVisualParameters, GameObject>("m_WeaponModel");
            return model(visualParams(runeblade));
        }
        catch (System.Exception)
        {
            Main.log.Log($"[ERROR] Armoury of the Expanse - jbp {guid} : model not found");
            throw;
        }
    }  

    private static UnityEngine.Sprite IconFetcher(string guid)
    {
        try
        {
            var runeblade = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(guid);
            //access the private asset guid
            var sprite = AccessTools.FieldRefAccess<BlueprintItemWeapon, UnityEngine.Sprite>("m_Icon");
            return sprite(runeblade);
        }
        catch (System.Exception)
        {
            Main.log.Log($"[ERROR] Armoury of the Expanse - jbp {guid} : icon not found");
            throw;
        }
    }


}
