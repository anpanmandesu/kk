using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tigger : MonoBehaviour
{
    private Collider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        StartCoroutine(StartCoroutineExample());

    }
    IEnumerator StartCoroutineExample()
    {
        yield return new WaitForSeconds(0.2f); // 0.2秒待機
        myCollider.isTrigger = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
