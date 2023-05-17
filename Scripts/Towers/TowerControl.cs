using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField] float timeBetweenAttack;
    [SerializeField] float attackRadius;
    [SerializeField] ProjectTile projectTile;
    Enemy targetEnemy = null;
    float attackCount;
    bool isAttacking = false;

    void Update()
    {
        attackCount -= Time.deltaTime;
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            targetEnemy = RetrieveNearestEnemyInRange();
        }
        else
        {
            if (attackCount <= 0)
            {
                isAttacking = true;
                attackCount = timeBetweenAttack;
            }
            else
            {
                isAttacking = false;
            }

            if (targetEnemy != null && Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }

        if (isAttacking)
        {
            Attack();
        }
    }

    private Enemy RetrieveNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        var smallestDistance = float.PositiveInfinity;

        foreach (var enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public void Attack()
    {
        isAttacking = false;
        if (targetEnemy == null)
        {
            return;
        }

        var newProjectTile = Instantiate(projectTile) as ProjectTile;
        newProjectTile.transform.position = transform.position + transform.up;
        StartCoroutine(MoveProjectTile(newProjectTile));
    }

    IEnumerator MoveProjectTile(ProjectTile projectTile)
    {
        while (targetEnemy != null && projectTile != null && GetTargetDistance(projectTile) > 0.20f)
        {
            projectTile.transform.position = Vector2.MoveTowards(projectTile.transform.position, targetEnemy.transform.position, 5f * Time.deltaTime);
            yield return null;
       
        }
        if (projectTile != null)
        {
            Destroy(projectTile.gameObject);
            if (targetEnemy != null)
            {
                targetEnemy.TakeDamage(projectTile.AttackDamage);
            }
        }
    }

    private float GetTargetDistance(ProjectTile projectTile)
    {
        if (targetEnemy == null)
        {
            targetEnemy = RetrieveNearestEnemyInRange();
            if (targetEnemy == null)
            {
                return 0f;
            }
        }
        return Vector2.Distance(projectTile.transform.position, targetEnemy.transform.position);
    }

    private List<Enemy> GetEnemiesInRange()
    {
        var enemiesInRange = new List<Enemy>();
        foreach (var enemy in Manager.instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

}