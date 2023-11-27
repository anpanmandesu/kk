using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class yuudoufollow2 : MonoBehaviour
{

    [Header("Steering")]
    public float speed = 2f;
    public float stoppingDistance = 0;
    public bool isTouched = false;//�Ԃ��������ǂ����̔���
    float kakudo = -90f;
    public bool force = true;
    int j = 0; //corner���ǂ邽�ё�����
    int k = 0;//poligon�ƂԂ�������

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
    List<string> rescue = new List<string>();//���܂łɏ������G�[�W�F���g�̃��X�g
    List<string> nowrescue = new List<string>();//現在救助中の高齢者

    private bool kyuujo = false;

    LineRenderer line;//�ǉz���̃G�[�W�F���g����������

    //受け取り場所
    [SerializeField] Transform receivepoint;


    Vector3 lastPos;

    private Vector2 AgentForce = new Vector2(0f, 0f);

    //ランダム地点に障害物があるかのwhile文で使用
    bool ObstacleHit = true;
    private NavMeshAgent navMeshAgent;
    private float castRadius = 0.3f;//スケールの1/2

    //ランダム歩行しているか
    bool randomwalk = true;



    void Start()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        Pathcorners = path.corners;

        //�ړ��J�n
        //MoveToWaypoint(waypoints[k]);

        //��]��rb
        rb = GetComponent<Rigidbody2D>();

        lastPos = transform.position;

        //ランダムな地点に目的地（その地点のエージェント半径いないに障害物がない場合）
        while (ObstacleHit)
        {

            //Debug.Log(speed);
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
            SetRandomDestination();
            // 半径内のすべてのCollider2Dを検出
            Collider2D[] colliders = Physics2D.OverlapCircleAll(AgentDestination, castRadius);

            ObstacleHit = false;
            // 各Collider2Dに対して処理
            foreach (Collider2D collider in colliders)
            {
                // タグが指定した障害物のタグと一致するか確認
                if (collider.CompareTag("obstacle") || collider.CompareTag("Player"))
                {
                    //Debug.Log("yaaaa");
                    ObstacleHit = true;
                    break; // 障害物が一つでも検出されたらループを抜ける
                }
            }
        }
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

        Trace(transform.position, AgentDestination);
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
        //高齢者は一般エージェントによって位置が変わる可能性があるから毎フレーム更新
        if (kyuujosha != null)
        {
            AgentDestination = kyuujosha.transform.position;
        }


        //三人以上救助してたら
        if (nowrescue.Count == 3)
        {
            AgentDestination = receivepoint.position;
        }

        if (randomwalk == true)
        {
            // 目的地に到達したら新しいランダムな目的地を設定
            float distanceToTarget = Vector3.Distance(transform.position, AgentDestination);

            if (distanceToTarget < 0.5f)
            {
                if (AgentDestination == receivepoint.position)
                {
                    nowrescue.Clear();
                }

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


    }
    private void Trace(Vector2 current, Vector2 target)
    {
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
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            isTouched = false;
        }
        if (other.gameObject == kyuujosha)
        {
            nowrescue.Add(kyuujosha.name);
            kyuujo = false;
            kyuujosha = null;
            randomwalk = true;
            
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {

    }
    //�q����̏����󂯎�郁�\�b�h
    public void hantei(GameObject otherObject)
    {


        //���ꂢ�ɏ�������Array.Resize(ref �z��I�u�W�F�N�g, �V�����T�C�Y);
        if (kyuujosha == null)
            // ���܂łɏ������G�[�W�F���g�̃��X�g�ɂ��Ȃ������������
            if (rescue.Contains(otherObject.name))
            {
                kyuujosha = null;
            }
            else
            {
                kyuujosha = otherObject;
                //���̒n�_�Ɍ��������߂�99
                kyuujo = true;
                Debug.Log("b");
                rescue.Add(otherObject.name);// ���܂łɏ������G�[�W�F���g�̃��X�g�ɒǉ�
                AgentDestination = kyuujosha.transform.position;
            }
    }

    //ランダムな目的地設定
    void SetRandomDestination()
    {
        // 2Dランダムな座標を取得(AgentのtransformおかしいからcolliderのInfoにある位置で見ること)
        float randomX = Random.Range(-8.66f, 21f);
        float randomY = Random.Range(receive.transform.position.y,19.5f);
        /*Debug.Log(randomX);
        Debug.Log(randomY);*/
        AgentDestination = new Vector3(randomX, randomY, 0.0f);
    }
    //受け取り場所が近ければそちらに向かう(現在救助者が1人以上)
    public void pointhantei()
    {
        if (nowrescue.Count >= 1)
        {
            AgentDestination = receivepoint.position;
        }
    }
}

