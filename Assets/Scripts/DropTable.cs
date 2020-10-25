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
        weaponDeeds.AddRange(Resources.LoadAll<WeaponDeed>(folderPath));

        foreach (WeaponDeed deed in weaponDeeds)
        {
            originalWeights.Add(deed.GetRarity());
        }
        StackWeightedList(originalWeights);
    }

    public void StackWeightedList(List<int> lst)
    {
        weights = new List<int>(lst);
        weights.StackList();
    }

    public WeaponDeed GetRandomWeaponDeed()
    {
        return weaponDeeds[Math.WeightedRandom(weights)];
    }
}
