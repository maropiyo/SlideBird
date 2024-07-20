using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Assets.Project.Scripts.Game.Model
{
    /// <summary>
    /// ブロック移動を制御するクラス
    /// </summary>
    public class BlockMovementController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        /// <summary>
        /// ブロックのリストを１マス上に移動する
        /// </summary>
        /// <param name="blocks">ブロックのリスト</param>
        public void MoveBlocksUp(List<GameObject> blocks)
        {
            // ブロックのリストを１マス上に移動
            foreach (var block in blocks)
            {
                block.transform.DOLocalMoveY(block.transform.position.y + 1, 0.2f).OnComplete(() =>
                {
                    OnMoveBlocksUpComplete();
                });

            }
        }

        /// <summary>
        /// 移動完了時の処理
        /// </summary>
        public void OnMoveBlocksUpComplete()
        {

            gameManager.isGenerating = false;

            gameManager.DropCurrentBlocks();
        }
    }
}
