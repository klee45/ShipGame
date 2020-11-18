using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : Singleton<DropTable>
{
    [SerializeField]
    private string folderPath = "Weapons";

    [SerializeField]
    List<WeaponDeed> weaponDeeds;

    private List<int> originalWeights;
    private List<int> weights;

    protected override void Awake()
    {
        base.Awake();
        originalWeights = new List<int>();
        weights = new List<int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponDeeds = new List<WeaponDeed>();
        List<WeaponDeed> deedBlueprints = new List<WeaponDeed>();
        deedBlueprints.AddRange(Resources.LoadAll<WeaponDeed>(folderPath));
        
        foreach (WeaponDeed deedBlueprint in deedBlueprints)
        {
            foreach (WeaponDeed.WeaponDeedInfo info in deedBlueprint.GetSizeRarityPairs())
            {
                WeaponDeed deed = Instantiate(deedBlueprint);
                deed.Setup(info);
                weaponDeeds.Add(deed);
                deed.transform.SetParent(transform);
                originalWeights.Add(info.rarity);
            }
        }
        StackWeightedList(originalWeights);
    }

    private void StackWeightedList(List<int> lst)
    {
        weights = new List<int>(lst);
        weights.StackList();
    }

    public WeaponDeed GetRandomWeaponDeed()
    {
        return weaponDeeds[Math.WeightedRandom(weights)];
    }
}
