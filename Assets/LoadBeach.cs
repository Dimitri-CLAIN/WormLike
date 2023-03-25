using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBeach : MonoBehaviour
{
    public LoadEventChannelSO _loadLocation = default;
    public LocationSO _beach = default;

    public void LoadLocation()
    {
        _loadLocation.RaiseEvent(_beach);
    }

}
