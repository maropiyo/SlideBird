using UnityEngine;

namespace Assets.Project.Game.Scripts.View
{
    /// <summary>
    /// スコアテキストのビュー
    /// </summary>
    public class ScoreTextView : MonoBehaviour
    {
        // スコアテキスト
        [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

        /// <summary>
        /// スコアを表示する
        /// </summary>
        /// <param name="score">スコア</param>
        public void ShowScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}
