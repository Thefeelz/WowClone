using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    UPlayerUI playerUI;
    [SerializeField] int maxStartTalkingRange = 5;
    [SerializeField] Dialogue[] dialogues;
    // Start is called before the first frame update
    void Start()
    {
        playerUI = FindObjectOfType<UPlayerUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Takes a transform and checks to see if it is close enough to interact with a friendly NPC. Returns true if in range, false if not.
    /// </summary>
    /// <param name="transformToCheck"></param>
    /// <returns></returns>
    public bool CheckInRangeToTalk(Transform transformToCheck)
    {
        if(Vector3.Distance(transform.position, transformToCheck.position) <= maxStartTalkingRange)
        {
            transform.LookAt(transformToCheck);
            StartCoroutine(AnimationBoolDelay(transformToCheck.GetComponentInChildren<Animator>(), "talking", 3.5f));
            if(!playerUI.GetDialogueToggle())
                playerUI.ToggleDialogueDisplay();
            FindObjectOfType<DialogueManager>().SetUpDialogueFromNPC(this.GetComponent<BaseFriendlyStats>());
            return true;
        }
        return false;
    }

    public Dialogue[] GetDialogues() { return dialogues; }

    IEnumerator AnimationBoolDelay(Animator playerAnim, string boolName, float delayTime)
    {
        GetComponentInChildren<Animator>().SetBool(boolName, true);
        playerAnim.SetBool(boolName, true);
        yield return new WaitForSeconds(delayTime);
        GetComponentInChildren<Animator>().SetBool(boolName, false);
        playerAnim.SetBool(boolName, false);
    }
}
