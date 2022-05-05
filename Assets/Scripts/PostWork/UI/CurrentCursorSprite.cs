using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCursorSprite : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexDefault, cursorTexEnemy, cursorTexEnemyAttackable, cursorTexLoot, cursorTexFriendly, cursorTexQuest;
    // Start is called before the first frame update
    Vector2 hotspot = new Vector2(24, 24);
    void Start()
    {
        SetCursorToDefault();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCursorToAttackableEnemy()
    {
        Cursor.SetCursor(cursorTexEnemyAttackable, hotspot, CursorMode.ForceSoftware);
    }
    public void SetCursorToLootable()
    {
        Cursor.SetCursor(cursorTexLoot, hotspot, CursorMode.ForceSoftware);
    }
    public void SetCursorToFriendly()
    {
        Cursor.SetCursor(cursorTexFriendly, hotspot, CursorMode.ForceSoftware);
    }
    public void SetCursorToQuestGiver()
    {
        Cursor.SetCursor(cursorTexQuest, hotspot, CursorMode.ForceSoftware);
    }
    public void SetCursorToDefault()
    {
        Cursor.SetCursor(cursorTexDefault, hotspot, CursorMode.ForceSoftware);
    }
}
