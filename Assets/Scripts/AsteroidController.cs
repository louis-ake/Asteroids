using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    private bool _gameWonSequenceStarted = false;

    [SerializeField] private float _asteroidCreationInnerBoundary = 1.5f;
    [SerializeField] private float _asteroidCreationOuterBoundary = 4.5f;

    [Header("Asteroids to Create")]

    [SerializeField] private int _largeAsteroidsToStartWith = 2;
    [SerializeField] private int _mediumAsteroidsOnLargeDestroyed = 2;
    [SerializeField] private int _smallAsteroidsOnMediumDestroyed = 2;

    [Header("Asteroid prefabs")]

    [SerializeField] private GameObject _largeAsteroid;
    [SerializeField] private GameObject _mediumAsteroid;
    [SerializeField] private GameObject _smallAsteroid;
    
    private void Start()
    {
        CreateAsteroids(_largeAsteroidsToStartWith);
    }

    private void Update()
    {
        CheckForWin();
    }

    private void CreateAsteroids(int noOfAsteroids)
    {
        for (int i = 0; i < noOfAsteroids; i++)
        {
            var asteroidX = Random.Range(-_asteroidCreationOuterBoundary, _asteroidCreationOuterBoundary);
            var asteroidY = Random.Range(-_asteroidCreationOuterBoundary, _asteroidCreationOuterBoundary);
            if (asteroidX < _asteroidCreationInnerBoundary && asteroidX > -_asteroidCreationInnerBoundary)
            {
                asteroidX = _asteroidCreationInnerBoundary;
            }
            if (asteroidY < _asteroidCreationInnerBoundary && asteroidY > -_asteroidCreationInnerBoundary)
            {
                asteroidY = _asteroidCreationInnerBoundary;
            }
            Instantiate(_largeAsteroid, new Vector2(asteroidX, asteroidY), transform.rotation, transform);
        }
        _gameWonSequenceStarted = false;
    }

    public void SplitAsteroid(string size, Transform transform)
    {
        Debug.Log("Asteroid Controller notified that asteroid has been hit with missile");
        if (size == "large")
        {
            Debug.Log("Destroyed large asteroid so instantiating medium asteroids");
            for (int i = 0; i < _mediumAsteroidsOnLargeDestroyed; i++)
            {
                Instantiate(_mediumAsteroid, transform.position, transform.rotation, this.transform);
            }
        } else if (size == "medium")
        {
            Debug.Log("Destroyed medium asteroid so instantiating small asteroids");
            for (int i = 0; i < _smallAsteroidsOnMediumDestroyed; i++)
            {
                Instantiate(_smallAsteroid, transform.position, transform.rotation, this.transform);
            }
        } else if (size == "small")
        {
            Debug.Log("Destroyed small asteroid so not instantiating anything");
        }
    }

    public void Reset()
    {
        Debug.Log("Resetting asteroids");
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        CreateAsteroids(_largeAsteroidsToStartWith);
    }

    private void CheckForWin()
    {
        if (transform.childCount < 1 && _gameWonSequenceStarted == false)
        {
            _gameWonSequenceStarted = true;
            StartCoroutine(GameInfo.Instance.GameWon());
        }
    }
}
