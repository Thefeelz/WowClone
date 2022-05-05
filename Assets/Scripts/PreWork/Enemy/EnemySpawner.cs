//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    [SerializeField] EnemyStats prefab;
//    [SerializeField] EnemyStats[] enemies = new EnemyStats[5];
//    Collider boundingBox;
//    int maxNumberOfEnemies;
//    int activeEnemies = 0;
//    void Start()
//    {
//        maxNumberOfEnemies = enemies.Length;
//        boundingBox = GetComponent<Collider>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(activeEnemies < maxNumberOfEnemies)
//            CheckEnemyFull();
//    }
//    void CheckEnemyFull()
//    {
//        for(int i = 0; i < enemies.Length; i++)
//        {
//            if(!enemies[i])
//            {
//                SpawnEnemy(i);
//            }
//        }
//    }
//    void SpawnEnemy(int index)
//    {
//        float xValue = Random.Range(-boundingBox.bounds.extents.x, boundingBox.bounds.extents.x);
//        float zValue = Random.Range(-boundingBox.bounds.extents.z, boundingBox.bounds.extents.z);
//        EnemyStats newEnemy = Instantiate(prefab, new Vector3(xValue, 0, zValue), Quaternion.identity);
//        enemies[index] = newEnemy;
//        activeEnemies++;
//    }
//}
