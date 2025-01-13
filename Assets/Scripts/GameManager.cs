using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject coin;
    [SerializeField] TextMeshProUGUI pauseScoreText;
    [SerializeField] TextMeshProUGUI lastText;
    [SerializeField] TextMeshProUGUI maxText;
    [SerializeField] GameObject pauseUI;

    public bool isPlaying = false;

    int maxCoin = 0;
    int score = 0;
    //int nowCoinCount = 0;

    Vector2 range = new Vector2(-10.0f, 10.0f); // Vector2, 3 같이 사용하는 구문
    public Vector2 yRange = new Vector2(0.5f, 1.0f);

    void Awake()
    {
        Instantiate(player, spawnPoint.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxText.text = PlayerPrefs.GetInt("maxScore").ToString("000");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
            lastText.text = score.ToString("000");
            pauseUI.SetActive(true);
        }
    }

    public void SpawnCoin()
    {
        
        Vector3 randomPosition = RandomPosition();
        Instantiate(coin, randomPosition, Quaternion.identity);
        score++;
        Debug.Log("Spawned Objects Count: " + score);
        pauseScoreText.text = score.ToString("000");

    }

    Vector3 RandomPosition()
    {
        float x = Random.Range(range.x, range.y);
        float y = Random.Range(yRange.x, yRange.y);
        float z = Random.Range(range.x, range.y);

        return new Vector3(x, y, z);
    }

    void OnApplicationQuit()
    {
        if (score > maxCoin)
        { 
            PlayerPrefs.SetInt("maxScore", score);
        }
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public void OnClickRePlayButton()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
