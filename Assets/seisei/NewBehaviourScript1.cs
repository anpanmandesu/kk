using UnityEngine;

public class Containtest : MonoBehaviour
{

    private void Start()
    {
        string str = "目の前にスライムが現れた。";
        string target = "スライム";

        if (str.Contains(target))
        {
            Debug.Log($"{target}が文章の中に含まれていました。");
        }
        else
        {
            Debug.Log($"{target}は文章の中に含まれていません。");
        }
    }

}