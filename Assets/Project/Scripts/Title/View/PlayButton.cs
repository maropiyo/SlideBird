using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project.Scripts.Title.View
{
    /// <summary>
    /// プレイボタン(View)
    /// </summary>
    public class PlayButton : MonoBehaviour
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
