using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project.Title.Scripts.View
{
    /// <summary>
    /// プレイボタンのビュー
    /// </summary>
    public class PlayButtonView : MonoBehaviour
    {
        // ボタン
        [SerializeField] private Button _button;

        /// <summary>
        /// ボタンのクリックイベントをObservableとして公開する
        /// </summary>
        public IObservable<Unit> OnClickAsObservable()
        {
            return _button.OnClickAsObservable();
        }
    }
}
