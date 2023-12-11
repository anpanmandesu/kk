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
    private int x = 0; //初期化でランダムな位置を見つけるまで開始しない(fps.cs)
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
            Time.timeScale = 0.2f; // 実行時間を1/10にする
            startgameObject.gameObject.SendMessage("Ac",  SendMessageOptions.DontRequireReceiver);
        }
    }
Vector3 GetRandomPosition()
{
    // ランダムな位置を生成して返すメソッド
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

                // 半径内のすべてのCollider2Dを検出
                Collider2D[] colliders = Physics2D.OverlapCircleAll(newObject.transform.position, castRadius);

                obstacleHit = false;

                // 各Collider2Dに対して処理
                foreach (Collider2D collider in colliders)
                {
                    // タグが指定した障害物のタグと一致するか確認
                    if (collider.CompareTag("obstacle"))
                    {
                        obstacleHit = true;
                        Destroy(newObject);  // 障害物があれば生成したオブジェクトを破棄
                        break;  // 障害物が一つでも検出されたらループを抜ける
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
