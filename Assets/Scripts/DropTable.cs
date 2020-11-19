using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : Singleton<DropTable>
{
    [SerializeField]
    private string folderPath = "Weapons";

    [SerializeField]
    private List<DropInfo> drops;

    [SerializeField]
    private Sprite[] borders;

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
        drops = new List<DropInfo>();
        List<WeaponDeed> deedBlueprints = new List<WeaponDeed>();
        deedBlueprints.AddRange(Resources.LoadAll<WeaponDeed>(folderPath));
        
        foreach (WeaponDeed deedBlueprint in deedBlueprints)
        {
            foreach (WeaponDeed.WeaponDeedInfo info in deedBlueprint.GetSizeRarityPairs())
            {
                GameObject obj = new GameObject(deedBlueprint.name + " " + info.weaponSize);
                DropInfo drop = obj.AddComponent<DropInfo>();
                drop.deed = deedBlueprint;
                drop.info = info;
                drops.Add(drop);
                drop.transform.SetParent(transform);
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

    public WeaponDeed CreateRandomWeaponDeed()
    {
        DropInfo info = drops[Math.WeightedRandom(weights)];
        WeaponDeed deed = Instantiate(info.deed);
        deed.Setup(info.info);
        return deed;
    }

    private class DropInfo : MonoBehaviour
    {
        public WeaponDeed deed;
        public WeaponDeed.WeaponDeedInfo info;
    }

    public Sprite GetBorder(Size size)
    {
        switch (size)
        {
            case Size.Small:
                return borders[0];
            case Size.Medium:
                return borders[1];
            case Size.Large:
                return borders[2];
            case Size.Huge:
                return borders[3];
        }
        return null;
    }
}
