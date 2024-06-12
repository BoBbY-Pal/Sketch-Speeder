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
    
    
    public delegate void MissionCompletedHandler(Mission mission);
    public event MissionCompletedHandler OnMissionCompleted;
    // Method to update progress
    public void UpdateProgress(int progress)
    {
        this.progress = progress;
        if (this.progress >= goal)
        {
            isCompleted = true;
            OnMissionCompleted?.Invoke(this);
        }
        Debug.Log(this.description + " Completed: " + isCompleted + " Progress: " + progress);
    }
    
}