using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class yuudoufollow2 : MonoBehaviour
{

    [Header("Steering")]
    private float speed = 6f;//現在の速度
    private float fullspeed = 6f;//速度の最大値
    public float stoppingDistance = 0;
    public bool isTouched = false;//�Ԃ��������ǂ����̔���
    public bool force = true;
    int j = 0; //corner���ǂ邽�ё�����
    int k = 0;//poligon�ƂԂ�������
    private bool xxx = false;//trueになるまで受け渡し場所から動けない

    private Rigidbody2D rb;//��]��rb

    [HideInInspector]//���Unity�G�f�B�^�����\��
    private Vector2 trace_area = Vector2.zero;

    yuudouin agent; //NavMeshAgent2D���g�p���邽�߂̕ϐ�
    public GameObject receive;
    private Vector3 AgentDestination;
    Vector3[] Pathcorners = new Vector3[100];

    [SerializeField] Transform target; //�ǐՂ���^�[�Q�b�g
    //�q����󂯎��Q�[���I�u�W�F�N�g
    public GameObject kyuujosha = null;
    public Transform[] waypoints;
    List<GameObject> rescue = new List<GameObject>();//���܂łɏ������G�[�W�F���g�̃��X�g
    List<GameObject> nowrescue = new List<GameObject>();//現在救助中の高齢者

    private bool kyuujo = false;

    LineRenderer line;//�ǉz���̃G�[�W�F���g����������
    private CircleCollider2D circleCollider;

    //受け取り場所
    [SerializeField] GameObject receivepoint;

    Vector3 lastPos;

    private Vector2 AgentForce = new Vector2(0f, 0f);

    //ランダム地点に障害物があるかのwhile文で使用
    bool ObstacleHit = true;
    private NavMeshAgent navMeshAgent;
    private float castRadius = 0.3f;//スケールの1/2

    //ランダム歩行しているか
    bool randomwalk = true;

    public GameObject lastrescue;//前のエリアの誘導エージェント
    public GameObject lastrecevepoint;//前のエリアのゲームオブジェクト
    private int rescount = 0;//前受け取り場所にいる高齢者の数
    private bool repo = false;//受け渡し場所に3人以上いたらtrue
    private GameObject child;//前受け渡し場所に来たら範囲内全体を判定するためにメッセージを送る子オブジェクト
    public float areaxm;//エリアのxの上限
    public float areaxs;//エリアのxの下限
    public float areaym;//エリアのyの上限
    public float areays;//エリアのyの下限

    void Start()
    {
        /*NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        Pathcorners = path.corners;*/

        //�ړ��J�n
        //MoveToWaypoint(waypoints[k]);

        //��]��rb
        rb = GetComponent<Rigidbody2D>();

        lastPos = transform.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        //ランダムな地点に目的地（その地点のエージェント半径いないに障害物がない場合）
        SetRandommain();
        circleCollider = GetComponent<CircleCollider2D>();
        child = transform.Find("recevepointhantei").gameObject;
    }
    void Update()
    {
        // 回転をゼロに設定
        transform.rotation = Quaternion.identity;//これがないとnavmeshAgentで回転してしまう
        AgentForce = Vector2.zero;//リセット
        /* if (k >= 0)
     {
         //���݂̖ڕW�l�ւ̋������v�Z
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
     }*/

        //Debug.Log(k);

       



       
        //高齢者は一般エージェントによって位置が変わる可能性があるから毎フレーム更新
        if (kyuujosha != null)
        {
            AgentDestination = kyuujosha.transform.position;
        }

        if (randomwalk == true && kyuujosha == null)
        {
            // 目的地に到達したら新しいランダムな目的地を設定
            float distanceToTarget = Vector3.Distance(transform.position, AgentDestination);

            if (distanceToTarget < 0.5f)
            {
                SetRandommain();

            }
        }
        //三人以上救助してたら
        if (nowrescue.Count == 3)
        {
            AgentDestination = receivepoint.transform.position;
        }

        //出口までと前の受け渡し場所がどちらが近いかと3人以上いたら0人じゃない場合
        if (nowrescue.Count != 3)
        {
            if (kyuujosha == null)
            {
                if (rescount >= 3)
                {
                    // パスの長さを取得
                    float pathLengthToTarget1 = GetPathLength(receivepoint.transform.position);
                    float pathLengthToTarget2 = GetPathLength(lastrecevepoint.transform.position);
                    // より近い方のターゲットを表示&&今引き連れている高齢者が0でなければ
                    if (pathLengthToTarget1 < pathLengthToTarget2 && nowrescue.Count != 0)
                    {
                        AgentDestination = receivepoint.transform.position;
                    }
                    else
                    {
                        AgentDestination = lastrecevepoint.transform.position;
                    }
                }
            }
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
    private void Trace(Vector2 current, Vector2 target)
    {
        navMeshAgent.speed = speed;
        navMeshAgent.SetDestination(target);
        /*if (Vector2.Distance(current, target) <= stoppingDistance)
        {
            return;
        }

        // NavMesh �ɉ����Čo�H�����߂�
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
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.yellow, 100); 

        }
        transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);*/
    }

    ///<summary>�Ԃ������������</summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        // 衝突したオブジェクトにのみメッセージを送信
        if (other.gameObject.tag == "Finish")
        {
            if (kyuujosha == other.gameObject)
            {
                other.gameObject.SendMessage("OnCollisionOccurred", this.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
        
        

    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject == kyuujosha)
        {
            //高齢者に前のエリアの誘導員情報をわたし、今までに助けられたリストにあった場合,下のrescuepointcountloss()でcountを減らす
            other.gameObject.SendMessage("reslist", lastrescue, SendMessageOptions.DontRequireReceiver);

            nowrescue.Add(kyuujosha);
            kyuujo = false;
            kyuujosha = null;
            randomwalk = true;
            //高齢者を運んでいるときは速度が変更される（一人につき時速-1km)
            speed -= fullspeed / 6;
            Debug.Log(speed);
            navMeshAgent.speed = speed;

        }
    }
    //受け取り場所に誘導員がついたら、高齢者を中心に移動させるよう情報を受け渡す
    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.gameObject.tag == "Confluence")
        {
            for (int i = 0; i < nowrescue.Count; i++)
            {
                nowrescue[i].gameObject.SendMessage("ReceiveCollision", receivepoint, SendMessageOptions.DontRequireReceiver);
            }
            //速度を戻す
            speed = fullspeed;
            navMeshAgent.speed = speed;
            nowrescue.Clear();
            SetRandommain();
        }*/

        //前の受け渡し場所にいるときは子オブジェクト(recevepointhantei.cs)にメッセージを送る
        if (other.gameObject == lastrecevepoint)
        {
            child.gameObject.SendMessage("receveEnter", SendMessageOptions.DontRequireReceiver);
        }

    }
    //前の受け渡し場所から出たとき、子オブジェクトにメッセージを送る
    void OnTriggerExit2D(Collider2D other)
    {
        child.gameObject.SendMessage("receveExit", SendMessageOptions.DontRequireReceiver);
    }
    

    //�q����̏����󂯎�郁�\�b�h
    public void hantei(GameObject otherObject)
    {

        if (nowrescue.Count <= 2)
        {
            //���ꂢ�ɏ�������Array.Resize(ref �z��I�u�W�F�N�g, �V�����T�C�Y);
            if (kyuujosha == null)
            {
                // ���܂łɏ������G�[�W�F���g�̃��X�g�ɂ��Ȃ������������
                if (!rescue.Contains(otherObject))
                {
                    otherObject.gameObject.SendMessage("tui", this.gameObject, SendMessageOptions.DontRequireReceiver);
                    if (!rescue.Contains(otherObject))
                    {
                        kyuujosha = otherObject;
                        //���̒n�_�Ɍ��������߂�99
                        kyuujo = true;
                        Debug.Log("b");
                        rescue.Add(otherObject);// ���܂łɏ������G�[�W�F���g�̃��X�g�ɒǉ�
                        AgentDestination = kyuujosha.transform.position;
                    }
                }
            }
        }
    }

    //ランダムな目的地設定
    void SetRandomDestination()
    {
        // 2Dランダムな座標を取得(AgentのtransformおかしいからcolliderのInfoにある位置で見ること)
        float randomX = Random.Range(areaxs, areaxm);
        float randomY = Random.Range(areays, areaym);
        AgentDestination = new Vector3(randomX, randomY, 0.0f);
    }
    //受け取り場所が近ければそちらに向かう(現在救助者が1人以上)
    public void pointhantei()
    {
        if (nowrescue.Count >= 1 && !kyuujo)
        {
            AgentDestination = receivepoint.transform.position;
        }
    }

    //ランダムな目的地設定main(障害物がそこにないか)
    void SetRandommain()
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
                    //Debug.Log("yaaaa");
                    ObstacleHit = true;
                    break; // 障害物が一つでも検出されたらループを抜ける
                }
            }
        }
    }
    //ループ
    private System.Collections.IEnumerator MyCoroutine()
    {

        // コルーチンの処理
        while (!xxx)
        {
            yield return null; // 1フレーム待機
        }
        circleCollider.isTrigger = false;
        speed = fullspeed;
        navMeshAgent.speed = speed;
        nowrescue.Clear();
        SetRandommain();
        xxx = false;

    }
    //高齢者エージェントから受け取る
    void senior()
    {
        xxx = true;
    }

    //子のcircleコライダーに入ったら出口に高齢者を渡す
    public void gole()
    {
        for (int i = 0; i < nowrescue.Count; i++)
        {
            nowrescue[i].gameObject.SendMessage("ReceiveCollision", receivepoint, SendMessageOptions.DontRequireReceiver);
            nowrescue.RemoveAll(obj => obj == nowrescue[i]);
            Debug.Log(nowrescue.Count);
        }
        //待機
        navMeshAgent.speed = 0f;
        //高齢者エージェントから受け取るまで待機
        StartCoroutine(MyCoroutine());
        circleCollider.isTrigger = true;

    }
    //子に現在誘導中リストの数を送る
    public void listcount()
    {
        transform.Find("goleyuudou").SendMessage("counting", nowrescue.Count, SendMessageOptions.DontRequireReceiver);
    }

    //今まで助けたリストに入れる
    void nowtuiju(GameObject other)
    {
        rescue.Add(other);
    }
    //今まで助けたリストから受け渡し場所に高齢者がついたら消去（実際に誘導していない場合）
    void guideclear(GameObject other)
    {
        rescue.RemoveAll(obj => obj == other);
    }
    //前受け取り場所にいる高齢者の人数
    void rescuepointcount()
    {
        rescount++;
    }
    //高齢者リストに前の誘導員がいた場合
    void rescuepointcountloss()
    {
        rescount--;
    }

    //このオブジェクトと引数のオブジェクトの間の総距離
    float GetPathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

        float pathLength = 0f;

        // パス上の各点の間の距離を累積
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            pathLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return pathLength;
    }
}

