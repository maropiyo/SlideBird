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
        /// スコアを加算する
        /// </summary>
        /// <param name="score">加算するスコア</param>
        public void Add(int score)
        {
            CurrentScore.Value += score;
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