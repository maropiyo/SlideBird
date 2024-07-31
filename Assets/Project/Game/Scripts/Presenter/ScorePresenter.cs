
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
        // レベルのモデル
        [SerializeField] private Level _level;
        // スコアテキストのビュー
        [SerializeField] private ScoreView _scoreView;

        private void Start()
        {
            // 現在のスコアの値を監視して、スコアテキストに表示する
            _score.CurrentScore
                .Subscribe(score =>
                {
                    _scoreView.ShowScore(score);

                    // レベルアップするかどうかを判定する
                    if (_level.IsLevelUp(score))
                    {
                        // レベルアップする
                        _level.LevelUp();
                        float prevScore = (_level.CurrentLevel.Value - 1) * (_level.CurrentLevel.Value - 1) * 100;
                        float fillAmount = (score - prevScore) / (_level.NextLevelScore.Value - prevScore);
                        _scoreView.UpdateExpGauge(fillAmount, true);
                    }
                    else
                    {

                        float prevScore = (_level.CurrentLevel.Value - 1) * (_level.CurrentLevel.Value - 1) * 100;
                        float fillAmount = (score - prevScore) / (_level.NextLevelScore.Value - prevScore);
                        _scoreView.UpdateExpGauge(fillAmount, false);
                    }
                }).AddTo(this);

            // 現在のレベルの値を監視して、レベルテキストに表示する
            _level.CurrentLevel
                .Subscribe(level =>
                {
                    // レベルに応じてスコアの倍率を変更する
                    _score.ChangeMultiplier(level);
                    // レベルテキストに表示する
                    _scoreView.ShowLevel(level);
                }).AddTo(this);

            // スコアの倍率を監視して、スコアの倍率テキストに表示する
            _score.ScoreMultiplier
                .Subscribe(multiplier =>
                {
                    _scoreView.ShowScoreMultiplier(multiplier);
                }).AddTo(this);
        }
    }
}