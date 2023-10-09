using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class tuiju : MonoBehaviour
{
    public GameObject target;
 
    public bool isTouched = false;//ぶつかったかどうかの判定
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 1.5f * Time.deltaTime);

    }
    
    ///<summary>ぶつかったら消える</summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            isTouched = false;
        }

    }
}