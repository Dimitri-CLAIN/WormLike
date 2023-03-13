using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This class is used for Rules that have a time(seconds) argument.
/// Example: An match length rule
/// </summary>

[CreateAssetMenu(menuName = "Rules/Item")]
public class ItemRuleSO : DescriptionBaseSO
{
    private (ItemSO item, bool isSelected)[] _items;
    // Todo:
    //  Mettre ça en type de IBasicRule
    //  Mettre itemRule
    
    
}