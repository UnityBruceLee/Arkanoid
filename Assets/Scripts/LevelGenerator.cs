using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    Transform creaturePrefab, roboPrefab, crabPrefab, questionPrefab;

    [SerializeField]
    Transform gridStartPoint;

    [SerializeField]
    CanvasGroup storytelling;


    public Transform upBound, downBound, leftBound, rightBound;

    public List<Transform> creatures;

    public bool levelStarted;

    float offsetX;
    float offsetY;
    string[][] loadLevel(string levelNum)
    {
        string text = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/Levels/" + levelNum + ".xml");
        string[] lines = Regex.Split(text, "\r\n");
        int rows = lines.Length;

        string[][] levelBase = new string[rows][];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] stringsOfLine = Regex.Split(lines[i], " ");
            levelBase[i] = stringsOfLine;
        }
        return levelBase;
    }

    void Start()
    {
        offsetX = creaturePrefab.GetComponent<SpriteRenderer>().bounds.size.x + 0.1f;
        offsetY = creaturePrefab.GetComponent<SpriteRenderer>().bounds.size.y + 0.1f;

        StartCoroutine(StoryTelling());

        string[][] jagged = loadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        
        for (int y = 0; y < jagged.Length; y++)
        {
            for (int x = 0; x < jagged[0].Length; x++)
            {
                if (jagged[y][x] == null)
                {
                    return;
                }
                switch (jagged[y][x])
                {
                    case "0":
                        break;
                    case "1":
                        creatures.Add(Instantiate(creaturePrefab, new Vector3(gridStartPoint.position.x + (offsetX * x), gridStartPoint.position.y - (offsetY * y), 0), Quaternion.identity));
                        break;
                    case "2":
                        creatures.Add(Instantiate(roboPrefab, new Vector3(gridStartPoint.position.x + (offsetX * x), gridStartPoint.position.y - (offsetY * y), 0), Quaternion.identity));
                        break;
                    case "3":
                        creatures.Add(Instantiate(crabPrefab, new Vector3(gridStartPoint.position.x + (offsetX * x), gridStartPoint.position.y - (offsetY * y), 0), Quaternion.identity));
                        break;
                    case "4":
                        creatures.Add(Instantiate(questionPrefab, new Vector3(gridStartPoint.position.x + (offsetX * x), gridStartPoint.position.y - (offsetY * y), 0), Quaternion.identity));
                        break;

                }
            }
        }
    }

    IEnumerator StoryTelling()
    {
        yield return new WaitForSeconds(3f);
        for (float t = 0f; t < 4f; t += Time.deltaTime)
        {
            float normalizedTime = t / 4f;
    
            storytelling.alpha = Mathf.Lerp(1, 0, normalizedTime);            
            yield return null;
        }
        storytelling.gameObject.SetActive(false);
        levelStarted = true;
    }
}
