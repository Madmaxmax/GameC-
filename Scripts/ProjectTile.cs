using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectTileType
{
    Bullet,
    BigBullet
};

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private ProjectTileType type;

    public int AttackDamage => attackDamage;
    public ProjectTileType Type => type;
}
