using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElementAnimator : MonoBehaviour
{
    #region プライベートフィールド
    [Tooltip("移動する座標量")]
    [SerializeField]
    private Vector3 move = new Vector3(0f, 0f, 0f);

    [Tooltip("持続秒数")]
    [SerializeField]
    private float duration = 1f;

    [Tooltip("遅延レベル")]
    [SerializeField]
    private float delayLevel = 0f;
    #endregion

    #region 変数
    private float delay;
    private Vector3 initialPosition;
    #endregion

    #region Unityのコールバック
    void Start()
    {
        initialPosition = transform.position;
        delay = delayLevel * WholeSettings.AnimationDelay;

        transform.position = initialPosition - move;
    }

    void Update()
    {
        transform
            .DOMove(initialPosition + move, duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(delay);
    }
    #endregion
}
