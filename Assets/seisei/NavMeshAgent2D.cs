using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshAgent2D : MonoBehaviour
{
    [Header("Steering")]
    public float speed;
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
    private Vector2 AgentForce = new Vector2(0f,0f);


    Vector3 randomPoint = Vector3.zero;
    //ランダム地点に障害物があるかのwhile文で使用
    bool ObstacleHit = true;
    //エージェントの半径（ランダム地点に障害物があるか判別するため)
    private float castRadius = 0.3f;//スケールの1/2
    private NavMeshAgent navMeshAgent;

    //ランダム歩行しているか
    bool randomwalk = true;
    void Start()
    {
        speed = Random.Range(0.5f, 3f);
        //Debug.Log(speed);

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;


        //ランダムな地点に目的地（その地点のエージェント半径いないに障害物がない場合）
        while (ObstacleHit)
        {
            SetRandomDestination();
            // 半径内のすべてのCollider2Dを検出
            Collider2D[] colliders = Physics2D.OverlapCircleAll(AgentDestination, castRadius);

            ObstacleHit = false;
            // 各Collider2Dに対して処理
            foreach (Collider2D collider in colliders)
            {
                // タグが指定した障害物のタグと一致するか確認
                if (collider.CompareTag("obstacle"))
                {
                    //Debug.Log("yaaaa");
                    ObstacleHit = true;
                    break; // 障害物が一つでも検出されたらループを抜ける
                }
            }
        }

        lastPos = transform.position;




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
        // 回転をゼロに設定
        transform.rotation = Quaternion.identity;//これがないとnavmeshAgentで回転してしまう
        

        AgentForce = Vector2.zero;//リセット
        /*//エージェント同士の衝突回避
        Collider2D[] nearbyAgents = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, agentLayer);
        foreach (var agentCollider in nearbyAgents)
        {
            Debug.Log(agentCollider.gameObject);
            if (agentCollider.gameObject != gameObject)
            {
                // エージェント同士の方向ベクトル
                Vector2 toAgent = agentCollider.transform.position - transform.position;

                // エージェント同士の角度差
                float angleDifference = Vector2.SignedAngle(velocity, toAgent);
                if (Mathf.Abs(angleDifference) < 20f)
                {
                    Debug.Log("foreach");
                    AgentForce += CalculateForce(agentCollider.transform.position);//検知したエージェント
                }
            }
        }*/

        // 一定の範囲内にいるエージェントを取得
        Collider2D[] nearbyAgents = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, agentLayer);
/*
        foreach (var agentCollider in nearbyAgents)
        {
            if (agentCollider.gameObject != gameObject)
            {
                // エージェント同士の方向ベクトル
                Vector2 toAgent = agentCollider.transform.position - transform.position;

                // エージェント同士の距離
                float distance = toAgent.magnitude;

                // エージェント同士の半径の合計
                float radiusSum = castRadius*2;

                // エージェント同士の単位ベクトル
                Vector2 normalizedDirection = toAgent.normalized;

                // エージェント同士の速度差
                Vector2 velocityDifference = agentCollider.GetComponent<Rigidbody2D>().velocity - rb.velocity;

                // エージェント同士の中心を結んだ線に対して垂直な単位ベクトル
                Vector2 tangentVector = new Vector2(-normalizedDirection.y, normalizedDirection.x);

                // 力の計算
                float forceMagnitude = A * Mathf.Exp((distance - radiusSum) / B) +
                                       gamma * Mathf.Max(0, distance - radiusSum) +
                                       kappa * Mathf.Max(0, distance - radiusSum) * Vector2.Dot(velocityDifference, tangentVector);
                
                AgentForce += forceMagnitude * normalizedDirection;
            }
            *//*// エージェントの速度を更新する
            Vector2 acceleration = AgentForce / rb.mass;
            rb.velocity += acceleration * Time.deltaTime;*//*
        }*/
            ///<summary>以下回転</summary>



            //Debug.Log(velocity);//フレームごとにtransformを変更して瞬間移動しているだけだから方向ベクトルは(0,0)
        //     if (velocity != Vector2.zero)
        // {
        //     Debug.Log("C");
        //     // 速度ベクトルから角度を計算（度数法）
        //     float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        //     // オブジェクトを回転
        //     /*transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));*/
        // }

        if(randomwalk == true){
            // 目的地に到達したら新しいランダムな目的地を設定
            float distanceToTarget = Vector3.Distance(transform.position, AgentDestination);
            if (distanceToTarget < 0.5f)
            {
                ObstacleHit = true;
                //ランダムな地点に目的地（その地点のエージェント半径いないに障害物がない場合）
                while (ObstacleHit)
                {
                    SetRandomDestination();
                    // 半径内のすべてのCollider2Dを検出
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(AgentDestination, castRadius);

                    ObstacleHit = false;
                    // 各Collider2Dに対して処理
                    foreach (Collider2D collider in colliders)
                    {
                        // タグが指定した障害物のタグと一致するか確認
                        if (collider.CompareTag("obstacle"))
                        {
                            Debug.Log("yaaaa");
                            ObstacleHit = true;
                            break; // 障害物が一つでも検出されたらループを抜ける
                        }
                    }
                }
            }
        }
        //出口を見つけて、目的地を出口にした場合
        else{
            AgentDestination = target.position;
        }
        Trace(transform.position, AgentDestination);

        ///<summary>�ȉ���]</summary>
    Vector2 velocity =(Vector2) (transform.position - lastPos);
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
        navMeshAgent.SetDestination(target);


        // NavMeshPath path = new NavMeshPath();
        // NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

        // Vector2 corner = path.corners[0];
        // if (Vector2.Distance(current, corner) <= 0.3f)
        // {
        //     corner = path.corners[1];
        // }
        // for (int i = 0; i < path.corners.Length; i++)
        // {


        //     //Debug.Log(path.corners[i]);

        //     if (i == path.corners.Length - 1) continue;
        //     Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);

        // }
        






        ////Vector2 direction = (corner - current).normalized;


        /*RaycastHit2D hit = Physics2D.Raycast(current, direction);

        Vector3 rayOrigin = transform.position;
        Debug.DrawRay(rayOrigin, direction * 2f, Color.yellow);


        if (hit.collider != null && hit.collider.CompareTag("obstacle"))
        {
            // 壁からの反発力を計算
            Vector2 repelDirection = (current - hit.point).normalized;
            direction = direction + (repulsionForce * repelDirection);
        }
*/
        // 移動
        ////transform.Translate(direction * speed * Time.deltaTime);


        //エージェントを進行方向に向く
        //transform.up = direction.normalized;



        /*if (Vector2.Distance(current, target) <= stoppingDistance)
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
        transform.position = Vector2.MoveTowards(current, corner+AgentForce, speed * Time.deltaTime);*/
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
    void OnCollisionStay2D(Collision2D other)
    {

    }
    //ランダムな目的地設定
    void SetRandomDestination()
    {
        // 2Dランダムな座標を取得(AgentのtransformおかしいからcolliderのInfoにある位置で見ること)
        float randomX = Random.Range(-8.66f, 21f);
        float randomY = Random.Range(-15f, 19.5f);
        /*Debug.Log(randomX);
        Debug.Log(randomY);*/
        AgentDestination = new Vector3(randomX, randomY, 0.0f);
    }


    /*//エージェント同士のよけ合い
        Vector2 CalculateForce(Vector2 agentPosition)
    {
        Vector2 direction = (Vector2)transform.position - agentPosition;
        float distance = direction.magnitude ;
        Vector2 normalizedDirection = direction.normalized;


        float radiusSum = castRadius + avoidanceRadius;
        float relativeDistance = radiusSum - distance;

        // 第一項の計算
        float firstTerm = A * Mathf.Exp((relativeDistance / B));

        // 第二項の計算
        Vector2 secondTerm = new Vector2(0f,0f);
        if (relativeDistance > 0.0f)
        {
            // エージェントの相対速度を計算
            Vector2 relativeVelocity = GetComponent<Rigidbody2D>().velocity;

            // 第二項のガンマ g 関数の計算
            float gFunction = relativeDistance > 0.0f ? relativeDistance : 0.0f;
            // 第二項の各成分の計算
            Vector2 secondTermPart1 = gamma * gFunction * normalizedDirection;
            Vector2 secondTermPart2 = kappa * gFunction * relativeVelocity;

            secondTerm = secondTermPart1 + secondTermPart2;
        }

        // 合力の計算
        
        return firstTerm * normalizedDirection + secondTerm;
        
    }*/

    public void hantei(GameObject otherObject)
    {
        randomwalk = false;
        target = otherObject.transform;
        }
    //誘導員を見つけた際、近くの出口が目的地になる
    public void golehantei(){
        randomwalk = false;
        target = FindNearestObjectWithTag("Player");
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

    float minDistance = Mathf.Infinity;
    foreach (Vector3 corner in path.corners)
            {
                float distance = Vector3.Distance(transform.position, corner);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            //Debug.Log("Nearest distance: " + minDistance);
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
            }
        }

        return nearestObject;
    }
}