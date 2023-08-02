using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnObstacles : MonoBehaviour
{
   [SerializeField] private GameObject obstaclePrefab;

   [SerializeField] private AxisRange xAxis;
   [SerializeField] private AxisRange yAxis;
   [SerializeField] private float timeBetweenSpawn;
   private float _spawnTime;
   private ObjectPooler<Obstacle> _objectPooler;
   private void Start()
   {
       _objectPooler = new ObjectPooler<Obstacle>(obstaclePrefab, transform);
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
        Obstacle obstacle = _objectPooler.GetPooledObject();
        obstacle.transform.SetPositionAndRotation(transform.position + new Vector3(randomX, randomY, 0),
                                                    obstacle.transform.rotation);
    }

    [Serializable]
    struct AxisRange
    {
        public float min;
        public float max;
    }
}