using BattleUnits;
using UnityEngine;

    public class Cavalry:  MeleeUnit
    {
        public override BattleUnitsEnum UnitType => BattleUnitsEnum.Cavalry;
        public override BattleUnitsEnum StrongVS => BattleUnitsEnum.Archer;
        public override BattleUnitsEnum WeakVS => BattleUnitsEnum.Spearmen;
    }