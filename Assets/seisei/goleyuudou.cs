using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goleyuudou : MonoBehaviour
{
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<yuudoufollow2>().listcount();
            if (count >= 1)
            {
                transform.parent.GetComponent<yuudoufollow2>().gole();
            }
        }
    }
    void counting(int c)
    {
        count = c;
    }
}
