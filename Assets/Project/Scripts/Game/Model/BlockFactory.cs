using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブロックの生成を行うクラス
/// </summary>
public class BlockFactory : MonoBehaviour
{
    // 横幅1のブロックのプレハブリスト
    [SerializeField] private List<GameObject> width1BlockPrefabs;
    // 横幅2のブロックのプレハブリスト
    [SerializeField] private List<GameObject> width2BlockPrefabs;
    // 横幅3のブロックのプレハブリスト
    [SerializeField] private List<GameObject> width3BlockPrefabs;
    // 横幅4のブロックのプレハブリスト
    [SerializeField] private List<GameObject> width4BlockPrefabs;

    /// <summary>
    /// 1行分のブロックを生成する
    /// 1~4の横幅のブロックとブロックを置かない範囲を合わせて横幅が8になるようにブロックを生成する
    /// </summary>
    /// <param name="columns">列数</param>
    /// <returns>生成したブロックのリスト</returns>
    public List<GameObject> GenerateRowBlocks(int columns)
    {
        // 空白マスの数を1~4の範囲でランダムに決定する
        int emptyBlockCount = Random.Range(1, 5);

        // 利用可能な横幅を計算する
        int availableWidth = columns - emptyBlockCount;

        // 横幅の合計が利用可能な横幅になるように複数のブロックを選択する
        List<GameObject> blockPrefabs = SelectBlockPrefabs(availableWidth);

        // 空白マスを含めた1列のブロックを生成する
        return GenerateRowBlocksIncludingEmptySpaces(blockPrefabs, emptyBlockCount);
    }

    /// <summary>
    /// 横幅の合計が利用可能な横幅になるように複数のブロックを選択する
    /// </summary>
    /// <param name="availableWidth">横幅の合計</param>
    /// <returns>選択されたブロックのリスト</returns>
    private List<GameObject> SelectBlockPrefabs(int availableWidth)
    {
        // 選択されたブロックのリスト
        List<GameObject> selectedBlockPrefabs = new();

        // 1~4の横幅のブロックのプレハブリストからランダムに1つ取得
        while (availableWidth > 0)
        {
            // ランダムにブロックの横幅を決定
            int width = Random.Range(1, 5);

            // 横幅が残りのマス数を超える場合はスキップ
            if (width > availableWidth)
            {
                continue;
            }

            // 決定した横幅のブロックを選択されたブロックのリストに追加
            switch (width)
            {
                // 横幅が1の場合
                case 1:
                    selectedBlockPrefabs.Add(width1BlockPrefabs[Random.Range(0, width1BlockPrefabs.Count)]);
                    break;
                // 横幅が2の場合
                case 2:
                    selectedBlockPrefabs.Add(width2BlockPrefabs[Random.Range(0, width2BlockPrefabs.Count)]);
                    break;
                // 横幅が3の場合
                case 3:
                    selectedBlockPrefabs.Add(width3BlockPrefabs[Random.Range(0, width3BlockPrefabs.Count)]);
                    break;
                // 横幅が4の場合
                case 4:
                    selectedBlockPrefabs.Add(width4BlockPrefabs[Random.Range(0, width4BlockPrefabs.Count)]);
                    break;
            }

            // 利用可能な横幅から取得したブロックの横幅を引く
            availableWidth -= width;
        }

        return selectedBlockPrefabs;
    }


    /// <summary>
    /// 空白マスを含めた1列のブロックを生成する
    /// </summary>
    /// <param name="blockPrefabs">ブロックのプレハブリスト</param>
    /// <param name="emptySpaceCount">空白マスの数</param>
    /// <returns>生成したブロックのリスト</returns>
    private List<GameObject> GenerateRowBlocksIncludingEmptySpaces(List<GameObject> blockPrefabs, int emptySpaceCount)
    {
        // 現在の列の位置
        float currentXPos = 0;
        // ブロックの個数
        int blockCount = blockPrefabs.Count;
        // 生成したブロックのリスト
        List<GameObject> generatedBlocks = new();

        // ブロックの個数が0と空白マスの数が0になるまで繰り返す
        while (blockCount > 0 || emptySpaceCount > 0)
        {
            // ランダムにブロックを生成するか空白マスを生成するか決定
            bool isBlock = Random.Range(0, 2) == 0;

            // ブロックのプレハブリストからランダムにブロックを生成する
            if (isBlock && blockPrefabs.Count > 0)
            {
                // ブロックのプレハブリストからランダムに1つ取得
                GameObject blockPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Count)];

                // 取得したプレファブをリストから削除
                blockPrefabs.Remove(blockPrefab);

                // ブロックの横幅を取得
                float width = blockPrefab.GetComponent<Block>().width;

                // ブロックの横幅に合わせてX座標を計算
                float XPos = currentXPos + (width / 2) - 0.5f;

                // ブロックのプレファブをゲームオブジェクトとして生成
                GameObject block = Instantiate(blockPrefab, new Vector3(XPos, 0, 0), Quaternion.identity);

                // ブロックをボードの子要素にする
                block.transform.parent = transform;

                // 生成したブロックをリストに追加
                generatedBlocks.Add(block);

                // 現在の位置を更新
                currentXPos += width;

                // ブロックの個数を減らす
                blockCount--;
            }

            // 空白マスを生成する
            if (!isBlock && emptySpaceCount > 0)
            {
                // 現在のX座標を1マス分進める
                currentXPos++;

                // 空白マスの数を減らす
                emptySpaceCount--;
            }
        }

        return generatedBlocks;
    }
}