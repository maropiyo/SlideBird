using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// ブロックの制御クラス
    /// </summary>
    public class BlockController : MonoBehaviour
    {
        // ボード
        [SerializeField] private Board board;
        // ブロックの生成クラス
        [SerializeField] private BlockGenerator blockGenerator;
        // メインカメラ
        [SerializeField] private Camera mainCamera;

        // 現在のブロックリスト
        private readonly List<GameObject> currentBlocks = new();
        // 次に生成するブロックリスト
        private List<GameObject> nextBlocks;

        // 操作中のブロック
        private GameObject holdingBlock;
        // タップ位置
        private Vector3 tapPosition = Vector3.zero;
        // ブロックの位置調整用のオフセット
        private float offsetX = 0;
        // ブロックの可動範囲のX最小値
        private float minX = 0;
        // ブロックの可動範囲のX最大値
        private float maxX = 0;

        void Start()
        {
            // ボードを生成する
            board.GenerateBoard();

            // 次にボードに追加するブロックを生成
            GenerateNextRowBlocks();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                // ブロックをボードに追加する
                PushNextBlocks();
            }

            // ブロックを持っていない状態でタップしたとき
            if (Input.GetMouseButtonDown(0) && holdingBlock == null)
            {
                // タップ位置を取得
                tapPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                // マウスの位置にあるオブジェクトを取得
                RaycastHit2D hit = Physics2D.Raycast(tapPosition, Vector2.zero);

                // ブロックがある場合
                if (hit.collider != null && hit.collider.CompareTag("Block") && hit.collider.gameObject.transform.position.y >= 0)
                {
                    // ブロックの落下を禁止する
                    SetBlockFallAllowed(false);
                    // ブロックを掴む
                    holdingBlock = hit.collider.gameObject;
                    // ブロックの座標とタップ位置のオフセットを計算
                    offsetX = holdingBlock.transform.position.x - tapPosition.x;
                    // ブロックの可動範囲のX最小値を設定
                    minX = tapPosition.x + offsetX - holdingBlock.GetComponent<Block>().EmptyCountBlockLeft();
                    // ブロックの可動範囲のX最大値を設定
                    maxX = tapPosition.x + offsetX + holdingBlock.GetComponent<Block>().EmptyCountBlockRight();
                }
            }

            // ブロックを掴んでいる間に左右に移動
            if (holdingBlock != null)
            {
                holdingBlock.transform.position = new Vector3(Mathf.Clamp(mainCamera.ScreenToWorldPoint(Input.mousePosition).x + offsetX, minX, maxX), holdingBlock.transform.position.y, holdingBlock.transform.position.z);
            }

            // ブロックを離したとき
            if (Input.GetMouseButtonUp(0) && holdingBlock != null)
            {
                // ブロックのX座標を調整
                AdjustBlockXPosition(holdingBlock);

                // ブロックを離す
                holdingBlock = null;

                // ブロックの落下を許可
                SetBlockFallAllowed(true);
            }
        }

        /// <summary>
        /// 次のブロックをボードに追加する
        /// </summary>
        private void PushNextBlocks()
        {
            // 次のブロックがnullか0の場合は処理を抜ける
            if (nextBlocks == null || nextBlocks.Count == 0)
            {
                return;
            }

            // 現在のブロックが落下しないようにする
            SetBlockFallAllowed(false);

            // ブロックをボードに追加する
            foreach (var block in nextBlocks)
            {
                // ブロックをボードの子要素にする
                block.transform.parent = board.transform;

                // ブロックの位置を調整
                block.transform.position += new Vector3(0, 0.1f, 0);
            }

            // 現在のブロックリストに追加
            currentBlocks.AddRange(nextBlocks);

            // 次のブロックリストをクリア
            nextBlocks.Clear();

            // 現在のブロックを１マス上に移動
            MoveBlocksUp(currentBlocks);

            // 次のブロックを生成
            GenerateNextRowBlocks();

            // ブロックの落下を許可
            SetBlockFallAllowed(true);
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
        /// ゲーム上のブロックの落下許可フラグを設定する
        /// </summary>
        /// <param name="isFallAllowed">落下許可フラグ</param>
        private void SetBlockFallAllowed(bool isFallAllowed)
        {
            foreach (var block in currentBlocks)
            {
                block.GetComponent<Block>().isFallAllowed = isFallAllowed;
            }
        }

        /// <summary>
        /// ブロックのリストを１マス上に移動する
        /// </summary>
        /// <param name="blocks">ブロックのリスト</param>
        private void MoveBlocksUp(List<GameObject> blocks)
        {
            // ブロックのリストを１マス上に移動
            foreach (var block in blocks)
            {
                block.GetComponent<Block>().MoveUp();
            }
        }

        /// <summary>
        /// ブロックのX座標を調整する
        /// </summary>
        /// <param name="block">調整するブロック</param>
        private void AdjustBlockXPosition(GameObject block)
        {
            // ブロックの横幅
            int width = block.GetComponent<Block>().width;
            // 調整後のX座標
            float adjustedXPosition;

            if (width % 2 == 0)
            {
                // ブロックの横幅が偶数の場合は、切り捨てた値に0.5を足す
                adjustedXPosition = Mathf.Floor(block.transform.position.x) + 0.5f;
            }
            else // 奇数幅のブロックの場合
            {
                // // ブロックの横幅が奇数の場合は、四捨五入する
                adjustedXPosition = Mathf.Round(block.transform.position.x);
            }

            // ブロックの位置を更新
            block.transform.position = new Vector3(adjustedXPosition, block.transform.position.y, block.transform.position.z);
        }
    }
}
