using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftController : MonoBehaviour
{
    public static CraftController instance;
    [SerializeField]
    private GameObject slotPref;
    [SerializeField]
    private Transform craftGrid;

    public CraftSlot[,] CraftTable { get; private set; }

    // 크레프팅 결과 슬롯.
    public CraftResultSlot ResultSlot;

    // 크레프팅 결과가 있는지 없는지 확인.
    public bool HasResultItem => ResultSlot.Item != null;

    public void Awake()
    {
        instance = this;
    }

    // 크레프팅 슬롯 생성
    public void Init()
    {
        CraftTable = new CraftSlot[3, 3];
        CreateSlotsPrefabs();
    }

    // 3X3 조합 슬롯 생성.
    private void CreateSlotsPrefabs()
    {
        for (int i = 0; i < CraftTable.GetLength(0); i++)
        {
            for (int k = 0; k < CraftTable.GetLength(1); k++)
            {
                var slot = Instantiate(slotPref, craftGrid, false);
                CraftTable[i, k] = slot.AddComponent<CraftSlot>();
            }
        }
    }

    public void CheckCraft()
    {
        ItemInSlot newItem = null;

        int currRecipeW = 0;
        int currRecipeH = 0;

        int currRecipeWStartIndex = -1;
        int currRecipeHStartIndex = -1;

        for (int i = 0; i < CraftTable.GetLength(0); i++)
        {
            for (int k = 0; k < CraftTable.GetLength(1); k++)
            {
                // 크레프팅 슬롯에 아이템 있을시 시작점 설정 밑 총 갯수 찾기.
                if (CraftTable[i, k].HasItem)
                {
                    // H 시작점 설정.
                    if (currRecipeHStartIndex == -1) currRecipeHStartIndex = i;

                    // 총 H 갯수 증가.
                    currRecipeH++;
                    break;
                }
            }
        }

        for (int i = 0; i < CraftTable.GetLength(1); i++)
        {
            for (int k = 0; k < CraftTable.GetLength(0); k++)
            {
                // 크레프팅 슬롯에 아이템 있을시 시작점 설정 밑 총 갯수 찾기.
                if (CraftTable[k, i].HasItem)
                {
                    // W 시작점 설정.
                    if (currRecipeWStartIndex == -1) currRecipeWStartIndex = i;

                    // 총 W 갯수 증가.
                    currRecipeW++;
                    break;
                }
            }
        }

        Item[] craftOrder = new Item[currRecipeH * currRecipeW];

        for (int orderId = 0, i = currRecipeHStartIndex; i < currRecipeHStartIndex + currRecipeH; i++)
        {
            for (int k = currRecipeWStartIndex; k < currRecipeWStartIndex + currRecipeW; k++)
            {
                Debug.Log(i + "," + k);
                craftOrder[orderId++] = CraftTable[i, k].Item?.Item;
            }
        }

        foreach (Item item in ItemsManager.Instance.Items)
        {
            // SequenceEqual - 두 배열이 같은지 확인
            // 만약 크레프팅 슬롯에 있는 배열하고 아이템 조합 레시피에 있는 배열 하고 같으면 그 아이템 으로 설정.
            if (item.HasRecipe && item.Recipe.ItemsOrder.SequenceEqual(craftOrder))
            {
                newItem = new ItemInSlot(item, item.Recipe.Amount);
                break;
            }
        }

        // 크레프팅 결과가 있으면 결과값 으로 아니면 NULL 로 설정.
        if (newItem != null) ResultSlot.SetItem(newItem);
        else ResultSlot.ResetItem();
    }

    public void CraftItem()
    {
        for (int i = 0; i < CraftTable.GetLength(0); i++)
        {
            for (int k = 0; k < CraftTable.GetLength(1); k++)
            {
                // 조합시 크레프팅 슬롯에 있는 아이템 갯수 -1.
                if (CraftTable[i, k].Item != null) CraftTable[i, k].DecreaseItemAmount(1);
            }
        }
        CheckCraft();
    }

    public void CraftItem(int input)
    {
        for (int i = 0; i < CraftTable.GetLength(0); i++)
        {
            for (int k = 0; k < CraftTable.GetLength(1); k++)
            {
                // 조합시 크레프팅 슬롯에 있는 아이템 갯수 -1.
                if (CraftTable[i, k].Item != null) CraftTable[i, k].DecreaseItemAmount(input);
            }
        }
        CheckCraft();
    }

    public int MaxCraftingAmount()
    {
        int temp = 99;

        for (int i = 0; i < CraftTable.GetLength(0); i++)
        {
            for (int k = 0; k < CraftTable.GetLength(1); k++)
            {
                if(CraftTable[i, k].Item != null)
                {
                    if (CraftTable[i, k].Item.Amount < temp) temp = CraftTable[i, k].Item.Amount;
                }
            }
        }

        return temp;
    }
}
