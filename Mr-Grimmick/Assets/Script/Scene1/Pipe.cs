using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3[] pos;

    [SerializeField] bool isActived;
    [SerializeField] bool isUp;

    [SerializeField] int indexPos;
    [SerializeField] MovingGameObject moving;
    MovingGameObject clone;
    List<GameObject> gameObjects = new List<GameObject>();
    void Start()
    {
        isActived = false;
    }
    public bool GetActive()
    {
        return isActived;
    }
    GameObject LastGameObject= null;
    public void Active(GameObject Object , bool isUp)
    {
        if (!CheckObject(Object))
            return;
        Debug.Log("trrrrr");
        clone = GameObject.Instantiate(moving);
        
        if (isUp)
            for (int i=1;i<pos.Length;i++)
            {
                clone.AddPos(pos[i]);
            }
        else
            for (int i = pos.Length-1; i >=0; i--)
            {
                clone.AddPos(pos[i]);
            }
        clone.SetGameObject(Object);
        gameObjects.Add(Object);
        Debug.Log("StartMoving");
        clone.Active();
    }
    public void ResetObject(GameObject @object)
    {
        foreach (GameObject g in gameObjects)
            if (g!=null && g == @object)
            {
                gameObjects.Remove(g);
            }
    }
    bool CheckObject(GameObject @object)
    {
        foreach (GameObject g in gameObjects)
            if (g != null && g == @object)
            {
                return false;
            }
        return true;
    }
}
