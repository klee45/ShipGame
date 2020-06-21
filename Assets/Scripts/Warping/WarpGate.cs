using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpGate : MonoBehaviour
{
    [SerializeField]
    private float duration = 5f;
    [SerializeField]
    private GalaxyMapVertex targetSector;

    private Dictionary<Ship, WarpEffect> warping;

    private void Awake()
    {
        warping = new Dictionary<Ship, WarpEffect>();
    }

    public void Setup(GalaxyMapVertex targetSector)
    {
        this.targetSector = targetSector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enter");
        Ship ship = collision.GetComponent<Ship>();
        ship.OnShipDestroy += Remove;
        WarpEffect effect = ship.AddShipEffect<WarpEffect>();
        effect.Setup(targetSector, duration);
        
        warping.Add(ship, effect);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Exit");
        Ship ship = collision.GetComponent<Ship>();
        if (warping.TryGetValue(ship, out WarpEffect force))
        {
            ship.OnShipDestroy -= Remove;
            Destroy(force);
            warping.Remove(ship);
        }
    }

    private void Remove(Ship s)
    {
        warping.Remove(s);
    }

    public class WarpEffect : ShipEffect, GeneralEffect.ITickEffect, EffectDict.IEffectUpdates
    {
        private GalaxyMapVertex targetSector;
        private float duration;
        private float time;

        public void Setup(GalaxyMapVertex targetSector, float duration)
        {
            this.targetSector = targetSector;
            this.duration = duration;
            this.time = 0;
        }

        public override void AddTo(EffectDictShip dict)
        {
            dict.tickEffects.AddUpdate(this);
        }

        public override string GetName()
        {
            return string.Format("Warping {0:0.#}", time);
        }

        public void Tick(float timeScale)
        {
            time += TimeController.DeltaTime(timeScale);
            if (time >= duration)
            {
                WarpManager.instance.StartWarp(targetSector);
                Destroy(this);
            }
        }
        
        public IEffect UpdateEffect(IEffect effect, out bool didReplace)
        {
            didReplace = false;
            return effect;
        }
    }
}
