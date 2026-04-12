using BattleUnits;
using UnityEngine;

    public class Archer: RangedUnit
    {
        public override BattleUnitsEnum UnitType => BattleUnitsEnum.Archer;
        
        public override BattleUnitsEnum StrongVS => BattleUnitsEnum.Spearmen;

        public override BattleUnitsEnum WeakVS => BattleUnitsEnum.Cavalry;

    }
