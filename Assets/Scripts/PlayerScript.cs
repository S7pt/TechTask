using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _launchPosition;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _attackSpeed;
    private GameObject _currentProjectile;
    private ProjectileScript _currentProjectileScript;
    private static event Action<Vector3> onSizeChanged;
    private static event Action onDeath;
    private Vector3 _chargeRate = new Vector3(0.002f,0.002f,0.002f);
    private List<ObstacleScript> _possibleObstacles;
    private float _impulsePower = 7;
    private float _projectileChargeMultiplier = 1.05f;
    private float _originalScaleValue;
    private float _timeToNextShot;
    private bool _hasObstacles;
    private bool _isCharging;
    private float _castleLocationByX;
    
    private void Start()
    {
        _castleLocationByX = FindObjectOfType<CastleScript>().gameObject.transform.position.x;
        _originalScaleValue = transform.localScale.x;
        _healthSlider.maxValue = _originalScaleValue;
        _healthSlider.value = _originalScaleValue;
        onSizeChanged(transform.localScale);
        InvokeRepeating("CheckForObstacles",0,0.5f);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Time.time >= _timeToNextShot)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase==TouchPhase.Moved)
            {
                if (!_isCharging || _currentProjectile == null)
                {
                    AdjustSpawnPosition();
                }
                else
                {
                    ChargeProjectile();
                }
            }

            _healthSlider.value = transform.localScale.x;
        }
        if (Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Ended)
        {
            Shoot(_launchPosition.localPosition);
        }

        if (!_hasObstacles && !_isCharging)
        {
            transform.Translate(Vector3.right * (2f * Time.deltaTime));
        }
    }

    private void ChargeProjectile()
    {
        if ((gameObject.transform.localScale - _chargeRate).x <= 0)
        {
            return;
        }

        if (_currentProjectile == null)
        {
            return;
        }
        gameObject.transform.localScale -= _chargeRate;
        CheckForPlayerScaling();
        _currentProjectile.transform.localScale += _chargeRate * _projectileChargeMultiplier;
        _currentProjectileScript.ExplosionRadius += _chargeRate.x * _projectileChargeMultiplier;
        onSizeChanged(gameObject.transform.localScale);
    }

    private void Shoot(Vector3 direction)
    {
        if (_currentProjectile != null)
        {
            _currentProjectile.GetComponent<Rigidbody>().AddForce(direction * _impulsePower,ForceMode.VelocityChange);
            Destroy(_currentProjectile,10);
            _isCharging = false;
            _timeToNextShot = Time.time + _attackSpeed;
        }
    }

    private void AdjustSpawnPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 position = hit.point;
            _launchPosition.localPosition = (position - transform.position).normalized;
            _currentProjectile = Instantiate(_projectilePrefab, _launchPosition.position,_launchPosition.rotation);
            _currentProjectileScript = _currentProjectile.GetComponent<ProjectileScript>();
            _isCharging = true;
        }
    }

    private void CheckForPlayerScaling()
    {
        if (_currentProjectile != null)
        {
            if (transform.localScale.x / _originalScaleValue < .2f)
            {
                Destroy(_currentProjectile);
                onDeath();
                Destroy(gameObject);
            }
        }
    }

    private void CheckForObstacles()
    {
        _possibleObstacles=new List<ObstacleScript>();
        _possibleObstacles = FindObjectsOfType<ObstacleScript>().ToList();
        foreach (var obstacle in _possibleObstacles)
        {
            float upperBound = transform.position.z + (transform.localScale.z-0.5f);
            float lowerBound = transform.position.z - (transform.localScale.z-0.5f);
            float possibleObstacleZ = obstacle.gameObject.transform.position.z;
            if (possibleObstacleZ < upperBound && possibleObstacleZ > lowerBound && obstacle.gameObject.transform.position.x<_castleLocationByX)
            {
                _hasObstacles = true;
                return;
            }
        }

        _hasObstacles = false;
    }

    public static void SubscribeToSizeChange(Action<Vector3> method)
    {
        onSizeChanged += method;
    }

    public static void SubscribeToPlayerDeath(Action method)
    {
        onDeath += method;
    }
    
}
