using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] ReactorParameterType parameterType;
    [SerializeField] List<int> statesWhenVisible = new List<int>();
    [SerializeField] GameObject vfxObject = null;

    private void Awake()
    {
        Reactor reactor = FindFirstObjectByType<Reactor>();
        if (reactor != null)
        {
            reactor.OnParameterReachInterval.AddListener(HandleParameterReachInterval);
            Debug.Log($"{gameObject.name} has conencted to a Reactor's OnParameterReachInterval event");
        }
        else
            Debug.LogWarning($"{gameObject.name} has not found a Reactor to connect to its OnParameterReachInterval event!");
    }

    private void HandleParameterReachInterval(ReactorParameterType type, int newState)
    {
        if (type != parameterType)
            // unrelated parameter's state was updated
            return;

        foreach (int state in statesWhenVisible)
        {
            if (state == newState)
            {
                if (!vfxObject.gameObject.activeSelf) // object is not currently active
                    vfxObject.SetActive(true);
                Debug.Log($"vfx ON: {gameObject.name}");
                return;
            }
        }

        // parameter in state which does not need this vfx -> hide the fvx object
        Debug.Log($"vfx OFF: {gameObject.name}");
        vfxObject.SetActive(false);
    }

}
