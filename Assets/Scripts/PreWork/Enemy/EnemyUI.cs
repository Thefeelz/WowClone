//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class EnemyUI : MonoBehaviour
//{
//    [SerializeField] Image healthBarImage;
//    [SerializeField] Image resourceBarImage;
//    EnemyStats stats;
//    // Start is called before the first frame update
//    void Start()
//    {
//        stats = GetComponent<EnemyStats>();
//        Debug.Log("Started");
//        healthBarImage.fillAmount = stats.GetCurrentEnemyHealth() / stats.GetMaxEnemyHealth();
//        resourceBarImage.fillAmount = stats.GetCurrentEnemyResource() / stats.GetMaxEnemyResource();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        healthBarImage.fillAmount = stats.GetCurrentEnemyHealth() / stats.GetMaxEnemyHealth();
//        resourceBarImage.fillAmount = stats.GetCurrentEnemyResource() / stats.GetMaxEnemyResource();
//    }

//}
