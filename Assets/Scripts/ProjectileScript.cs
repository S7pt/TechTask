using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float _explosionRadius;

    public float ExplosionRadius
    {
        get => _explosionRadius;
        set => _explosionRadius = value;
    }
    private void Start()
    {
        _explosionRadius = transform.localScale.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Ground")&&!other.CompareTag("Castle"))
        {
            Destroy(gameObject);
        }
    }
}
