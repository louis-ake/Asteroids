using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance;

    private int _livesNo = 2;

    [SerializeField] private int _startingLives = 2;
    [SerializeField] private float _popUpTimeOnScreen = 2f;

    [Header("Objects To Reset")]

    [SerializeField] private Ship _ship;
    [SerializeField] private AsteroidController _asteroidController;

    [Header("Text Objects")]

    [SerializeField] private Text _popUpText;
    [SerializeField] private Text _lives;

    [Header ("Win/Lose messages")]

    [SerializeField] private string _lifeLostMessage = "YOU LOST A LIFE :(";
    [SerializeField] private string _loseMessage = "YOU LOST :( :( :(";
    [SerializeField] private string _winMessage = "YOU WON :)";

    [Header("Win/Lose colours")]

    [SerializeField] private Color _loseColour;
    [SerializeField] private Color _winColour;

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

    public IEnumerator LoseLife()
    {
        Debug.Log("Ship has crashed into asteroid so losing life");
        _livesNo -= 1;
        SetLivesText(_livesNo);
        Color c = _loseColour;
        c.a = 1;
        _popUpText.color = c;
        var lost = false;
        if (_livesNo > 0)
        {
            _popUpText.text = _lifeLostMessage;
        } else
        {
            _popUpText.text = _loseMessage;
            lost = true;
        }
        _popUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(_popUpTimeOnScreen);
        _popUpText.gameObject.SetActive(false);
        if (lost)
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        _asteroidController.Reset();
        _ship.Reset();
        _livesNo = _startingLives;
        SetLivesText(_startingLives);
    }

    public IEnumerator GameWon()
    {
        Debug.Log("All asteroids destroyed so game won");
        Color c = _winColour;
        c.a = 1;
        _popUpText.color = c;
        _popUpText.text = _winMessage;
        _popUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(_popUpTimeOnScreen);
        _popUpText.gameObject.SetActive(false);
        ResetGame();
    }

    private void SetLivesText(int lives)
    {
        _lives.text = "LIVES: " + lives;
    }
}
