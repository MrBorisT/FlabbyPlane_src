using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    float scrollSpeed;
    [SerializeField] float minScrollSpeed = -3f;
    [SerializeField] float maxScrollSpeed = -2.8f;
    [SerializeField] ObstacleGenerator obstacleGenerator;
    [SerializeField] BackgroundStuffGenerator backgroundStuffGenerator;
    [SerializeField] Player player;
    [SerializeField] WeatherSO[] weathers;
    [SerializeField] ScrollingBackground[] scrollingBackgrounds;
    [SerializeField] GameObject tap;
    [SerializeField] TextMeshProUGUI scoreText;
    int score;

    private void Start()
    {
        player.OnStateChange += Player_OnStateChange;
        ResetScore();
        SetWeather();
    }

    private void Player_OnStateChange(object sender, Player.OnStateChangeEventArgs e)
    {
        switch (e.state)
        {
            case Player.State.GameStart:
                GameStart();
                break;
            case Player.State.GameReset:
                GameReset();
                break;
            case Player.State.GameOver:
                break;
            case Player.State.GameOverSequence:
                GameOverSequence();
                break;
        }
    }

    private void GameStart()
    {
        obstacleGenerator.gameObject.SetActive(true);
        backgroundStuffGenerator.gameObject.SetActive(true);
        tap.gameObject.SetActive(false);
    }

    private void GameReset()
    {
        ResetScore();
        SetWeather();
        ScrollingObject[] scrollies = FindObjectsOfType<ScrollingObject>();
        foreach (ScrollingObject scrollie in scrollies)
        {
            if (!scrollie.Infinite)
                Destroy(scrollie.gameObject);
        }

        Init();
        SetSpeedToScrollingsInObstacleGenerator(scrollSpeed);
        SetSpeedToScrollings(scrollSpeed);

        foreach (ScrollingBackground scrollingBG in scrollingBackgrounds)
        {
            scrollingBG.ResetBG();
        }
    }

    private void SetSpeedToScrollingsInObstacleGenerator(float scrollSpeed)
    {
        obstacleGenerator.ScrollingsSpeed = scrollSpeed;
    }

    private void GameOverSequence()
    {
        SetSpeedToScrollings(0f);

        foreach (ScrollingBackground scrollingBG in scrollingBackgrounds)
        {
            scrollingBG.ScrollSpeed = 0f;
        }

        obstacleGenerator.gameObject.SetActive(false);
        backgroundStuffGenerator.gameObject.SetActive(false);
    }

    private void Init()
    {
        scrollSpeed = Random.Range(minScrollSpeed, maxScrollSpeed);
        obstacleGenerator.gameObject.SetActive(false);
        backgroundStuffGenerator.gameObject.SetActive(false);
        tap.gameObject.SetActive(true);
    }    

    private void SetSpeedToScrollings(float speed)
    {
        ScrollingObject[] scrollings = FindObjectsOfType<ScrollingObject>();
        foreach (ScrollingObject scrolling in scrollings)
        {
            scrolling.ScrollingSpeed = speed;
        }
    }

    private void SetWeather()
    {
        WeatherSO weatherSO = weathers[Random.Range(0, weathers.Length)];
        obstacleGenerator.CeilingObstacle = weatherSO.UpObstacle;
        obstacleGenerator.FloorObstacle = weatherSO.DownObstacle;
        Ground[] grounds = FindObjectsOfType<Ground>();
        foreach (Ground ground in grounds)
        {
            ground.GetComponent<SpriteRenderer>().sprite = weatherSO.Ground.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void AddPoint(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    private void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
