using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance;

    private bool _gameStarted = false;

    private int _livesNo = 2;

    [SerializeField] private int _startingLives = 2;
    [SerializeField] private float _popUpTimeOnScreen = 2f;

    [Header("Objects To Reset")]

    [SerializeField] private Ship _ship;
    [SerializeField] private AsteroidController _asteroidController;

    [Header("Text Objects")]

    [SerializeField] private Text _popUpText;
    [SerializeField] private Text _lives;

    [Header("Player messages")]

    [SerializeField] private string _gameStartMessage = "PRESS SPACE TO START";
    [SerializeField] private string _lifeLostMessage = "YOU LOST A LIFE :(";
    [SerializeField] private string _loseMessage = "YOU LOST :( :( :(";
    [SerializeField] private string _winMessage = "YOU WON :)";

    [Header("Colours")]

    [SerializeField] private Color _loseColour;
    [SerializeField] private Color _winColour;
    [SerializeField] private Color _startGameColour;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetLivesText(_startingLives);
    }

    private void Update()
    {
        if(_gameStarted)
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            BeginGame();
        }
    }

    private void BeginGame()
    {
        _popUpText.gameObject.SetActive(false);
        _ship.gameObject.gameObject.SetActive(true);
        _asteroidController.gameObject.SetActive(true);
        _lives.gameObject.SetActive(true);
        _gameStarted = true;
    }

    public IEnumerator LoseLife()
    {
        Debug.Log("Ship has crashed into asteroid so losing life");
        _livesNo -= 1;
        SetLivesText(_livesNo);
        var lost = false;
        if (_livesNo > 0)
        {
            SetActivatePopupText(_loseColour, _lifeLostMessage);
        } else
        {
            SetActivatePopupText(_loseColour, _loseMessage);
            lost = true;
        }
        yield return new WaitForSeconds(_popUpTimeOnScreen);
        if (lost)
        {
            EndRound();
        } else
        {
            _popUpText.gameObject.SetActive(false);
        }
    }

    private void EndRound()
    {
        _asteroidController.Reset();
        _ship.Reset();
        _ship.gameObject.gameObject.SetActive(false);
        _asteroidController.gameObject.SetActive(false);
        _lives.gameObject.SetActive(false);
        _livesNo = _startingLives;
        SetLivesText(_startingLives);
        SetActivatePopupText(_startGameColour, _gameStartMessage);
        _gameStarted = false;
    }

    public IEnumerator GameWon()
    {
        Debug.Log("All asteroids destroyed so game won");
        SetActivatePopupText(_winColour, _winMessage);
        yield return new WaitForSeconds(_popUpTimeOnScreen);
        EndRound();
    }

    private void SetLivesText(int lives)
    {
        _lives.text = "LIVES: " + lives;
    }

    private void SetActivatePopupText(Color c, string s)
    {
        _popUpText.color = c;
        _popUpText.text = s;
        _popUpText.gameObject.SetActive(true);
    }
}