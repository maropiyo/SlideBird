using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボードのクラス
/// </summary>
public class Board : MonoBehaviour
{
    // ボードの1マスを表すタイルのプレハブ
    [SerializeField] private GameObject tilePrefab;

    // ボードの行数, 列数
    public int rows = 10, columns = 8;

    void Start()
    {
        // ボードを生成する
        GenerateBoard();
    }

    /// <summary>
    /// ボードを生成する
    /// ボード上に定義した行数, 列数のタイルを生成する
    /// </summary>
    public void GenerateBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // プレハブからタイルのゲームオブジェクトを生成
                GameObject tile = Instantiate(tilePrefab, new Vector3(col, row, 0), Quaternion.identity);

                // タイルをボードの子要素にする
                tile.transform.parent = transform;
            }
        }
    }
}
