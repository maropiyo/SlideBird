using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Project.Game.Scripts.View
{
    /// <summary>
    /// レベルのビュー
    /// </summary>
    public class LevelView : MonoBehaviour
    {
        // レベルテキスト
        [SerializeField] private TextMeshProUGUI _levelText;
        // 次のレベルまでのスコアテキスト
        [SerializeField] private TextMeshProUGUI _nextLevelScoreText;

        /// <summary>
        /// レベルテキストを表示する
        /// </summary>
        /// <param name="level">レベル</param>
        public void ShowLevel(int level)
        {
            _levelText.text = "レベル" + level;
        }

        /// <summary>
        /// 次のレベルまでのスコアを表示する
        /// </summary>
        /// <param name="nextLevelScore">次のレベルまでのスコア</param>
        public void ShowNextLevelScore(int nextLevelScore)
        {
            _nextLevelScoreText.text = "NEXT: " + nextLevelScore;
        }
    }
}