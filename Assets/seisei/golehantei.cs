using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golehantei : MonoBehaviour
{
    public float viewRadius = 10f;        // 視野の半径
    public float viewAngle = 40f;        // 視野の角度(左右にはそれぞれviewAngle/2)
    public GameObject kyuujo;
    public bool isTouched = false;//�Ԃ��������ǂ����̔���
    // Start is called before the first frame update
    private LayerMask ignoreLayer;
    void Start()
    {
        ignoreLayer = 4 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    ///�@����ipolygon)�ɓ����Ȃ��G�[�W�F���g(tag==Finish)���������Ƃ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        /*RaycastHit hit;
        Ray ray = new Ray()*/
        Vector2 direction = other.gameObject.transform.position - this.transform.position;
        float distance = direction.magnitude;

        // Raycast�𔭎˂��ďՓ˂����o
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 10f, ignoreLayer);

        if (hit.collider != null)
        {
            Debug.Log("��Q�������o����܂���: " + hit.collider.name);
            // �����ɏ�Q�������o���ꂽ�ꍇ�̏�����ǉ�
        }


        if (other.gameObject.tag == "Player")
        {
            DetectCollidersInFieldOfView(other.gameObject);
            
        }

    }   
    //範囲（viewAngle度)の中にあるオブジェクトだけ検知
    void DetectCollidersInFieldOfView(GameObject otherObject)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        foreach (Collider2D collider in colliders)
        {
            Vector2 dirToCollider = (collider.transform.position - transform.position).normalized;
            float angleToCollider = Vector2.Angle(transform.up, dirToCollider);

            // 視野角度内のColliderだけを検知
            if (angleToCollider < viewAngle * 0.5f&&collider.name == otherObject.gameObject.name)
            {
                transform.parent.GetComponent<NavMeshAgent2D>().hantei(otherObject);
                isTouched = false;
            }
        }
    }
}
