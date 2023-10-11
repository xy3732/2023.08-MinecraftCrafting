using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryWindow : MonoBehaviour
{
    public static InventoryWindow Instance;

    public CraftController CraftController;
    public InventoryController InventoryController;

    [Space()]
    [SerializeField]
    private Image currentItemImage;
    [SerializeField]
    private TextMeshProUGUI currentAmountText;

    public ItemInSlot CurrentItem;

    public bool HasCurrentItem => CurrentItem != null;

    private void Awake()
    {
        Instance = this;
    }

    bool init = false;
    public void Open()
    {  
        if(!init)
        {
            CraftController.Init();
            InventoryController.Init();
            init = true;
        }
    }

    public void SetCurrentItem(ItemInSlot item)
    {
        CurrentItem = item;
        currentAmountText.text = CurrentItem.Amount.ToString();
        currentItemImage.gameObject.SetActive(true);
        currentItemImage.sprite = CurrentItem.Item.Sprite;
    }

    // 마우스가 들고 있는 아이템 초기화.
    public void ResetCurrentItem()
    {
        CurrentItem = null;
        currentItemImage.gameObject.SetActive(false);
    }

    // 현재 마우스가 들고 있는 아이템이 존재하는지 채크.
    public void CheckCurrentItem()
    {
        currentAmountText.text = CurrentItem.Amount.ToString();
        if (HasCurrentItem && CurrentItem.Amount < 1) ResetCurrentItem();
    }

    private void Update()
    {
        if (!HasCurrentItem) return;

        // 현재 선택한 아이템 마우스 위치로 이동시키기.
        currentItemImage.transform.position = Input.mousePosition;
    }
}
