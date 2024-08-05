using System.Collections.Generic;
using Assets.Project.Common.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// ブロックの制御クラス
    /// ボードとブロックの生成、移動、削除などの処理を行う
    /// </summary>
    public class BlockController : MonoBehaviour
    {
        // ゲームマネージャー
        [SerializeField] private GameManager gameManager;
        // スコアマネージャー
        [SerializeField] private ScoreManager scoreManager;
        // ボード
        [SerializeField] private Board board;
        // ブロックジェネレーター
        [SerializeField] private BlockGenerator blockGenerator;
        // メインカメラ
        [SerializeField] private Camera mainCamera;
        // ブロックのレイヤーマスク
        [SerializeField] private LayerMask blockLayer;

        // 現在のブロックリスト
        private readonly List<GameObject> currentBlocks = new();
        // 次に生成するブロックリスト
        private List<GameObject> nextBlocks;
        // ブロックが移動中か
        private bool isMoving = false;

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

        async void Update()
        {
            // ブロックを持っていない状態でタップしたとき
            if (Input.GetMouseButtonDown(0) && holdingBlock == null && !isMoving)
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
            if (holdingBlock != null && !isMoving)
            {
                holdingBlock.transform.position = new Vector3(Mathf.Clamp(mainCamera.ScreenToWorldPoint(Input.mousePosition).x + offsetX, minX, maxX), holdingBlock.transform.position.y, holdingBlock.transform.position.z);
            }

            // ブロックを離したとき
            if (Input.GetMouseButtonUp(0) && holdingBlock != null && !isMoving)
            {
                // ブロックの移動が終了したときの処理を実行
                await OnMoveEnd();
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public async UniTask Initialize()
        {
            // ボードを生成する
            board.GenerateBoard();

            // ブロックのセットアップ
            await SetupBlocks();
        }

        /// <summary>
        /// ブロックのセットアップ
        /// </summary>
        public async UniTask SetupBlocks()
        {
            // 次にボードに追加するブロックを生成
            GenerateNextRowBlocks();

            // 0.5秒待機
            await UniTask.Delay(500);

            // ブロックの移動中フラグをtrueにする
            isMoving = true;

            // ブロックを３段分追加
            await PushNextBlocks();
            await PushNextBlocks();
            await PushNextBlocks();

            // ブロックを落下させる
            await FallBlocks();

            // 横一列が揃っているブロックがなくなるまで削除と落下を繰り返す
            while (await LineClear())
            {
                // ブロックを落下させる
                await FallBlocks();
            }

            // ブロックの移動中フラグをfalseにする
            isMoving = false;
        }

        /// <summary>
        /// ブロックの移動が終了したときの処理
        /// </summary>
        private async UniTask OnMoveEnd()
        {
            // ブロックの移動中フラグをtrueにする
            isMoving = true;

            // ブロックのX座標を調整
            AdjustBlockXPosition(holdingBlock);

            // 初期位置から移動している場合
            if (initialHoldingBlockPosition != holdingBlock.transform.position)
            {
                // ブロックを離す
                holdingBlock = null;

                // 0.1秒待機
                await UniTask.Delay(100);

                // ブロックを落下させる
                await FallBlocks();

                // 横一列が揃っているブロックがなくなるまで削除と落下を繰り返す
                while (await LineClear())
                {
                    // ブロックを落下させる
                    await FallBlocks();
                }

                // ブロックを追加する
                await PushNextBlocks();

                // ブロックを落下させる
                await FallBlocks();

                // 横一列が揃っているブロックがなくなるまで削除と落下を繰り返す
                while (await LineClear())
                {
                    // ブロックを落下させる
                    await FallBlocks();

                    // ブロックを追加する
                    await PushNextBlocks();

                    // ブロックを落下させる
                    await FallBlocks();
                }

                // コンボ数をリセット
                scoreManager.ResetCombo();

                // ブロックのY座標が9以上の場合はゲームオーバー処理を行う
                if (currentBlocks.Exists(block => block.transform.position.y >= 9))
                {
                    // ゲームオーバー処理
                    gameManager.GameOver();
                }
            }
            // ブロックを離す
            holdingBlock = null;

            // ブロックの移動中フラグをfalseにする
            isMoving = false;
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

            SoundManager.Instance.PlaySound(SoundType.BlockUp);

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

        /// <summary>
        /// 横一列が揃っているブロックを削除する
        /// </summary>
        /// <returns>削除したか</returns>
        private async UniTask<bool> LineClear()
        {
            // 削除したか
            bool isCleared = false;

            // ボードの行数分ループ
            for (int y = 0; y < board.rows; y++)
            {
                // ブロックのリスト
                HashSet<GameObject> blockList = new();

                // ボードの列数分ループ
                for (int x = 0; x < board.columns; x++)
                {
                    // 確認する位置
                    Vector3 checkPosition = new(x, y, 0);

                    // ブロックがない場合は次の行へ
                    if (Physics2D.OverlapPoint(checkPosition, blockLayer))
                    {
                        GameObject hitBlock = Physics2D.OverlapPoint(checkPosition, blockLayer).gameObject;

                        // ブロックがある場合はリストに追加(HashSetなので重複はしない)
                        blockList.Add(hitBlock);

                        // もし最後の列までブロックがある場合はブロックを削除する。
                        if (x == board.columns - 1)
                        {
                            // ポイントの合計
                            int point = 0;

                            // タスクのリストを生成する
                            List<UniTask> tasks = new();

                            // コンボ数を加算
                            scoreManager.PlusCombo();

                            // ブロックを削除
                            foreach (var blockObject in blockList)
                            {
                                // ブロックのスクリプトを取得
                                Block block = blockObject.GetComponent<Block>();

                                // ポイントを加算
                                point += block.point;

                                // ブロックを削除するタスクをリストに追加
                                tasks.Add(block.DestroyBlock());

                                // 現在のブロックリストから削除
                                currentBlocks.Remove(blockObject);
                            }

                            SoundManager.Instance.PlaySound(SoundType.LineClear);

                            // すべてのタスクが完了するまで待機
                            await UniTask.WhenAll(tasks);

                            // スコアを加算
                            scoreManager.AddScore(point);

                            // ブロックを削除したので削除フラグをtrueにする
                            isCleared = true;
                        }
                    }
                    else
                    {
                        // ブロックがない場合は次の行へ
                        break;
                    }
                }
            }

            return isCleared;
        }

        /// <summary>
        /// 全てのブロックを削除する
        /// </summary>
        public async UniTask DestroyAllBlocks()
        {
            // タスクのリストを生成する
            List<UniTask> tasks = new();

            // ブロックを削除
            foreach (var block in currentBlocks)
            {
                tasks.Add(block.GetComponent<Block>().DestroyBlock());
            }

            foreach (var block in nextBlocks)
            {
                tasks.Add(block.GetComponent<Block>().DestroyBlock());
            }

            // すべてのタスクが完了するまで待機
            await UniTask.WhenAll(tasks);

            // 現在のブロックリストをクリア
            currentBlocks.Clear();
        }
    }
}
