using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer spriteStart, spriteContinue;

    int indexbutton = 0;
    void Start()
    {
        SetState();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.DownArrow))
        {
            indexbutton = (indexbutton + 1) % 2;
            SetState();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetPlayerperfab();
            switch (indexbutton)
            {
                case 0:
                    PlayerPrefs.SetInt("currentLevel", 0);
                    Application.LoadLevel("Map");
                    break;
                case 1:
                    Application.LoadLevel("Map");
                    break;
            }
        }
    }
    void SetState()
    {
        switch (indexbutton)
        {
            case 0:
                spriteStart.enabled = true;
                spriteContinue.enabled = false;
                break;
            case 1:
                spriteStart.enabled = false;
                spriteContinue.enabled = true;
                break;
        }
    }
    void ResetPlayerperfab()
    {
        PlayerPrefs.SetInt("scoreneed", 10000);
        PlayerPrefs.SetInt("res", 7);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("maxHp", 2);
        PlayerPrefs.SetInt("currentHp", 2);
        PlayerPrefs.SetInt("countItem", 0);
        PlayerPrefs.SetInt("countres", 2);
        PlayerPrefs.SetInt("isRevive", 0);
        PlayerPrefs.SetInt("StateShip", 0);

    }
}
