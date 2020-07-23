using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMover : MonoBehaviour
{
    [SerializeField]
    float ballSpeedY;

    float rotationAngle;

    public float ballSpeedX;

    Bounds ballBounds;

    PlatformMover platform;
    LevelGenerator level;

    bool touchedCreature;

    bool touchedPlatformX;

    bool dead;

    public Vector2 direction;

    void Start()
    {
        platform = FindObjectOfType<PlatformMover>();
        level = FindObjectOfType<LevelGenerator>();
        ballSpeedX = 0;
        direction = new Vector2(1, 1);
    }

    void FixedUpdate()
    {
        if (platform.started)
        {
            transform.Rotate(0f, 0.0f, rotationAngle, Space.Self);
            CheckForWallsCollisions();
            CheckForPlatformCollisions();
            CheckForCreatureCollisions();
            MoveBall();
        }
        else
        {
            transform.position = new Vector2(platform.transform.position.x, transform.position.y);
        }
        ballBounds = GetComponent<SpriteRenderer>().bounds;
    }

    void MoveBall()
    {
        Vector2 newPosition = transform.position;
        newPosition = new Vector2 (newPosition.x + ballSpeedX * Time.deltaTime * direction.x, newPosition.y + ballSpeedY * Time.deltaTime * direction.y);
        transform.position = newPosition;
    }

    void CheckForWallsCollisions()
    {
        if (transform.position.x < level.leftBound.position.x + ballBounds.extents.x || transform.position.x > level.rightBound.position.x - ballBounds.extents.x)
        {
            PlaySound();
            direction.x = -direction.x;
        }
        if (transform.position.y > level.upBound.position.y - ballBounds.extents.y)
        {
            PlaySound();
            direction.y = -direction.y;
        }
        if (transform.position.y < level.downBound.position.y)
        {
            if(dead)
            {
                return;
            }
            StartCoroutine(Dying());
        }
    }

    IEnumerator Dying()
    {
        dead = true;
        FindObjectOfType<SoundSystem>().PlayLooseLife();
        yield return new WaitForSeconds(0.5f);
        platform.started = false;
        ballSpeedX = 0;
        transform.position = new Vector2(platform.transform.position.x, platform.platformBounds.max.y + ballBounds.extents.y + 0.08f);
        platform.Lives--;
        dead = false;
    }

    void CheckForPlatformCollisions()
    {
        if (ballBounds.Intersects(platform.platformBounds))
        {
            if (touchedPlatformX)
            {
                return;
            }
            PlaySound();
            touchedPlatformX = true;
            StartCoroutine(ResetBool());
            if (platform.platformBounds.ClosestPoint(transform.position).x == platform.platformBounds.max.x && platform.state == PlatformMover.State.MovingRight || platform.platformBounds.ClosestPoint(transform.position).x == platform.platformBounds.min.x && platform.state == PlatformMover.State.MovingLeft)
            {
                ballSpeedX += 3f;
                return;
            }
            else if (platform.platformBounds.ClosestPoint(transform.position).x == platform.platformBounds.min.x || platform.platformBounds.ClosestPoint(transform.position).x == platform.platformBounds.max.x)
            {
                direction.x = -direction.x;
                return;
            }
            direction.y = -direction.y;
            ballSpeedX += Random.Range(-1f, 1f);
            if ((platform.state == PlatformMover.State.MovingRight && direction.x > 0) || (platform.state == PlatformMover.State.MovingLeft && direction.x < 0))
            {
                ballSpeedX = Mathf.Clamp(ballSpeedX + Random.Range(0, 2f), 0, 10);
                if (ballSpeedX == 0)
                {
                    direction.x = -direction.x;
                    ballSpeedX += Random.Range(0f, 2f);
                }
                rotationAngle = -45f;
            }
            else if ((platform.state == PlatformMover.State.MovingRight && direction.x < 0) || (platform.state == PlatformMover.State.MovingLeft && direction.x > 0))
            {
                ballSpeedX = Mathf.Clamp(ballSpeedX - Random.Range(0, 2f), 0, 10);
                if (ballSpeedX == 0)
                {
                    direction.x = -direction.x;
                    ballSpeedX += Random.Range(0, 2f);
                }
                rotationAngle = 45f;
            }
        }
    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(0.3f);
        touchedPlatformX = false;
    }

    void CheckForCreatureCollisions()
    {
        if (level.creatures.Count != 0)
        {
            for (int i = 0; i < level.creatures.Count; i++)
            {
                if (ballBounds.Intersects(level.creatures[i].GetComponent<SpriteRenderer>().bounds))
                {
                    if (!touchedCreature)
                    {
                        PlaySound();
                        touchedCreature = true;
                        Bounds creatureBounds = level.creatures[i].GetComponent<SpriteRenderer>().bounds;
                        if (creatureBounds.ClosestPoint(transform.position).x == creatureBounds.max.x || creatureBounds.ClosestPoint(transform.position).x == creatureBounds.min.x)
                        {
                            direction.x = -direction.x;
                            if (ballSpeedX <= 1f)
                            {
                                ballSpeedX += Random.Range(0.5f, 1f);
                            }
                        }
                        else if (creatureBounds.ClosestPoint(transform.position).y == creatureBounds.max.y || creatureBounds.ClosestPoint(transform.position).y == creatureBounds.min.y)
                        {
                            direction.y = -direction.y;
                        }
                        level.creatures[i].GetComponent<Creature>().TakeDamage();
                    }
                    touchedCreature = false;
                }
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
