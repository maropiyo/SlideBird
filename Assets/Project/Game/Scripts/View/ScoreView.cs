using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Project.Game.Scripts.View
{
    /// <summary>
    /// スコアのビュー
    /// </summary>
    public class ScoreView : MonoBehaviour
    {
        // 現在のスコアを表示するテキスト
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        // スコアの倍率を表示するテキスト
        [SerializeField] private TextMeshProUGUI _scoreMultiplierText;

        /// <summary>
        /// スコアを表示する
        /// </summary>
        /// <param name="score">スコア</param>
        public void ShowScore(int score)
        {
            // スコアまでカウントアップのアニメーションを再生する
            DOTween.To(() => int.Parse(_currentScoreText.text), x => _currentScoreText.text = x.ToString(), score, 0.5f);
        }

        /// <summary>
        /// スコアの倍率を表示する
        /// </summary>
        /// <param name="multiplier">倍率</param>
        public void ShowScoreMultiplier(int multiplier)
        {
            _scoreMultiplierText.text = "x" + multiplier;
        }
    }
}
