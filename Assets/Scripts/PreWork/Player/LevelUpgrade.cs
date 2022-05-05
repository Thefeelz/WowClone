//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LevelUpgrade : MonoBehaviour
//{
//    [SerializeField] float healthIncrimentPerLevel;
//    [SerializeField] float resourceIncrimentPerLevel;
//    [SerializeField] float experienceIncrimentPerLevel;
//    [SerializeField] float healthRegenIncrimentPerLevel;
//    PlayerStats playerStats;
//    // Start is called before the first frame update
//    void Start()
//    {
//        playerStats = GetComponent<PlayerStats>();
//    }

//    public void UpdatePlayerStats()
//    {
//        playerStats.SetMaxPlayerHealth(playerStats.GetMaxPlayerHealth() * healthIncrimentPerLevel + playerStats.GetMaxPlayerHealth());
//        playerStats.SetCurrentPlayerHealth(playerStats.GetMaxPlayerHealth());
//        playerStats.SetMaxPlayerResource(playerStats.GetMaxPlayerResource() * resourceIncrimentPerLevel + playerStats.GetMaxPlayerResource());
//        playerStats.SetCurrentPlayerResource(playerStats.GetMaxPlayerResource());
//        playerStats.SetPlayerNeededExperience(Mathf.RoundToInt(playerStats.GetPlayerNeededExperience() * experienceIncrimentPerLevel) + playerStats.GetPlayerNeededExperience());
//        playerStats.SetCurrentPlayerPassiveHealthRegen(playerStats.GetCurrentPlayerPassiveHealthRegen() * healthRegenIncrimentPerLevel + playerStats.GetCurrentPlayerPassiveHealthRegen());
//        playerStats.SetPlayerLevel(playerStats.GetPlayerLevel() + 1);
//    }
//}
