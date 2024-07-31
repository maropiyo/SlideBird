
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Project.Game.Scripts.View
{
    /// <summary>
    /// コンボのビュー
    /// </summary>
    public class ComboView : MonoBehaviour
    {
        // コンボラベルテキスト
        [SerializeField] private TextMeshProUGUI _comboLabelText;
        // コンボ倍率テキスト
        [SerializeField] private TextMeshProUGUI _comboMultiplierText;
        // 現在のTween
        private Tween _currentTween;

        /// <summary>
        /// コンボテキストを表示する
        /// </summary>
        public async UniTask ShowComboText(int comboCount)
        {
            // 前のTweenが存在する場合、それをキャンセル
            _currentTween?.Kill();

            try
            {
                // 表示するコンボのテキストを設定する
                _comboLabelText.text = "COMBO";
                _comboMultiplierText.text = "x" + comboCount;

                // コンボテキストのアニメーション
                _comboLabelText.transform.localScale = Vector3.one;
                _comboMultiplierText.transform.localScale = Vector3.one;
                _currentTween = _comboLabelText.transform.DOScale(1.2f, 1f).SetEase(Ease.OutBounce);
                _currentTween = _comboMultiplierText.transform.DOScale(1.2f, 1f).SetEase(Ease.OutBounce);

                await _currentTween.OnComplete(() => HideComboText()).AsyncWaitForCompletion();
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合、何もしない
            }
        }

        /// <summary>
        /// コンボテキストを非表示にする
        /// </summary>
        public void HideComboText()
        {
            _comboLabelText.text = "";
            _comboMultiplierText.text = "";
        }
    }
}
