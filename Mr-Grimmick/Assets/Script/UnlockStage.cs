using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockStage : MonoBehaviour
{
    // Start is called before the first frame update
    int stage;
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5)
        {
            PlayerPrefs.SetInt("currentLevel", stage);
            Application.LoadLevel("Map");
        }
    }
    public void SetStage(int stage)
    {
        this.stage = stage;
    }
}
