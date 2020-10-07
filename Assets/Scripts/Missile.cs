using System.Linq;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _travelDistance = 5;
    [SerializeField] private float _travelSpeed = 400;

    [SerializeField] string[] _asteroidNames = new string[] {"Large Asteroid(Clone)", "Medium Asteroid(Clone)", "Small Asteroid(Clone)"};

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * _travelSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_asteroidNames.Contains(collision.collider.name))
        {
            Destroy(gameObject);
        }
    }
}
