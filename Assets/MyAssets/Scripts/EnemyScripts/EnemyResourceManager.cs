using Unity.VisualScripting;
using UnityEngine;

public class EnemyResourceManager : MonoBehaviour
{
    public static EnemyResourceManager instance;
    public int foodCounter;
    public int metalCounter;
    public int stoneCounter;
    public int woodCounter;
    private float timer = 0f;

    [SerializeField] float timerForOneUpResources;
    [Header("UnitCosts")]
    [Header("Archer")]
    [SerializeField] int archerFoodCost;
    [SerializeField] int archerMetalCost;
    [SerializeField] int archerWoodCost;
    [Header("Spearman")]
    [SerializeField] int SpearmanFoodCost;
    [SerializeField] int SpearmanMetalCost;
    [SerializeField] int SpearmanWoodCost;
    [Header("Cavalry")]
    [SerializeField] int CavalryFoodCost;
    [SerializeField] int CavalryMetalCost;
    [SerializeField] int CavalryWoodCost;




    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timerForOneUpResources)
        {
            timer = 0f;
            foodCounter++;
            metalCounter++;
            stoneCounter++;
            woodCounter++;
        }

    }
    public bool TryTrainArcher()
    {
        if(foodCounter >= archerFoodCost && woodCounter >= archerWoodCost && metalCounter >= archerMetalCost)
        {
            foodCounter -= archerFoodCost;
            woodCounter -= archerWoodCost;
            metalCounter -= archerMetalCost;
            return true;
        }
        return false;
    }
    public bool TryTrainSpearman()
    {
        if(foodCounter >= SpearmanFoodCost && woodCounter >= SpearmanWoodCost && metalCounter >= SpearmanMetalCost)
        {
            foodCounter -= SpearmanFoodCost;
            woodCounter -= SpearmanWoodCost;
            metalCounter -= SpearmanMetalCost;
            return true;
        }
        return false;
    }
    public bool TryTrainCavalry()
    {
        if(foodCounter >= CavalryFoodCost && woodCounter >= CavalryWoodCost && metalCounter >= CavalryMetalCost)
        {
            foodCounter -= CavalryFoodCost;
            woodCounter -= CavalryWoodCost;
            metalCounter -= CavalryMetalCost;
            return true;
        }
        return false;
    }
}
