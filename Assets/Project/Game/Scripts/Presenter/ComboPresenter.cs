
using Assets.Project.Game.Scripts.Model;
using Assets.Project.Game.Scripts.View;
using UnityEngine;
using UniRx;

namespace Assets.Project.Game.Scripts.Presenter
{
    /// <summary>
    /// コンボのプレゼンター
    /// </summary>
    public class ComboPresenter : MonoBehaviour
    {
        // コンボのモデル
        [SerializeField] private Combo _combo;
        // コンボのビュー
        [SerializeField] private ComboView _comboTextView;

        private void Start()
        {
            // 現在のコンボの値が2以上場合、コンボテキストを表示する
            _combo.CurrentCombo
                .Subscribe(async comboCount =>
                {
                    if (comboCount > 1)
                    {
                        await _comboTextView.ShowComboTextAsync(comboCount);
                    }
                    else
                    {
                        _comboTextView.HideComboText();
                    }
                }).AddTo(this);
        }
    }
}