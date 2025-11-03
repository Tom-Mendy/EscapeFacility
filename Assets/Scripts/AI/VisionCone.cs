using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float range = 10f;
    public float fov = 60f;
    public LayerMask obstructionMask;

    public float ambientMultiplierInDark = 0.5f;

    public bool CanSee(Transform observer, Transform target)
    {
        Vector3 dir = target.position - observer.position;

        if (dir.magnitude > range)
            return false;
        if (Vector3.Angle(observer.forward, dir) > fov * 0.5f)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(observer.position + Vector3.up * 1.2f, dir.normalized, out hit, range, ~0))
        {
            if (hit.transform != target) return false;
        }

        // estimate light at target (simple: sample nearby lights)
        float lightFactor = SampleLightAtPosition(target.position);
        float threshold = 0.3f; // tune
        return lightFactor > threshold;
    }

    float SampleLightAtPosition(Vector3 position)
    {
        // Simplified heuristic: count nearby point lights & intensities
        float sum = 0f;
        Collider[] hits = Physics.OverlapSphere(position, 3f);

        foreach (Collider collider in hits)
        {
            Light light = collider.GetComponent<Light>();
            if (light != null)
                sum += light.intensity;
        }

        // normalize
        return Mathf.Clamp01(sum / 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
        if (Application.isPlaying == false)
        {
            Vector3 left = Quaternion.Euler(0, -fov / 2f, 0) * transform.forward;
            Vector3 right = Quaternion.Euler(0, fov / 2f, 0) * transform.forward;
            Gizmos.DrawLine(transform.position + Vector3.up * 1.2f, transform.position + Vector3.up * 1.2f + left * range);
            Gizmos.DrawLine(transform.position + Vector3.up * 1.2f, transform.position + Vector3.up * 1.2f + right * range);
        }
    }
}
