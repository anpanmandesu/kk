using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recevepointhantei2 : MonoBehaviour
{
    private bool x = false;//true‚Ì‚Í‘Oó‚¯“n‚µêŠ‚É‚¢‚é
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
        if (other.gameObject.tag == "Finish")
        {
            if (x)
            {
                other.gameObject.SendMessage("tui", gameObject.transform.parent.gameObject, SendMessageOptions.DontRequireReceiver);
                transform.parent.GetComponent<yuudoufollow2>().hantei(other.gameObject);
            }
        }
    }
    void receveEnter()
    {
        x = true;
    }
    void receveExit()
    {
        x = false;
    }
}
