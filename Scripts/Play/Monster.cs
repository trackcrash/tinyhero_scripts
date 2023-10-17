using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData monsterdata;
    public float health;
    public float fullhealth;
    public float damage;
    public float defense;
    public string monsterName;
    public bool isDead = false;
    public Sprite monsterImage;
    Image img;

    void Start(){
        fullhealth = monsterdata.health;
        health = monsterdata.health;
        damage = monsterdata.damage;
        defense = monsterdata.defense;
        monsterName = monsterdata.monsterName;
        monsterImage = monsterdata.monsterImage;
        img = GetComponent<Image>();
    }

    void Update(){
        img.sprite = monsterImage;

    }


}
