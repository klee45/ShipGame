using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFade : MonoBehaviour
{
    [SerializeField]
    private float min = 0.0f;
    [SerializeField]
    private float max = 1.0f;
    [SerializeField]
    private float changePerSec = 0.5f;
    [SerializeField]
    private bool flipOnMin = false;

    private bool isIncreasing;
    private float current;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        current = max;
        isIncreasing = false;
    }

    private void Start()
    {
        current = max;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = spriteRenderer.color.SetAlpha(current);
        ChangeAlpha();
    }

    private void ChangeAlpha()
    {
        if (isIncreasing)
        {
            current += changePerSec * TimeController.StaticDeltaTime();
            if (current >= max)
            {
                current = max;
                isIncreasing = false;
            }
        }
        else
        {
            current -= changePerSec * TimeController.StaticDeltaTime();
            if (current <= min)
            {
                current = min;
                isIncreasing = true;
                if (flipOnMin)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }
            }
        }
    }
}
