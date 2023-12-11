using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject Agent;
    public GameObject Agent2;
    public GameObject Finish;
    public GameObject Finish2;
    public GameObject startgameObject;
    public int Agentnumber = 10;
    public int Agentnumber2 = 10;
    public int Finishnumber = 10;
    public int Finishnumber2 = 10;
    public float castRadius = 0.3f;
    private bool obstacleHit = true;
    private int x = 0; //�������Ń����_���Ȉʒu��������܂ŊJ�n���Ȃ�(fps.cs)
    // Start is called before the first frame update
    public bool R = false;
    void Start()
    {
        seisei(Agent, Agentnumber);
        seisei(Agent2, Agentnumber2);
        seisei(Finish, Finishnumber);
        seisei(Finish2, Finishnumber2);
        Time.timeScale = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if (R)
        {
            //Debug.Log(x);
            Time.timeScale = 0.1f; // ���s���Ԃ�1/10�ɂ���
            startgameObject.gameObject.SendMessage("Ac",  SendMessageOptions.DontRequireReceiver);
        }
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
                    if (collider.CompareTag("obstacle")&&collider.CompareTag("Confluence"))
                    {
                        obstacleHit = true;
                        Destroy(newObject);  // ��Q��������ΐ��������I�u�W�F�N�g��j��
                        break;  // ��Q������ł����o���ꂽ�烋�[�v�𔲂���
                    }
                }
            }
        }
    }
    /*void xplus()
    {
        x++;
    }*/
}
