using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    NavMeshAgent2D agent; //NavMeshAgent2Dを使用するための変数
    [SerializeField] Transform target; //追跡するターゲット

    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
    }

    void Update()
    {
        agent.destination = target.position; //agentの目的地をtargetの座標にする
        //agent.SetDestination(target.position); //こっちの書き方でもオッケー
    }
}
