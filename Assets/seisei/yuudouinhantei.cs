using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yuudouinhantei : MonoBehaviour
{
    

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


        if (other.gameObject.tag == "GameController")
        {
            bool Ob = ObstacleBetween(transform.position,other.transform.position,other.gameObject);
            if(Ob == false){
                transform.parent.GetComponent<yuudoufollow>().hantei(other.gameObject);
            
                isTouched = false;
            }
        }

    }

    
    
    bool ObstacleBetween(Vector2 start, Vector2 target,GameObject otherObject)
    {
        Vector2 rayDirection = otherObject.transform.position - transform.position;
        float rayDistance = rayDirection.magnitude;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, rayDistance );
            //int i = 1;
          // hitsをループして処理
         foreach (RaycastHit2D hit in hits)//rayにはOnTriggerStay2Dのオブジェクトは検知されない
        {
            //Debug.Log(i);
            //Debug.Log(hit.collider);
            // 当たったColliderが検知したオブジェクトでない場合
           if(hit.collider != null){
                if(hit.collider.CompareTag("Agent")||hit.collider.CompareTag("obstacle")||!otherObject == transform.root.gameObject){
                    return true;
                }
            }
            //i++;
        }
        return false;
    }
}
