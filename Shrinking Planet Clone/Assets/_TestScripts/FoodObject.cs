using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int RestoreHealthValue;

    public void Awake()
    {
        Type = ItemType.Food;
    }
}