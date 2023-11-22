using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class yuudoufollow : MonoBehaviour
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
    //�q����󂯎��Q�[���I�u�W�F�N�g
    public GameObject kyuujosha = null;
    public Transform[] waypoints;
    List<string> rescue = new List<string>();//���܂łɏ������G�[�W�F���g�̃��X�g

    LineRenderer line;//�ǉz���̃G�[�W�F���g����������

    Vector3 lastPos;

    

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

        
    }
void Update()
    {
        if (k >= 0)
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
        }
        
        Debug.Log(k);

        ///<summary>�ȉ���]</summary>

        Vector2 velocity =(Vector2) (transform.position - lastPos);
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
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.yellow, 100); 

        }
        transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
    }

    ///<summary>�Ԃ������������</summary>
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
                k -= 99;
                Debug.Log("b");
                rescue.Add(otherObject.name);// ���܂łɏ������G�[�W�F���g�̃��X�g�ɒǉ�
            }
        }
    

}

