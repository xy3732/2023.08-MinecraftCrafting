using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftResultSlot : Slot
{
    public override void LeftClick(bool isShift)
    {
        // 크레프팅 결과가 없으면 리턴.
        if (!InventoryWindow.Instance.CraftController.HasResultItem) return;

        int amount = isShift ? InventoryWindow.Instance.CraftController.MaxCraftingAmount() : 1;

        // 현재 들고 있는 오브젝트가 있을시
        if (InventoryWindow.Instance.HasCurrentItem)
        {
            // 크레프팅 슬롯에 있는 아이템하고 다르면 리턴.
            if (InventoryWindow.Instance.CurrentItem.Item != Item.Item) return;

            // 같을시 들고 있는 오브젝트에 크레프팅 결과의 갯수를 더함.
            InventoryWindow.Instance.CurrentItem.Amount += Item.Amount * amount;
            InventoryWindow.Instance.CheckCurrentItem();
        }
        else
        {
            Item.Amount *= amount;

            // 크레프팅 결과를 들고 있는 아이템으로 설정.
            InventoryWindow.Instance.SetCurrentItem(Item);
            ResetItem();
        }

        //
        InventoryWindow.Instance.CraftController.CraftItem(amount);
    }


    // 크레프팅 결과 슬롯에는 아이템 추가 못하게 설정
    public override void RightClick()
    {
        if (InventoryWindow.Instance.HasCurrentItem) return;
    }

}
