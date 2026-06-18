using System.Collections.Generic;
using BattleUnits;
using UnityEngine;

public class EnemySpawner2D : Spawner2D
{
    private BattleUnitsEnum PlayerFrontUnit = BattleUnitsEnum.Spearmen;
    private GameObject playerUnitsList;
    [SerializeField] float timerForUnitSpawn;
    protected float timer2;


    void Start()
    {
        playerUnitsList = GameObject.FindWithTag("PlayerTeam");
    }
    void LateUpdate()
    {
        timer2 += Time.deltaTime;

        if(timer2 >= timerForUnitSpawn)
        {
            timer2 = 0f;
            
            if(playerUnitsList.GetComponentAtIndex(0).GetComponentInChildren<BattleUnit>() != null)
            {
                PlayerFrontUnit = playerUnitsList.GetComponentAtIndex(0).GetComponentInChildren<BattleUnit>().UnitType;
            }
            switch (PlayerFrontUnit)
            {
            case BattleUnitsEnum.Archer:
                if (EnemyResourceManager.instance.TryTrainCavalry())
                {
                    EnqueueUnit(BattleUnitsEnum.Cavalry);
                }
            break;
            case BattleUnitsEnum.Cavalry:
                if (EnemyResourceManager.instance.TryTrainSpearman())
                {
                    EnqueueUnit(BattleUnitsEnum.Spearmen);
                }
            break;
            case BattleUnitsEnum.Spearmen:
                if (EnemyResourceManager.instance.TryTrainArcher())
                {
                    EnqueueUnit(BattleUnitsEnum.Archer);
                }
            break;
            
            }
        }
    }
}
