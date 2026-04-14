using ArmouryOfTheExpanse;
using Epic.OnlineServices.Ecom;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Visual.Blueprints;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Visual.Particles;
using System.Collections.Generic;
using UnityEngine;




public static class DPAssetsManager
{
    private static readonly Dictionary<DPAsset, DPGuids> _guids =
        new()
        {
                { DPAsset.DarktideA,
                    new DPGuids(DPGuids.DarktideAAsset(), DPGuids.DarktideAIcon()) },
                { DPAsset.Angelic,
                    new DPGuids(DPGuids.AngelicAsset(), DPGuids.AngelicIcon()) },
                { DPAsset.Aquila,
                    new DPGuids(DPGuids.AquilaAsset(), DPGuids.AquilaIcon()) },
                { DPAsset.Holy,
                    new DPGuids(DPGuids.HolyAsset(), DPGuids.HolyIcon()) },
                { DPAsset.Sunblade,
                    new DPGuids(DPGuids.SunbladeAsset(), DPGuids.SunbladeIcon()) },
                { DPAsset.Runeblade,
                    new DPGuids(DPGuids.RunebladeAsset(), DPGuids.RunebladeIcon()) },
                { DPAsset.Runeblade_On,
                    new DPGuids(DPGuids.Runeblade_OnAsset(), DPGuids.Runeblade_OnIcon()) },
                { DPAsset.Lascannon_Black,
                    new DPGuids(DPGuids.Lascannon_BlackAsset(), DPGuids.Lascannon_BlackIcon()) },
                { DPAsset.Lascannon_Blue,
                    new DPGuids(DPGuids.Lascannon_BlueAsset(), DPGuids.Lascannon_BlueIcon()) },
                { DPAsset.Lascannon_Green,
                    new DPGuids(DPGuids.Lascannon_GreenAsset(), DPGuids.Lascannon_GreenIcon()) },
                { DPAsset.Lascannon_Red,
                    new DPGuids(DPGuids.Lascannon_RedAsset(), DPGuids.Lascannon_RedIcon()) },
                { DPAsset.BoltSniper_SM,
                    new DPGuids(DPGuids.Boltsniper_AstartesAsset(), DPGuids.Boltsniper_AstartesIcon()) },
                { DPAsset.BoltSniper_HM,
                    new DPGuids(DPGuids.Boltsniper_HumanAsset(), DPGuids.Boltsniper_HumanIcon()) },
                { DPAsset.Plasmacannon_Black,
                    new DPGuids(DPGuids.Plasmacannon_BlackAsset(), DPGuids.Plasmacannon_BlackIcon()) },
                { DPAsset.Plasmacannon_Green,
                    new DPGuids(DPGuids.Plasmacannon_GreenAsset(), DPGuids.Plasmacannon_GreenIcon()) },
                { DPAsset.Plasmacannon_Red,
                    new DPGuids(DPGuids.Plasmacannon_RedAsset(), DPGuids.Plasmacannon_RedIcon()) },
                { DPAsset.Plasmacannon_Yellow,
                    new DPGuids(DPGuids.Plasmacannon_YellowAsset(), DPGuids.Plasmacannon_YellowIcon()) },
                { DPAsset.Autocannon_Green,
                    new DPGuids(DPGuids.Autocannon_GreenAsset(), DPGuids.Autocannon_GreenIcon()) },
                { DPAsset.Autocannon_Black,
                    new DPGuids(DPGuids.Autocannon_BlackAsset(), DPGuids.Autocannon_BlackIcon()) },
                { DPAsset.Autocannon_Blue,
                    new DPGuids(DPGuids.Autocannon_BlueAsset(), DPGuids.Autocannon_BlueIcon()) },
                { DPAsset.Autocannon_Red,
                    new DPGuids(DPGuids.Autocannon_RedAsset(), DPGuids.Autocannon_RedIcon()) }
        };

    /// <summary> both asset and icon  </summary>
    public static DPGuids GetGuids(DPAsset type)
        => _guids[type];

