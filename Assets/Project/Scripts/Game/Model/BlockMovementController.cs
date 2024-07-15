using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ブロック移動を制御するクラス
/// </summary>
public class BlockMovementController : MonoBehaviour
{
    /// <summary>
    /// ブロックのリストを１マス上に移動する
    /// </summary>
    /// <param name="blocks">ブロックのリスト</param>
    public void MoveBlocksUp(List<GameObject> blocks)
    {
        // ブロックのリストを１マス上に移動
        foreach (var block in blocks)
        {
            block.transform.DOLocalMoveY(block.transform.position.y + 1, 0.1f);
        }
    }
}
