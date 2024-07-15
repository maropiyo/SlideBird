using System.Collections.Generic;
using Unity.VisualScripting;
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

    // ブロックのレイヤーマスク
    [SerializeField] private LayerMask blockLayer;

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

        GenerateBlocks();

        GenerateBlocks();

        GenerateBlocks();
    }

    void Update()
    {
        DropBlocks();
    }

    /// <summary>
    /// ブロックを生成する
    /// </summary>
    public void GenerateBlocks()
    {
        // ブロックをボードに追加
        AddBlocksToBoard();
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

    /// <summary>
    /// 次にボードに追加するブロックを生成する
    /// </summary>
    private void GenerateNextRowBlocks()
    {
        // 次に生成するブロックリストを取得
        nextBlocks = blockGenerator.GenerateRowBlocks(board.columns);
    }

    /// <summary>
    /// ドロップ可能なブロックを落下させる
    /// </summary>
    public void DropBlocks()
    {

        foreach (var block in currentBlocks)
        {
            // ブロックの下が空白の場合はブロックを下に移動
            while (IsEmptyBlockUnder(block))
            {
                blockMovementController.MoveBlockDown(block);
            }
        }
    }

    /// <summary>
    /// ブロックの下が空白か
    /// </summary>
    /// <param name="block">判定するブロック</param>
    /// <returns>ブロックの下が空白か</returns>
    private bool IsEmptyBlockUnder(GameObject block)
    {
        // ブロックの座標が0の場合はfalseを返す
        if (block.transform.position.y == 0)
        {
            return false;
        }

        // ブロックの横幅を取得
        float width = block.GetComponent<Block>().width;

        // ブロックの横幅分下に空白があるかチェック
        for (int x = 0; x < width; x++)
        {
            // X座標の初期位置を計算
            float initialX = block.transform.position.x - (width * 0.5f - 0.5f);

            // チェックをする位置を計算
            Vector3 position = new(initialX + x, block.transform.position.y - 1, block.transform.position.z);

            // ブロックがある場合はfalseを返す
            if (Physics2D.OverlapPointAll(position, blockLayer).Length > 0)
            {
                return false;
            }
        }
        return true;
    }
}
