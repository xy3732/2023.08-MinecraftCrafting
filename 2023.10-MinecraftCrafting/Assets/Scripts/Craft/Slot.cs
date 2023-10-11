using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemAmount;

    private Color defaultColor = new Color32(140, 140, 140, 255);
    private Color highlightedColor = new Color32(121, 121, 121, 255);

    public ItemInSlot Item { get; private set; }

    public bool HasItem => Item != null;

    private void Awake()
    {
        image = GetComponent<Image>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemAmount = transform.GetChild(1).GetComponent<Text>();

        itemImage.preserveAspect = true;
    }

    public void SetItem(ItemInSlot item)
    {
        Item = item;
        RefreshUI();
    }

    public void AddItem(ItemInSlot item, int amount)
    {
        item.Amount -= amount;

        if (!HasItem) SetItem(new ItemInSlot(item.Item, amount));
        else
        {
            Item.Amount += amount;
            RefreshUI();
        }
    }

    public void ResetItem()
    {
        Item = null;
        RefreshUI();
    }

    protected void RefreshUI()
    {
        itemImage.gameObject.SetActive(HasItem);
        itemImage.sprite = Item?.Item.Sprite;

        itemAmount.gameObject.SetActive(HasItem && Item.Amount > 1);
        itemAmount.text = Item?.Amount.ToString();
    }

    float clickTime = 0;
    public void OnPointerClick(PointerEventData eventData)
    {
        if((Time.time - clickTime) < 0.3f && eventData.button == PointerEventData.InputButton.Left)
        {
            DoubleClick();
            clickTime = -1f;
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                var isSift = Input.GetKey(KeyCode.LeftShift);
                LeftClick(isSift);
            }
            else RightClick();

            clickTime = Time.time;
        }

    }

    public virtual void DoubleClick()
    {
        var currItem = InventoryWindow.Instance.CurrentItem;

        InventoryController inven = InventoryController.instance;
        CraftController craft = CraftController.instance;

        if (currItem == null) return;

        for(int i = 0; i < inven.MainSlots.GetLength(1); i++)
        {
            if(inven.MainSlots[0,i].Item != null)
            {
                if (currItem.Item == inven.MainSlots[0, i].Item.Item)
                {
                    currItem.Amount += inven.MainSlots[0, i].Item.Amount;
                    inven.MainSlots[0, i].ResetItem();
                }
            }
        }

        for(int i = 0; i< inven.AdditionalSlots.GetLength(0); i++)
        {
            for (int k = 0; k < inven.AdditionalSlots.GetLength(1); k++)
            {
                if (inven.AdditionalSlots[i, k].Item != null)
                {
                    if(currItem.Item == inven.AdditionalSlots[i, k].Item.Item)
                    {
                        currItem.Amount += inven.AdditionalSlots[i, k].Item.Amount;
                        inven.AdditionalSlots[i, k].ResetItem();
                    }
                }
            }
        }

        for(int i = 0; i< craft.CraftTable.GetLength(0); i++)
        {
            for (int k = 0; k < craft.CraftTable.GetLength(1); k++)
            {
                if (craft.CraftTable[i, k].Item != null)
                {
                    if (currItem.Item == craft.CraftTable[i, k].Item.Item)
                    {
                        currItem.Amount += craft.CraftTable[i, k].Item.Amount;
                        craft.CraftTable[i, k].ResetItem();
                    }
                }
            }
        }

        InventoryWindow.Instance.CheckCurrentItem();
    }

    public virtual void LeftClick(bool isShift = false)
    {
        // 현재 들고 있는 아이템.    
        var currItem = InventoryWindow.Instance.CurrentItem;

        if (HasItem)
        {
            // 들고 있는 아이템이랑 슬롯의 아이템이 다르면 서로 위치 바꾸기.
            if (currItem == null || Item.Item != currItem.Item)
            {
                // 현재 슬롯의 아이템을 들고 있는 아이템으로 설정.
                InventoryWindow.Instance.SetCurrentItem(Item);
                ResetItem();
            }
            // 들고 있는 아이템이랑 현재 슬롯의 아이템이 같으면 해당 슬롯에 들고있는 아이템 추가.
            else
            {   
                AddItem(currItem, currItem.Amount);
                InventoryWindow.Instance.CheckCurrentItem();
                return;
            }
        }
        else InventoryWindow.Instance.ResetCurrentItem();

        if (currItem != null) SetItem(currItem);
    }

    public virtual void RightClick()
    {

        // 현재 들고 있는 아이템이 없으면 리턴.
        if (!InventoryWindow.Instance.HasCurrentItem) return;

        // 
        if (!HasItem || InventoryWindow.Instance.CurrentItem.Item == Item.Item)
        {
            // 해당 슬롯에 아이템 추가.
            AddItem(InventoryWindow.Instance.CurrentItem, 1);
            InventoryWindow.Instance.CheckCurrentItem();
        }
    }

    // 마우스가 해당 슬롯에 올라가면 하이라이트.
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = highlightedColor;
    }

    // 마우스가 해당 슬롯에서 없어지면 디폴트.
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
    }
}
