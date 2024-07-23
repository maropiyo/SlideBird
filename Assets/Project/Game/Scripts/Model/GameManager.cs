using UnityEngine;

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

        async void Start()
        {
            // ブロックの初期化処理を行う
            await blockController.Initialize();
        }

        /// <summary>
        /// ゲームオーバー処理
        /// </summary>
        public void GameOver()
        {
            // TODO: ゲームオーバー処理を実装する
        }
    }
}