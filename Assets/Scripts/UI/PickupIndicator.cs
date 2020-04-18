using UnityEngine;
using DG.Tweening;

public class PickupIndicator : MonoBehaviour
{
    public GameObject indicator;
    public RectTransform arrow;
    public float moveDistance = 1;
    public float duration = 1;
    public Ease easeing = Ease.InOutCirc;

    private Tween arrowTween;
    private float originalYPos;

    void Awake()
    {
        originalYPos = arrow.rect.position.y;
    }
    void OnEnable()
    {
        var startYPos = originalYPos;
        var endYPos = originalYPos + moveDistance;
        arrowTween = arrow.DOAnchorPosY(endYPos, duration, false).SetEase(easeing).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        this.transform.LookAt(PlayerManager.instance.transform.position);
    }

    void OnDisable()
    {
        arrowTween = null;
    }


    public void enableIndicator()
    {
        indicator.SetActive(true);
    }

    public void disableIndicator()
    {
        indicator.SetActive(false);

    }
}
