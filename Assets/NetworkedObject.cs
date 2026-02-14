using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class NetworkedObject : NetworkBehaviour
{
    // Used when the object is initialized
    public abstract void Initialize();

    // unique identifier for this object
    public abstract string GetNetworkId();
}
