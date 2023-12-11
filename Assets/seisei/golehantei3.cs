using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golehantei3 : MonoBehaviour
{
    public float viewRadius = 100f;        // 視野の半径
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


        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Finish")
        {

            bool De = DetectCollidersInFieldOfView(other.gameObject);
            if (De == true)
            {
                bool Ob = ObstacleBetween(transform.position, other.transform.position, other.gameObject);
                if (Ob == false)
                {
                    transform.parent.GetComponent<tuiju2>().hantei(other.gameObject);
                }
            }
        }


    }
    //範囲（viewAngle度)の中にあるオブジェクトだけ検知
    bool DetectCollidersInFieldOfView(GameObject otherObject)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        foreach (Collider2D collider in colliders)
        {
            Vector2 customDirection = Quaternion.Euler(0, 0, 270f) * transform.up;
            Vector2 dirToCollider = (collider.transform.position - transform.position).normalized;
            float angleToCollider = Vector2.Angle(customDirection, dirToCollider);

            //Debug.Log(transform.up);
            // 視野角度内のColliderだけを検知
            if (angleToCollider < viewAngle * 0.5f && collider.name == otherObject.gameObject.name)
            {

                isTouched = false;
                return true;
            }
        }
        return false;
    }

    //現在位置から検知したオブジェクトの間に障害物があるかどうか
    bool ObstacleBetween(Vector2 start, Vector2 target, GameObject otherObject)
    {
        Vector2 rayDirection = otherObject.transform.position - transform.position;
        float rayDistance = rayDirection.magnitude;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, rayDistance);
        //int i = 1;
        // hitsをループして処理
        foreach (RaycastHit2D hit in hits)//rayにはOnTriggerStay2Dのオブジェクトは検知されない
        {
            //Debug.Log(i);
            //Debug.Log(hit.collider);
            // 当たったColliderが検知したオブジェクトでない場合
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Agent") && !otherObject == transform.root.gameObject || hit.collider.CompareTag("rescue") || hit.collider.CompareTag("GameController") || hit.collider.CompareTag("obstacle"))
                {
                    return true;
                }
            }
            //i++;
        }
        return false;
    }
}
