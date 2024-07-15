using DG.Tweening;
using UnityEngine;

/// <summary>
/// ブロック
/// </summary>
public class Block : MonoBehaviour
{
    // ブロックの横幅
    public int width;
    // 落下フラグ
    public bool isFalling;

    // ブロックのレイヤー
    [SerializeField] private LayerMask blockLayer;

    void Update()
    {
        // 下にブロックがない場合は1マス下に移動する
        if (IsEmptyBlockUnder() && transform.position.y > 0 && transform.position.y % 1 == 0 && isFalling)
        {
            MoveBlockDown();
        }
    }

    private void MoveBlockDown()
    {
        transform.DOMoveY(transform.position.y - 1, 0.1f);
    }

    /// <summary>
    /// ブロックの下が空白か
    /// </summary>
    /// <param name="block">判定するブロック</param>
    /// <returns>ブロックの下が空白か</returns>
    private bool IsEmptyBlockUnder()
    {
        // ブロックの横幅分下に空白があるかチェック
        for (int x = 0; x < width; x++)
        {
            // X座標の初期位置を計算
            float initialX = transform.position.x - (width * 0.5f - 0.5f);

            // チェックをする位置を計算
            Vector3 position = new(initialX + x, transform.position.y - 0.51f, transform.position.z);

            // ブロックがある場合はfalseを返す
            if (Physics2D.OverlapPoint(position, blockLayer))
            {
                return false;
            }
        }
        return true;
    }
}
