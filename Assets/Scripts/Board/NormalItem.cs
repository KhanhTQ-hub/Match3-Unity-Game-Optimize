using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class NormalItem : Item
{
    public enum eNormalType
    {
        TYPE_ONE,
        TYPE_TWO,
        TYPE_THREE,
        TYPE_FOUR,
        TYPE_FIVE,
        TYPE_SIX,
        TYPE_SEVEN
    }

    public eNormalType ItemType;

    public void SetType(eNormalType type)
    {
        ItemType = type;
    }

    protected override string GetPrefabName(SkinType skinType)
    {
        string prefabname = string.Empty;
        switch (ItemType)
        {
            case eNormalType.TYPE_ONE:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_ONE : Constants.PREFAB_FISH_TYPE_ONE;
                break;
            case eNormalType.TYPE_TWO:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_TWO : Constants.PREFAB_FISH_TYPE_TWO;
                break;
            case eNormalType.TYPE_THREE:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_THREE : Constants.PREFAB_FISH_TYPE_THREE;
                break;
            case eNormalType.TYPE_FOUR:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_FOUR : Constants.PREFAB_FISH_TYPE_FOUR;
                break;
            case eNormalType.TYPE_FIVE:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_FIVE : Constants.PREFAB_FISH_TYPE_FIVE;
                break;
            case eNormalType.TYPE_SIX:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_SIX : Constants.PREFAB_FISH_TYPE_SIX;
                break;
            case eNormalType.TYPE_SEVEN:
                prefabname = skinType == SkinType.FOOD ? Constants.PREFAB_FOOD_TYPE_SEVEN : Constants.PREFAB_FISH_TYPE_SEVEN;
                break;
        }

        return prefabname;
    }

    internal override bool IsSameType(Item other)
    {
        NormalItem it = other as NormalItem;

        return it != null && it.ItemType == this.ItemType;
    }
}
