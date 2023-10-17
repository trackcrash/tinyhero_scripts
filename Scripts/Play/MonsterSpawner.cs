using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawner : MonoBehaviour
{   
    //몬스터 데이터
    public MonsterData monsterdata;
    //몬스터 데이터 리스트
    public List<MonsterData> monsterdatalist = new List<MonsterData>();
    public GameObject monster;

    
    public Canvas canvas;
    public void SpawnMonster(int num){
        monstergenerator(num);
        
    }
    //몬스터 인수 따라서 생성 ex int monsterindex
    public Monster monstergenerator(int num){
        int i = num;
        var newmonster = Instantiate(monster).GetComponent<Monster>();
        newmonster.monsterdata = monsterdatalist[i];
        newmonster.transform.SetParent(canvas.transform);
        return newmonster;
    }
    

    void Update(){
        
    }
}
