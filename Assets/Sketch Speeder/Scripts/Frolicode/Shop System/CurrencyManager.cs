using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CurrencyManager : Frolicode.Singleton<CurrencyManager>
{
    public event Action<int, int, Vector2> OnCurrencyIncrease;
    public event Action<int, int, Vector2> OncurrencyDecrease;
    public event Action OnCurrencyLoads;
    private int currentBalance;
    private bool hasInitialized = false;

    public Transform parentCoinTransform;

    public TextMeshProUGUI myCoinsTxt;
    private void Start()
    {
        Initialize();
    }

    public void OnDataLoads()
    {
        Initialize();
        OnCurrencyLoads?.Invoke();
    }

    private void Initialize()
    {
        hasInitialized = true;
        SaveData saveData = SavingSystem.Instance.Load();
        currentBalance = saveData.totalCoin;
        myCoinsTxt.text = currentBalance.ToString();
    }

    public int GetCurrency()
    {
        if(!hasInitialized)
        {
            Initialize();
        }
        return currentBalance;
    }

    public void AddCoins(int value, Vector2 buttonPos)
    {
        // AudioManager.Instance.PlaySFX(AudioNames.Coins.ToString());
        currentBalance += value;
        OnCurrencyIncrease?.Invoke(currentBalance, value, buttonPos);
        SaveData();
    }

    public void DuductCoins(int value, Vector2 buttonPos)
    {
        // AudioManager.Instance.PlaySFX(AudioNames.Coins.ToString());
        if (currentBalance <= 0) { return; }
        currentBalance -= value;
        currentBalance = Mathf.Max(0, currentBalance);
        OncurrencyDecrease?.Invoke(currentBalance, value, buttonPos);
        SaveData();
    }
    
    public void PlayCoinCollectAnim(Transform coinTransform)
    {
        // Disable the collider to prevent further interactions
        Collider2D collider = coinTransform.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Use DoTween to animate the coin collection
        coinTransform.DOMove(parentCoinTransform.position, 2.0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => OnCollectionComplete(coinTransform));
        
        coinTransform.DOScale(Vector3.one * 0.2f, 1.0f);
    }

    private void OnCollectionComplete(Transform coinTransform)
    {
        // You can perform additional actions after the collection animation is complete
        // For example, you might want to play a sound, update a score, etc.

        // Destroy the collected coin GameObject (optional)
        Destroy(coinTransform.gameObject);
    }
    private void SaveData()
    {
        SaveData saveData = SavingSystem.Instance.Load();
        saveData.totalCoin = currentBalance;
        SavingSystem.Instance.Save(saveData);
        myCoinsTxt.text = currentBalance.ToString();
    }
}