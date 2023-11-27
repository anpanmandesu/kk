using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class confluence : MonoBehaviour
{
    public float viewRadius = 5f; //視野半径(現在インスペクターで変更できる)
    public float viewAngle = 40f;// 視野の角度(左右にはそれぞれviewAngle/2)
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
        //DetectVisibleObjects(); //視野角をスクリプトで制御
    }
    /// <summary>
    ///�@����ipolygon)�ɓ����Ȃ��G�[�W�F���g(tag==Finish)���������Ƃ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {


        if (other.gameObject.tag == "Confluence")
        {

                transform.parent.GetComponent<yuudoufollow>().pointhantei();

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
