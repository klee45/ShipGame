using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private int pos;
    [SerializeField]
    private SpriteManager.SpriteSpeed speed;
    [SerializeField]
    private AnimationType type = AnimationType.REPEAT;
    
    // Start is called before the first frame update
    void Start()
    {
        SpriteManager manager = SpriteManager.Instance();
        switch (speed)
        {
            case SpriteManager.SpriteSpeed.s12:
                manager.TwelveTick += Tick;
                break;
            case SpriteManager.SpriteSpeed.s20:
                manager.TwentyTick += Tick;
                break;
            case SpriteManager.SpriteSpeed.s30:
                manager.ThirtyTick += Tick;
                break;
            case SpriteManager.SpriteSpeed.s60:
                manager.SixtyTick += Tick;
                break;
        }
    }

    private void OnDestroy()
    {
        SpriteManager manager = SpriteManager.Instance();
        switch (speed)
        {
            case SpriteManager.SpriteSpeed.s12:
                manager.TwelveTick -= Tick;
                break;
            case SpriteManager.SpriteSpeed.s20:
                manager.TwentyTick -= Tick;
                break;
            case SpriteManager.SpriteSpeed.s30:
                manager.ThirtyTick -= Tick;
                break;
            case SpriteManager.SpriteSpeed.s60:
                manager.SixtyTick -= Tick;
                break;
        }
    }

    private void Tick(int count)
    {
        Sprite sprite = null;
        switch(type)
        {
            case AnimationType.END:
                pos = pos + count;
                if (pos < sprites.Length)
                {
                    sprite = sprites[pos];
                }
                break;
            case AnimationType.HOLD:
                pos = Mathf.Min((pos + count), sprites.Length - 1);
                sprite = sprites[pos];
                break;
            case AnimationType.REPEAT:
                pos = (pos + count) % sprites.Length;
                sprite = sprites[pos];
                break;
        }
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public enum AnimationType
    {
        REPEAT,
        HOLD,
        END
    }
}
