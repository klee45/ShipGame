using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRepeat : AWeapon
{
    [SerializeField]
    private ProjectileTemplate template;
    [SerializeField]
    private float[] delays;

    protected override void FireHelper()
    {
        foreach (float delay in delays)
        {
            StartCoroutine(CreateProjectileCoroutine(template, delay));
        }
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.Estimate(template);
    }

    protected override void SetProjectileTemplateTeams(Team team)
    {
        template.SetTeam(team);
    }
}
