using Sketch_Speeder.Enums;
using Sketch_Speeder.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("Item Details")]
    public int quantity;
    public int cost;
    [FormerlySerializedAs("powerUpTypeType")] [FormerlySerializedAs("powerUpType")] public PowerUpsType powerUpsTypeType;

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
            UiManager.Instance.ActivatePopUp("Transaction Completed", $"{powerUpsTypeType.ToString()} PowerUp : {quantity} \n Cost : {cost}", 0);
        }
        else
        {
            UiManager.Instance.ActivatePopUp("Transaction Failed", "You Don't have enough money.", 0);
        }
    }
}
