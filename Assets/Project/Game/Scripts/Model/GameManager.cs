using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// ゲームマネージャー
    /// ゲームの進行を管理する
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // ブロックのコントローラー
        [SerializeField] private BlockController blockController;
        // スコアマネージャー
        [SerializeField] private ScoreManager scoreManager;

        async void Start()
        {
            // ブロックの初期化処理を行う
            await blockController.Initialize();
        }

        /// <summary>
        /// ゲームオーバー処理
        /// </summary>
        public async void GameOver()
        {
            // ブロックを全て破棄する
            await blockController.DestroyAllBlocks();
            // スコアをリセットする
            scoreManager.ResetScore();
            // レベルをリセットする
            scoreManager.ResetLevel();
            // 初期化処理を行う
            await blockController.SetupBlocks();
        }
    }
}