using System.Threading.Tasks;
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

        /// <summary>
        /// コンボテキストを1秒間表示する
        /// </summary>
        public async UniTask ShowComboTextAsync(int comboCount)
        {
            _comboLabelText.text = "COMBO";
            _comboMultiplierText.text = "x" + comboCount;

            // アニメーション
            await _comboLabelText.transform.DOScale(1.2f, 1f).AsyncWaitForCompletion();

            HideComboText();
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
