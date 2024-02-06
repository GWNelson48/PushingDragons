using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    EnemySpawner enemySpawner;
    WaveConfigSO waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;

    [Header("Enemy Wait Point")]
    [SerializeField] int waitWaypoint;
    [SerializeField] float waitTime = 2f;
    public bool waitActive;

    [Tooltip("This will control if an enemy should come onto the screen and loop between waypoints")]
    public bool isLoopingEnemy;

    private float timer;

    void Awake() 
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        waveConfig = enemySpawner.GetCurrentWave();
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }

    void Update() 
    {
        FollowPath();
    }

    void FollowPath()
    {
        if (!isLoopingEnemy && waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);

            if (!waitActive)
            {
                ContinuousPointMovement(targetPosition);
            }

            if (waitActive)
            {
                WaitAtPointMovement(targetPosition);
            }
        }

        else if (isLoopingEnemy)
        {
            if (waypointIndex < waypoints.Count)
            {
                Vector3 targetPosition = waypoints[waypointIndex].position;
                float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);

                if (transform.position == targetPosition)
                {
                    waypointIndex++;
                }
            }
            else
            {
                waypointIndex = 1;
            }
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void ContinuousPointMovement(Vector3 targetPosition)
    {
        if (transform.position == targetPosition)
        {
            waypointIndex++;
        }
    }

    void WaitAtPointMovement(Vector3 targetPosition)
    {
        if (transform.position == targetPosition)
        {
            if (transform.position == targetPosition && waypointIndex == waitWaypoint)
            {
                timer += Time.deltaTime;

                if (timer > waitTime)
                {
                    timer = 0;
                    waypointIndex++;
                }
            }
            else if (transform.position == targetPosition && waypointIndex != waitWaypoint)
            {
                waypointIndex++;
            }
        }
    }
}
