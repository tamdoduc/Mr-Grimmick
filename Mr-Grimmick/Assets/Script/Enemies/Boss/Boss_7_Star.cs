using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_7_Star : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D body;
    [SerializeField] Boss_7_Star star;
    Boss_7_Star cloneStar;
    [SerializeField] float groundLim;
    private bool generate = false;
    private float timeCount = 0;
    void Start()
    {
        body.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        int r = Random.Range(1, 3);
        if (r == 1)
            body.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
        else
            body.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > 2f && !generate)
        {
            for (int i = 0; i < 10; i++)
            {
                cloneStar = GameObject.Instantiate(star);
                int rx = Random.Range(5, 30);
                int ry = Random.Range(5, 30);
                cloneStar.SetPos(transform.position + new Vector3(2 + rx / 10, ry / 10, 0));
            }
            generate = true;
        }
        if (transform.position.y < groundLim)
            GameObject.Destroy(this.gameObject);
    }
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
