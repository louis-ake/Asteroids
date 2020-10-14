using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private string _size;

    private AsteroidController _asteroidController;

    private Rigidbody2D _rb;

    private float _gridbounds = 5;
    [SerializeField] private float _wrapOffset = 0.2f;

    [Header("Asteroid speed boundaries")]

    [SerializeField] private float _largeMinSpeed = 5;
    [SerializeField] private float _largeMaxSpeed = 15;
    [SerializeField] private float _mediumMinSpeed = 15;
    [SerializeField] private float _mediumMaxSpeed = 30;
    [SerializeField] private float _smallMinSpeed = 30;
    [SerializeField] private float _smallMaxSpeed = 60;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        MoveOffInRandomDirection();
    }
    
    private void FixedUpdate()
    {
        CheckForWrapping();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Ship")
        {
            Debug.Log("Asteroid collided with ship");
            StartCoroutine(GameInfo.Instance.LoseLife());
        } else if (collision.collider.name == "Missile(Clone)")
        {
            AsteroidController asController = transform.parent.GetComponent<AsteroidController>();
            asController.SplitAsteroid(_size, transform);
            Destroy(gameObject);
        }
    }

    private void MoveOffInRandomDirection()
    {
        Vector2 target = new Vector2(Random.Range(-_gridbounds, _gridbounds), Random.Range(-_gridbounds, _gridbounds));
        float minSpeed;
        float maxSpeed;
        if (_size == "small")
        {
            minSpeed = _smallMinSpeed;
            maxSpeed = _smallMaxSpeed;
        } else if (_size == "medium")
        {
            minSpeed = _mediumMinSpeed;
            maxSpeed = _mediumMaxSpeed;
        } else
        {
            minSpeed = _largeMinSpeed;
            maxSpeed = _largeMaxSpeed;
        }
        float speed = Random.Range(minSpeed, maxSpeed);
        _rb.AddForce(target * speed);
    }

    private void CheckForWrapping()
    {
        if (System.Math.Abs(transform.position.x) > _gridbounds)
        {
            Debug.Log("Asteroid OOB on x axis");
            if (transform.position.x > _gridbounds + _wrapOffset)
            {
                transform.position = new Vector2(-transform.position.x + _wrapOffset, transform.position.y);
            }
            else if (transform.position.x < _gridbounds - _wrapOffset)
            {
                transform.position = new Vector2(-transform.position.x - _wrapOffset, transform.position.y);
            }
        }
        else if (System.Math.Abs(transform.position.y) > _gridbounds)
        {
            Debug.Log("Asteroid OOB on y axis");
            if (transform.position.y > _gridbounds + _wrapOffset)
            {
                transform.position = new Vector2(transform.position.x, -transform.position.y + _wrapOffset);
            }
            else if (transform.position.y < _gridbounds - _wrapOffset)
            {
                transform.position = new Vector2(transform.position.x, -transform.position.y - _wrapOffset);
            }
        }
    }
}
