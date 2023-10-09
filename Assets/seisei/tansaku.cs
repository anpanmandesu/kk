using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tansaku : MonoBehaviour
{
    public static tansaku instance;
    // Start is called before the first frame update
    void Awake()
    {

            if(instance == null)
        {
            instance = this;
        }
        

       // NavMeshSurface navMeshSurface = this.GetComponent<NavMeshSurface>();
        //navMeshSurface.buildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Vector3[] keirotansaku(Vector3 start, Vector3 goal)
    {
        NavMeshPath nav = new NavMeshPath();
        NavMesh.CalculatePath(start, goal, NavMesh.AllAreas, nav);
        return nav.corners;
    }
    
    ///<summary>経路計算メソッド</summary>
    public void keirokeisan(float x,float y,float gx, float gy){
    Vector3[] a = keirotansaku(new Vector3(x, y,0f), new Vector3(gx, gy,0f));
        for (int i = 0; i < a.Length; i++) {


            Debug.Log(a[i]);

            if (i == a.Length - 1) continue;
            Debug.DrawLine(a[i], a[i+1], Color.yellow,100);
        }
    }
}
