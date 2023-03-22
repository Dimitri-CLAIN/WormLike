using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerCanvas : NetworkBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject turnDisplay;
    [SerializeField]
    private TextMeshProUGUI chrono;
    
    public override void OnStartAuthority()
    {
        enabled = true;
        canvas.enabled = true;
    }
}
