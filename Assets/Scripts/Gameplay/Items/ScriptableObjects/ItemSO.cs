using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This class is used for Items.
/// Example: An sword
/// </summary>

[CreateAssetMenu(menuName = "Item")]
public class ItemSO : DescriptionBaseSO
{
    private string _name;
    private int _ammo;
    private float _range;
}
// TODO: Add prefab ?