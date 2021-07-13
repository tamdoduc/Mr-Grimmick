using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HUBManage : MonoBehaviour
{
    enum StateItem
    {
        NULL,
        SHADOW_BOMB, SHADOW_BOTTLE, SHADOW_ENERGYBALL,
        IDLE_BOMB, IDLE_BOTTLE, IDLE, IDLE_ENERGYBALL,
        ACTIVE_BOMB, ACTIVE_BOTTLE, ACTIVE_ENERGYBALL
    }
    [SerializeField] Animator aniMaxHp;
    [SerializeField] Animator aniCurrentHp;
    [SerializeField] Animator[] aniScores;
    [SerializeField] Animator[] aniRes;
    [SerializeField] Animator[] aniItem;

    int score;
    int res;
    int maxHP;
    int currentHP;
    int[] stateItem = new int[3];

    Player player;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i < 3; i++)
        {
            stateItem[i] = 0;
        }
        int score2 = PlayerPrefs.GetInt("score");
        score = score2;
        for (int i = 7; i >= 0; i--)
        {
            aniScores[i].SetInteger("Number", score2 % 10);
            score2 = score2 / 10;
        }
        maxHP = 0;
        currentHP = 0;
    }
    void SetAnimationScore()
    {

        int score2 = PlayerPrefs.GetInt("score");
        if (score2 - score >= 100)
            score += 100;
        else
            score = score2;
        score2 = score;
        for (int i = 7; i >= 0; i--)
        {
            aniScores[i].SetInteger("Number", score2 % 10);
            score2 = score2 / 10;
        }
    }
    void SetAnimationRes()
    {
        int res = PlayerPrefs.GetInt("res");
        for (int i = 1; i >= 0; i--)
        {
            aniRes[i].SetInteger("Number", res % 10);
            res = res / 10;
        }
    }
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.05f)
        {
            if (score != PlayerPrefs.GetInt("score"))
            {
                time = 0;
                if (score >= PlayerPrefs.GetInt("scoreneed"))
                {
                    Debug.Log(PlayerPrefs.GetInt("scoreneed"));
                    PlayerPrefs.SetInt("res", PlayerPrefs.GetInt("res") + 1);
                    PlayerPrefs.SetInt("scoreneed", PlayerPrefs.GetInt("scoreneed")+PlayerPrefs.GetInt("countres")*10000);
                    PlayerPrefs.SetInt("countres", PlayerPrefs.GetInt("countres") + 1);
                }
                SetAnimationScore();
            }
        }
        if (res != PlayerPrefs.GetInt("res"))
        {
            res = PlayerPrefs.GetInt("res");
            SetAnimationRes();
        }
        if (maxHP != PlayerPrefs.GetInt("maxHp"))
        {
            maxHP = PlayerPrefs.GetInt("maxHp");
            aniMaxHp.SetInteger("MaxHp", maxHP);
        }
        if (currentHP != PlayerPrefs.GetInt("currentHp"))
        {
            currentHP = PlayerPrefs.GetInt("currentHp");
            aniCurrentHp.SetInteger("CurrentHp", currentHP);
        }

        for (int i = 0; i < 3; i++)
        {
            if (stateItem[i] != PlayerPrefs.GetInt("item" + i.ToString()))
            {
                stateItem[i] = PlayerPrefs.GetInt("item" + i.ToString());
                aniItem[i].SetInteger("ID", stateItem[i]);
                CheckAnimationItem();
                Debug.Log("changed");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !player.IsHoldingSkill())
            SwitchItem();
        if (stateItem[0] == 0)
            PlayerPrefs.SetInt("IDSkill", 0);
        else
            PlayerPrefs.SetInt("IDSkill", (stateItem[0] - (stateItem[0] - 1) / 3 * 3) - 1);
        if (PlayerPrefs.GetInt("countItem") > 0 && stateItem[0] == 0)
            UsedItem();
        if (Input.GetKeyDown(KeyCode.V) && PlayerPrefs.GetInt("countItem") > 0 && stateItem[0] == 4)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                PlayerPrefs.SetInt("currentHp", PlayerPrefs.GetInt("maxHp"));
                UsedItem();
            }
        }
    }
    void CheckAnimationItem()
    {
        if (stateItem[0] > 0 && stateItem[0] <= 3)
            stateItem[0] += 3;
        if (stateItem[1] > 3)
            stateItem[1] -= (stateItem[1] - 1) / 3 * 3;
        if (stateItem[2] > 3)
            stateItem[2] -= (stateItem[2] - 1) / 3 * 3;

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("item" + i.ToString(), stateItem[i]);
            aniItem[i].SetInteger("ID", stateItem[i]);
        }
    }
    void UsedItem()
    {
        stateItem[0] = stateItem[1];
        stateItem[1] = stateItem[2];
        stateItem[2] = 0;
        PlayerPrefs.SetInt("countItem", PlayerPrefs.GetInt("countItem") - 1);
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("item" + i.ToString(), stateItem[i]);
            aniItem[i].SetInteger("ID", stateItem[i]);
        }
        CheckAnimationItem();
        Debug.Log(PlayerPrefs.GetInt("countItem"));
    }
    void SwitchItem()
    {
        int countItem = PlayerPrefs.GetInt("countItem");
        if (countItem == 0 || countItem == 1)
            return;
        int tg = stateItem[0];
        UsedItem();
        PlayerPrefs.SetInt("countItem", countItem);
        stateItem[countItem - 1] = tg;

        CheckAnimationItem();
    }
}
