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
        // 移動許可フラグ
        public bool isMoveAllowed = false;
        // ブロックのレイヤーマスク
        [SerializeField] private LayerMask blockLayer;

        void Update()
        {
            // ブロックが落下可能であれば1マス下に移動
            if (CanFall())
            {
                MoveDown();
            }
        }

        /// <summary>
        /// ブロックの左何マスが空白か
        /// </summary>
        public int EmptyCountBlockLeft()
        {
            int emptyCount = 0;

            while (true)
            {
                // チェックをする位置を計算
                Vector3 position = new(transform.position.x - (width * 0.5f + 0.05f) - emptyCount, transform.position.y, transform.position.z);

                if (position.x < 0)
                {
                    break;
                }

                // ブロックがある場合はループを抜ける
                if (Physics2D.OverlapPoint(position, blockLayer))
                {
                    break;
                }
                else
                {
                    emptyCount++;
                }
            }

            return emptyCount;
        }

        /// <summary>
        /// ブロックの右に何マスが空白があるか
        /// </summary>
        public int EmptyCountBlockRight()
        {
            int emptyCount = 0;

            while (true)
            {
                // チェックをする位置を計算
                Vector3 position = new(transform.position.x + (width * 0.5f + 0.05f) + emptyCount, transform.position.y, transform.position.z);

                if (position.x > 7)
                {
                    break;
                }

                // ブロックがある場合はループを抜ける
                if (Physics2D.OverlapPoint(position, blockLayer))
                {
                    break;
                }
                else
                {
                    emptyCount++;
                }
            }

            return emptyCount;
        }

        /// <summary>
        /// 1マス下に移動する
        /// </summary>
        private void MoveDown()
        {
            // ブロックを1マス下に移動
            transform.DOMoveY(transform.position.y - 1, 0.1f);
        }

        /// <summary>
        /// ブロックは落下可能か
        /// 以下の条件をすべて満たす場合にtrueを返す
        /// ・下にブロックがない
        /// ・オブジェクトのy座標が整数値である
        /// ・オブジェクトがグリッドの2段目(y=1)以上にある
        /// ・移動許可フラグが立っている
        /// </summary>
        /// <returns>ブロックは落下可能か</returns>
        private bool CanFall()
        {
            // オブジェクトのy座標が整数値であるか
            bool isOnGrid = transform.position.y % 1 == 0;
            // オブジェクトがグリッドの2段目(y=1)以上にあるか
            bool isAboveFirstRow = transform.position.y >= 1;

            return IsEmptyBlockUnder() && isOnGrid && isAboveFirstRow && isMoveAllowed;
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
    }
}