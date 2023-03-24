using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Worm))]
public class PlayerCanvas : NetworkBehaviour
{
    [SerializeField]
    private Worm worm;
    public Worm Worm
    {
        get
        {
            if (worm != null) return worm;
            return worm = GetComponent<Worm>();
        }
    }
    
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject turnDisplay;
    [SerializeField]
    private TextMeshProUGUI chrono;
    private Color defaultColor;
    
    private Coroutine countdownCoroutine;
    public event Action OnButtonEndTurnPressed;
    
    public override void OnStartAuthority()
    {
        enabled = true;
        canvas.enabled = true;
        defaultColor = chrono.color;
    }


    #region Client

    [TargetRpc]
    public void TargetEnableTurnHUD(NetworkConnection conn, int time)
    {
        turnDisplay.gameObject.SetActive(true);
        countdownCoroutine = StartCoroutine(RunCountdown(time));
    }



    [TargetRpc]
    public void TargetDisableTurnHUD(NetworkConnection conn)
    {
        turnDisplay.gameObject.SetActive(false);
        StopCoroutine(countdownCoroutine);
    }



    [Client]
    private IEnumerator RunCountdown(int time)
    {
        chrono.color = defaultColor;

        while (time >= 0)
        {
            if (time <= 5)
                chrono.color = Color.red;
            chrono.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
    }
    

    [Command]
    public void EndTurnButton() 
    {
        Debug.Log($"<color=yellow>Button clicked    </color>");
        OnButtonEndTurnPressed?.Invoke();
    }
    

    #endregion
}
