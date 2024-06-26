﻿using System.Collections.Generic;
using System.Linq;
using Sketch_Speeder.Managers;
using Sketch_Speeder.UI;
using Sketch_Speeder.Utils;
using UnityEngine;

namespace Sketch_Speeder.Mission_System
{
    public class MissionManager : Singleton<MissionManager>
    {
        public GameObject missionsParent;
        public MissionUI missionUiPrefab;
        public List<Mission> missions = new List<Mission>();
        public List<MissionUI> missionsUiList = new List<MissionUI>();
        void Start()
        {
            LoadMissions();
        }

        public void AddMission(Mission mission)
        {
            missions.Add(mission);
            SaveMissions();
        }

        public void UpdateMissions(MissionType type, int progress)
        {
            foreach (var mission in missions.Where(m => m.type == type && !m.isCompleted))
            {
                mission.UpdateProgress(progress);
            }
            SaveMissions();
        }

        private void SaveMissions()
        {
            foreach (var mission in missions)
            {
                // PlayerPrefs.SetInt("MissionProgress_" + mission.id, mission.progress);
                PlayerPrefs.SetInt("MissionCompleted_" + mission.id, mission.isCompleted ? 1 : 0);
            }
            PlayerPrefs.Save();
        }

        private void LoadMissions()
        {
            foreach (var mission in missions)
            {
                if (PlayerPrefs.HasKey("MissionCompleted_" + mission.id))
                {
                    // mission.progress = PlayerPrefs.GetInt("MissionProgress_" + mission.id);
                    mission.isCompleted = PlayerPrefs.GetInt("MissionCompleted_" + mission.id) == 1;
                }

                // Subscribe to mission events if it's not been completed.
                if (!mission.isCompleted)
                {
                    SubscribeToMissionEvent(mission);
                }
            }
        }

        public void CreateMissionsList()
        {
            foreach (var mission in missions)
            {
                MissionUI missionUI = Instantiate(missionUiPrefab, missionsParent.transform);
                missionUI.descriptionTxt.text = mission.description;
                missionUI.goalTxt.text = mission.goal.ToString();
                missionUI.progressSlider.maxValue = mission.goal;
                missionsUiList.Add(missionUI);
                if (mission.isCompleted)
                {
                    missionUI.progressTxt.text = mission.goal.ToString();
                    missionUI.progressSlider.value = mission.goal;
                }
            }
        }
        public void DestroyMissionsList()
        {
            foreach (var missionUI in missionsUiList)
            {
                Destroy(missionUI.gameObject);
            }

            missionsUiList.Clear();
        }
    
        // Subscribe to event for a single mission
        private void SubscribeToMissionEvent(Mission mission)
        {
            mission.OnMissionCompleted += HandleMissionCompleted;
        }

        // Handle the mission completed event
        private void HandleMissionCompleted(Mission mission)
        {
            UiManager.Instance.ActivatePopUp("Mission Completed", mission.description, 0.5f);
        }
    }
}