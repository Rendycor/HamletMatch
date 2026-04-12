using UnityEngine;
using TMPro;
using HamletMatchCoreDataStructures;
using UnityEngine.UI;

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

    private void Awake()
    {
        SetExchangeRates();
    }

    private void SetExchangeRates()
    {
        woodRateText.text = woodRate.ToString();
        foodRateText.text = foodRate.ToString();
        metalRateText.text = metalRate.ToString();
        stoneRateText.text = stoneRate.ToString();
        
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
    }
}

