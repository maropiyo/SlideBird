using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンのUIを管理するクラス
/// </summary>
public class TitleSceneUIManager : MonoBehaviour
{
    // プレイボタンを押した時
    public void OnClickPlayButton()
    {
        // ゲームシーンに遷移する
        SceneManager.LoadScene("GameScene");
    }
}
