using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Modding;
using Kingmaker.ResourceLinks;
using Kingmaker.View;
using Kingmaker.View.Animation;
using Kingmaker.View.Mechanics.Entities;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.Core.Logging;
using Owlcat.Runtime.Visual.FogOfWar;
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
                log.Log("Armoury of the Expanse - Applying conditional patches for PsykersOfTheExpanse");
            }

            if (ModsDependanciesManager.modList?.Where(x => x.Name == "OriginsOfTheExpanse"
                && x.isLoaded == true).Any() ?? false)
            {
                log.Log("Armoury of the Expanse - Applying conditional patches for OriginsOfTheExpanse");

            }

            if (ModsDependanciesManager.modList?.Where(x => x.Name == "DPWeaponAssetPack"
                && x.isLoaded == true).Any() ?? false)
            {
                log.Log("Armoury of the Expanse - Applying conditional patches for DPWeaponAssetPack");
                DPAssetsManager.Apply();
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



}

