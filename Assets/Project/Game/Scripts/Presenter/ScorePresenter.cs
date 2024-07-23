
using Assets.Project.Game.Scripts.Model;
using Assets.Project.Game.Scripts.View;
using UnityEngine;
using UniRx;

namespace Assets.Project.Game.Scripts.Presenter
{
    /// <summary>
    /// スコアのプレゼンター
    /// </summary>
    public class ScorePresenter : MonoBehaviour
    {
        // スコアのモデル
        [SerializeField] private Score _score;
        // スコアテキストのビュー
        [SerializeField] private ScoreTextView _scoreTextView;

        private void Start()
        {
            // 現在のスコアの値を監視して、スコアテキストに表示する
            _score.CurrentScore
                .Subscribe(score =>
                {
                    _scoreTextView.ShowScore(score);
                }).AddTo(this);
        }
    }
}