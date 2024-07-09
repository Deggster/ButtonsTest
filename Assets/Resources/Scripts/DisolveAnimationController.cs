using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Coffee.UIEffects;
using TMPro;

public class DisolveAnimationController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject buttonImage;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private UIDissolve disolveEffect;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private CanvasGroup Text;
    [SerializeField]
    [Range(0, 2)]
    float _animationTimeReleased = 1f;

    public Vector3 _scaleFactor = new Vector3(1f, 0.9f, 1f);

    private float _animationTimePressed = 0.2f;
    private Vector3 originalScale;
    private Color _backgroundImageColor;
    private float originalTextAlpha;

    bool buttonPressed;

    void Start()
    {
        if (button == null)
        {
            Debug.Log("Button not set in inspector");
        }
        _backgroundImageColor = backgroundImage.color;

        originalScale = buttonImage.transform.localScale;
        originalTextAlpha = Text.alpha;

        disolveEffect.effectFactor = 0;
        particleSystem.gameObject.SetActive(false);
    }

    private void PreessAnimation()
    {
        buttonImage.transform.DOScale(_scaleFactor, _animationTimePressed).SetEase(Ease.InBack);
    }
    private void ReleasedAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(buttonImage.transform.DOScale(originalScale, _animationTimePressed).SetEase(Ease.OutBack));
        sequence.Join(backgroundImage.DOColor(new Color(_backgroundImageColor.r, _backgroundImageColor.g, _backgroundImageColor.b, 0f), _animationTimeReleased));
        sequence.Join(Text.DOFade(0, _animationTimeReleased));
        button.interactable = false;
        disolveEffect.Play(true);
        particleSystem.gameObject.SetActive(true);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        PreessAnimation();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ReleasedAnimation();
    }

    public void ResetAnimations()
    {
        buttonImage.transform.localScale = originalScale;
        backgroundImage.color = _backgroundImageColor;
        Text.alpha = originalTextAlpha;
        particleSystem.gameObject.SetActive(false);
        disolveEffect.effectFactor = 0;
        button.interactable = true;
    }
}