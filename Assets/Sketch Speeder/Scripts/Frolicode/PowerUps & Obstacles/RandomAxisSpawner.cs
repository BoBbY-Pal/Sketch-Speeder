using System;
using Sketch_Speeder.PowerUps;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAxisSpawner : MonoBehaviour
{
   [SerializeField] private GameObject obstaclePrefab;
   [SerializeField] private GameObject powerUpPrefab;
   [SerializeField] private Transform parentTransform;
   [SerializeField] private AxisRange xAxis;
   [SerializeField] private AxisRange yAxis;
   [SerializeField] private float timeBetweenSpawn;
   private float _spawnTime;
   private ObjectPooler<Obstacle> _obstacleObjectPooler;
   private ObjectPooler<PowerUp> _powerUpsObjectPooler;
   private void Start()
   {
       _powerUpsObjectPooler = new ObjectPooler<PowerUp>(powerUpPrefab, parentTransform);
       _obstacleObjectPooler = new ObjectPooler<Obstacle>(obstaclePrefab, parentTransform);
   }

   void Update()
    {
        if(Time.time > _spawnTime)
        {
            Spawn ();
            _spawnTime = Time.time + timeBetweenSpawn;
        }
    }
    
    void Spawn ()
    {
        float randomX = Random. Range (xAxis.min, xAxis.max);
        float randomY = Random. Range (yAxis.min, yAxis.max);
        // Obstacle obstacle = _obstacleObjectPooler.GetPooledObject();
        // obstacle.transform.SetPositionAndRotation(transform.position + new Vector3(randomX, randomY, 0),
        //                                             obstacle.transform.rotation);
        
        PowerUp powerUp = _powerUpsObjectPooler.GetPooledObject();
        powerUp.transform.SetPositionAndRotation(transform.position + new Vector3(randomX, randomY, 0),
            powerUp.transform.rotation);
    }

    [Serializable]
    struct AxisRange
    {
        public float min;
        public float max;
    }
}