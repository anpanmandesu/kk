using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class yuudouin : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
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
    public GameObject siya;
    Vector3[] Pathcorners = new Vector3[100];

    [SerializeField] Transform target; //�ǐՂ���^�[�Q�b�g

    LineRenderer line;//�ǉz���̃G�[�W�F���g����������

    Vector3 lastPos;
    //�ǁE�G�[�W�F���g���m�̔�������

    public float desiredSeparation = 1.5f;
    public float wallAvoidanceDistance = 3.0f;
    private Vector3 AgentDestination;

    public float A = 1.0f;
    public float B = 1.0f;
    public float gamma = 1.0f;
    public float kappa = 1.0f;
    public float avoidanceRadius = 0.3f;
    public LayerMask agentLayer;

    public float wallAvoidanceForce = 5.0f;
    private Vector2 AgentForce = new Vector2(0f, 0f);



    Vector3 randomPoint = Vector3.zero;
    //�����_���n�_�ɏ�Q�������邩��while���Ŏg�p
    bool ObstacleHit = true;
    //�G�[�W�F���g�̔��a�i�����_���n�_�ɏ�Q�������邩���ʂ��邽��)
    private float castRadius = 0.3f;//�X�P�[����1/2

    void Start()
    {


        //�����_���Ȓn�_�ɖړI�n�i���̒n�_�̃G�[�W�F���g���a���Ȃ��ɏ�Q�����Ȃ��ꍇ�j
        while (ObstacleHit)
        {
            SetRandomDestination();
            // ���a���̂��ׂĂ�Collider2D�����o
            Collider2D[] colliders = Physics2D.OverlapCircleAll(AgentDestination, castRadius);

            ObstacleHit = false;
            // �eCollider2D�ɑ΂��ď���
            foreach (Collider2D collider in colliders)
            {
                // �^�O���w�肵����Q���̃^�O�ƈ�v���邩�m�F
                if (collider.CompareTag("obstacle"))
                {
                    Debug.Log("yaaaa");
                    ObstacleHit = true;
                    break; // ��Q������ł����o���ꂽ�烋�[�v�𔲂���
                }
            }
        }




        //trace�Ŏg��navmesh��path�̏����ݒ�
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, AgentDestination, NavMesh.AllAreas, path);
        Pathcorners = path.corners;

        //�ړ��J�n
        //MoveToWaypoint(waypoints[k]);

        //��]��rb
        rb = GetComponent<Rigidbody2D>();

        lastPos = transform.position;


    }
    void Update()
    {

        AgentForce = Vector2.zero;//���Z�b�g
        //�G�[�W�F���g���m�̏Փˉ��
        Collider2D[] nearbyAgents = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, agentLayer);
        foreach (var agentCollider in nearbyAgents)
        {
            Debug.Log(agentCollider.gameObject);
            if (agentCollider.gameObject != gameObject)
            {
                Debug.Log("foreach");
                AgentForce += CalculateForce(agentCollider.transform.position);
            }
        }
        ///<summary>�ȉ���]</summary>

        Vector2 velocity = (Vector2)(transform.position - lastPos);
        lastPos = transform.position;

        //Debug.Log(velocity);//�t���[�����Ƃ�transform��ύX���ďu�Ԉړ����Ă��邾������������x�N�g����(0,0)
        if (velocity != Vector2.zero)
        {
            Debug.Log("C");
            // ���x�x�N�g������p�x���v�Z�i�x���@�j
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // �I�u�W�F�N�g����]
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }


        // �ړI�n�ɓ��B������V���������_���ȖړI�n��ݒ�
        float distanceToTarget = Vector3.Distance(transform.position, AgentDestination);
        if (distanceToTarget < 0.5f)
        {
            ObstacleHit = true;
            //�����_���Ȓn�_�ɖړI�n�i���̒n�_�̃G�[�W�F���g���a���Ȃ��ɏ�Q�����Ȃ��ꍇ�j
            while (ObstacleHit)
            {
                SetRandomDestination();
                // ���a���̂��ׂĂ�Collider2D�����o
                Collider2D[] colliders = Physics2D.OverlapCircleAll(AgentDestination, castRadius);

                ObstacleHit = false;
                // �eCollider2D�ɑ΂��ď���
                foreach (Collider2D collider in colliders)
                {
                    // �^�O���w�肵����Q���̃^�O�ƈ�v���邩�m�F
                    if (collider.CompareTag("obstacle"))
                    {
                        Debug.Log("yaaaa");
                        ObstacleHit = true;
                        break; // ��Q������ł����o���ꂽ�烋�[�v�𔲂���
                    }
                }
            }
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

    private void Trace(Vector2 current, Vector2 target)
    {
        if (Vector2.Distance(current, target) <= stoppingDistance)
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
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);

        }
        transform.position = Vector2.MoveTowards(current, corner + AgentForce, speed * Time.deltaTime);
        Debug.Log(AgentForce+"aaa");
        Debug.Log(current+"bbb");
        Debug.Log(corner + AgentForce+"ccc");
    }

    ///<summary>�Ԃ������������</summary>
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
    //�����_���ȖړI�n�ݒ�
    void SetRandomDestination()
    {
        // 2D�����_���ȍ��W���擾(Agent��transform������������collider��Info�ɂ���ʒu�Ō��邱��)
        float randomX = Random.Range(-8.66f, 21f);
        float randomY = Random.Range(-15f, 19.5f);
        /*Debug.Log(randomX);
        Debug.Log(randomY);*/
        AgentDestination = new Vector3(randomX, randomY, 0.0f);
    }


    //�G�[�W�F���g���m�̂悯����
    Vector2 CalculateForce(Vector2 agentPosition)
    {
        Vector2 direction = (Vector2)transform.position - agentPosition;
        float distance = direction.magnitude;
        Debug.Log(castRadius);
        Vector2 normalizedDirection = direction.normalized;


        float radiusSum = GetComponent<Collider2D>().bounds.extents.magnitude + avoidanceRadius;
        float relativeDistance = radiusSum - distance;

        // ��ꍀ�̌v�Z
        float firstTerm = A * Mathf.Exp((relativeDistance / B));

        // ��񍀂̌v�Z
        Vector2 secondTerm = new Vector2(0f, 0f);
        if (relativeDistance > 0.0f)
        {
            // �G�[�W�F���g�̑��Α��x���v�Z
            Vector2 relativeVelocity = GetComponent<Rigidbody2D>().velocity;

            // ��񍀂̃K���} g �֐��̌v�Z
            float gFunction = relativeDistance > 0.0f ? 1.0f : 0.0f;

            // ��񍀂̊e�����̌v�Z
            Vector2 secondTermPart1 = gamma * gFunction * normalizedDirection;
            Vector2 secondTermPart2 = kappa * gFunction * relativeVelocity;

            secondTerm = secondTermPart1 + secondTermPart2;
        }

        // ���͂̌v�Z
        
        return firstTerm * normalizedDirection + secondTerm;

    }
}