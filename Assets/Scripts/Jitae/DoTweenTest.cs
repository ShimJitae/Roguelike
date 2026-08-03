using UnityEngine;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.DORotate(new Vector3(180f, 180f, 180f), 1f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Restart);
        // 회전 방향, 시간 / 변화곡선 / 루프상태와 반복형태
    }
}
