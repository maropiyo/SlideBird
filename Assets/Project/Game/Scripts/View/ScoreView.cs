using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        // レベルテキスト
        [SerializeField] private TextMeshProUGUI _levelText;
        // 経験値ゲージのイメージ
        [SerializeField] private Image _expGauge;

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

        /// <summary>
        /// レベルテキストを表示する
        /// </summary>
        /// <param name="level">レベル</param>
        public void ShowLevel(int level)
        {
            _levelText.text = "レベル" + level;
        }

        /// <summary>
        /// 経験値ゲージを更新する
        /// </summary>
        /// <param name="fillAmount">ゲージのパーセント</param>
        /// <param name="isLevelUp">レベルアップしたかどうか</param>
        public void UpdateExpGauge(float fillAmount, bool isLevelUp)
        {

            if (isLevelUp)
            {
                _expGauge.DOFillAmount(1, 0.5f * (1 - fillAmount)).OnComplete(() =>
                {
                    // ゲージをリセットしてから再生する
                    _expGauge.fillAmount = 0;
                    _expGauge.DOFillAmount(fillAmount, 0.5f * fillAmount);
                });
            }
            else
            {
                _expGauge.DOFillAmount(fillAmount, 0.5f);
            }
        }

        /// <summary>
        /// 経験値ゲージをリセットする
        /// </summary>
        public void ResetExpGauge()
        {
            _expGauge.fillAmount = 0;
        }
    }
}