    public static void Apply()
    {
        Main.log.Log($"Begin assets switching:");

        List<ArmouryElement> catalogue = ArmouryCatalogue.Get();

        foreach (var catalogueItem in catalogue)
        {
            try
            {
                var item = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(catalogueItem.Guid);
                if (item != null)
                {
                    AssetsSwitching(item, catalogueItem.NewVisual);
                    Main.log.Log($" - jbp {catalogueItem.Guid} is now a {catalogueItem.NewVisual}");
                }
                else
                {
                    Main.log.Log($"[ERROR] jbp {catalogueItem.Guid} not found, can't switch to {catalogueItem.NewVisual}");
                }

            }
            catch (System.Exception)
            {
                Main.log.Log($"[ERROR] jbp {catalogueItem.Guid} crashed while switching to {catalogueItem.NewVisual}");
                throw;
            }

        }

        //ApplyBigLaserVFX();

        ////VFX test
        //var laser = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("40d8a98cd786488c87103c97cbdb8631");
        //Main.log.Log($"Got laser weapon");
        //BlueprintProjectile projectile = laser.WeaponAbilities.Ability2.FXSettings.VisualFXSettings.Projectiles[0];
        //Main.log.Log($"Got the projectile");

        //ProjectileView test = projectile.GetComponent<ProjectileView>();
        //test.ReloadFromInstanceID();
        //Texture2D primary = new Texture2D(1, 1);
        //primary.SetPixel(1, 1, Color.red);
        //projectile.View.LoadAsset();
    }

    private static void AssetsSwitching(BlueprintItemWeapon item, DPAsset newVisual)
    {
        try
        {
            var sprite = AccessTools.FieldRefAccess<BlueprintItemWeapon, UnityEngine.Sprite>("m_Icon");

            sprite(item) = _guids[newVisual].icon ?? sprite(item);

            //access the private asset guid
            var visualParams = AccessTools.FieldRefAccess<BlueprintItemWeapon, WeaponVisualParameters>("m_VisualParameters");
            var model = AccessTools.FieldRefAccess<WeaponVisualParameters, GameObject>("m_WeaponModel");
            model(visualParams(item)) = _guids[newVisual].asset ?? model(visualParams(item));
        }
        catch (System.Exception)
        {
            Main.log.Log($"[ERROR] {newVisual} have no references");
            throw;
        }
    }


    private static void ApplyBigLaserVFX()
    {
        try
        {
            var bigLaser = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityFXSettings>("78e14970df814b71b714f9bd82764abc");
            var targetLaser = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityFXSettings>("6cd13d0464794ec0b1f8746d0a475b96");
            var VisualFXSettings = AccessTools.FieldRefAccess<BlueprintAbilityFXSettings, BlueprintAbilityVisualFXSettings.Reference>("m_VisualFXSettings");
            VisualFXSettings(targetLaser) = VisualFXSettings(bigLaser) ?? VisualFXSettings(targetLaser);
            Main.log.Log($"Switched BigLaser VFX from DP");

        }
        catch (System.Exception)
        {
            Main.log.Log($"[ERROR] BigLaser projectile switching failed");
        }
    }


    //private static void VFXColorizer()
    //{
    //    BlueprintWeaponEnchantment ench = new BlueprintWeaponEnchantment();
    //        //copy of an existing jbp's enchantment on ench
    //    FxFadeOut var = ench.WeaponFxPrefab.GetComponent<FxFadeOut>();
    //    Renderer.Instantiate(var);
    //    Material mat = var.GetComponent<Material>();
    //    Object.Instantiate(mat);
    //    mat.color = Color.red;
    //}


}


public enum DPAsset
{
    //Powerswords
    //1Handed
    DarktideA,
    Angelic,
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
    BoltSniper_HM,
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
    //Autocannon
    Autocannon_Green,
    Autocannon_Black,
    Autocannon_Blue,
    Autocannon_Red,
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

    internal static UnityEngine.Sprite Lascannon_BlueIcon() => IconFetcher("6feb09acb59d4c03aeb8970c7cb49a8f");
    internal static GameObject Lascannon_BlueAsset() => ModelFetcher("6feb09acb59d4c03aeb8970c7cb49a8f");

    internal static UnityEngine.Sprite Lascannon_BlackIcon() => IconFetcher("d4168d1e9bdc40ec99ffc03f1f5bf86c");
    internal static GameObject Lascannon_BlackAsset() => ModelFetcher("d4168d1e9bdc40ec99ffc03f1f5bf86c");

    internal static UnityEngine.Sprite Lascannon_GreenIcon() => IconFetcher("2b34d77a1cdb4309b3b28c2c89622e14");
    internal static GameObject Lascannon_GreenAsset() => ModelFetcher("2b34d77a1cdb4309b3b28c2c89622e14");

    internal static UnityEngine.Sprite Lascannon_RedIcon() => IconFetcher("5aec63a3c36d4059838b3419d32ef565");
    internal static GameObject Lascannon_RedAsset() => ModelFetcher("5aec63a3c36d4059838b3419d32ef565");

