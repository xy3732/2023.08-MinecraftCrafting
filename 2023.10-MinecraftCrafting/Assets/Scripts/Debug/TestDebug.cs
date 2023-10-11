using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebug : MonoBehaviour
{
    public static TestDebug instance;


    private void Awake()
    {
        instance = this;
    }

    public void SetInt(int value)
    {
        Debug.Log("set " + value + " int");
    }

    public void SetItem(string input, int amount)
    {
        Debug.Log(input + ", " + amount);
        
        for (int i = 0; i < ItemsManager.Instance.Items.Count; i++)
        {
           if(input == ItemsManager.Instance.Items[i].Name)
            {
                addItem(i,amount);
            }
        }
    }

    void addItem(int input, int amount)
    {
        var inven = InventoryController.instance.AdditionalSlots;
        for (int i = 0; i < inven.GetLength(0); i++)
        {
            for (int k = 0; k < inven.GetLength(1); k++)
            {
                if (inven[i, k].Item == null)
                {
                    inven[i, k].SetItem(new ItemInSlot(ItemsManager.Instance.Items[input], amount));
                    return;
                }
            }
        }
    }
}
