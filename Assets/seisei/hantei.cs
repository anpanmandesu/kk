using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hantei : MonoBehaviour
{
    public GameObject kyuujo;
    public bool isTouched = false;//�Ԃ��������ǂ����̔���
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    /// <summary>
    ///�@����ipolygon)�ɓ����Ȃ��G�[�W�F���g(tag==Finish)���������Ƃ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
        {
            transform.parent.GetComponent<yuudoufollow>().hantei(other.gameObject);
            isTouched = false;
        }

    }
}
