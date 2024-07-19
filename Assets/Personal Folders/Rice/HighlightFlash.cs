using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightFlash : MonoBehaviour
{
    private bool isFlashing = false;
    [SerializeField] float rateOfFlash;
    private float flashProgress = 0;
    private int flashDirection = 1;
    private Color originColor;

    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    private void Update()
    {
        spriteRenderer.color = Vector4.Lerp(originColor, Color.white, flashProgress);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFlashing) flashProgress += rateOfFlash * flashDirection;
        if (flashProgress > 1) flashDirection = -1;
        if (flashProgress < 0)
        {
            flashProgress = 0;
            flashDirection = 1;
            isFlashing = false;
        }
    }

    public void StartFlash()
    {
        isFlashing = true;
    }
}
