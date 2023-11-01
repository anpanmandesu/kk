using UnityEngine;
using UnityEngine.AI;

public class yuudouin : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
    public float stoppingDistance = 0;
    public bool isTouched = false;//ぶつかったかどうかの判定
    float kakudo = -90f;
    public bool force = true;

    [HideInInspector]//常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;
    public Vector2 destination
    {
        get { return trace_area; }
        set
        {
            trace_area = value;
            Trace(transform.position, value);
        }
    }
    public bool SetDestination(Vector2 target)
    {
        destination = target;
        return true;
    }

    
    private void Trace(Vector2 current, Vector2 target)
    {
       if (force == true)
           {
                if (Vector2.Distance(current, target) <= stoppingDistance)
            {
                return;
            }

            // NavMesh に応じて経路を求める
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

            Vector2 corner = path.corners[0];

            if (Vector2.Distance(current, corner) <= 0.05f)
            {
                corner = path.corners[1];
            }
            for (int i = 0; i < path.corners.Length; i++)
            {
                //Debug.Log(path.corners[i]);

                if (i == path.corners.Length - 1) continue;
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);
                kakudo += (path.corners[i].x - path.corners[i + 1].x) * (path.corners[i].x - path.corners[i + 1].x) + (path.corners[i].y - path.corners[i + 1].y) * (path.corners[i].y - path.corners[i + 1].y);
                transform.rotation = Quaternion.Euler(0, 0, kakudo);//誘導員を移動方向に回転させる
            }
            transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);

            
        }
        force = false;
    }
    ///<summary>ぶつかったら消える</summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            isTouched = false;
        }
        force = true;
    }
}

