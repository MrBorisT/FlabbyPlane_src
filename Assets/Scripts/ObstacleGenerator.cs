using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    float generationTimer;
    [SerializeField] GameObject collectible;
    [SerializeField] float spireHeight = 4f;
    [SerializeField] Vector2 collectibleOffset = new Vector2(-0.3f, 0.3f);
    [Header("Min/Max Timers")]
    [SerializeField] float minGenerationTime = 1f;
    [SerializeField] float maxGenerationTime = 5f;
    Obstacle ceilingObstacle;
    Obstacle floorObstacle;
    [Header("Spawn points & end point")]
    [SerializeField] Transform endPoint;
    [SerializeField] Transform upPoint;
    [SerializeField] Transform downPoint;
    [Header("Variables")]
    [SerializeField] float minScale = 0.8f;
    [SerializeField] float maxScale = 1.2f;
    
    float currentGenerationTime;
    float scrollingsSpeed = -2.8f;

    public Obstacle CeilingObstacle { set => ceilingObstacle = value; }
    public Obstacle FloorObstacle { set => floorObstacle = value; }
    public float ScrollingsSpeed { get => scrollingsSpeed; set => scrollingsSpeed = value; }

    void Start()
    {
        InitNewGenerationTimeAndResetTimer();
    }

    void Update()
    {
        TimerTick();
    }

    private void TimerTick()
    {
        generationTimer += Time.deltaTime;
        if (generationTimer >= currentGenerationTime)
        {
            GenerateObstacle();
            InitNewGenerationTimeAndResetTimer();
        }
    }

    void InitNewGenerationTimeAndResetTimer()
    {
        generationTimer = 0f;
        currentGenerationTime = Random.Range(minGenerationTime, maxGenerationTime);
    }

    void GenerateObstacle()
    {
        Obstacle newObstacleUp = Instantiate(ceilingObstacle);
        Obstacle newObstacleDown = Instantiate(floorObstacle);
        GameObject newCollectible = Instantiate(collectible);

        Vector3 newPositionUp = new Vector3(upPoint.position.x, upPoint.position.y, 0);
        newObstacleUp.transform.position = newPositionUp;
        Vector3 newPositionDown = new Vector3(downPoint.position.x, downPoint.position.y, 0);
        newObstacleDown.transform.position = newPositionDown;

        float newScale = Random.Range(minScale, maxScale);
        float xMirrored = 1f;
        if (Random.Range(0, 2) == 1)
            xMirrored = -1f;
                        
        newObstacleUp.transform.localScale = new Vector3(xMirrored, newScale, 1);
        ScrollingObject newObstacleUpScrollingObject = newObstacleUp.GetComponent<ScrollingObject>();
        newObstacleUpScrollingObject.EndPoint = endPoint;
        newObstacleUpScrollingObject.ScrollingSpeed = scrollingsSpeed;

        newScale = (minScale + maxScale) - newScale;
        newObstacleDown.transform.localScale = new Vector3(xMirrored, newScale, 1);
        ScrollingObject newObstacleDownScrollingObject = newObstacleDown.GetComponent<ScrollingObject>();
        newObstacleDownScrollingObject.EndPoint = endPoint;
        newObstacleDownScrollingObject.ScrollingSpeed = scrollingsSpeed;
                
        Vector3 collectiblePosition = new Vector3(newPositionUp.x + collectibleOffset.x, newPositionDown.y + newScale * spireHeight + collectibleOffset.y, newPositionUp.z);
        newCollectible.transform.position = collectiblePosition;
        ScrollingObject newCollectibleScrollingObject = newCollectible.GetComponent<ScrollingObject>();
        newCollectibleScrollingObject.EndPoint = endPoint;
        newCollectibleScrollingObject.ScrollingSpeed = scrollingsSpeed;
    }
}
