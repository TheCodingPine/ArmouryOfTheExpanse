using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Attributes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Facts.Damage;
using Kingmaker.Designers.Mechanics.Facts.Restrictions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence.Versioning;
using Kingmaker.EntitySystem.Properties;
using Kingmaker.EntitySystem.Properties.Getters;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Modding;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Levelup.Selections;
using Kingmaker.UnitLogic.Levelup.Selections.Feature;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Progression.Features;
using Kingmaker.UnitLogic.Progression.Paths;
using Kingmaker.View;
using Kingmaker.View.Animation;
using Kingmaker.View.Mechadendrites;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.QA.Validation;
using StateHasher.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityModManagerNet;


namespace ArmouryOfTheExpanse
{
    public static class ArmouryCatalogue
    {
        internal static List<ArmouryElement> _catalogue;

        public static List<ArmouryElement> Get()
        {
            try
            {
                if (_catalogue == null)
                {
                    _catalogue = new List<ArmouryElement>
                    {
                        new ArmouryElement("cf3ba0d76633406ba9876633ebf3cfa6", DPAsset.Runeblade), //Mindshard
                        new ArmouryElement("4b697b99238849e6a18a0b9ce9e1a0f8", DPAsset.Aquila), //FrozenVigil
                        new ArmouryElement("3b1cdbdec35f4d249c5d0eaf57778046", DPAsset.DarktideA) //new asset [Kiava Gamma Pattern] Power Sword
                   
                    };
                }
                Main.log.Log($"Armoury of the Expanse - ArmouryCatalogue found");
                return _catalogue;               
            }
            catch (Exception)
            {
                Main.log.Log($"[ERROR] Armoury of the Expanse - ArmouryCatalogue crashed");
                throw;
            }
        }
    }

    public class ArmouryElement
    {
        public string Guid { get; }
        public DPAsset NewVisual { get; }

        public ArmouryElement(string guid, DPAsset name)
        {
            Guid = guid;
            NewVisual = name;
        }
    }


}


//DOCS
// > New Asset by Darth?
//      - add a enum DPAsset entry with the asset name
//      - find the dummy blueprint ID in Darth's mod
//      - add a sprite and asset getter in DPGuids
//      - add the two getters and the enum to DPAssetsManager._guids
// > New blueprint?
//      - add the jbp guid in ArmouryCatalogue with the desired visual enum
