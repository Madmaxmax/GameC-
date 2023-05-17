using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    [SerializeField] Transform exit;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float navigation;
    [SerializeField] int health;
    [SerializeField] bool isDead;
    [SerializeField] Collider2D enemyCollaider;
    [SerializeField] int remardAmount;

    int target = 0;
    Transform enemy;
    float navigationTime = 0;

    public bool IsDead
    {
        get{return isDead;}
    }
    
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollaider = GetComponent<Collider2D>();
        Manager.instance.RegisterEnemy(this);
    }

    void Update()
    {
        if(waypoints != null && isDead == false) 
        {
            navigationTime += Time.deltaTime;
            if(navigationTime > navigation) 
            {
                if(target < waypoints.Length) 
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
                }
                else 
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision) 
    {
        Debug.Log("OnTriggerEnter2D called");
        
        if (collision.tag == "Waypoint")
        {
            target++;
        }
        else if(collision.tag == "Finish")
        {
            Manager.instance.UnregisterEnemy(this);
            Manager.instance.livesLeft--;
            Manager.instance.roundEscaped++;
            Manager.instance.IsWaveOver();
        }
        else if(collision.tag == "ProjectTile")
        {
            var newP = collision.gameObject.GetComponent<ProjectTile>();
            TakeDamage(newP.AttackDamage);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int hitPoints)
    {
        if(health - hitPoints > 0)
        {
            health -= hitPoints;
        }
        else
        {
            Die();
        }
    }
    public void Die() 
    {
        isDead = true;
        enemyCollaider.enabled = false;
        Manager.instance.Killed++;
        Manager.instance.AddMoney(remardAmount);
        Manager.instance.IsWaveOver();
        gameObject.SetActive(false);
    }
}