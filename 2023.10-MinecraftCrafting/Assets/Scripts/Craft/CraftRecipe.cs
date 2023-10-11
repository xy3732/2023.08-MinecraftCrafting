﻿public class CraftRecipe
{
    // 크레프팅 레시피 리스트
    public Item[,] Items { get; private set; }
    // 크레프팅 후 만들어질 갯수
    public int Amount { get; private set; }

    public Item[] ItemsOrder { get; private set; }

    public CraftRecipe(Item[,] items, int amount)
    {
        Items = items;
        Amount = amount;

        ItemsOrder = new Item[Items.Length];

        for (int orderId = 0, i = 0; i < Items.GetLength(0); i++)
        {
            for (int k = 0; k < Items.GetLength(1); k++)
            {
                ItemsOrder[orderId++] = Items[i, k];
            }
        }
    }
}
