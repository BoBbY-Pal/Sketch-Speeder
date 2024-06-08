using Sketch_Speeder.Enums;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("Item Details")]
    public int quantity;
    public int cost;
    public PowerUps powerUpType;

    [Header("Item Details")] 
    [SerializeField] private Text costTxt;
    [SerializeField] private Text quantityTxt;
    
    void Start()
    {
        costTxt.text = cost.ToString();
        quantityTxt.text = quantity.ToString();
    }

    // Update is called once per frame
    public void PurchaseItem()
    {
        if (CurrencyManager.Instance.GetCurrency() >= cost)
        {
            CurrencyManager.Instance.DuductCoins(cost, Vector2.zero);
            SavingSystem.Instance.AddPowerUp(quantity);
        }
        else
        {
            Debug.Log("You Don't have enough money.");
        }
    }
}
