using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private float Speed = 3f;//避難速度
    private bool isEvacuating = false; // 避難中かどうかのフラグ
    private Vector3 evacuationDestination; // 避難先の位置
    public bool isTouched = false;//ぶつかったかどうかの判定
 
    ///<summary>目的地</summary>
    public GameObject square; 
    ///<summary>いずれ目的地を座標で入れる時用</summary>
    [SerializeField]
    private float x = 17.5f;
    [SerializeField]
    private float y = 19f;


    void Start()
    {
        // エージェントの初期配置
        // CircleChild.csに初期位置
        
        tansaku.instance.keirokeisan(transform.position.x , transform.position.y , 17.5f, 19f);

        ///<summary>移動</summary>
        NavMeshAgent nav_mesh_agent = GetComponent<NavMeshAgent>();
        nav_mesh_agent.SetDestination(square.transform.position);
    }
     
    void Update()
    {
        if (isEvacuating)
        {
            // 避難中の処理
            //transform.position = Vector3.MoveTowards(transform.position, evacuationDestination, 0.02f);
            //Debug.Log(transform.position);

            if (transform.position == evacuationDestination)
            {
                // 避難先に到着したら避難終了
                //isEvacuating = false;
            }
        }


    }
    //ぶつかったら消える
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            isTouched = false;
        }

    }
}
