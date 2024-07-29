using UnityEngine;
using UniRx;
using Assets.Project.Game.Scripts.Model;
using Assets.Project.Game.Scripts.View;

namespace Assets.Project.Game.Scripts.Presenter
{
    /// <summary>
    /// レベルのプレゼンター
    /// </summary>
    public class LevelPresenter : MonoBehaviour
    {
        // レベルモデル
        [SerializeField] private Level _level;
        // スコアモデル
        [SerializeField] private Score _score;
        // レベルのビュー
        [SerializeField] private LevelView _levelView;

        private void Start()
        {
            // 現在のレベルの値を監視して、レベルテキストに表示する
            _level.CurrentLevel
                .Subscribe(level =>
                {
                    // レベルに応じてスコアの倍率を変更する
                    _score.ChangeMultiplier(level);
                    // レベルテキストに表示する
                    _levelView.ShowLevel(level);
                }).AddTo(this);

            // 次のレベルに必要なスコアを監視して、レベルテキストに表示する
            _level.NextLevelScore
                .Subscribe(score =>
                {
                    _levelView.ShowNextLevelScore(score);
                }).AddTo(this);
        }
    }
}