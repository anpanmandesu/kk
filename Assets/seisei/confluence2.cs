using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class confluence2 : MonoBehaviour
{
    public bool isTouched = false;//�Ԃ��������ǂ����̔���
    // Start is called before the first frame update
    private LayerMask ignoreLayer;
    public GameObject receivepoint;//次の受け渡し場所・出口
    public GameObject rescue;//前の受け渡し場所

    void Start()
    {
        ignoreLayer = 4 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        //DetectVisibleObjects(); //視野角をスクリプトで制御
    }
    /// <summary>
    ///�@����ipolygon)�ɓ����Ȃ��G�[�W�F���g(tag==Finish)���������Ƃ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {


        if (other.gameObject == receivepoint)
        {

           
                transform.parent.GetComponent<yuudoufollow2>().pointhantei();

                isTouched = false;
            
        }

    }
    //colliderの指定した角度の中にオブジェクトがある場合のみの判定（途中に障害物がない）
    // void DetectVisibleObjects()
    //     {
    //         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, viewRadius);

    //         foreach (Collider2D collider in colliders)
    //         {
    //             Vector2 dirToCollider = (collider.transform.position - transform.position).normalized;
    //             float angleToCollider = Vector2.Angle(transform.up, dirToCollider);

    //             // 視野角度内のColliderだけを検知
    //             if (angleToCollider < viewAngle * 0.5f)
    //             {
    //                 ObstacleBetween(transform.position, collider.transform.position, viewRadius,collider.gameObject);

    //             }
    //         }
    //     }
    
}
