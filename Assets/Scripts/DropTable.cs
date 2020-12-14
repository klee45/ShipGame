using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : Singleton<DropTable>
{
    [SerializeField]
    private string folderPath = "Weapons";

    [SerializeField]
    private Sprite[] borders;

    [SerializeField]
    private List<DropInfo>[] drops;

    private List<int>[] weights;

    protected override void Awake()
    {
        base.Awake();
        weights = new List<int>[typeof(Size).GetCount()];
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupLists();
        SetupDropTable();
    }

    private void SetupDropTable()
    {
        List<WeaponDeed> deedBlueprints = new List<WeaponDeed>();
        deedBlueprints.AddRange(Resources.LoadAll<WeaponDeed>(folderPath));

        foreach (WeaponDeed deedBlueprint in deedBlueprints)
        {
            foreach (WeaponDeed.WeaponDeedInfo info in deedBlueprint.GetSizeRarityPairs())
            {
                int size = (int)info.weaponSize;
                GameObject obj = new GameObject(deedBlueprint.name + " " + info.weaponSize);
                DropInfo drop = obj.AddComponent<DropInfo>();
                drop.deed = deedBlueprint;
                drop.info = info;
                drops[size].Add(drop);
                drop.transform.SetParent(transform);
                weights[size].Add(info.rarity);
            }
        }
        StackWeightedList();
    }

    private void SetupLists()
    {
        int numSizes = typeof(Size).GetCount();
        weights = new List<int>[numSizes];
        drops = new List<DropInfo>[numSizes];
        for (int i = 0; i < numSizes; i++)
        {
            drops[i] = new List<DropInfo>();
            weights[i] = new List<int>();
        }
    }

    private void StackWeightedList()
    {
        foreach (List<int> lst in weights)
        {
            lst.StackList();
        }
    }

    public bool GetDrops(Size slotSize, int minRarity, int maxRarity, out List<int> weights, out List<DropInfo> drops)
    {
        try
        {
            int sizePos = (int)slotSize;
            if (minRarity > 0 || maxRarity > 0)
            {
                weights = new List<int>();
                drops = new List<DropInfo>();

                int pos = 0;
                foreach (DropInfo drop in this.drops[sizePos])
                {
                    int rarity = drop.info.rarity;
                    if (rarity >= minRarity && rarity <= maxRarity)
                    {
                        drops.Add(drop);
                        weights.Add(this.weights[sizePos][pos]);
                    }
                    pos++;
                }
                if (weights.Count > 0)
                {
                    weights.StackList();
                }
                else
                {
                    Debug.LogWarning(string.Format("Couldn't find any drops that satisfied range {0} <= r <= {1}", minRarity, maxRarity));
                    return false;
                }
            }
            else
            {
                weights = this.weights[sizePos];
                drops = this.drops[sizePos];
            }
            return true;
        }
        catch (System.IndexOutOfRangeException e)
        {
            Debug.Log("Tried to access index out of range for getting drops from drop table\n" + e);
            weights = null;
            drops = null;
            return false;
        }
    }

    public WeaponDeed CreateRandomWeaponDeed(Size slotSize)
    {
        int slot = (int)slotSize;
        DropInfo info = drops[slot][Math.WeightedRandom(weights[slot])];
        WeaponDeed deed = Instantiate(info.deed);
        deed.Setup(info.info);
        return deed;
    }

    public class DropInfo : MonoBehaviour
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
