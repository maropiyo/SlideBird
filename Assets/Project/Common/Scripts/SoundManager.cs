using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Common.Scripts
{
    /// <summary>
    /// サウンドマネージャー
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        // シングルトンインスタンス
        public static SoundManager Instance { get; private set; }

        // 効果音を再生するためのAudioSource
        [SerializeField] private AudioSource audioSource;
        // サウンドタイプとオーディオクリップのリスト
        [SerializeField] private List<SoundTypeAudioClipPair> soundList = new();

        // サウンドタイプとオーディオクリップのDictionary
        private readonly Dictionary<SoundType, AudioClip> soundDictionary = new();

        private void Awake()
        {
            // シングルトンの設定
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitializeDictionary();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void InitializeDictionary()
        {
            soundDictionary.Clear();
            foreach (var sound in soundList)
            {
                if (!soundDictionary.ContainsKey(sound.soundType))
                {
                    soundDictionary.Add(sound.soundType, sound.audioClip);
                }
            }
        }

        /// <summary>
        /// サウンドを再生する
        /// </summary>
        /// <param name="soundType">サウンドタイプ</param>
        public void PlaySound(SoundType soundType)
        {
            if (soundDictionary.ContainsKey(soundType))
            {
                audioSource.PlayOneShot(soundDictionary[soundType]);
            }
        }
    }
}
