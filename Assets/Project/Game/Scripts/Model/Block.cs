using DG.Tweening;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// ブロック
    /// </summary>
    public class Block : MonoBehaviour
    {
        // ブロックの横幅
        public int width;
        // 落下フラグ
        public bool isFalling = false;

        // ブロックのレイヤー
        [SerializeField] private LayerMask blockLayer;

        // ホールド中か
        public bool isHolding;

        void Update()
        {
            // 下にブロックがない場合は1マス下に移動する
            if (IsEmptyBlockUnder() && transform.position.y > 0 && transform.position.y % 1 == 0 && isFalling)
            {
                MoveBlockDown();
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
                Vector3 position = new(initialX + x, transform.position.y - 0.55f, transform.position.z);

                // ブロックがある場合はfalseを返す
                if (Physics2D.OverlapPoint(position, blockLayer))
                {
                    return false;
                }
            }
            return true;
        }
    }
}