using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golehantei : MonoBehaviour
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


        if (other.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<NavMeshAgent2D>().hantei(other.gameObject);
            isTouched = false;
        }

    }
}
