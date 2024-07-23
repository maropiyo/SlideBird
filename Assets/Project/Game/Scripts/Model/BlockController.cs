using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        // 操作中のブロックの初期位置
        private Vector3 initialHoldingBlockPosition = Vector3.zero;
        // タップ位置
        private Vector3 tapPosition = Vector3.zero;
        // ブロックの位置調整用のオフセット
        private float offsetX = 0;
        // ブロックの可動範囲のX最小値
        private float minX = 0;
        // ブロックの可動範囲のX最大値
        private float maxX = 0;

        async void Start()
        {
            // ボードを生成する
            board.GenerateBoard();

            // 次にボードに追加するブロックを生成
            GenerateNextRowBlocks();

            // 0.5秒待機
            await UniTask.Delay(500);

            // ブロックを追加
            await PushNextBlocks();
            // ブロックを追加
            await PushNextBlocks();
            // ブロックを追加
            await PushNextBlocks();

            // ブロックを落下させる
            await FallBlocks();
        }

        async void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                // ブロックをボードに追加する
                await PushNextBlocks();
                // ブロックを落下させる
                await FallBlocks();
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
                    // ブロックを掴む
                    holdingBlock = hit.collider.gameObject;
                    // 操作中のブロックの初期位置を設定
                    initialHoldingBlockPosition = holdingBlock.transform.position;
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
                // ブロックの移動が終了したときの処理を実行
                OnMoveEnd();
            }
        }

        /// <summary>
        /// ブロックの移動が終了したときの処理
        /// </summary>
        private async void OnMoveEnd()
        {
            // ブロックのX座標を調整
            AdjustBlockXPosition(holdingBlock);

            // 初期位置から移動している場合
            if (initialHoldingBlockPosition != holdingBlock.transform.position)
            {
                // ブロックを離す
                holdingBlock = null;

                // ブロックを落下させる
                await FallBlocks();

                // ブロックを追加する
                await PushNextBlocks();

                // ブロックを落下させる
                await FallBlocks();
            }
            // ブロックを離す
            holdingBlock = null;
        }


        /// <summary>
        /// 次のブロックをボードに追加する
        /// </summary>
        private async UniTask PushNextBlocks()
        {
            // 次のブロックがnullか0の場合は処理を抜ける
            if (nextBlocks == null || nextBlocks.Count == 0)
            {
                return;
            }

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

            // 次のブロックを生成
            GenerateNextRowBlocks();

            // 現在のブロックを１マス上に移動
            await MoveBlocksUp(currentBlocks);
        }

        /// <summary>
        /// 落下可能なブロックを落下させる
        /// </summary>
        /// <returns></returns>
        async UniTask FallBlocks()
        {
            // 落下可能なブロックがある間はループする
            while (currentBlocks.Exists(block => block.GetComponent<Block>().CanFall()))
            {
                // タスクのリストを作成
                List<UniTask> tasks = new();

                // 落下可能なブロックのMoveDownタスクをリストに追加して落下させる
                tasks.AddRange(currentBlocks.FindAll(block => block.GetComponent<Block>().CanFall()).ConvertAll(block => block.GetComponent<Block>().MoveDown()));

                // すべてのタスクが完了するまで待機
                await UniTask.WhenAll(tasks);
            }
        }

        /// <summary>
        /// 次にボードに追加するブロックを生成する
        /// </summary>
        private void GenerateNextRowBlocks()
        {
            // 次に生成するブロックリストがある場合は削除
            nextBlocks?.Clear();

            // 次に生成するブロックリストを取得
            nextBlocks = blockGenerator.GenerateRowBlocks(board.columns);
        }

        /// <summary>
        /// ブロックのリストを１マス上に移動する
        /// </summary>
        /// <param name="blocks">ブロックのリスト</param>
        private async UniTask MoveBlocksUp(List<GameObject> blocks)
        {
            // タスクのリストを作成
            List<UniTask> tasks = new List<UniTask>();

            // 各ブロックのMoveUpタスクをリストに追加
            foreach (var block in blocks)
            {
                tasks.Add(block.GetComponent<Block>().MoveUp());
            }

            // すべてのタスクが完了するまで待機
            await UniTask.WhenAll(tasks);
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
