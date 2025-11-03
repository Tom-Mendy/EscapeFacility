using System;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;

    public static event Action<Transform> OnPlayerSpotted;
    public static event Action<Vector3, float> OnNoiseRaised;

    public bool globalAlarm = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayerSpotted(Transform player)
    {
        globalAlarm = true;
        OnPlayerSpotted?.Invoke(player);
    }

    public void RaiseNoise(Vector3 pos, float radius)
    {
        OnNoiseRaised?.Invoke(pos, radius);
    }
}
