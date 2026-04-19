using UnityEngine;
using TMPro;
using HamletMatchCoreDataStructures;
using UnityEngine.UI;
using BattleUnits;

public class ResourceCounterManager : MonoBehaviour
{
    [SerializeField] int moneyCounter;
    [SerializeField] int woodCounter;
    [SerializeField] int foodCounter;
    [SerializeField] int metalCounter;
    [SerializeField] int stoneCounter;
    [SerializeField] TMP_Text moneyCounterText;
    [SerializeField] TMP_Text woodCounterText;
    [SerializeField] TMP_Text foodCounterText;
    [SerializeField] TMP_Text metalCounterText;
    [SerializeField] TMP_Text stoneCounterText;
    [Header("PopUpLabels")]
    [SerializeField] TMP_Text moneyPopUpText;
    [SerializeField] TMP_Text woodPopUpText;
    [SerializeField] TMP_Text foodPopUpText;
    [SerializeField] TMP_Text metalPopUpText;
    [SerializeField] TMP_Text stonePopUpText;
    [Header("ExchangeRates")]
    [SerializeField] int woodRate;
    [SerializeField] int foodRate;
    [SerializeField] int metalRate;
    [SerializeField] int stoneRate;
    [SerializeField] TMP_Text woodRateText;
    [SerializeField] TMP_Text foodRateText;
    [SerializeField] TMP_Text metalRateText;
    [SerializeField] TMP_Text stoneRateText;
    [Header("Buttons")]
    [SerializeField] Button buyWoodButton;
    [SerializeField] Button sellWoodButton;
    [SerializeField] Button buyFoodButton;
    [SerializeField] Button sellFoodButton;
    [SerializeField] Button buyMetalButton;
    [SerializeField] Button sellMetalButton;
    [SerializeField] Button buyStoneButton;
    [SerializeField] Button sellStoneButton;
    [Header("Units")]
    [SerializeField] GameObject PlayerUnitSpawner;
    [Header("Archer")]
    [SerializeField] Button TrainArcherButton;
    [SerializeField] int archerFoodCost;
    [SerializeField] TMP_Text archerFoodCostText;
    [SerializeField] int archerMetalCost;
    [SerializeField] TMP_Text archerMetalCostText;
    [SerializeField] int archerWoodCost;
    [SerializeField] TMP_Text archerWoodCostText;
    [Header("Cavalry")]
    [SerializeField] Button TrainCavalryButton;
    [SerializeField] int cavalryFoodCost;
    [SerializeField] TMP_Text cavalryFoodCostText;
    [SerializeField] int cavalryMetalCost;
    [SerializeField] TMP_Text cavalryMetalCostText;
    [SerializeField] int cavalryWoodCost;
    [SerializeField] TMP_Text cavalryWoodCostText;
    [Header("SpearMan")]
    [SerializeField] Button TrainSpearManButton;
    [SerializeField] int spearmanFoodCost;
    [SerializeField] TMP_Text spearmanFoodCostText;
    [SerializeField] int spearmanMetalCost;
    [SerializeField] TMP_Text spearmanMetalCostText;
    [SerializeField] int spearmanWoodCost;
    [SerializeField] TMP_Text spearmanWoodCostText;
    private Spawner2D playerUnitSpawnScript;

    private void Awake()
    {
        playerUnitSpawnScript = PlayerUnitSpawner.GetComponent<Spawner2D>();
        SetExchangeRates();
        SetTrainPrices();
    }

    private void SetExchangeRates()
    {
        woodRateText.text = woodRate.ToString();
        foodRateText.text = foodRate.ToString();
        metalRateText.text = metalRate.ToString();
        stoneRateText.text = stoneRate.ToString();
        
    }
        private void UpdateButtons()
    {
        if(moneyCounter > 0)
        {
            buyFoodButton.interactable = true;
            buyMetalButton.interactable = true;
            buyStoneButton.interactable = true;
            buyWoodButton.interactable = true;

        }
        else
        {
            buyFoodButton.interactable = false;
            buyMetalButton.interactable = false;
            buyStoneButton.interactable = false;
            buyWoodButton.interactable = false;
        }
        sellWoodButton.interactable = woodCounter >= woodRate;
        sellFoodButton.interactable = foodCounter >= foodRate;
        sellMetalButton.interactable = metalCounter >= metalRate;
        sellStoneButton.interactable = stoneCounter >= stoneRate;

        TrainArcherButton.interactable = foodCounter >= archerFoodCost && woodCounter >= archerWoodCost && metalCounter >= archerMetalCost;
        TrainCavalryButton.interactable =foodCounter >= cavalryFoodCost && woodCounter >= cavalryWoodCost && metalCounter >= cavalryMetalCost;
        TrainSpearManButton.interactable = foodCounter >= spearmanFoodCost && woodCounter >= spearmanWoodCost && metalCounter >= spearmanMetalCost;
    }

