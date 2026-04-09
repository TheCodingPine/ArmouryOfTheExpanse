using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Attributes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
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
using Kingmaker.View;
using Kingmaker.View.Animation;
using Kingmaker.View.Mechanics.Entities;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.Core.Logging;
using Owlcat.Runtime.Visual.FogOfWar;
using StateHasher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace ArmouryOfTheExpanse
{

#if DEBUG
    [EnableReloading]
#endif
    public static class Main
    {

        static Kingmaker.Modding.OwlcatModification mod;
        static Harmony harmony;
        internal static LogChannel log;
        public static GUIStyle headerStyle = null;
        public static GUIStyle panelStyle = null;
        public static Texture2D panelTexture = null;

        [OwlcatModificationEnterPoint]
        public static void Load(Kingmaker.Modding.OwlcatModification entry)
        {
            mod = entry;
            harmony = new(entry.Manifest.UniqueName);
            entry.OnDrawGUI = MyOnGuiMethod;
            log = mod.Logger;

            log.Log("Armoury Of The Expanse: Load");
            harmony.PatchAll();

        }

        [HarmonyPriority(Priority.LowerThanNormal)]
        [HarmonyPatch(typeof(BlueprintsCache))]
        public static class BlueprintsCache_Init_Patch
        {
            public static bool loaded = false;
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            public static void Postfix()
            {
                if (loaded) return;
                loaded = true;
                log.Log("Postfix applied to Init [BlueprintsCache]");
                ModsDependanciesManager.Init(); //check 3rd party mods & logger
                ApplyPatches();
            }
        }

        //---


        static void MyOnGuiMethod()
        {
            InitGUIStyle();

            //vertical div with styles
            GUILayout.BeginVertical(panelStyle);
            GUILayout.Label("Check if mod exist", GUILayout.ExpandWidth(true));


            GUILayout.EndVertical();
        }

        public static void ApplyPatches()
        {
            if (ModsDependanciesManager.modList?.Where(x => x.Name == "PsykersOfTheExpanse"
                && x.isLoaded == true).Any() ?? false) 
            {
                log.Log("Applying conditional patches for PsykersOfTheExpanse");
                log.Log("Patching for PsykersOfTheExpanse complete");
            }

            if (ModsDependanciesManager.modList?.Where(x => x.Name == "OriginsOfTheExpanse"
                && x.isLoaded == true).Any() ?? false)
            {
                log.Log("Applying conditional patches for OriginsOfTheExpanse");
                log.Log("Patching for OriginsOfTheExpanse complete");
            }

            if (ModsDependanciesManager.modList?.Where(x => x.Name == "DPWeaponAssetPack"
                && x.isLoaded == true).Any() ?? false)
            {
                log.Log("Applying conditional patches for DPWeaponAssetPack");
                DPAssetsManager.Apply();
                log.Log("Patching for DPWeaponAssetPack complete");
            }

        }





        //--


        #region Stylesheet my beloved

        public static void Bullet(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("•", GUILayout.Width(15));
            GUILayout.Label(text, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
        }

        public static void InitGUIStyle()
        {
            //onGui draw every frame; I'm trying to cache the header style
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(GUI.skin.label);
                headerStyle.fontSize = 25;
                headerStyle.fontStyle = FontStyle.Bold;
            }
            //same for the vertical box/div behind
            if (panelStyle == null)
            {
                panelTexture = new Texture2D(1, 1);
                panelTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f)); // black, 60% opacity
                panelTexture.Apply();

                panelStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = panelTexture },
                    padding = new RectOffset(12, 12, 12, 12)
                };
            }
        }
        #endregion






















    }

    //[Serializable]
    //[Kingmaker.Blueprints.JsonSystem.Helpers.TypeId("a5f300ea2efc4c8d8f5a03d6ea14a5fe")]
    //[AllowedOn(typeof(BlueprintUnitFact))]
    //[AllowedOn(typeof(BlueprintPlayerUpgrader))]
    //[AllowedOn(typeof(BlueprintAbilityAdditionalEffect))]
    //[AllowMultipleComponents]
    //public class Armoury_EE_Recolorize : UnitFactComponentDelegate, IHashable
    //{
    //    [SerializeField]
    //    private KingmakerEquipmentEntityReference m_EquipmentEntity;

    //    public KingmakerEquipmentEntity EquipmentEntity => m_EquipmentEntity;

    //    public Color color;

    //    protected override void OnActivateOrPostLoad()
    //    {
    //        if (TryGetCharacter(out var resultCharacter))
    //        {
    //            EquipmentEntityLink ee = GetEquipmentLinks().First();

    //            Texture2D primary = new Texture2D(1, 1);
    //            primary.SetPixel(1, 1, color);

    //            ee.Load().PrimaryColorsProfile = ScriptableObject.CreateInstance<CharacterColorsProfile>();
    //            ee.Load().PrimaryColorsProfile.Ramps.Add(primary);


    //            resultCharacter.EquipmentEntitiesForPreload.Add(ee);
    //            resultCharacter.AddEquipmentEntity(ee.Load(), false);

    //            //resultCharacter.EquipmentEntitiesForPreload.Add(EquipmentEntity);
    //            //    EquipmentEntitiesForPreload.Add(eel);
    //            //AddEquipmentEntity(eel.Load(), saved);


    //            //var entity = resultCharacter.AddEquipmentEntities(GetEquipmentLinks());











    //        }
    //    }

    //    protected override void OnDeactivate()
    //    {

    //    }

    //    private bool TryGetCharacter(out Character resultCharacter)
    //    {
    //        resultCharacter = null;
    //        UnitEntityView view = base.Owner.View;
    //        if (view == null)
    //        {
    //            return false;
    //        }

    //        resultCharacter = view.CharacterAvatar;
    //        return resultCharacter != null;
    //    }

    //    private IEnumerable<EquipmentEntityLink> GetEquipmentLinks()
    //    {
    //        Race race = base.Owner.Progression.Race?.RaceId ?? Race.Human;
    //        if (base.Owner?.Gender == Kingmaker.Blueprints.Base.Gender.Male)
    //        {
    //            return EquipmentEntity.m_MaleArray;
    //        }
    //        else
    //        {
    //            return EquipmentEntity.m_FemaleArray;
    //        }
    //    }

    //    protected override void OnViewDidAttach()
    //    {
    //        OnActivateOrPostLoad();
    //    }

    //    public override Hash128 GetHash128()
    //    {
    //        Hash128 result = default(Hash128);
    //        Hash128 val = base.GetHash128();
    //        result.Append(ref val);
    //        return result;
    //    }



    //}

    //class ClassesWithGuid
    //{
    //    public static List<(Type, string)> Classes = new List<(Type, string)>()
    //{
    //    (typeof(Armoury_EE_Recolorize), "a5f300ea2efc4c8d8f5a03d6ea14a5fe")
    //};
    //}

}

