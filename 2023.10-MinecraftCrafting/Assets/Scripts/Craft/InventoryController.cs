using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;

    public Slot[,] MainSlots { get; private set; }
    public Slot[,] AdditionalSlots { get; private set; }

    [SerializeField]
    private GameObject slotPref;
    [SerializeField]
    private Transform mainSlotsGrid;
    [SerializeField]
    private Transform additionalSlotsGrid;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        InitTestInventory();
    }

    private void InitTestInventory()
    {
        MainSlots = new Slot[1, 9];
        AdditionalSlots = new Slot[3, 9];

        CreateSlotsPrefabs();

        MainSlots[0, 0].SetItem(new ItemInSlot(ItemsManager.Instance.Items[0], 4));
        MainSlots[0, 1].SetItem(new ItemInSlot(ItemsManager.Instance.Items[3], 2));
        MainSlots[0, 2].SetItem(new ItemInSlot(ItemsManager.Instance.Items[5], 6));
        MainSlots[0, 3].SetItem(new ItemInSlot(ItemsManager.Instance.Items[8], 2));
        MainSlots[0, 4].SetItem(new ItemInSlot(ItemsManager.Instance.Items[9], 1));
    }

    private void CreateSlotsPrefabs()
    {
        for (int i = 0; i < MainSlots.GetLength(1); i++)
        {
            var slot = Instantiate(slotPref, mainSlotsGrid, false);
            MainSlots[0, i] = slot.AddComponent<Slot>();
        }

        for (int i = 0; i < AdditionalSlots.GetLength(0); i++)
        {
            for (int k = 0; k < AdditionalSlots.GetLength(1); k++)
            {
                var slot = Instantiate(slotPref, additionalSlotsGrid, false);
                AdditionalSlots[i, k] = slot.AddComponent<Slot>();
            }
        }
    }
}
