using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject Agent;
    public GameObject Finish;
    public int Agentnumber = 10;
    public int Finishnumber = 10;
    public float castRadius = 0.3f;
    private bool obstacleHit = true;
    // Start is called before the first frame update
    void Start()
    {
        seisei(Agent, Agentnumber);
        seisei(Finish, Finishnumber);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
Vector3 GetRandomPosition()
{
    // �����_���Ȉʒu�𐶐����ĕԂ����\�b�h
    float randomX = Random.Range(-34.5f, 34.5f);
    float randomY = Random.Range(-50f, 50f);
    return new Vector3(randomX, randomY, 0f);
    }

    void seisei(GameObject obj,int generatenumber)
    {
        for (int i = 0; i < generatenumber; i++)
        {
            obstacleHit = true;
            while (obstacleHit)
            {
                GameObject newObject = Instantiate(obj, GetRandomPosition(), Quaternion.identity);

                // ���a���̂��ׂĂ�Collider2D�����o
                Collider2D[] colliders = Physics2D.OverlapCircleAll(newObject.transform.position, castRadius);

                obstacleHit = false;

                // �eCollider2D�ɑ΂��ď���
                foreach (Collider2D collider in colliders)
                {
                    // �^�O���w�肵����Q���̃^�O�ƈ�v���邩�m�F
                    if (collider.CompareTag("obstacle"))
                    {
                        obstacleHit = true;
                        Destroy(newObject);  // ��Q��������ΐ��������I�u�W�F�N�g��j��
                        break;  // ��Q������ł����o���ꂽ�烋�[�v�𔲂���
                    }
                }
            }
        }
    }
}
