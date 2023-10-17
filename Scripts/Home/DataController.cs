using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Serialization<Item>
{
    public Serialization(List<Item> _items) => items = _items;
    public List<Item> items;
}

[System.Serializable]
public class Item{
        public Item(string _Id, string _Name, string _Description, string _Rank, string _Number, bool _IsEquipped, bool _IsActive)
        {
            Id = _Id;
            Name = _Name;
            Description = _Description;
            Rank = _Rank;
            Number = _Number;
            IsEquipped = _IsEquipped;
            IsActive = _IsActive;
        }   
        public string Id,Name,Description,Rank,Number;
        public bool IsEquipped;
        public bool IsActive; 
}

public class DataController : MonoBehaviour
{
    
    public TextAsset ItemData;
    public List<Item> AllItemList,MyItemList;
    public GameObject[] ItemView,Selected;
    public Image[] ItemImages;
    public Sprite[] ItemSprites;
    public GameObject DescriptionPanel;
    public GameObject UpgradeConfirmPanel;
    public GameObject RequireMorePanel;
    public GameObject MaxUpgradePanel;
    string filePath;
    private DatabaseReference reference;
    private FirebaseAuth auth = null;
    private FirebaseUser user = null;
    public void Awake(){
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        
    }

    void Start()
    {
       
    
        string[] ItemDataLines = ItemData.text.Substring(0, ItemData.text.Length - 1).Split('\n');
        for(int i = 0; i < ItemDataLines.Length; i++){
            string[] row = ItemDataLines[i].Split('\t');
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], bool.Parse(row[5]), bool.Parse(row[6])));
        }
        filePath = Application.persistentDataPath + "/ItemData.txt";
         if(PlayerPrefs.GetInt("First") == 0){
            NewSave();
        }else{
            Load();
        }
    }
    public void ViewItems(){
        for(int i = 0; i < ItemView.Length; i++){
            ItemView[i].SetActive(false);
        }
        for(int i = 0; i < MyItemList.Count; i++){
            ItemView[i].SetActive(true);
            ItemView[i].GetComponentInChildren<Text>().text = MyItemList[i].Name;
            //ItemImages[i].sprite = ItemSprites[int.Parse(MyItemList[i].Id)];
        }
    }

    void Update(){
        if(auth == null){
            Load();
        }
    }

    public void ItemSelect(int slotNum)
    {
        Item CurrentItem = MyItemList[slotNum];
        Item EquippedItem = MyItemList.Find(x => x.IsEquipped == true);
        if (EquippedItem == null){
            MyItemList[0].IsEquipped = true;
            //씬체인저에 보냄
            SceneChanger.itemindex = CurrentItem.Id;
            SceneChanger.itemrank = int.Parse(CurrentItem.Rank);
        }
        else if (EquippedItem != null)
        {
            EquippedItem.IsEquipped = false;
            CurrentItem.IsEquipped = true;
            Selected[MyItemList.IndexOf(EquippedItem)].SetActive(false);
            Selected[MyItemList.IndexOf(CurrentItem)].SetActive(true);
            //씬체인저에 보냄
            SceneChanger.itemindex = CurrentItem.Id;
            SceneChanger.itemrank = int.Parse(CurrentItem.Rank);
        }
        //currentItem에 Description +뒤에 문자열 랭크에따라 다르게 변경하기
        int stridx = CurrentItem.Description.IndexOf("+");
        string str = CurrentItem.Description.Substring(stridx+1);
        int sum = int.Parse(str) + int.Parse(CurrentItem.Rank);
        int sum2 = int.Parse(str) + (int.Parse(CurrentItem.Rank) + 1);
        string str2 = CurrentItem.Description.Substring(0,stridx+1);
        string str3 = str2 + sum.ToString();
        if(slotNum == 1 || slotNum == 3){
            sum = int.Parse(str) + int.Parse(CurrentItem.Rank) * 5;
            sum2 = int.Parse(str) + (int.Parse(CurrentItem.Rank)+1) * 5;
            str3 = str2 + sum.ToString();
        }
        //DescriptionPanel 텍스트 넣기
        DescriptionPanel.GetComponentInChildren<Text>().text = CurrentItem.Name;
        DescriptionPanel.transform.GetChild(1).GetComponent<Text>().text = str3;
        var itemvalue = int.Parse(CurrentItem.Id)-1;
        DescriptionPanel.transform.GetChild(2).GetComponent<Image>().sprite = ItemSprites[itemvalue];
        DescriptionPanel.transform.GetChild(3).GetComponent<Text>().text = "+"+CurrentItem.Rank;
        int stabilizer = int.Parse(CurrentItem.Number)-1;
        DescriptionPanel.transform.GetChild(4).GetComponent<Text>().text = "보유 수량: "+stabilizer.ToString();
        //UpgradeConfirmPanel 텍스트 넣기
        UpgradeConfirmPanel.GetComponentInChildren<Text>().text = "강화에 필요한 수량: "+CurrentItem.Rank;
        UpgradeConfirmPanel.transform.GetChild(1).GetComponent<Text>().text = "강화 전 능력치: "+sum.ToString();
        UpgradeConfirmPanel.transform.GetChild(2).GetComponent<Text>().text = "강화 후 능력치: "+sum2+"";

    }

    public void NewSave(){
        MyItemList = new List<Item>();
        MyItemList.Add(AllItemList[0]);
    }

    void Save(){
        //json으로 저장
        string json = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
        File.WriteAllText(filePath, json);
        reference.Child("users").Child(user.UserId).Child("ItemData").SetRawJsonValueAsync(json).ContinueWith(task => {
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

    void Load(){
        //json firebase로 불러오기
        if(auth.CurrentUser != null){
            reference.Child("users").Child(user.UserId).Child("ItemData").GetValueAsync().ContinueWith(task => {
                if(task.IsFaulted){
                    Debug.Log("Error");
                }
                else if(task.IsCompleted){
                    DataSnapshot snapshot = task.Result;
                    string json = snapshot.GetRawJsonValue();
                    if(json != null)
                    {
                        MyItemList = JsonUtility.FromJson<Serialization<Item>>(json).items;
                    }
                    else{
                        Load();
                    }
                }
            });
        }
        else{
            //json 파일로 불러오기
            if(File.Exists(filePath)){
                string json = File.ReadAllText(filePath);
                MyItemList = JsonUtility.FromJson<Serialization<Item>>(json).items;
            }
            else{
                MyItemList = new List<Item>();
                MyItemList.Add(AllItemList[0]);
                MyItemList.Add(AllItemList[1]);
                MyItemList.Add(AllItemList[2]);
                MyItemList.Add(AllItemList[3]);
            }
        }
        
        
    }

    public void RankUpgrade(){
        int i = MyItemList.FindIndex(x => x.IsEquipped == true);
        int currentNumber = int.Parse(MyItemList[i].Number);
        int currentRank = int.Parse(MyItemList[i].Rank);
        int requireNumber = currentNumber - currentRank;
        if(requireNumber > 1&& currentRank < 6 && currentNumber -1 != 0){
            MyItemList[i].Number = requireNumber.ToString();
            MyItemList[i].Rank = (currentRank + 1).ToString();
            Invoke("UpgradeConfirmPanelSetActive", .5f);
            ItemSelect(i);
        }
        else if (currentRank >= 5){
            MaxUpgradePanel.SetActive(true);
        }else{
            RequireMorePanel.SetActive(true);
        }
        Save();
        
    }

    public void UpgradeConfirmPanelSetActive(){
        UpgradeConfirmPanel.SetActive(false);
    }

    public void NumberDataUpdate(int i){
        int currentNumber = int.Parse(MyItemList[i].Number);
        MyItemList[i].Number = (currentNumber + 1).ToString();
        Save();
    }
}
