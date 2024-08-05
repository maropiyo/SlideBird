using UnityEngine;

namespace Assets.Project.Common.Scripts
{
    /// <summary>
    /// サウンドタイプとオーディオクリップのペア
    /// </summary>
    [System.Serializable]
    public class SoundTypeAudioClipPair
    {
        public SoundType soundType;
        public AudioClip audioClip;
    }
}
