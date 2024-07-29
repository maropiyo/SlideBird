using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// スコアマネージャー
    /// スコアとコンボのモデルを管理する
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        // スコアのモデル
        [SerializeField] private Score _score;
        // コンボのモデル
        [SerializeField] private Combo _combo;
        // レベルのモデル
        [SerializeField] private Level _level;

        /// <summary>
        /// スコアを加算する
        /// </summary>
        /// <param name="point">ポイント</param>
        public void AddScore(int point)
        {
            // スコアを加算する(ポイント * コンボ数 * スコア倍率)
            _score.Add(point * _combo.CurrentCombo.Value * _score.ScoreMultiplier.Value);
            Debug.Log("ポイント: " + point + " コンボ: " + _combo.CurrentCombo.Value + " 倍率: " + _score.ScoreMultiplier.Value);
        }

        /// <summary>
        /// スコアをリセットする
        /// </summary>
        public void ResetScore()
        {
            _score.Reset();
        }

        /// <summary>
        /// コンボをプラスする
        /// </summary>
        public void PlusCombo()
        {
            _combo.Plus();
        }

        /// <summary>
        /// コンボをリセットする
        /// </summary>
        public void ResetCombo()
        {
            _combo.Reset();
        }

        /// <summary>
        /// レベルをリセットする
        /// </summary>
        public void ResetLevel()
        {
            _level.Reset();
        }
    }
}