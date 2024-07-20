using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum runes
{
    N = 0,
    T = 1,
    L = 2,
    I = 3,
    O = 4,
    none = -1
}
public class RitualNode : MonoBehaviour
{
    private bool isFlashing = false;
    [SerializeField] float rateOfFlash;
    private float flashProgress = 0;
    private int flashDirection = 1;
    private Color originColor;
    private Color targetColor;

    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] correctRuneSprites;
    [SerializeField] Sprite[] incorrectRuneSprites;

    public runes requiredRune = runes.none;
    [HideInInspector] public bool chantAttempted = false;
    [HideInInspector] public bool chantSucceeded = false;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
        targetColor = Color.black;
    }

    private void Update()
    {
        spriteRenderer.color = Vector4.Lerp(originColor, targetColor, flashProgress);
    }

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

    private void StartFlash()
    {
        flashProgress = 0;
        flashDirection = 1;
        isFlashing = true;
    }

    public bool CompareRune(runes rune)
    {
        if (chantAttempted == true) return false;
        chantAttempted = true;
        if (rune == requiredRune)
        {
            chantSucceeded = true;
            targetColor = Color.red;
        }
        else targetColor = Color.black;
        StartFlash();
        return chantSucceeded;
    }
    public void ResetState()
    {
        targetColor = Color.black;
        StartFlash();
        chantAttempted = false;
        chantSucceeded = false;
    }

    public void ChangeRune(runes _rune, bool isCorrect)
    {
        requiredRune = _rune;
        if (_rune == runes.none) spriteRenderer.sprite = null;
        else if (isCorrect) spriteRenderer.sprite = correctRuneSprites[(int)_rune];
        else spriteRenderer.sprite = incorrectRuneSprites[(int)_rune];
    }
}
