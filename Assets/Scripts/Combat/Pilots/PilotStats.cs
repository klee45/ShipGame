using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PilotStats : MonoBehaviour
{
    public const int MIN_AGGRESSION = 1;
    public const int MAX_AGGRESSION = 5;
    public const int MIN_SKILL = 1;
    public const int MAX_SKILL = 5;


    [SerializeField]
    [Range(MIN_AGGRESSION, MAX_AGGRESSION)]
    private float aggression = (MAX_AGGRESSION + MIN_AGGRESSION) / 2;
    [SerializeField]
    [Range(MIN_SKILL, MAX_SKILL)]
    private float skill = (MAX_SKILL + MIN_SKILL) / 2;

    private void Awake()
    {
    }

    public float GetAggression()
    {
        return aggression;
    }

    public float GetSkill()
    {
        return skill;
    }
}   