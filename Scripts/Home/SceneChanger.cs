using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataSaver;

public class SceneChanger : MonoBehaviour
{
    public static int characterindex;
    public static string itemindex;
    public static int itemrank;
    public static int characterrank;
    //확인 패널
    public GameObject[] confirmPanel;
    //선택하세요 패널
    public GameObject selectPanel;
    //캐릭터 뷰
    public GameObject charaview;
    //선택된 캐릭터 ex."wizard"
    public string selectcharacter;
    //json에서 불러올 캐릭터
    public string curcharcterindex;
    //아이템
    public int currentItem;
    public Dictionary<int, string> CharacterKey = new Dictionary<int, string>();
    public Dictionary<string, int> ItemChange = new Dictionary<string, int>();
    public string filePath;
    public void Awake(){
        //character
        CharacterKey.Add(0, "wizard");
        CharacterKey.Add(1, "fighter");
        CharacterKey.Add(2, "warrior");
        CharacterKey.Add(3, "knight");
        //item
        ItemChange.Add("1", 1);
        ItemChange.Add("2", 2);
        ItemChange.Add("3", 3);
        ItemChange.Add("4", 4);
        //filePath
        filePath = Application.persistentDataPath + "/savefile.json";
        
    }
    public void ChangeScene(int num)
    {
        var number = num;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            curcharcterindex = data.currentCharacter;
            selectcharacter = CharacterKey[characterindex];
            currentItem = data.currentItem;
            Debug.Log("filepath if문 들어옴" + selectcharacter + "" + currentItem);
            if(curcharcterindex == selectcharacter && ItemChange[itemindex] ==  currentItem){
                NewStart();
                Debug.Log("if문 들어옴");
            }
            else{
                ConfirmScreen(number);
            }
        }
        else{
            NewStart();
        }
    
    }
    public void ConfirmScreen(int num){
        confirmPanel[num].SetActive(true);
    }

    public void NewStart(){
        if(itemindex != null){
        GameManager.currentItem = ItemChange[itemindex];
        GameManager.currentCharacter = CharacterKey[characterindex];
        GameManager.itemrank = itemrank;
        GameManager.characterrank = characterrank;
        LoadingManager.Instance.LoadScene("main");
        }
        else{
            selectPanel.SetActive(true);
        }
    }

    public void DeleteStart(){
        File.Delete(filePath);
        NewStart();
    }

    public void CountinueStart(){
        GameManager.currentItem = currentItem;
        GameManager.currentCharacter = curcharcterindex;
        LoadingManager.Instance.LoadScene("main");
    }
}
