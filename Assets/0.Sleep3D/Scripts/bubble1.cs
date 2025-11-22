using UnityEngine;

public class BubbleBreath : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 0.2f;
    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float s = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localScale = baseScale + new Vector3(s, s, s);
    }
}