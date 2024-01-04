using UnityEngine;

public class SavingSystem : Frolicode.Singleton<SavingSystem>
{
    private const string SaveKey = "SaveData";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            SaveData saveData = new SaveData();

            saveData.hasIAP = false;
            saveData.IAPAt = 0;
            saveData.matchPlayed = 0;
            saveData.matchWin = 0;
            saveData.totalCoin = 1000;
            saveData.vibrationState = true;
            saveData.globalVolume = 1;
            saveData.isFirstMatchTutorialComplete = false;
            saveData.isSecondMatchTutorialComplete = false;
            // AudioManager.Instance?.PlayBackgroundMusic(AudioNames.BGM1.ToString());

            Save(saveData);

            CurrencyManager.Instance.OnDataLoads();
        }
    }

    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

   
    public SaveData Load()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return default(SaveData);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }

}

[System.Serializable]
public struct SaveData
{
    public int matchPlayed;
    public int matchWin;
    public bool hasIAP;
    public int IAPAt;
    public int totalCoin;
    public float globalVolume;
    public bool vibrationState;
    public bool isFirstMatchTutorialComplete;
    public bool isSecondMatchTutorialComplete;
}

