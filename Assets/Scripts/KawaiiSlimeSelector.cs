using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kawaii Slime Selector")]
public class KawaiiSlimeSelector : ScriptableObject
{
    public List<GameObject> slimesList;

    public enum KawaiiSlime : int
    {
        Slime = 0, SlimeKing = 1, SlimeSoldier = 2, SlimeViking = 3, SlimeBunny = 4,
        ChongusSlime = 5, ChongusSlimeKing = 6, ChongusSlimeLeaf = 7, ChongusSlimeSprout = 8 
    }

    public GameObject SelectSlime(KawaiiSlime slime)
    {
        return slimesList[(int)slime];
    }


    public GameObject SelectSlimeByInt(int index)
    {
        if (index >= slimesList.Count) return slimesList[0];
        
        return slimesList[index];
    }
}
