using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;
using System.IO;

public class TestMessage_Selection : MonoBehaviour
{
    public DialogManager DialogManager;
    public GameManager GameManager;
    public Player player;
    //event1 check
    public string currentCharacter;



    //이벤트 선택
    public void event_updater(int num){
        if(num == 0){
            event1_dialog();
        }
        else if(num == 1){
            event2_dialog();
        }
        else if(num == 2){
            event3_dialog();
        }
    }
    //씬전환
    public void callchange(){
        GameManager.ChangeMainCanvas();
    }
    //마법사 1번
    public void event1_dialog(){
        var dialogTexts = new List<DialogData>();
        var event1 = new DialogData("안녕 나는 마법사야 이런 곳에서 만나다니 우연이네.", "Wizard");
        event1.SelectList.Add("Correct", "만나서 영광입니다 마법사님");
        event1.SelectList.Add("Wrong", "저도 마법사입니다");

        event1.Callback = () => Check_Correct();
        dialogTexts.Add(event1);
        DialogManager.Show(dialogTexts);
    }
    //마법사 2번
    public void event2_dialog(){
        var dialogTexts = new List<DialogData>();
        var event1 = new DialogData("또 만났네 반가워 여행자", "Wizard");
        event1.SelectList.Add("Correct", "저번에 봐주셨던 마법을..");
        event1.SelectList.Add("Wrong", "이쯤되면 운명이 아닐까요");

        event1.Callback = () => Check_Correct2();
        dialogTexts.Add(event1);
        DialogManager.Show(dialogTexts);
    }
    //마법사 3번
    public void event3_dialog(){
        var dialogTexts = new List<DialogData>();
        var event1 = new DialogData("또만났네요 이번에는 저를 좀 도와주시겠어요?", "Wizard");
        event1.SelectList.Add("Correct", "네 물론이죠");
        event1.SelectList.Add("Wrong", "아니요 저는 볼일이 있어서");

        event1.Callback = () => Check_Correct3();
        dialogTexts.Add(event1);
        DialogManager.Show(dialogTexts);
    }

    //선택지 체크에리어
    //1번 이벤트
    public void Check_Correct(){
        if(DialogManager.Result == "Correct"){
            var event1Texts = new List<DialogData>();
            event1Texts.Add(new DialogData("처음 보는 아이인데 예의가 바르구나 상을줄게", "Wizard"));
            event1Texts.Add(new DialogData("상을 받았다. 스테미너가 20증가했다 신앙심이 10증가했다"));
            event1Texts.Add(new DialogData(" "));
            event1Texts.Add(new DialogData("."));
            GameManager.stemina += 20;
            GameManager.piety += 10;
            DialogManager.Show(event1Texts);
            for(int i=0; i<event1Texts.Count; i++){
                event1Texts[i].Callback = () => callchange();
            }
        }
        else{
            var event1Texts = new List<DialogData>();
            event1Texts.Add(new DialogData("어머 그렇니 반가우니 마법을 가르쳐줄게", "Wizard"));
            event1Texts.Add(new DialogData("마법을 지도받았다. 마법 + 10 증가, 신앙심 + 20 증가"));
            event1Texts.Add(new DialogData(" "));

            DialogManager.Show(event1Texts);
            GameManager.ability += 10;
            GameManager.piety += 20;
            for(int i=0; i<event1Texts.Count; i++){
                event1Texts[i].Callback = () => callchange();
            }
        }
    }
    //2번 이벤트
    public void Check_Correct2(){
        if(DialogManager.Result == "Correct"){
            var event1Texts = new List<DialogData>();
            event1Texts.Add(new DialogData("어느 부분이 어려운거니 이정도도 못해?", "Wizard"));
            event1Texts.Add(new DialogData("마법을 교정받았다. 마법 + 20 증가"));
            event1Texts.Add(new DialogData(" "));
            GameManager.ability += 20;
            DialogManager.Show(event1Texts);
            for(int i=0; i<event1Texts.Count; i++){
                event1Texts[i].Callback = () => callchange();
            }
        }
        else{
            var event1Texts = new List<DialogData>();
            event1Texts.Add(new DialogData("후후 지나가다 만난 인연이 몇인데 아직 그정돈 아니란다 꼬마야", "Wizard"));
            event1Texts.Add(new DialogData("기분이 안좋아보인다. 스테미나 15 감소, 신앙심 10 감소"));
            event1Texts.Add(new DialogData(" "));
            DialogManager.Show(event1Texts);
            GameManager.stemina -= 15;
            GameManager.piety -= 10;
            for(int i=0; i<event1Texts.Count; i++){
                event1Texts[i].Callback = () => callchange();
            }
        }
    }
    //3번 이벤트
    public void Check_Correct3(){
        if(DialogManager.Result == "Correct"){
            var event1Texts = new List<DialogData>();
            event1Texts.Add(new DialogData("그럼 이 물약을 먹어봐요", "Wizard"));
            event1Texts.Add(new DialogData("기분은 좋지않은데 몸은 좋아진것 같다 스테미나 15감소 방어력 10증가"));
            event1Texts.Add(new DialogData(" "));
            GameManager.defense += 10;
            GameManager.stemina -= 15;
            DialogManager.Show(event1Texts);
            for(int i=0; i<event1Texts.Count; i++){
                event1Texts[i].Callback = () => callchange();
            }
        }
        else{
            var event1Texts = new List<DialogData>();
            event1Texts.Add(new DialogData("무서워 하지마 실험은 잠깐이야", "Wizard"));
            event1Texts.Add(new DialogData("도망쳤다. 최대스테미나 20증가"));
            event1Texts.Add(new DialogData(" "));
            DialogManager.Show(event1Texts);
            GameManager.maxStemina += 20;
            for(int i=0; i<event1Texts.Count; i++){
                event1Texts[i].Callback = () => callchange();
            }
        }
    }
}
