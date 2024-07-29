using UniRx;
using UnityEngine;

namespace Assets.Project.Game.Scripts.Model
{
    /// <summary>
    /// コンボ
    /// </summary>
    public class Combo : MonoBehaviour
    {
        /// <summary>
        /// 現在のコンボ
        /// </summary>
        public ReactiveProperty<int> CurrentCombo { get; } = new ReactiveProperty<int>();

        /// <summary>
        /// コンボをプラスする
        /// </summary>
        public void Plus()
        {
            CurrentCombo.Value++;
        }

        /// <summary>
        /// コンボをリセットする
        /// </summary>
        public void Reset()
        {
            CurrentCombo.Value = 0;
        }
    }
}