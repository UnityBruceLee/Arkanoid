using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformMover : MonoBehaviour
{
    [SerializeField]
    float platformSpeed;

    [SerializeField]
    Text uiLives;

    int lives;
    public int Lives
    {
        get { return lives;  }
        set
        {
            lives = value;
            SetUILives();
            if (value <= 0)
            {
                EndGame();
            }
        }
    }

    float midPoint;

    public Bounds platformBounds;

    public bool started;

    BallMover ball;
    LevelGenerator level;

    public enum State
    {
        Standing,
        MovingLeft,
        MovingRight
    }

    public State state;
    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<BallMover>();
        level = FindObjectOfType<LevelGenerator>();
        state = State.Standing;
        Lives = 3;
        platformBounds = GetComponent<SpriteRenderer>().bounds;
        midPoint = (level.leftBound.position.x + level.rightBound.position.x) / 2;
        transform.position = new Vector2(midPoint, transform.position.y);
    }
    void FixedUpdate()
    {
        MovePlatform(Input.GetAxis("Horizontal"));
        platformBounds = GetComponent<SpriteRenderer>().bounds;
    }
    void MovePlatform(float direction)
    {
        Vector2 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x + direction * platformSpeed * Time.deltaTime, level.leftBound.position.x + platformBounds.extents.x, level.rightBound.position.x - platformBounds.extents.x);
        transform.position = newPosition;
        if (direction > 0.1f)
        {
            state = State.MovingRight;
            GetComponent<SpriteRenderer>().flipX = false;
        } else if (direction < -0.1f)
        {
            state = State.MovingLeft;
            GetComponent<SpriteRenderer>().flipX = true;
        } else
        {
            state = State.Standing;
        }       
    }
    private void Update()
    {
        if (!level.levelStarted)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            started = true;
            if (state == State.MovingLeft)
            {
                ball.direction.x = -1;
                ball.ballSpeedX = Random.Range(2, 5);
            } else if (state == State.MovingRight)
            {
                ball.direction.x = 1;
                ball.ballSpeedX = Random.Range(2, 5);
            }
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    void SetUILives()
    {
        uiLives.text = Lives.ToString();
    }
}
