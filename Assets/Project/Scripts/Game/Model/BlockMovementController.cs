using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブロックの移動を制御するクラス
/// </summary>
public class BlockMovementController : MonoBehaviour
{
    /// <summary>
    /// ブロックを下に移動する
    /// </summary>
    /// <param name="distance">移動距離</param>
    public void MoveBlockDown(int distance)
    {
        // ブロックを下に移動
        transform.position -= new Vector3(0, distance, 0);
    }
}
