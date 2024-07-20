using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFlyAndFlip : MonoBehaviour
{
    public static UIFlyAndFlip uIFlyAndFlip;
    public RectTransform uiImage; // Assign the RectTransform of the UI image in the Inspector
    public float flyToCenterDuration = 1f; // Duration for flying to the center
    public float scaleUpDuration = 1f; // Duration for scaling up
    public float flipDuration = 1f; // Duration for flipping horizontally
    public float flyToTopRightDuration = 1f; // Duration for flying to the top right
    public float scaleDownDuration = 1f; // Duration for scaling down
    public float maxScale = 3f;

    private Sequence mySequence;

    private void Awake()
    {
        uIFlyAndFlip = this;
    }

    void Start()
    {
        // Get the canvas to work with its RectTransform
        Canvas canvas = uiImage.GetComponentInParent<Canvas>();
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // Convert screen positions to local canvas positions
        Vector2 centerScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 topRightScreenPosition = new Vector2(Screen.width, Screen.height);

        Vector2 centerCanvasPosition;
        Vector2 topRightCanvasPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, centerScreenPosition, canvas.worldCamera, out centerCanvasPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, topRightScreenPosition, canvas.worldCamera, out topRightCanvasPosition);

        // Calculate the offset based on the size of the RectTransform
        Vector2 offset = uiImage.rect.width * 0.5f * Vector2.right + uiImage.rect.height * 0.5f * Vector2.up + Vector2.up * 20;

        // Apply the offset to the top right position
        Vector2 offsetTopRightCanvasPosition = topRightCanvasPosition - offset;

        // Create a new Sequence and set it to paused
        mySequence = DOTween.Sequence().Pause();

        // Add a move to center and scale up tween to the sequence
        mySequence.Append(uiImage.DOAnchorPos(centerCanvasPosition, flyToCenterDuration))
                  .Join(uiImage.DOScale(Vector3.one * maxScale, scaleUpDuration)); // Scale up

        // Add a flip and move to the top right with offset tween to the sequence
        mySequence.Append(uiImage.DOScaleX(-2, flipDuration)) // Flip horizontally
                  .Append(uiImage.DOAnchorPos(offsetTopRightCanvasPosition, flyToTopRightDuration))
                  .Join(uiImage.DOScale(Vector3.one, scaleDownDuration)); // Scale down
    }

    // Call this method to start the sequence
    public void StartSequence()
    {
        mySequence.Play();
    }
}
