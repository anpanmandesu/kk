﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMe2 : MonoBehaviour
{
    [Header("Steering")]
    public float speed;
    private float maxspeed = 6f / 3.6f;
    private float minspeed = 8f / 3.6f;
    private float rescuespeed = (6f - 6f / 6) / 3.6f;//高齢者を救助したときの移動速度
    public float stoppingDistance = 0;
    public bool isTouched = false;//ぶつかったかどうかの判定
    float kakudo = -90f;
    public bool force = true;
    int j = 0; //cornerたどるたび増える　
    int k = 0;//poligonとぶつかったら
    //壁からの斥力
    public string wallTag = "obstacle";
    public float repulsionForce = 10f;//壁からの斥力
    public float raycastDistance = 1f;

    private Rigidbody2D rb;//回転のrb

    [HideInInspector]//常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;

    yuudouin agent; //NavMeshAgent2Dを使用するための変数
    public GameObject siya;
    Vector3[] Pathcorners = new Vector3[100];

    [SerializeField] Transform target; //追跡するターゲット

    LineRenderer line;//壁越しのエージェントを見分ける

    Vector3 lastPos;
    //壁・エージェント同士の避け合い

    public float desiredSeparation = 1.5f;
    public float wallAvoidanceDistance = 3.0f;
    private Vector3 AgentDestination;

    public float A = 1.0f;
    public float B = 0.1f;
    public float gamma = 1.0f;
    public float kappa = 1.0f;
    public float avoidanceRadius = 0.3f;
    public LayerMask agentLayer;

    public float wallAvoidanceForce = 5.0f;
    private Vector2 AgentForce = new Vector2(0f, 0f);
    public GameObject startgameObject;//初期化でランダム地点を見つけたときに送るオブジェクト（Generate.cs)


    Vector3 randomPoint = Vector3.zero;
    //ランダム地点に障害物があるかのwhile文で使用
    bool ObstacleHit = true;
    //エージェントの半径（ランダム地点に障害物があるか判別するため)
    private float castRadius = 0.3f;//スケールの1/2
    private NavMeshAgent navMeshAgent;

    //ランダム歩行しているか
    bool randomwalk = true;
    public bool R; //このエージェントが救助するか。するならtrue(10%)
    bool s = false;//現在高齢者を救助中ならtrue
    //
    private GameObject sinior = null;//助ける高齢者を格納
    private List<GameObject> rescue = new List<GameObject>();//追跡できないエージェント
    private GameObject goleobj;//出口
    //前までの目的地
    private Vector3 lasttarget;
    public float areaxm = 35f;//エリアのxの上限
    public float areaxs = -35f;//エリアのxの下限
    public float areaym = 50f; //エリアのyの上限
    public float areays = -50f;//エリアのyの下限
    private bool starting = false;
            void Start()
            {
                R = (1 == Random.Range(1, 11));
                speed = Random.Range(minspeed, maxspeed);
                //Debug.Log(speed);

                navMeshAgent = GetComponent<NavMeshAgent>();
                navMeshAgent.speed = speed;

                lasttarget = FindNearestObjectWithTag("Player").position;
                AgentDestination = lasttarget;




                //traceで使うnavmeshのpathの初期設定
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, AgentDestination, NavMesh.AllAreas, path);
                Pathcorners = path.corners;

                //移動開始
                //MoveToWaypoint(waypoints[k]);


                //回転のrb
                rb = GetComponent<Rigidbody2D>();

                lastPos = transform.position;


            }
    void Update()
    {
        /*if (ObstacleHit == false && starting == false)
        {
            starting = true;
            startgameObject.SendMessage("xplus", SendMessageOptions.DontRequireReceiver);
        }*/
        // 回転をゼロに設定
        transform.rotation = Quaternion.identity;//これがないとnavmeshAgentで回転してしまう


        AgentForce = Vector2.zero;//リセット
       
      
        //高齢者を救助中
        if (s)
        {
            AgentDestination = sinior.transform.position;
        }



        ///<summary>�ȉ���]</summary>
        Vector2 velocity = (Vector2)(transform.position - lastPos);
        lastPos = transform.position;

        //Debug.Log(velocity);//�t���[�����Ƃ�transform��ύX���ďu�Ԉړ����Ă��邾������������x�N�g����(0,0)
        if (velocity != Vector2.zero)
        {
            //Debug.Log("C");
            // ���x�x�N�g������p�x���v�Z�i�x���@�j
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // �I�u�W�F�N�g����]
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        Trace(transform.position, AgentDestination);
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetComponent<Rigidbody2D>().velocity.normalized, wallAvoidanceDistance);

        if (hit.collider != null && hit.collider.CompareTag("obstacle"))
        {
            Vector2 wallAvoidanceDirection = ((Vector2)transform.position - hit.point).normalized;
            GetComponent<Rigidbody2D>().AddForce(wallAvoidanceDirection * wallAvoidanceForce);
        }
    }



    //navmesh
    private void Trace(Vector2 current, Vector2 target)
    {
        navMeshAgent.speed = speed;
        navMeshAgent.SetDestination(target);
    }



    ///<summary>ぶつかったら消える</summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (sinior != null)
            {
                sinior.gameObject.SendMessage("gole", goleobj, SendMessageOptions.DontRequireReceiver);
            }
            Destroy(this.gameObject);
            isTouched = false;
        }
        // 衝突したオブジェクトにのみメッセージを送信
        if (other.gameObject.tag == "Finish")
        {
            if (sinior == other.gameObject)
            {
                other.gameObject.SendMessage("OnCollisionOccurred", this.gameObject, SendMessageOptions.DontRequireReceiver);
                navMeshAgent.speed = rescuespeed;
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (sinior != null)
        {
            if (other.gameObject == sinior && s)
            {
                AgentDestination = lasttarget;
                s = false;
            }
        }
    }



    public void hantei(GameObject otherObject)
    {
        if (otherObject.tag == "Finish" && R)
        {
            if (sinior == null)
            {
                otherObject.gameObject.SendMessage("tui", this.gameObject, SendMessageOptions.DontRequireReceiver);
                if (!rescue.Contains(otherObject))
                {
                    lasttarget = AgentDestination;
                    sinior = otherObject;
                    s = true;
                }
            }
        }
        if (otherObject.tag == "Player")
        {
            randomwalk = false;
            lasttarget = otherObject.transform.position;
            goleobj = otherObject;
            if (!s)
            {
                AgentDestination = otherObject.transform.position;
            }

        }

    }
    //誘導員を見つけた際、近くの出口が目的地になる
    public void golehantei()
    {
        randomwalk = false;
        lasttarget = FindNearestObjectWithTag("Player").position;
        if (!s)
        {
            AgentDestination = lasttarget;
        }
    }

    Transform FindNearestObjectWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        Transform nearestObject = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objectsWithTag)
        {
            float distance = Vector3.Distance(obj.transform.position, currentPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestObject = obj.transform;
                goleobj = obj;
            }
        }

        return nearestObject;
    }
    //追跡エージェントがいたらリストに追加
    void nowtuiju(GameObject other)
    {
        rescue.Add(other);
    }
    //今まで助けたリストから受け渡し場所に高齢者がついたら消去（実際に誘導していない場合）
    void guideclear(GameObject other)
    {
        rescue.RemoveAll(obj => obj == other);
    }
}