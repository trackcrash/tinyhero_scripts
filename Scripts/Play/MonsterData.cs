using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private string MonsterName;
    public string monsterName
    {
        get { return MonsterName; }
    }
    [SerializeField]
    private float Health;
    public float health
    {
        get { return Health; }
    }
    [SerializeField]
    private float Damage;
    public float damage
    {
        get { return Damage; }
    }
    [SerializeField]
    private float Defense;
    public float defense
    {
        get { return Defense; }
    }
    [SerializeField]
    //img
    private Sprite MonsterImage;
    public Sprite monsterImage
    {
        get { return MonsterImage; }
    }
}