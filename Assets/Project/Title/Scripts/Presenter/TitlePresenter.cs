using Assets.Project.Title.Scripts.View;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Project.Title.Scripts.Presenter
{
    /// <summary>
    /// タイトル画面のプレゼンター
    /// </summary>
    public class TitlePresenter : MonoBehaviour
    {
        // プレイボタンビュー
        [SerializeField] private PlayButtonView _playButtonView;

        private void Start()
        {
            // プレイボタンビューのクリックイベントを監視する
            _playButtonView.OnClickAsObservable()
                .Subscribe(_ => OnClickPlayButton())
                .AddTo(this);
        }

        /// <summary>
        /// プレイボタンがクリックされた時の処理
        /// </summary>
        private void OnClickPlayButton()
        {
            // ゲームシーンに遷移する
            SceneManager.LoadScene("GameScene");
        }
    }
}
