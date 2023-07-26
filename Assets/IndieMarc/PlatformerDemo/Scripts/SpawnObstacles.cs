using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnObstacles : MonoBehaviour
{
   [SerializeField] private GameObject obstacle;

   [SerializeField] private AxisRange xAxis;
   [SerializeField] private AxisRange yAxis;
   [SerializeField] private float timeBetweenSpawn;
   private float _spawnTime;

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
        Instantiate(obstacle, transform.position + new Vector3(randomX, randomY, 0), obstacle.transform.rotation);
    }

    [Serializable]
    struct AxisRange
    {
        public float min;
        public float max;
    }
}