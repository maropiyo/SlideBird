using UniRx;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// レベル
    /// </summary>
    public class Level : MonoBehaviour
    {
        /// <summary>
        /// 現在のレベル
        /// </summary>
        public ReactiveProperty<int> CurrentLevel { get; } = new ReactiveProperty<int>(1);
        /// <summary>
        /// 次のレベルに必要なスコア
        /// </summary>
        public ReactiveProperty<int> NextLevelScore { get; } = new ReactiveProperty<int>(100);

        /// <summary>
        /// レベルアップする
        /// </summary>
        public void LevelUp()
        {
            // 現在のレベルを1つ上げる
            CurrentLevel.Value++;

            // 次のレベルに必要なスコアを設定する
            NextLevelScore.Value = CurrentLevel.Value * CurrentLevel.Value * 100;
        }

        /// <summary>
        /// レベルをリセットする
        /// </summary>
        public void Reset()
        {
            // 現在のレベルをリセットする
            CurrentLevel.Value = 1;

            // 次のレベルに必要なスコアをリセットする
            NextLevelScore.Value = 100;
        }

        /// <summary>
        /// レベルアップするか
        /// </summary>
        /// <param name="score">スコア</param>
        /// <returns>レベルアップするか</returns>
        public bool IsLevelUp(int score)
        {
            return score >= NextLevelScore.Value;
        }
    }
}