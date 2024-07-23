using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// ブロックのクラス
    /// </summary>
    public class Block : MonoBehaviour
    {
        // 横幅
        public int width;
        // ブロックのレイヤーマスク
        [SerializeField] private LayerMask blockLayer;

        /// <summary>
        /// ブロックの左に何マス空白があるか
        /// </summary>
        /// <returns>ブロックの左に何マス空白があるか</returns>
        public int EmptyCountBlockLeft()
        {
            return GetEmptyBlockCount(-1);
        }

        /// <summary>
        /// ブロックの右に何マスが空白があるか
        /// </summary>
        /// <returns>ブロックの右に何マスが空白があるか</returns>
        public int EmptyCountBlockRight()
        {
            return GetEmptyBlockCount(1);
        }

        /// <summary>
        /// 1マス上に移動する
        /// </summary>
        public async UniTask MoveUp()
        {
            await transform.DOMoveY(transform.position.y + 1, 0.3f).AsyncWaitForCompletion();
        }

        /// <summary>
        /// 落下可能であれば1マス下に移動する
        /// </summary>
        public async UniTask MoveDown()
        {
            if (CanFall())
            {
                await transform.DOMoveY(transform.position.y - 1, 0.1f).AsyncWaitForCompletion();
            }
        }

        /// <summary>
        /// ブロックは落下可能か
        /// 以下の条件をすべて満たす場合にtrueを返す
        /// ・下にブロックがない
        /// ・オブジェクトのy座標が整数値である
        /// ・オブジェクトがグリッドの2段目(y=1)以上にある
        /// </summary>
        /// <returns>ブロックは落下可能か</returns>
        public bool CanFall()
        {
            // オブジェクトのy座標が整数値であるか
            bool isOnGrid = transform.position.y % 1 == 0;
            // オブジェクトがグリッドの2段目(y=1)以上にあるか
            bool isAboveFirstRow = transform.position.y >= 1;

            return IsEmptyBlockUnder() && isOnGrid && isAboveFirstRow;
        }

        /// <summary>
        /// ブロックの下が空白か
        /// </summary>
        /// <returns>ブロックの下が空白か</returns>
        private bool IsEmptyBlockUnder()
        {
            // ブロックの横幅分ブロックの下に空白があるかチェックする
            for (int x = 0; x < width; x++)
            {
                // ブロックの先頭マスのX座標を取得 現在の座標-(ブロックの半分の幅+1マスの半分の幅)
                float blockStartPositionX = transform.position.x - (width * 0.5f) + 0.5f;

                // チェックをする位置を計算
                Vector3 checkPosition = new(blockStartPositionX + x, transform.position.y - 1.0f, transform.position.z);

                // ブロックがある場合はfalseを返す
                if (Physics2D.OverlapPoint(checkPosition, blockLayer))
                {
                    return false;
                }
            }
            // ブロックがない場合はtrueを返す
            return true;
        }

        /// <summary>
        /// ブロックの左右何マスが空白かを確認して返す
        /// </summary>
        /// <param name="direction">-1で左、1で右</param>
        /// <returns>空白のマス数</returns>
        private int GetEmptyBlockCount(int direction)
        {
            // 空白のマスの数
            int emptyBlockCount = 0;

            // チェックをする初期位置を計算
            Vector3 checkPosition = new(
                transform.position.x + direction * (width * 0.5f + 0.5f),
                transform.position.y,
                transform.position.z
            );

            // ボードの範囲内で確認する
            while (checkPosition.x >= 0 && checkPosition.x <= 7)
            {
                // ブロックがある場合はループを抜ける、空白の場合は空白のマス数を増やしてチェックをする位置を更新
                if (Physics2D.OverlapPoint(checkPosition, blockLayer))
                {
                    break;
                }
                else
                {
                    // 空白のマス数を増やす
                    emptyBlockCount++;
                    // チェックをする位置を更新
                    checkPosition.x += direction;
                }
            }

            return emptyBlockCount;
        }
    }
}