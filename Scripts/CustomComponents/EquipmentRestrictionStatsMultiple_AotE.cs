using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats.Base;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArmouryOfTheExpanse
{

    
	[Kingmaker.Blueprints.JsonSystem.Helpers.TypeId("413af50bdff935f47b1e347650b051da")]
    public class EquipmentRestrictionStatMultiple : EquipmentRestriction
    {

        public StatType Stat;

        public int MinValue;

        [SerializeField]
        private bool m_ChangeRestrictionStatIfHasFact;

        [ShowIf("m_ChangeRestrictionStatIfHasFact")]
        [SerializeField]
        private List<BlueprintUnitFactReference> m_Fact;

        [ShowIf("m_ChangeRestrictionStatIfHasFact")]
        [SerializeField]
        private int m_SubtractionValue;

        public override bool CanBeEquippedBy(MechanicEntity unit)
        {

            if (m_ChangeRestrictionStatIfHasFact && m_Fact.Any(fact => unit.Facts.Contains(fact.Get())))
            {
                return unit.GetStatOptional(Stat)?.ModifiedValue >= MinValue - m_SubtractionValue;
            }

            return unit.GetStatOptional(Stat)?.ModifiedValue >= MinValue;
        }
    }
}

