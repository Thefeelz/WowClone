using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [SerializeField] List<Buffs> magicBuffs = new List<Buffs>();
    GameObject buffManagerHandler;
    List<Image> buffImagesPlayer = new List<Image>();
    List<Image> buffImagesPlayerFill = new List<Image>();
    List<Image> buffImagesTarget = new List<Image>();
    List<Image> buffImagesTargetFill = new List<Image>();

    BasePlayerStats player;
    BaseFriendlyStats friendly;
    BaseEnemyStats enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BasePlayerStats>();
        friendly = GetComponent<BaseFriendlyStats>();
        enemy = GetComponent<BaseEnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(magicBuffs.Count > 0)
            RunThroughBuffListPlayer();
    }
    public void AddBuffToList(Buffs newBuff) 
    {
        for (int i = 0; i < magicBuffs.Count; i++)
        {
            if (magicBuffs[i].GetBuffID() == newBuff.GetBuffID())
            {
                HandleRemovingBuffs(magicBuffs[i]);
                magicBuffs.RemoveAt(i);
                buffImagesPlayer[i].gameObject.SetActive(false);
            }
        }
        magicBuffs.Add(newBuff);
        newBuff.AddAStackToTheBuff();
        HandleAddingStats(newBuff);
        UpdateBuffImages(newBuff);
    }
    public void SetUpBuffManager(Transform _buffManagerHandler)
    {
        buffManagerHandler = _buffManagerHandler.gameObject;
        foreach (Transform transform in buffManagerHandler.transform)
        {
            buffImagesPlayer.Add(transform.GetComponent<Image>());
            foreach (Transform baby in transform)
            {
                buffImagesPlayerFill.Add(baby.GetComponent<Image>());
            }
            transform.gameObject.SetActive(false);
        }
    }
    void RunThroughBuffListPlayer()
    {
        if (player)
        {
            for (int i = 0; i < magicBuffs.Count; i++)
            {
                magicBuffs[i].DecrementBuffDuration();
                buffImagesPlayerFill[i].fillAmount = magicBuffs[i].GetBuffFillAmount();
                CheckToRemove(magicBuffs[i]);
            }
        }
    }
    void CheckToRemove(Buffs buffToCheck)
    {
        if(buffToCheck.GetBuffCurrentDuration() <= 0)
        {
            magicBuffs.Remove(buffToCheck);
        }
    }
    void UpdateBuffImages(Buffs newBuff)
    {
        int i = 0;
        foreach (Image image in buffImagesPlayer)
        {
            if (!image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(true);
                image.sprite = newBuff.GetBuffImage();
                buffImagesPlayerFill.ToArray()[i].sprite = newBuff.GetBuffImage();
                break;
            }
            i++;
        }
    }
    void HandleAddingStats(Buffs newBuff)
    {
        if (player)
        {
            if (player.GetClassType() == BasePlayerStats.ClassType.Intellect)
                player.GetComponent<MagicUser>().HandleMagicBuffs(newBuff, true);
        }
        else if (friendly)
        {

        }
        else if (enemy)
        {

        }
    }
    void HandleRemovingBuffs(Buffs buffToRemove)
    {
        if (player)
        {
            if (player.GetClassType() == BasePlayerStats.ClassType.Intellect)
                player.GetComponent<MagicUser>().HandleMagicBuffs(buffToRemove, false);
        }
        else if (friendly)
        {

        }
        else if (enemy)
        {

        }
    }
}
