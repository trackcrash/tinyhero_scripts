using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gatcha : MonoBehaviour
{
    public CharacterDataControl characterdatacontrol;
    public DataController itemdatacontrol;
    public UserManagement usermanagement;
    public GameObject GatchaPanel;
    public GameObject RequireMorePanel;
    public Text statustext;
    public GameObject[] CharacterIcon;
    public GameObject[] ItemIcon;

    public void ClearPanel()
    {
        //패널 초기화
        foreach (Transform child in GatchaPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void GatchaButton()
    {
        if(usermanagement.gold < 100){
            RequireMorePanel.SetActive(true);
        }else{
        usermanagement.GatchaGold();
        int random = Random.Range(0, 2);
        Debug.Log("캐릭터를 뽑았습니다." + random + characterdatacontrol.AllCharacterList[random]);
        if(characterdatacontrol.MyCharacterList[random] == null){
            characterdatacontrol.MyCharacterList.Add(characterdatacontrol.AllCharacterList[random]);
            Instantiate(CharacterIcon[random], GatchaPanel.transform);
        }
        else{
            characterdatacontrol.NumberDataUpdate(random);
            Instantiate(CharacterIcon[random], GatchaPanel.transform);
        }
        }
    }

    public void GatchaTenTimes(){
        if(usermanagement.gold < 1000){
            RequireMorePanel.SetActive(true);
        }else{
        for(int i=0; i<10; i++){
            GatchaButton();  
        } 
        }
    }

    public void ItemGatchaButton(){
        usermanagement.GatchaGold();
        if(usermanagement.gold < 100){
            RequireMorePanel.SetActive(true);
        }else{
        int random = Random.Range(0, itemdatacontrol.AllItemList.Count);
        Debug.Log("아이템을 뽑았습니다." + random + itemdatacontrol.AllItemList[random]);
        if(itemdatacontrol.MyItemList[random] == null){
            itemdatacontrol.MyItemList.Add(itemdatacontrol.AllItemList[random]);
            Instantiate(ItemIcon[random], GatchaPanel.transform);
        }
        else{
            itemdatacontrol.NumberDataUpdate(random);
            Instantiate(ItemIcon[random], GatchaPanel.transform);
        }
        }
    }

    public void ItemGatchaTenTimes(){
        if(usermanagement.gold < 1000){
            RequireMorePanel.SetActive(true);
        }else{
        for(int i=0; i<10; i++){
            ItemGatchaButton();  
        }
        }
    }

    public void GatchaTextChange(int i){
        if(i == 0){
            statustext.text = "캐릭터 뽑기";
        }
        else if(i == 1){
            statustext.text = "아이템 뽑기";
        }
    }
}
