using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuiju : MonoBehaviour
{
    public GameObject target;
    private GameObject lasttarget = null;//今助けに来ている誘導員
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

    void start()
    {
    }
    void Update()
    {
        
        if (active == false)
        {
            //速度がTime.deltaTimeを使わないとバグる
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);//誘導員に邪魔にならない速度
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, receivespeed);
        }
        //受け取り場所の近くに来たら中心に動くようにする
        if (Con)
        {
            receivespeed -= 0.01f * Time.deltaTime;
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
        }
        targetx = FindNearestObject();
    }
///<summary>ぶつかったら消える</summary>
void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            a.gameObject.SendMessage("senior", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
            isTouched = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
    void OnCollisionOccurred(bool u)
    {
        if (u)
        {
            //今までに助けられていない誘導員かつ現在追跡していないかどうか
            if (/*other.gameObject != target &&*/ !guide.Contains(targetx))
            {
                target = targetx;
                a = targetx;
            }         
            receivespeed = 1.5f * Time.deltaTime;
            active = true;
        }
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
        Debug.Log(other);
        if (lasttarget != null)
        {
            if (lasttarget.gameObject.tag == "rescue" && other != lasttarget)
                b.Add(other);
            other.gameObject.SendMessage("nowtuiju", this.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
    void me(GameObject other)
    {
        if (lasttarget == null)
        {
            lasttarget = other;
        }
        Debug.Log(lasttarget);
    }
    //受け取り場所についたら
    void lastnull(){
    lasttarget = null;
    }
    //listに前誘導された誘導員がいた場合受け渡し場所にいる高齢者数をマイナス
    void reslist(GameObject las)
    {
        if (guide.Contains(las))
        {
            targetx.gameObject.SendMessage("rescuepointcountloss", SendMessageOptions.DontRequireReceiver);
        }
    }
}