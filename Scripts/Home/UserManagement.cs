using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserManagement : MonoBehaviour
{
    //firebase auth init
    private FirebaseAuth auth = null;
    private FirebaseUser user = null;
    //firebase database init
    private DatabaseReference reference;
    //itemdata
    public DataController dataController;
    //unity
    public Text Lvtext;
    public Text Goldtext;
    public Text Diamondtext;
    public Slider steminaSlider;

    public int gold, diamond, stemina, level;
    public void deleteuser()
    {
        if(auth != null)
        {
            auth.CurrentUser.DeleteAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User deleted successfully.");
            });
        }
    }
    bool StartItem = false;

    void Awake()
    {   
        //튜토리얼이 끝났는지 확인
        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }

        // else if(PlayerPrefs.GetInt("Tutorial2") == 0){
        //     //Home scene에서 튜토리얼진행
        // }
        //초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //만약 데이터베이스에 Userinfo가 없다면 생성
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Userinfo").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value == null)
                {
                    reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("gold").SetValueAsync(1000);
                    reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("diamond").SetValueAsync(100);
                    reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("stemina").SetValueAsync(100);
                    reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("level").SetValueAsync(1);
                }
            }
        });
        loadUserData();
        info = true;
    }

    void loadUserData(){
        reference.Child("Userinfo").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("loadUserData 에러");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null)
                {
                    gold = int.Parse(snapshot.Child("gold").Value.ToString());
                    diamond = int.Parse(snapshot.Child("diamond").Value.ToString());
                    stemina = int.Parse(snapshot.Child("stemina").Value.ToString());
                    level = int.Parse(snapshot.Child("level").Value.ToString());
                }
            }
        });
    }
    //stemina가 100미만일때 5분당 1씩 증가
    // void steminaControll(){
    //     if(stemina < 100){
    //        stemina++;
    //        reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("stemina").SetValueAsync(stemina);   
    //     }
    // }
    // Update is called once per frame
    public bool info = true;
    public void StateChanger(int i){
        if(i == 0){
            info = false;
        }
        else if(i == 1){
            info = true;
        }
    }
    void Update()
    {
        if(info == true){
        Lvtext = GameObject.Find("Lv").GetComponent<Text>();
        Goldtext = GameObject.Find("Gold").GetComponent<Text>();
        Diamondtext = GameObject.Find("Diamond").GetComponent<Text>();
        steminaSlider = GameObject.Find("Slider").GetComponent<Slider>();
        }
        Lvtext.text = level.ToString();
        Goldtext.text = gold.ToString();
        Diamondtext.text = diamond.ToString();
        steminaSlider.value = stemina;
        if (level == 0){
            loadUserData();
        }
    }
    public void GatchaGold(){
        gold -= 100;
        reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("gold").SetValueAsync(gold);
    }

    public void TestverGetGold(){
        gold += 10000;
        reference.Child("Userinfo").Child(auth.CurrentUser.UserId).Child("gold").SetValueAsync(gold);
    }
}
