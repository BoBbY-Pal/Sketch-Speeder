using System;
using UnityEngine;

[Serializable]
public class Mission 
{
    public int id;
    public MissionType type;
    public string description;
    public int goal;
    public int progress = 0;
    public bool isCompleted = false;
    
    // Method to update progress
    public void UpdateProgress(int progress)
    {
        this.progress = progress;
        if (this.progress >= goal)
        {
            isCompleted = true;
            
            UiManager.Instance.ActivatePopUp("Mission Completed", description, 0.5f);
        }
        Debug.Log(this.description + " Completed: " + isCompleted + " Progress: " + progress);
    }
    
}