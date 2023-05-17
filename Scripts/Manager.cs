using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum gameStatus
{
    play, win, gameover, nextLevel
}

public class Manager : Loader<Manager>
{
    [SerializeField] private int totalWave = 10;
    [SerializeField] private TMP_Text Money;
    [SerializeField] private TMP_Text currentWave;
    [SerializeField] private TMP_Text livesLeftText;
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int totalEnemies = 10;
    [SerializeField] private int enemiesPerSpawn;


    int waveNumber = 0;
    int totalMoney = 140;
    public int livesLeft = 10;
    int killed = 0;
    public int roundEscaped = 0;
    gameStatus currentStatus = gameStatus.play;
    private const float spawnDelay = 0.5f;
    public List<Enemy> EnemyList = new List<Enemy>();

    public int TotalMoney
    {
        get{return totalMoney;}
        set
        {
            totalMoney = value;
            Money.text = TotalMoney.ToString();
        }
    }

    public int LivesLeft
    {
        get{return livesLeft;}
        set{livesLeft = value;}
    }

    public int Killed
    {
        get{return killed;}
        set{killed = value;}
    }

    public int RoundEscaped
    {
        get{return roundEscaped;}
        set{roundEscaped = value;}
    }

    void Start()
    {
        playButton.gameObject.SetActive (false);
        ShowMenu();
    }
    void Update()
    {
        HandleEscape();
    }
    
    IEnumerator Spawn()
    {
        if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) 
        {
            for(var i = 0; i < enemiesPerSpawn; i++) 
            {
                if(EnemyList.Count < totalEnemies)
                {
                    var newEnemy = Instantiate(enemies[0]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy) 
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) 
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemy()
    {
        foreach(var enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount) 
    {
        TotalMoney += amount;
    }

    public void RemoveMoney(int amount) 
    {
        TotalMoney -= amount;
    }


    public void IsWaveOver()
    {
        livesLeftText.text = livesLeft.ToString();
        if((roundEscaped + killed) == totalEnemies)
        {
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if(livesLeft <= 0)
        {
            currentStatus = gameStatus.gameover;
        }
        else if(waveNumber == 0 && (RoundEscaped + killed) == 0)
        {
            currentStatus = gameStatus.play;
        }
        else if(waveNumber >= totalWave)
        {
            currentStatus = gameStatus.win;
        }
        else
        {
            currentStatus = gameStatus.nextLevel;
        }
    }
    public void PlayButtonPressed()
    {
        switch(currentStatus)
        {
            case gameStatus.nextLevel:
                waveNumber++;
                totalEnemies += 10;
                break;
            default:
                totalEnemies = 5;
                livesLeft = 10;
                totalMoney = 140;
                Money.text = totalMoney.ToString();
                livesLeftText.text = livesLeft.ToString();
                break;
                }
        DestroyEnemy();
        killed = 0;
        currentWave.text = (waveNumber + 1).ToString();
        StartCoroutine(Spawn());
        playButton.gameObject.SetActive(false);

    }

    public void ShowMenu() 
    {
        switch (currentStatus)
        {
            case gameStatus.gameover:
                playButtonText.text = "Играть заново";
                break;

            case gameStatus.nextLevel:
                playButtonText.text = "Следующая волна";
                break;

            case gameStatus.play:
                playButtonText.text = "Начать играть";
                break;

            case gameStatus.win:
                playButtonText.text = "Начать играть";
                break;
        }
        playButton.gameObject.SetActive(true);
    }

    private void HandleEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.instance.EndDrag();
            TowerManager.instance.towerButtonPrest = null;
        }
    }
}