    public void UpCounterByType(TileType type)
    {
        switch (type)
        {
            case TileType.Money:
                moneyCounter++;
                moneyCounterText.text = moneyCounter.ToString();
                NumberChangePopUp.Instance.ShowMessage(1, moneyPopUpText);
                break;
            case TileType.Wood:
                woodCounter++;
                woodCounterText.text = woodCounter.ToString();
                NumberChangePopUp.Instance.ShowMessage(1, woodPopUpText);
                break;
            case TileType.Food:
                foodCounter++;
                foodCounterText.text = foodCounter.ToString();
                NumberChangePopUp.Instance.ShowMessage(1, foodPopUpText);
                break;
            case TileType.Metal:
                metalCounter++;
                metalCounterText.text = metalCounter.ToString();
                NumberChangePopUp.Instance.ShowMessage(1, metalPopUpText);
                break;
            case TileType.Stone:
                stoneCounter++;
                stoneCounterText.text = stoneCounter.ToString();
                NumberChangePopUp.Instance.ShowMessage(1, stonePopUpText);
                break;
            default:
                break;
        }
        UpdateButtons();
    }
    #region ExchangeButtons Buy/Sell
    public void BuyWood()
    {
        if(moneyCounter > 0)
        {
            moneyCounter--;
            NumberChangePopUp.Instance.ShowMessage(-1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            woodCounter += woodRate;
            NumberChangePopUp.Instance.ShowMessage(woodRate, woodPopUpText);
            woodCounterText.text = woodCounter.ToString();
        }
        UpdateButtons();
    }
    public void SellWood()
    {
        if(woodCounter >= woodRate)
        {
            moneyCounter++;
            NumberChangePopUp.Instance.ShowMessage(1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            woodCounter -= woodRate;
            NumberChangePopUp.Instance.ShowMessage(-woodRate, woodPopUpText);
            woodCounterText.text = woodCounter.ToString();
        }
        UpdateButtons();
    }
    public void BuyFood()
    {
        if(moneyCounter > 0)
        {
            moneyCounter--;
            NumberChangePopUp.Instance.ShowMessage(-1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            foodCounter += foodRate;
            NumberChangePopUp.Instance.ShowMessage(foodRate, foodPopUpText);
            foodCounterText.text = foodCounter.ToString();
        }
        UpdateButtons();
    }
    public void SellFood()
    {
        if(foodCounter >= foodRate)
        {
            moneyCounter++;
            NumberChangePopUp.Instance.ShowMessage(1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            foodCounter -= foodRate;
            NumberChangePopUp.Instance.ShowMessage(-foodRate, foodPopUpText);
            foodCounterText.text = foodCounter.ToString();
        }
        UpdateButtons();
    }
    public void BuyMetal()
    {
        if(moneyCounter > 0)
        {
            moneyCounter--;
            NumberChangePopUp.Instance.ShowMessage(-1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            metalCounter += metalRate;
            NumberChangePopUp.Instance.ShowMessage(metalRate, metalPopUpText);
            metalCounterText.text = metalCounter.ToString();
        }
        UpdateButtons();
    }
    public void SellMetal()
    {
        if(metalCounter >= metalRate)
        {
            moneyCounter++;
            NumberChangePopUp.Instance.ShowMessage(1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            metalCounter -= metalRate;
            NumberChangePopUp.Instance.ShowMessage(-metalRate, metalPopUpText);
            metalCounterText.text = metalCounter.ToString();
        }
        UpdateButtons();
    }
    public void BuyStone()
    {
        if(moneyCounter > 0)
        {
            moneyCounter--;
            NumberChangePopUp.Instance.ShowMessage(-1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            stoneCounter += stoneRate;
            NumberChangePopUp.Instance.ShowMessage(stoneRate, stonePopUpText);
            stoneCounterText.text = stoneCounter.ToString();
        }
        UpdateButtons();
    }
    public void SellStone()
    {
        if(stoneCounter >= stoneRate)
        {
            moneyCounter++;
            NumberChangePopUp.Instance.ShowMessage(1, moneyPopUpText);
            moneyCounterText.text = moneyCounter.ToString();
            stoneCounter -= stoneRate;
            NumberChangePopUp.Instance.ShowMessage(-stoneRate, stonePopUpText);
            stoneCounterText.text = stoneCounter.ToString();
        }
        UpdateButtons();
    }
    #endregion
    #region TrainButtons
    private void SetTrainPrices()
    {
        archerFoodCostText.text = archerFoodCost.ToString();
        archerMetalCostText.text = archerMetalCost.ToString();
        archerWoodCostText.text = archerWoodCost.ToString();
        cavalryFoodCostText.text = cavalryFoodCost.ToString();
        cavalryMetalCostText.text = cavalryMetalCost.ToString();
        cavalryWoodCostText.text = cavalryWoodCost.ToString();
        spearmanFoodCostText.text = spearmanFoodCost.ToString();
        spearmanMetalCostText.text = spearmanMetalCost.ToString();
        spearmanWoodCostText.text = spearmanWoodCost.ToString();
    }
    public void TrainArcher()
    {
        if(foodCounter >= archerFoodCost && woodCounter >= archerWoodCost && metalCounter >= archerMetalCost)
        {
            foodCounter -= archerFoodCost;
            NumberChangePopUp.Instance.ShowMessage(-archerFoodCost, foodPopUpText);
            foodCounterText.text = foodCounter.ToString();

            woodCounter -= archerWoodCost;
            NumberChangePopUp.Instance.ShowMessage(-archerWoodCost, woodPopUpText);
            woodCounterText.text = woodCounter.ToString();

            metalCounter -= archerMetalCost;
            NumberChangePopUp.Instance.ShowMessage(-archerMetalCost, metalPopUpText);
            metalCounterText.text = metalCounter.ToString();

            playerUnitSpawnScript.EnqueueUnit(BattleUnitsEnum.Archer);
        }
        UpdateButtons();
    }
        public void TrainCavalry()
    {
        if(foodCounter >= cavalryFoodCost && woodCounter >= cavalryWoodCost && metalCounter >= cavalryMetalCost)
        {
            foodCounter -= cavalryFoodCost;
            NumberChangePopUp.Instance.ShowMessage(-cavalryFoodCost, foodPopUpText);
            foodCounterText.text = foodCounter.ToString();

            woodCounter -= cavalryWoodCost;
            NumberChangePopUp.Instance.ShowMessage(-cavalryWoodCost, woodPopUpText);
            woodCounterText.text = woodCounter.ToString();

            metalCounter -= cavalryMetalCost;
            NumberChangePopUp.Instance.ShowMessage(-cavalryMetalCost, metalPopUpText);
            metalCounterText.text = metalCounter.ToString();

            playerUnitSpawnScript.EnqueueUnit(BattleUnitsEnum.Cavalry);
        }
        UpdateButtons();
    }
        public void TrainSpearman()
    {
        if(foodCounter >= spearmanFoodCost && woodCounter >= spearmanWoodCost && metalCounter >= spearmanMetalCost)
        {
            foodCounter -= spearmanFoodCost;
            NumberChangePopUp.Instance.ShowMessage(-spearmanFoodCost, foodPopUpText);
            foodCounterText.text = foodCounter.ToString();

            woodCounter -= spearmanWoodCost;
            NumberChangePopUp.Instance.ShowMessage(-spearmanWoodCost, woodPopUpText);
            woodCounterText.text = woodCounter.ToString();

            metalCounter -= spearmanMetalCost;
            NumberChangePopUp.Instance.ShowMessage(-spearmanMetalCost, metalPopUpText);
            metalCounterText.text = metalCounter.ToString();

            playerUnitSpawnScript.EnqueueUnit(BattleUnitsEnum.Spearmen);
        }
        UpdateButtons();
    }
    #endregion
}

