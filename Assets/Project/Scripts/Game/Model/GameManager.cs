using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの管理クラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // ボード
    [SerializeField] private Board board;

    // ブロックの生成クラス
    [SerializeField] private BlockGenerator blockGenerator;

    // ブロックの移動クラス
    [SerializeField] private BlockMovementController blockMovementController;

    // 現在のブロックリスト
    private readonly List<GameObject> currentBlocks = new();

    // 次に生成するブロックリスト
    private List<GameObject> nextBlocks;

    void Start()
    {
        // ボードを生成する
        board.GenerateBoard();

        // 次にボードに追加するブロックを生成
        GenerateNextRowBlocks();
    }

    public void GenerateBlocks()
    {
        // ブロックをボードに追加
        AddBlocksToBoard();
    }

    /// <summary>
    /// 次にボードに追加するブロックを生成する
    /// </summary>
    void GenerateNextRowBlocks()
    {
        // 次に生成するブロックリストを取得
        nextBlocks = blockGenerator.GenerateRowBlocks(board.columns);
    }

    /// <summary>
    /// ブロックをボードに追加する
    /// </summary>
    void AddBlocksToBoard()
    {
        // ブロックをボードに追加
        foreach (var block in nextBlocks)
        {
            // ブロックをボードの子要素にする
            block.transform.parent = board.transform;

            // ブロックの位置を調整
            block.transform.position += new Vector3(0, 0.1f, 0);
        }

        // 現在のブロックリストに追加
        currentBlocks.AddRange(nextBlocks);

        // 現在のブロックを１マス上に移動
        blockMovementController.MoveBlocksUp(currentBlocks);

        // 次にボードに追加するブロックを生成
        GenerateNextRowBlocks();
    }
}