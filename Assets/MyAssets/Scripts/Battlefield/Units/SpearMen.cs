using BattleUnits;
using UnityEngine;

    public class Spearmen: MeleeUnit
    {
        public override BattleUnitsEnum UnitType => BattleUnitsEnum.Spearmen;
        public override BattleUnitsEnum StrongVS => BattleUnitsEnum.Cavalry;
        public override BattleUnitsEnum WeakVS => BattleUnitsEnum.Archer;
    }
