
using Assets.Project.Game.Scripts.Model;
using Assets.Project.Game.Scripts.View;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using Assets.Project.Common.Scripts;

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
            // 現在のコンボの値が2以上場合、コンボテキストを表示し、効果音を再生する
            _combo.CurrentCombo
                .Where(comboCount => comboCount > 1)
                .Subscribe(comboCount =>
                {
                    // コンボテキストを表示する
                    _comboTextView.ShowComboText(comboCount).ToObservable();

                    // コンボ数に応じた効果音を再生する
                    switch (comboCount)
                    {
                        case 2:
                            SoundManager.Instance.PlaySound(SoundType.Combo2);
                            break;
                        case 3:
                            SoundManager.Instance.PlaySound(SoundType.Combo3);
                            break;
                        case 4:
                            SoundManager.Instance.PlaySound(SoundType.Combo4);
                            break;
                        case 5:
                            SoundManager.Instance.PlaySound(SoundType.Combo5);
                            break;
                        default:
                            // 6以上のコンボ数の場合は6の効果音を再生する
                            SoundManager.Instance.PlaySound(SoundType.Combo6);
                            break;
                    }
                }).AddTo(this);
        }
    }
}