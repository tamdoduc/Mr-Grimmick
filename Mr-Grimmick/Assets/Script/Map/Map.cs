using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer[] point;
    [SerializeField] SpriteRenderer[] enemy;

    [SerializeField] Vector3[] posStarts;
    [SerializeField] string[] scenes;

    [SerializeField] SpriteRenderer FinalLevel;

    int currentLevel = 0;
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        SetCureentLevel();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    currentLevel = (currentLevel+1) % 3;
        //    SetCureentLevel();
        //    Debug.Log(currentLevel);
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    currentLevel = (currentLevel-1) % 3;
        //    SetCureentLevel();
        //    Debug.Log(currentLevel);
        //}
        if (Input.GetKeyDown(KeyCode.Return))
            LoadScene();
    }
    void LoadScene()
    {
        Debug.Log("Enter");
        PlayerPrefs.SetFloat("posXStart", posStarts[currentLevel].x);
        PlayerPrefs.SetFloat("posYStart", posStarts[currentLevel].y);
        PlayerPrefs.SetFloat("posZStart", posStarts[currentLevel].z);
        PlayerPrefs.SetFloat("timeAppear", 2);
        PlayerPrefs.SetInt("isRevive", 0);
        Application.LoadLevel(scenes[currentLevel]);
    }
    void SetCureentLevel()
    {
        switch(currentLevel)
        {
            case 0:
                point[0].enabled = true;
                point[1].enabled = false;
                point[2].enabled = false;
                enemy[0].enabled = false;
                enemy[1].enabled = false;
                FinalLevel.enabled = false;
                break;
            case 1:
                point[0].enabled = false;
                point[1].enabled = true;
                point[2].enabled = false;
                enemy[0].enabled = true;
                enemy[1].enabled = false;
                FinalLevel.enabled = false;
                break;
             case 2:
                point[0].enabled = false;
                point[1].enabled = false;
                point[2].enabled = true;
                enemy[0].enabled = true;
                enemy[1].enabled = true;
                FinalLevel.enabled = true;
                break;
        }    
    }
}
