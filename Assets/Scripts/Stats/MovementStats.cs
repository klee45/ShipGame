using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStats : MonoBehaviour
{
    private static float INPUT_LIMIT = 0.25f;

    [SerializeField]
    public float rotationMaxInitial;
    [SerializeField]
    public float rotationAccelerationInitial;
    [SerializeField]
    public float velocityForwardsMaxInitial;
    [SerializeField]
    public float velocityForwardsAccelerationInitial;
    [SerializeField]
    public float velocityBackwardsMaxInitial;
    [SerializeField]
    public float velocityBackwardsAccelerationInitial;

    [SerializeField]
    public float rotationDampening;
    [SerializeField]
    public float velocityDampening;

    private StatGroup rotation, velocity;
    
    private void Awake()
    {
        rotation = new StatGroup(rotationDampening, rotationMaxInitial, rotationAccelerationInitial);
        velocity = new StatGroup(
            velocityDampening,
            velocityForwardsMaxInitial,
            velocityForwardsAccelerationInitial,
            velocityBackwardsMaxInitial,
            velocityBackwardsAccelerationInitial);
    }

    public StatGroup GetRotationStatGroup() { return rotation; }
    public StatGroup GetVelocityStatGroup() { return velocity; }

    public float GetRotationValue() { return rotation.GetValue() * Mathf.Rad2Deg; }
    public float GetVelocityValue() { return velocity.GetValue(); }

    public class StatGroup
    {
        private FloatStat acceleration, deceleration, max, min;
        private float dampening;
        private float currentValue;

        public StatGroup(float dampening, float max, float acceleration) : this(dampening, max, acceleration, max, acceleration)
        {
        }

        public StatGroup(float dampening, float max, float acceleration, float min, float deceleration)
        {
            this.dampening = dampening;
            this.max = new FloatStat(max);
            this.acceleration = new FloatStat(acceleration);
            this.min = new FloatStat(min);
            this.deceleration = new FloatStat(deceleration);
            currentValue = 0;
        }

        public void Tick(float scale)
        {
            Debug.Log(string.Format("Max: {0}, Min: {1}, Current: {2}", max.GetValue(), -min.GetValue(), this.currentValue));
            if (scale > INPUT_LIMIT)
            {
                this.currentValue -= acceleration.GetValue() * scale * Time.deltaTime;
                this.currentValue = Mathf.Max(this.currentValue, -max.GetValue());
            }
            else if (scale < -INPUT_LIMIT)
            {
                this.currentValue -= deceleration.GetValue() * scale * Time.deltaTime;
                this.currentValue = Mathf.Min(this.currentValue, min.GetValue());
            }
            else
            {
                float mod = (1 - Mathf.Abs(scale)) * dampening * Time.deltaTime;
                if (this.currentValue > mod)
                {
                    this.currentValue -= mod;
                }
                else if (this.currentValue < -mod)
                {
                    this.currentValue += mod;
                }
                else
                {
                    this.currentValue = 0;
                }
            }
        }

        public float GetValue()
        {
            return currentValue;
        }

        public FloatStat GetAcceleration() { return acceleration; }
        public FloatStat GetDeceleration() { return deceleration; }
        public FloatStat GetMax() { return max; }
        public FloatStat GetMin() { return min; }
    }
}
