//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LootableObject : MonoBehaviour
//{
//    EnemyStats enemy;
//    PlayerStats player;
//    // Start is called before the first frame update
//    void Start()
//    {
//         enemy = GetComponentInParent<EnemyStats>();
//        player = FindObjectOfType<PlayerStats>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(Vector3.Distance(transform.position, player.transform.position) < 0.5f)
//        {
//            player.GetComponentInParent<Inventory>().ChangeGoldAmount(enemy.GoldToDrop());
//            Destroy(gameObject);
//        }
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Trigger");
//        if (other.transform.CompareTag("Player"))
//        {
//            Debug.Log("Trigger Player");
//            other.transform.GetComponentInParent<Inventory>().ChangeGoldAmount(enemy.GoldToDrop());
//            Destroy(gameObject);
//        }
//    }
//}
