using UniRx;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// スコア
    /// </summary>
    public class Score : MonoBehaviour
    {
        /// <summary>
        /// 現在のスコア
        /// </summary>
        public ReactiveProperty<int> CurrentScore { get; } = new ReactiveProperty<int>();

        /// <summary>   
        /// スコアの倍率
        /// </summary>
        public ReactiveProperty<int> ScoreMultiplier { get; } = new ReactiveProperty<int>();

        /// <summary>
        /// スコアを加算する
        /// </summary>
        /// <param name="score">加算するスコア</param>
        public void Add(int score)
        {
            // スコアを加算する
            CurrentScore.Value += score;
        }

        /// <summary>
        /// スコアの倍率を変更する
        /// </summary>
        /// <param name="multiplier">倍率</param>
        public void ChangeMultiplier(int multiplier)
        {
            ScoreMultiplier.Value = multiplier;
        }

        /// <summary>
        /// スコアをリセットする
        /// </summary>
        public void Reset()
        {
            CurrentScore.Value = 0;
        }
    }
}