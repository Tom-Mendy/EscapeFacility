using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NoiseEmitter : MonoBehaviour
{
    // Emit a noise event when running or interacting
    public float walkNoiseRadius = 2f;
    public float runNoiseRadius = 6f;

    [HideInInspector] public bool isRunning = false;

    public void EmitWalkNoise()
    {
        AIManager.Instance?.RaiseNoise(transform.position, walkNoiseRadius);
    }

    public void EmitRunNoise()
    {
        AIManager.Instance?.RaiseNoise(transform.position, runNoiseRadius);
    }
}
