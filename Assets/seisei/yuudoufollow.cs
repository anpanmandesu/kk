using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class yuudoufollow : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
    public float stoppingDistance = 0; 
    public bool isTouched = false;//ぶつかったかどうかの判定
    float kakudo = -90f;
    public bool force = true;
    int j = 0; //cornerたどるたび増える
    int k = 0;//poligonとぶつかったら

    private Rigidbody2D rb;//回転のrb

    [HideInInspector]//常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;

    yuudouin agent; //NavMeshAgent2Dを使用するための変数
    public GameObject siya;
    Vector3[] Pathcorners = new Vector3[100];

    [SerializeField] Transform target; //追跡するターゲット
    //子から受け取るゲームオブジェクト
    public GameObject kyuujosha = null;
    public Transform[] waypoints;
    List<string> rescue = new List<string>();//今までに助けたエージェントのリスト


    Vector3 lastPos;

    void Start()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        Pathcorners = path.corners;

        //移動開始
        //MoveToWaypoint(waypoints[k]);

        //回転のrb
        rb = GetComponent<Rigidbody2D>();

        lastPos = transform.position;
    }
void Update()
    {
        if (k >= 0)
        {
            //現在の目標値への距離を計算
            float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[k].position);

            if (distanceToWaypoint < 0.5f)
            {
                k++;
            }
            Trace(transform.position, waypoints[k].transform.position);
        }
        if (k < 0)
        {
            Trace(transform.position, kyuujosha.transform.position);
        }
        
        Debug.Log(k);

        ///<summary>以下回転</summary>

        Vector2 velocity =(Vector2) (transform.position - lastPos);
        lastPos = transform.position;

        Debug.Log(velocity);
        if (velocity != Vector2.zero)
        {
            Debug.Log("C");
            // 速度ベクトルから角度を計算（度数法）
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // オブジェクトを回転
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }



    }
    private void Trace(Vector2 current, Vector2 target)
    {
        if (Vector2.Distance(current, target) <= stoppingDistance)
        {
            return;
        }

        // NavMesh に応じて経路を求める
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

        Vector2 corner = path.corners[0];

        if (Vector2.Distance(current, corner) <= 0.2f)
        {
            corner = path.corners[1];
        }
        for (int i = 0; i < path.corners.Length; i++)
        {


            //Debug.Log(path.corners[i]);

            if (i == path.corners.Length - 1) continue;
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100); 

        }
        transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
    }

    ///<summary>ぶつかったら消える</summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            isTouched = false;
        }
         if(other.gameObject == kyuujosha)
        {
            k += 100;
            kyuujosha = null;
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        
    }
    //子からの情報を受け取るメソッド
    public void hantei(GameObject otherObject)
    {
        

        //きれいに書き換えArray.Resize(ref 配列オブジェクト, 新しいサイズ);
        if (kyuujosha == null)
        // 今までに助けたエージェントのリストにいなかったら向かう
            if (rescue.Contains(otherObject.name))
            {
                kyuujosha = null;
            }
            else
            {
                kyuujosha = otherObject;
                //次の地点に向かうために99
                k -= 99;
                Debug.Log("b");
                rescue.Add(otherObject.name);// 今までに助けたエージェントのリストに追加
            }
        }
    

}

