using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuiju : MonoBehaviour
{
    public GameObject target = null;


    public bool isTouched = false;//ぶつかったかどうかの判定
    public bool active = false;
    List<string> guide = new List<string>();//今までに助けられた誘導員リスト
        void Update()
        {
        if (active == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 1.5f * Time.deltaTime);
        }
        
        }
    ///<summary>ぶつかったら消える</summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            isTouched = false;
        }
        //今までに助けられていない誘導員かつ現在追跡していないかどうか(target==nullがないと追跡中もう一度誘導員に接触したら停止してしまう。)
        if (target == null &&guide.Contains(other.gameObject.name))
        {
            target = null;
        }
        else
        {
            if (other.gameObject.tag == "GameController")//誘導員のtagがGamecontroller
            {
                target = other.gameObject;//追跡するターゲットに誘導員を割り当て
                guide.Add(other.gameObject.name);//今までに助けられた誘導員リストに追加
                active = true;//動き出す
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("D");
        //合流地点についたら停止する
        if (other.gameObject.tag == "Confluence")
        {
            active = false;
            target = null;
        }
    }
}