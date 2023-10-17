using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Serialization_c<Character>{
    public Serialization_c(List<Character> _characters) => characters = _characters;
    public List<Character> characters;
}
[System.Serializable]
public class Character{
    public Character(string _Id,string _Name,string _Description,string _Rank,string _Number,bool _IsEquipped){
        Id = _Id;
        Name = _Name;
        Description = _Description;
        Rank = _Rank;
        Number = _Number;
        IsEquipped = _IsEquipped;
    }
    public string Id,Name,Description,Rank,Number;
    public bool IsEquipped;
}


public class CharacterDataControl : MonoBehaviour
{
    public TextAsset CharacterData;
    public List<Character> AllCharacterList,MyCharacterList;
    public GameObject[] CharacterView,Selected;
    public Image[] CharacterImages;
    public Sprite[] CharacterSprites;
    public GameObject DescriptionPanel;
    public InputField CharacterNameInput;
    string filePath;
    private DatabaseReference reference;
    private FirebaseAuth auth = null;
    private FirebaseUser user = null;
    public void Awake(){
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser; 
    }
    public void Start(){
        string[] CharacterDataLines = CharacterData.text.Substring(0, CharacterData.text.Length - 1).Split('\n');
        for(int i = 0; i < CharacterDataLines.Length; i++){
            string[] row = CharacterDataLines[i].Split('\t');
            AllCharacterList.Add(new Character(row[0], row[1], row[2], row[3], row[4], bool.Parse(row[5])));
        }
        filePath = Application.persistentDataPath + "/CharacterData.txt";
        Load();
    }
    public void Update(){
        //만약 MyCharacterList에 아무것도 없다면
        if(MyCharacterList.Count == 0){
            MyCharacterList = new List<Character>();
            MyCharacterList.Add(AllCharacterList[0]);
            MyCharacterList.Add(AllCharacterList[1]);
            save();
            Load();
        }
        Character EquippedCharacter = MyCharacterList.Find(x => x.IsEquipped == true);
        if(EquippedCharacter == null){
            MyCharacterList[0].IsEquipped = true;
        }
    }
    public void ViewCharacters(){
        for(int i = 0; i < CharacterView.Length; i++){
            CharacterView[i].SetActive(false);
            
        }
        for(int i = 0; i < MyCharacterList.Count; i++){
            CharacterView[i].SetActive(true);
            CharacterView[i].GetComponentInChildren<Text>().text = MyCharacterList[i].Name;
            // CharacterImages[i].sprite = CharacterSprites[int.Parse(MyCharacterList[i].Id)];
        }
    }
    public void CharacterSelect(int slotNum){
        Character CurrentCharacter = MyCharacterList[slotNum];
        Character EquippedCharacter = MyCharacterList.Find(x => x.IsEquipped == true);
        if(EquippedCharacter == null){
            MyCharacterList[0].IsEquipped = true;
            Selected[0].SetActive(true);
        }
        else if (EquippedCharacter != null){
            EquippedCharacter.IsEquipped = false;
            CurrentCharacter.IsEquipped = true;
            Selected[MyCharacterList.IndexOf(EquippedCharacter)].SetActive(false);
            Selected[MyCharacterList.IndexOf(CurrentCharacter)].SetActive(true);
        }
        SceneChanger.characterindex = slotNum;
        DescriptionPanel.GetComponentInChildren<Text>().text = CurrentCharacter.Name;
        DescriptionPanel.transform.GetChild(1).GetComponent<Text>().text = CurrentCharacter.Description;
        DescriptionPanel.transform.GetChild(2).GetComponent<Image>().sprite = CharacterSprites[int.Parse(CurrentCharacter.Id)-1];
    }
    public void save(){
        string json = JsonUtility.ToJson(new Serialization_c<Character>(MyCharacterList));
        File.WriteAllText(filePath, json);
        reference.Child("users").Child(user.UserId).Child("CharacterData").SetRawJsonValueAsync(json).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("취소됨");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError(task.Exception);
                return;
            }
            Debug.Log("성공");
        });
    }

    public void Load(){
        if(auth.CurrentUser != null){
            reference.Child("users").Child(user.UserId).Child("CharacterData").GetValueAsync().ContinueWith(task => {
                if(task.IsFaulted){
                    Debug.Log("Error");
                }
                else if(task.IsCompleted){
                    DataSnapshot snapshot = task.Result;
                    string json = snapshot.GetRawJsonValue();
                    if(json != null){
                        MyCharacterList = JsonUtility.FromJson<Serialization_c<Character>>(json).characters;
                    }else{
                        MyCharacterList = new List<Character>();
                        MyCharacterList.Add(AllCharacterList[0]);
                        MyCharacterList.Add(AllCharacterList[1]);
                    }
                }
            });
        }
        else{
            if(File.Exists(filePath)){
                string json = File.ReadAllText(filePath);
                MyCharacterList = JsonUtility.FromJson<Serialization_c<Character>>(json).characters;
            }
            else{
                MyCharacterList = new List<Character>();
                MyCharacterList.Add(AllCharacterList[0]);
                MyCharacterList.Add(AllCharacterList[1]);
            }
        }
    }

    public void RankUpgrade(int i){
        int currentNumber = int.Parse(MyCharacterList[i].Number);
        int currentRank = int.Parse(MyCharacterList[i].Rank);
        MyCharacterList[i].Rank = (currentRank + 1).ToString();
        MyCharacterList[i].Number = (currentNumber - 1).ToString();
        save();
    }

    public void NumberDataUpdate(int i){
        int currentNumber = int.Parse(MyCharacterList[i].Number);
        MyCharacterList[i].Number = (currentNumber + 1).ToString();
        save();
    }

}

