using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    public float totalTimeInSeconds = 30f; // 5分 = 300秒
    public string agentTag = "Agent";
    public string FinishTag = "Finish";

    private float elapsedTime = 0f;
    [SerializeField]
    private int FPS = 60;
    public float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.1f; // 実行時間を1/10にする
        Application.targetFrameRate = FPS;
    }

    // Update is called once per frame
    void Update()
    {
        // 経過時間を更新
        elapsedTime += Time.deltaTime;

        // 経過時間が指定の時間を超えたら終了
        if (elapsedTime >= totalTimeInSeconds)
        {
            // ゲーム終了の処理をここに記述
            Debug.Log("5分経過しました。ゲームを終了します。");
            // タグ名

        // タグが付いているゲームオブジェクトの配列を取得
        GameObject[] agents = GameObject.FindGameObjectsWithTag(agentTag);
        GameObject[] finishs = GameObject.FindGameObjectsWithTag(FinishTag);

        // 配列の要素数（ゲームオブジェクトの数）を表示
        Debug.Log($"Number of agents with tag '{agentTag}': {agents.Length}");
        Debug.Log($"Number of agents with tag '{FinishTag}': {finishs.Length}");
            // アプリケーションを終了
            Time.timeScale = time;
        }

    }
}