    internal static UnityEngine.Sprite Boltsniper_AstartesIcon() => IconFetcher("9128160802b84f5da717b53d2fd2619a");
    internal static GameObject Boltsniper_AstartesAsset() => ModelFetcher("9128160802b84f5da717b53d2fd2619a");

    internal static UnityEngine.Sprite Boltsniper_HumanIcon() => IconFetcher("edf93f4d8723472594a37e73629d8daf");
    internal static GameObject Boltsniper_HumanAsset() => ModelFetcher("edf93f4d8723472594a37e73629d8daf");

    internal static UnityEngine.Sprite Plasmacannon_YellowIcon() => IconFetcher("53130ace69e942c38961d21262d87012");
    internal static GameObject Plasmacannon_YellowAsset() => ModelFetcher("53130ace69e942c38961d21262d87012");

    internal static UnityEngine.Sprite Plasmacannon_RedIcon() => IconFetcher("76108157e1724810b869c2e7924ee17b");
    internal static GameObject Plasmacannon_RedAsset() => ModelFetcher("76108157e1724810b869c2e7924ee17b");

    internal static UnityEngine.Sprite Plasmacannon_GreenIcon() => IconFetcher("599dab57850748c28df8aafd1386016a");
    internal static GameObject Plasmacannon_GreenAsset() => ModelFetcher("599dab57850748c28df8aafd1386016a");

    internal static UnityEngine.Sprite Plasmacannon_BlackIcon() => IconFetcher("f2245fbf2eeb4166a099d41f775bd5c2");
    internal static GameObject Plasmacannon_BlackAsset() => ModelFetcher("f2245fbf2eeb4166a099d41f775bd5c2");

    internal static UnityEngine.Sprite Autocannon_GreenIcon() => IconFetcher("ff9036159320461f99c9929c34857342");
    internal static GameObject Autocannon_GreenAsset() => ModelFetcher("ff9036159320461f99c9929c34857342");

    internal static UnityEngine.Sprite Autocannon_BlackIcon() => IconFetcher("dc25e303d3e7459c81a8e2b5ef0f2981");
    internal static GameObject Autocannon_BlackAsset() => ModelFetcher("dc25e303d3e7459c81a8e2b5ef0f2981");

    internal static UnityEngine.Sprite Autocannon_BlueIcon() => IconFetcher("0f3df98bce3f4da98263fa171b0f610b");
    internal static GameObject Autocannon_BlueAsset() => ModelFetcher("0f3df98bce3f4da98263fa171b0f610b");

    internal static UnityEngine.Sprite Autocannon_RedIcon() => IconFetcher("b2d93f8fe06e4e7a9ab78355d13cfd3b");
    internal static GameObject Autocannon_RedAsset() => ModelFetcher("b2d93f8fe06e4e7a9ab78355d13cfd3b");

    internal static UnityEngine.Sprite AngelicIcon() => IconFetcher("0840082384904430a87009f6a42c6196");
    internal static GameObject AngelicAsset() => ModelFetcher("0840082384904430a87009f6a42c6196");

    private static GameObject ModelFetcher(string guid)
    {
        try
        {
            Main.log.Log($" - Loading jbp {guid} model");
            var runeblade = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(guid);
            var visualParams = AccessTools.FieldRefAccess<BlueprintItemWeapon, WeaponVisualParameters>("m_VisualParameters");
            var model = AccessTools.FieldRefAccess<WeaponVisualParameters, GameObject>("m_WeaponModel");
            return model(visualParams(runeblade));
        }
        catch (System.Exception)
        {
            Main.log.Log($"[ERROR] jbp {guid} model not found, applying empty gameobject");
            Main.log.Log($"[FIX] Can't find the blueprint or load the new weapon model: update DPAssetPack and MicroPatches to the latest version");
            return new GameObject();
        }
    }  

    private static UnityEngine.Sprite IconFetcher(string guid)
    {
        try
        {
            Main.log.Log($" - Loading jbp {guid} icon");
            var runeblade = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(guid);
            //access the private asset guid
            var sprite = AccessTools.FieldRefAccess<BlueprintItemWeapon, UnityEngine.Sprite>("m_Icon");
            return sprite(runeblade);
        }
        catch (System.Exception)
        {
            //failed to fetch DP's blueprint - update Assetpack?
            Main.log.Log($"[ERROR] jbp {guid} icon not found, applying a fallback");
            var runeblade = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("4d4a35e52d564c7ea5462c1c70237aa9"); //swordbasetemplate
            var sprite = AccessTools.FieldRefAccess<BlueprintItemWeapon, UnityEngine.Sprite>("m_Icon");
            return sprite(runeblade);
        }
    }


}
