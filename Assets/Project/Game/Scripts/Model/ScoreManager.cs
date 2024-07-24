using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// スコアマネージャー
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        // スコアのモデル
        [SerializeField] private Score _score;

        /// <summary>
        /// コンボ数に応じたスコアを加算する
        /// </summary>
        /// <param name="comboCount">コンボ数</param>
        public void AddScore(int comboCount)
        {
            // コンボ数に応じたスコアを計算する
            var score = 100 * comboCount * comboCount;
            // スコアを加算する
            _score.Add(score);
        }

        /// <summary>
        /// スコアをリセットする
        /// </summary>
        public void ResetScore()
        {
            _score.Reset();
        }
    }
}