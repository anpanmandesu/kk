using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuiju : MonoBehaviour
{
    public GameObject target;
    private GameObject lasttarget = null;//今助けに来ている誘導員
    private float speedx = 6f;//追跡中の速度
    private float acceleration = 0.1f;//減速加速度（追跡中の速度に沿って大きくなる）
    private GameObject targetx;//
    private GameObject a;//直前に助けてた
    List<GameObject> b = new List<GameObject>();//以前に助けないでと送信したことがある誘導員

    public bool isTouched = false;//ぶつかったかどうかの判定
    public bool active = false;
    List<GameObject> guide = new List<GameObject>();//今までに助けられた誘導員リスト
    private float receivespeed = 0f;//受け渡し場所に来た時に移動する速度
    private float speed = 0f;//普段の速度
    private bool Con = false;//受け取り場所についたら誘導員からtrueを受け取る
    private GameObject receivepoint;

    void Start()
    {

    }
    void Update()
    {
        
        
        //受け取り場所の近くに来たら中心に動くようにする
        if (Con)
        {
            receivespeed -= acceleration * Time.deltaTime;
        }
        if (receivespeed < 0)
        {
            a.gameObject.SendMessage("senior",SendMessageOptions.DontRequireReceiver);
            target = this.gameObject;
            receivespeed = 0;
            Con = false;
            lastnull();
            for (int i = 0; i < b.Count; i++)
            {
                b[i].gameObject.SendMessage("guideclear",this.gameObject, SendMessageOptions.DontRequireReceiver);
            }
            b.Clear();
        }
        targetx = FindNearestObject();
        if (active == false)
        {
            //速度がTime.deltaTimeを使わないとバグる
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);//誘導員に邪魔にならない速度
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, receivespeed);
        }
    }
///<summary>ぶつかったら消える</summary>
void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (a != null)
            {
                a.gameObject.SendMessage("senior", SendMessageOptions.DontRequireReceiver);
            }
            Destroy(this.gameObject);
            isTouched = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
    void OnCollisionOccurred(GameObject other)
    {
            //今までに助けられていない誘導員かつ現在追跡していないかどうか
            if (/*other.gameObject != target &&*/ !guide.Contains(other))
            {
                target = other;
                a = other;
            guide.Add(other);
            }         
            receivespeed = speedx * Time.deltaTime;
            active = true;
        //一般エージェントに追跡
        
    }

    //一番近い誘導員を出す
    GameObject FindNearestObject()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("rescue");
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject targetObject in targetObjects)
        {
            float distance = Vector3.Distance(currentPosition, targetObject.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObject = targetObject;
            }
        }

        return nearestObject;
    }
    void ReceiveCollision(GameObject point)
    {
        if (point.gameObject.tag != "Player")
        {
            Con = true;
        }
        target = point;
    }
    void tui(GameObject other)
    {
        if (lasttarget != null)
        {
            if (other != lasttarget)
                b.Add(other);
            other.gameObject.SendMessage("nowtuiju", this.gameObject, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            lasttarget = other;
        }
    }
    //受け取り場所についたら
    void lastnull(){
    lasttarget = null;
    }
    //listに前誘導された誘導員がいた場合受け渡し場所にいる高齢者数をマイナス
    void reslist(GameObject las)
    {
        if (guide.Exists(obj => obj.CompareTag(las.tag)))
        {
            a.gameObject.SendMessage("rescuepointcountloss", SendMessageOptions.DontRequireReceiver);
        }
    }
    //一般エージェントが出口に到着したら目的地を一時的に出口にする
    void gole(GameObject other)
    {
        target = other;
    }
}