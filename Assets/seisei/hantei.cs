using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hantei : MonoBehaviour
{
    public GameObject kyuujo;
    public bool isTouched = false;//ぶつかったかどうかの判定
    // Start is called before the first frame update
    private LayerMask ignoreLayer;
    void Start()
    {
        ignoreLayer = 4 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    /// <summary>
    ///　視野（polygon)に動けないエージェント(tag==Finish)が入ったとき
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        /*RaycastHit hit;
        Ray ray = new Ray()*/
        Vector2 direction = other.gameObject.transform.position - this.transform.position;
        float distance = direction.magnitude;

        // Raycastを発射して衝突を検出
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 10f, ignoreLayer);

        if (hit.collider != null)
        {
            Debug.Log("障害物が検出されました: " + hit.collider.name);
            // ここに障害物が検出された場合の処理を追加
        }


        if (other.gameObject.tag == "Finish")
        {
            transform.parent.GetComponent<yuudoufollow>().hantei(other.gameObject);
            isTouched = false;
        }

    }
}
