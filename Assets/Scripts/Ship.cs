using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private Vector2 _startPosition = new Vector2(0f, 0f);

    [SerializeField] private float _gridbounds = 5;
    [SerializeField] private float _wrapOffset = 0.2f;

    [Header ("Movement variables")]

    [SerializeField] private float _accelerationSpeed = 8f;
    [SerializeField] private float _rotationSpeed = 2f;

    [Header ("Missile Objects")]

    [SerializeField] private GameObject _missile;
    [SerializeField] private GameObject _missileHolder;
    [SerializeField] private Transform _missileStartPoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovementControls();
        ShootingControls();
        CheckForWrapping();
    }

    private void ShootingControls()
    {
        if (Input.GetKeyDown("space"))
        {
            Instantiate(_missile, _missileStartPoint.position, transform.rotation, _missileHolder.transform);
        }
    }

    private void MovementControls()
    {
        float rotation = -Input.GetAxis("Horizontal");
        float acceleration = Input.GetAxis("Vertical");
        transform.Rotate(0, 0, rotation * Time.deltaTime * 360);
        _rb.AddForce(transform.up * _accelerationSpeed * acceleration);
    }

    private void CheckForWrapping()
    {
        if (System.Math.Abs(transform.position.x) > _gridbounds)
        {
            Debug.Log("Ship OOB on x axis");
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
            Debug.Log("Ship OOB on y axis");
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

    public void Reset()
    {
        Debug.Log("Resetting ship");
        transform.position = _startPosition;
        _rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _rb.angularVelocity = 0f;
        transform.eulerAngles = new Vector2(0f, 0f);
    }
}
