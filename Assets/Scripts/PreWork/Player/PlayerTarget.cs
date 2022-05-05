//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerTarget : MonoBehaviour
//{
//    [SerializeField] GameObject playerTarget = null;
//    Camera mainCam;
//    PlayerUI myUI;
//    void Start()
//    {
//        mainCam = GetComponentInChildren<Camera>();
//        myUI = GetComponent<PlayerUI>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        { 
//            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit))
//            {
//                if (hit.transform.parent && hit.transform.parent.gameObject.layer == 3)
//                {
//                    playerTarget = hit.transform.parent.gameObject;
//                    myUI.SetUITarget(playerTarget);
//                } else if (hit.transform.gameObject.layer == 3)
//                {
//                    playerTarget = hit.transform.gameObject;
//                    myUI.SetUITarget(playerTarget);
//                } else
//                {
//                    myUI.ClearTargetFrame();
//                }
//            }
//            else
//            {
//                myUI.ClearTargetFrame();
//            }
//        }
//        if(Input.GetKeyDown(KeyCode.Escape))
//        {
            
//            playerTarget = null;
//            myUI.ClearTargetFrame();
//        }
//    }
//    public GameObject GetPlayerTarget()
//    {
//        return playerTarget;
//    }

//    public bool CheckTargetNotDead()
//    {
//        if (playerTarget != null)
//        {
//            if (playerTarget.GetComponent<EnemyStats>())
//            {
//                if (playerTarget.GetComponent<EnemyStats>().GetCurrentEnemyHealth() > 0)
//                {

//                    return true;
//                } 
//                else
//                {
//                    return false;
//                }
//            }
//        }
//        return false;
//    }
//}
