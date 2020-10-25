using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRow : MonoBehaviour
{
    [SerializeField]
    private int[] positions;

    public WeaponButtonShop[] SetupButtons(WeaponButtonShop button)
    {
        WeaponButtonShop[] buttons = new WeaponButtonShop[positions.Length];

        Debug.Log("Setting up buttons");
        int pos = 0;
        foreach (int x in positions)
        {
            WeaponButtonShop buttonObj = Instantiate(button);
            buttonObj.transform.SetParent(transform); 
            buttonObj.transform.localPosition = new Vector3(x, 0, 0);
            buttonObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            buttonObj.name = "Button " + pos;
            buttons[pos++] = buttonObj;
        }

        return buttons;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
