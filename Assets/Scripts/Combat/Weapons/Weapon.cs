using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AWeapon
{
    [SerializeField]
    protected ProjectileTemplate[] projectileTemplates;

    protected override void SetProjectileTemplateTeams(Team team)
    {
        foreach (ProjectileTemplate template in projectileTemplates)
        {
            template.SetTeam(team);
        }
    }

    protected override void FireHelper()
    {
        foreach (ProjectileTemplate template in projectileTemplates)
        {
            StartCoroutine(CreateProjectileCoroutine(template, template.GetDelay()));
        }
    }

    protected override void InitializeRangeEstimator()
    {
        rangeEstimator.Estimate(projectileTemplates);
    }
}