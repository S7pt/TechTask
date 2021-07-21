using UnityEngine;

    public class ObstacleScript : MonoBehaviour
    {
        [SerializeField] private Material _damagedMaterial;
        [SerializeField] private LayerMask _obstacleMask;
        private MeshRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                Collider[] objectsInRadius=Physics.OverlapSphere(transform.position, other.GetComponent<ProjectileScript>().ExplosionRadius,
                    _obstacleMask);
                foreach (var obstacle in objectsInRadius)
                {
                    obstacle.gameObject.GetComponent<ObstacleScript>().Die();
                }
                Die();
            }
        }

        public void Die()
        {
            _renderer.material = _damagedMaterial;
            Destroy(gameObject, 0.5f); 
        }
    }