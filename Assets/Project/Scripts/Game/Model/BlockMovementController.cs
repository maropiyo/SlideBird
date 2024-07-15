using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブロックの移動を制御するクラス
/// </summary>
public class BlockMovementController : MonoBehaviour
{
    /// <summary>
    /// ブロックを１マス下に移動する
    /// </summary>
    /// <param name="block">移動するブロック</param>
    public void MoveBlockDown(GameObject block)
    {
        block.transform.position += new Vector3(0, -1, 0);
    }

    /// <summary>
    /// ブロックのリストを１マス上に移動する
    /// </summary>
    /// <param name="blocks">ブロックのリスト</param>
    public void MoveBlocksUp(List<GameObject> blocks)
    {
        // ブロックのリストを１マス上に移動
        foreach (var block in blocks)
        {
            block.transform.position += new Vector3(0, 1, 0);
        }
    }
}
