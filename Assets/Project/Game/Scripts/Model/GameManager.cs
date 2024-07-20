using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
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

        // 生成中か
        public bool isGenerating = false;
        // 操作中のブロック
        private GameObject holdingBlock;
        // タップ位置
        Vector3 tapPosition = Vector3.zero;
        // ブロックの位置調整用のオフセット
        Vector3 offset = Vector3.zero;
        // 移動範囲の最小値
        private float minX = 0;
        // 移動範囲の最大値
        private float maxX = 7;

        void Start()
        {
            // ボードを生成する
            board.GenerateBoard();

            // 次にボードに追加するブロックを生成
            GenerateNextRowBlocks();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                DropCurrentBlocks();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                GenerateBlocks();
            }

            MoveBlock();
        }

        /// <summary>
        /// ブロックを左右に移動する
        /// </summary>
        private void MoveBlock()
        {
            // マウスの位置を取得
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // マウスクリックの開始時にブロックを掴む
            if (Input.GetMouseButtonDown(0))
            {
                // クリック位置
                tapPosition = mousePosition;

                // 落下フラグをfalseにする
                foreach (var block in currentBlocks)
                {
                    // ブロックの落下フラグをfalseにする
                    block.GetComponent<Block>().isFalling = false;
                }

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                // ブロックがある場合
                if (hit.collider != null && hit.collider.CompareTag("Block"))
                {
                    // 操作中のブロックを取得
                    holdingBlock = hit.collider.gameObject;

                    // ブロックの位置調整用のオフセットを計算
                    offset = holdingBlock.transform.position - mousePosition;

                    // 可動範囲の最小値を設定
                    minX = tapPosition.x + offset.x - holdingBlock.GetComponent<Block>().EmptyCountBlockLeft();
                    // 可動範囲の最大値を設定
                    maxX = tapPosition.x + offset.x + holdingBlock.GetComponent<Block>().EmptyCountBlockRight();

                    Debug.Log(holdingBlock.GetComponent<Block>().EmptyCountBlockLeft() + "::" + holdingBlock.GetComponent<Block>().EmptyCountBlockRight());
                    Debug.Log(minX + "::" + maxX);
                }
            }

            // ブロックを掴んでいる間に左右に移動
            if (holdingBlock != null)
            {
                holdingBlock.transform.position = new Vector3(Mathf.Clamp(mousePosition.x + offset.x, minX, maxX), holdingBlock.transform.position.y, holdingBlock.transform.position.z);
            }

            // マウスを離した時にブロックを離す
            if (Input.GetMouseButtonUp(0) && holdingBlock != null)
            {
                // ブロックの位置を調整
                AdjustBlockPosition(holdingBlock);

                holdingBlock = null;

                DropCurrentBlocks();
            }
        }

        /// <summary>
        /// ブロックの位置を調整する
        /// </summary>
        /// <param name="block">調整するブロック</param>
        private void AdjustBlockPosition(GameObject block)
        {
            // ブロックの横幅
            int width = block.GetComponent<Block>().width;

            switch (width)
            {
                case 1:
                    // ブロックの位置を調整
                    block.transform.position = new Vector3(Mathf.Round(block.transform.position.x), block.transform.position.y, block.transform.position.z);
                    break;
                case 2:
                    // ブロックの位置を調整
                    block.transform.position = new Vector3(Mathf.Floor(block.transform.position.x) + 0.5f, block.transform.position.y, block.transform.position.z);
                    break;
                case 3:
                    // ブロックの位置を調整
                    block.transform.position = new Vector3(Mathf.Round(block.transform.position.x), block.transform.position.y, block.transform.position.z);
                    break;
                case 4:
                    // ブロックの位置を調整
                    block.transform.position = new Vector3(Mathf.Floor(block.transform.position.x) + 0.5f, block.transform.position.y, block.transform.position.z);
                    break;
            }
        }

        /// <summary>
        /// ブロックを生成する
        /// </summary>
        public void GenerateBlocks()
        {
            if (isGenerating)
            {
                return;
            }

            foreach (var block in currentBlocks)
            {
                // ブロックの落下フラグをfalseにする
                block.GetComponent<Block>().isFalling = false;
            }
            isGenerating = true;
            // ブロックをボードに追加
            AddBlocksToBoard();
        }

        /// <summary>
        /// 現在のブロックを落下させる
        /// </summary>
        public void DropCurrentBlocks()
        {
            if (isGenerating)
            {
                return;
            }

            foreach (var block in currentBlocks)
            {
                // ブロックの落下フラグをtrueにする
                block.GetComponent<Block>().isFalling = true;
            }
        }

        /// <summary>
        /// ブロックをボードに追加する
        /// </summary>
        private void AddBlocksToBoard()
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
    }
}
