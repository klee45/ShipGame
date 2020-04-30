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
        pos = (pos + count) % sprites.Length;
        GetComponent<SpriteRenderer>().sprite = sprites[pos];
    }
}
