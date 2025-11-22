using UnityEngine;

public class FloatSlowly : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(
            Mathf.Sin(Time.time * 0.2f) * 0.001f,
            Mathf.Sin(Time.time * 0.15f) * 0.002f,
            Mathf.Cos(Time.time * 0.1f) * 0.001f
        );
    }
}