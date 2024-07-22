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
        // プレイボタン(View)
        [SerializeField] private PlayButton _playButton;

        private void Start()
        {
            // プレイボタンのクリックイベントを監視する
            _playButton.OnClickAsObservable()
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
