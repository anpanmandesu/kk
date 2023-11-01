using UnityEngine;

// オブジェクトの向きを取得する
public class CalcDirection : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        // transformに毎回アクセスすると重いので、キャッシュしておく
        _transform = transform;
    }

    private void Update()
    {
        // 正面・右・上向き
        var forward = _transform.forward;
        var right = _transform.right;
        var up = _transform.up;

        // 後ろ・左・下向き
        var back = -forward;
        var left = -right;
        var down = -up;

        // ログ表示
        //Debug.Log($"正面:{forward}, 右:{right}, 上:{up}, 後ろ{back}, 左{left}, 下{down}");
    }
}