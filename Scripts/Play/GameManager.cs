using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //prefab
    public GameObject playerPrefab;
    public GameObject player;
    public GameObject hud;
    public Transform hudpos;
    public Text StrengthText;
    public Text DefenseText;
    public Text ToughText;
    public Text AbilityText;
    public Text PietyText;
    public Text TurnText;
    public GameObject DiePanel;
    public Text ObjectText;
    public GameObject effectspawner;
    public GameObject FireballPrefab;
    //Buttons
    public Button StrengthButton;
    public Button DefenseButton;
    public Button ToughButton;
    public Button AbilityButton;
    public Button PietyButton;
    public Button TurnButton;
    public Pause pause;
    public MonsterSpawner monsterSpawner;
    public Monster monster;
    public Slider monsterhpbar;
    //Canvas list
    public GameObject Maincanvas;
    public GameObject Dialogcanvas;
    public GameObject TestMessage_Selection;
    public GameObject hpbar;
    public GameObject endingPanel;
    public string filePath;
    // Current Turn
    public int turn = 0;
    public int eventCount = 0;
    // Remain Turn
    public int remainTurn = 0;
    public bool ending = false;
    //playerstrengthtext
    public string playerStrengthText;
    public bool isfighting = false;
    public static string currentCharacter;
    public static int currentItem;
    public static int characterrank;
    public static int itemrank;
    public void Awake(){
        filePath = Application.persistentDataPath + "/savefile.json";
        SetTurn();
        hpbar.SetActive(false);
        Time.timeScale = 1;
        playerPrefab = Resources.Load(currentCharacter) as GameObject;
        GameObject spawnplayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        spawnplayer.transform.SetParent(GameObject.Find("Canvas").transform);
        spawnplayer.transform.position = new Vector3(-66, 0, 0);
        spawnplayer.gameObject.GetComponent<Player>().enabled = true;
        DontDestroyOnLoad(spawnplayer);
        player = GameObject.FindWithTag("Player").gameObject;
        //to player
        healthbar = GameObject.Find("PlayerHealth").GetComponent<Slider>();
        playerhpbar = GameObject.Find("Hp").GetComponent<Slider>();
        hptext = GameObject.Find("HpText").GetComponent<Text>();
        failText = GameObject.Find("FailText").GetComponent<Text>();
        statusTextPrefab = GameObject.Find("StatusText").GetComponent<Text>();
        }
    public void Update(){
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerhpbar.value = stemina / maxStemina;
        hptext.text = stemina + "/" + maxStemina;
        healthbar.value = health / maxHealth;
        //만약 statusTextPrefab이 4개 이상이면 제일 오래된 statusTextPrefab을 삭제
        if (GameObject.Find("Viewport").transform.childCount > 3)
        {
            Destroy(GameObject.Find("Viewport").transform.transform.GetChild(0).gameObject);
        }
        if (stemina <= 100 / 2)
        {
            failText.text = "실패확률:" + (100 / 2 - stemina) / (100 / 2) * 100;
        }
        else
        {
            failText.text = "실패확률: 0";
        }
        if(eventCount > 4 && ending == true){
            endingPanel.SetActive(true);
        }




        //texts
        StrengthText.text = "힘: " + strength;
        DefenseText.text = "방어: " + defense;
        ToughText.text = "저항력: " + tough;
        AbilityText.text = "마법: " + ability;
        PietyText.text = "신앙: " + piety;
        TurnText.text = "목표까지 앞으로" + remainTurn +"턴";
        
        //monsterspawner에 hpbar를 넣어줘야함
        if(isfighting == true){
            ButtonControlFight();
        }
        
        

    }
    //목표텍스트
    public void SetObjectText(){
        //ObjectText.text = text;
    }
    
    //턴설정
    public void SetTurn(){
        if(eventCount == 0){
            turn = 10;
        }
        else if(eventCount == 1){
            turn = 8;
        }
        else if(eventCount == 2){
            turn = 10;
        }
        else if(eventCount == 3){
            turn = 15;
        }
        else if(eventCount == 4){
            turn = 8;
        }
        remainTurn = turn;
    }
    //남은턴 감소
    public void SetRemainTurn(){
        remainTurn -= 1;
    }
    public void startBattle(){
        monsterSpawner.SpawnMonster(eventCount);
        hpbar.SetActive(true);
        //ResetAnimation
        player.GetComponent<Player>().ResetAnimation();
        isfighting = true;
        ButtonControlFight();
        InvokeRepeating( "Player_Attack", 1f, 2f);
        InvokeRepeating( "attackMotion", 1f, 1f);
        InvokeRepeating( "Monster_attack", 3f, 2f);

    }
    //다음턴으로 넘어갈때
    public void NextTurn(){
        GameObject monster = GameObject.FindWithTag("Monster");
        SetRemainTurn();
        if(remainTurn == 5){
            if(eventCount < 3){
            ChangeDialogCanvas();
            TestMessage_Selection.GetComponent<TestMessage_Selection>().event_updater(eventCount);
            }
        }else if(remainTurn == 1){
            QuitSave();
        }
        else if (remainTurn == 0){
            startBattle();
        }
    }
    //버튼 컨트롤 2초동안 (비활성화) 
    public void ButtonControlOff(){
        StrengthButton.interactable = false;
        DefenseButton.interactable = false;
        ToughButton.interactable = false;
        AbilityButton.interactable = false;
        PietyButton.interactable = false;
        TurnButton.interactable = false;
        Invoke("ButtonControlOn", 2f);
    }
    //전투중멈추게
    public void ButtonControlFight(){
        StrengthButton.interactable = false;
        DefenseButton.interactable = false;
        ToughButton.interactable = false;
        AbilityButton.interactable = false;
        PietyButton.interactable = false;
        TurnButton.interactable = false;
    }
    //버튼 컨트롤(활성화)
    public void ButtonControlOn(){
        StrengthButton.interactable = true;
        DefenseButton.interactable = true;
        ToughButton.interactable = true;
        AbilityButton.interactable = true;
        PietyButton.interactable = true;
        TurnButton.interactable = true;
    }
    //플레이어 공격모션
    public void attackMotion(){
        player.GetComponent<Player>().Anim.SetTrigger("attack");
    }
    // 캔버스 전환 함수
    public void ChangeDialogCanvas(){
        Maincanvas.SetActive(false);
        Dialogcanvas.SetActive(true);
    }
    public void ChangeMainCanvas(){
        Maincanvas.SetActive(true);
        Dialogcanvas.SetActive(false);
    }
    //몬스터공격
    void Monster_attack(){
        GameObject monster = GameObject.FindWithTag("Monster");
        health -= monster.GetComponent<Monster>().damage / (defense/2);
        if(health <= 0){
            //죽음 애니메이션
            player.GetComponent<Player>().Anim.SetTrigger("die");
            DiePanel.SetActive(true);
            CancelInvoke("Player_Attack");
            CancelInvoke("Monster_attack");
            CancelInvoke("attackMotion");
            DeleteSave();
            //죽음 사운드
            //player.GetComponent<Player>().DieSound();
        }
        else{
            //피격 애니메이션
            player.GetComponent<Player>().Anim.SetTrigger("hurt");
            //피격 사운드
            // player.GetComponent<Player>().HitSound();
        }
    }
    //플레이어 공격
    public void Player_Attack()
    {
        //몬스터컴포넌트를 가져옴
        GameObject monster = GameObject.FindWithTag("Monster");
        //calculate the damage to monster
        if (strength > ability){
            //piety 수치에 따라서 랜덤으로 크리티컬 데미지를 줌
            int random = Random.Range(0, 100);
            if(random < piety/10){
                damage = (strength / monster.GetComponent<Monster>().defense) * 2;
            }
            else{
                damage = strength / monster.GetComponent<Monster>().defense;
            }
        }
        else if (ability > strength){
           int random = Random.Range(0, 100);
            if(random < piety/10){
                damage = (ability / monster.GetComponent<Monster>().defense) * 2;
            }
            else{
                damage = ability / monster.GetComponent<Monster>().defense;
            }
        }
        else if (strength == ability){
            int random = Random.Range(0, 100);
            if(random < piety/10){
                damage = (strength / monster.GetComponent<Monster>().defense) * 2;
            }
            else{
                damage = strength / monster.GetComponent<Monster>().defense;
            }
        }
        

        //데미지 0보다 낮으면 0으로 오버플로우방지
        if (damage < 0)
        {
            damage = 0;
        }

        //공격
        //effectspawner포지션에서 fireball을 생성
        // GameObject fireball = Instantiate(FireballPrefab, effectspawner.transform.position, Quaternion.identity);
        // fireball.transform.SetParent(GameObject.Find("Canvas").transform, false);
        //데미지
        monster.GetComponent<Monster>().health -= damage;
        GameObject hudText = Instantiate(hud);
        hudText.transform.position = hudpos.position;
        hudText.transform.SetParent(hudpos.transform);
        hud.GetComponent<FloatingDam>().damage = damage;
        monsterhpbar.value = monster.GetComponent<Monster>().health / monster.GetComponent<Monster>().fullhealth;
        //만약 몹뒤지면
        //invoke나중에 coroutine으로 바꿔야함
        if (monster.GetComponent<Monster>().health <= 0)
        {
            monster.GetComponent<Monster>().isDead = true;
            health = maxHealth;
            CancelInvoke("Player_Attack");
            CancelInvoke("Monster_attack");
            CancelInvoke("attackMotion");
            Destroy(monster);
            hpbar.SetActive(false);
            monsterhpbar.value = monster.GetComponent<Monster>().fullhealth;
            isfighting = false;
            if(isfighting == false){
                eventCount += 1;
                SetTurn();
                ButtonControlOn();
            }
            if(eventCount > 4 && isfighting == false){
                ending = true;
            }
        }

    }

    //플레이어
    public List<CharacterData> characterData = new List<CharacterData>();
    public Slider playerhpbar;
    public Slider healthbar;
    public Text hptext;
    public Text failText;

    //item
    public int strtraining = 0;
    public int deftraining = 0;
    public int toughtraining = 0;
    public int abilitytraining = 0;
    public int pietytraining = 0;
    public int strstatbouns = 0;
    public int defstatbouns = 0;
    public int toughstatbouns = 0;
    public int abistatbouns = 0;
    public int piestatbouns = 0;
    //dictionary
    // public Dictionary<string, int> equipment = new Dictionary<string, int>();
    //status text
    public Text statusTextPrefab;
    //player stats
    [SerializeField]
    public float health;
    [SerializeField]
    public float maxHealth;
    [SerializeField]
    public float stemina;
    [SerializeField]
    public float maxStemina;
    [SerializeField]
    public int strength;
    [SerializeField]
    public int defense;
    [SerializeField]
    public int tough;
    [SerializeField]
    public int ability;
    [SerializeField]
    public int piety;
    public int StrengthLevel = 1;
    public int DefenseLevel = 1;
    public int ToughLevel = 1;
    public int AbilityLevel = 1;
    public int PietyLevel = 1;
    public float damage = 0;
    public Dictionary<string, int> characterKey = new Dictionary<string, int>();
    public Dictionary<int, int> itemKey = new Dictionary<int, int>();
    private void Start()
    {
        itemKey.Add(1, 3);
        itemKey.Add(2, 20);
        itemKey.Add(3, 3);
        itemKey.Add(4, 20);
        ItemStat(currentItem, itemrank);
        characterKey.Add("wizard", 0);
        characterKey.Add("fighter", 1);
        characterKey.Add("warrior", 2);
        StartStat(characterKey[currentCharacter]);
    }
    public void ItemStat(int key,int rank){
        if (key == 1){
            abilitytraining += itemKey[key] + rank;
        }
        else if(key == 2){
            abistatbouns += itemKey[key] + (rank * 5);
        }
        else if(key == 3){
            strtraining += itemKey[key] + rank;
        }
        else if(key == 4){
            strstatbouns += itemKey[key] + (rank * 5);
        }
        
    }
    // Character Data scriptable object to get player stats
    public void StartStat(int key)
    {
        if(File.Exists(filePath)){
            LoadSave();
        }
        else{
        health = characterData[key].health;
        maxHealth = characterData[key].health;
        stemina = characterData[key].stemina;
        maxStemina = characterData[key].stemina;
        strength = characterData[key].strength + strstatbouns;
        defense = characterData[key].defense + defstatbouns;
        tough = characterData[key].tough + toughstatbouns;
        ability = characterData[key].ability + abistatbouns;
        piety = characterData[key].piety + piestatbouns;
        }
    }



    public void StaminaCheck(int what){
        if(stemina < maxStemina / 2){
            float failChance = (maxStemina / 2 - stemina) / (maxStemina / 2) * 100;
            float random = Random.Range(0, 100);
            if (random <= failChance){
                //stat down
                if(what == 1){
                    strength -= 5 * StrengthLevel;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int strengthDown = 5 * StrengthLevel;
                    statusText.text = "힘 -" + strengthDown +"감소ㅠㅠ";
                }
                else if(what == 2){
                    defense -= 5 * DefenseLevel;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int defenseDown = 5 * DefenseLevel;
                    statusText.text = "방어 -" + defenseDown+"감소ㅠㅠ";
                }
                else if(what == 3){
                    tough -= 5 * ToughLevel;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int toughDown = 5 * ToughLevel;
                    statusText.text = "저항력 -" + toughDown+"감소ㅠㅠ";
                }
                else if(what == 4){
                    ability -= 5 * AbilityLevel;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int abilityDown = 5 * AbilityLevel;
                    statusText.text = "마법 -" + abilityDown+"감소ㅠㅠ";
                }
                else if(what == 5){
                    piety -= 5 * PietyLevel;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int pietyDown = 5 * PietyLevel;
                    statusText.text = "신앙 -" + pietyDown+"감소ㅠㅠ";
                }
            }else{
                //stat up
                int posibility = (int)failChance;
                int posibility_cal = posibility / 5;
                if(what == 1){
                    strength += (10 * StrengthLevel + strtraining)+(posibility_cal);
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int strengthstat = 10 * StrengthLevel+strtraining+posibility_cal;
                    statusText.text = "힘" + strengthstat + " 증가!";
                }
                else if(what == 2){
                    defense += (10 * DefenseLevel + deftraining)+(posibility_cal);
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int defensestat = 10 * DefenseLevel+deftraining+posibility_cal;
                    statusText.text = "방어" + defensestat + " 증가!";
                }
                else if(what == 3){
                    tough += (10 * ToughLevel + toughtraining)+(posibility_cal);
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int toughstat = 10 * ToughLevel+toughtraining+posibility_cal;
                    statusText.text = "저항력" + toughstat + " 증가!";
                }
                else if(what == 4){
                    ability += (10 * AbilityLevel + abilitytraining)+(posibility_cal);
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int abilitystat = 10 * AbilityLevel+abilitytraining+posibility_cal;
                    statusText.text = "마법" + abilitystat+ " 증가!";
                }
                else if(what == 5){
                    piety += (10 * PietyLevel + pietytraining)+(posibility_cal);
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int pietystat = 10 * PietyLevel+pietytraining+posibility_cal;
                    statusText.text = "신앙" + pietystat+ " 증가!";
                }
            }
        }else{
            //stat up
            if (what == 1){
                    strength += 10 * StrengthLevel + strtraining;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int strengthstat = 10 * StrengthLevel+strtraining;
                    statusText.text = "힘" + strengthstat + " 증가!";
                }
                else if(what == 2){
                    defense +=10 * DefenseLevel + deftraining;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int defensestat = 10 * DefenseLevel+deftraining;
                    statusText.text = "방어" + defensestat + " 증가!";
                }
                else if(what == 3){
                    tough += 10 * ToughLevel + toughtraining;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int toughstat = 10 * ToughLevel+toughtraining;
                    statusText.text = "저항력" + toughstat + " 증가!";
                }
                else if(what == 4){
                    ability += 10 * AbilityLevel + abilitytraining;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int abilitystat = 10 * AbilityLevel+abilitytraining;
                    statusText.text = "마법" + abilitystat + " 증가!";
                }
                else if(what == 5){
                    piety += 10 * PietyLevel + pietytraining;
                    Text statusText = Instantiate(statusTextPrefab, GameObject.Find("Viewport").transform);
                    int pietystat = 10 * PietyLevel+pietytraining;
                    statusText.text = "신앙" + pietystat + " 증가!";
                }

            
        }
        stemina -= 15;
    }

    //스테미나가 100이하면 1로유지 100보다 크면 100으로 유지
    public void SteminaControll(){
        if(stemina < 1){
            stemina = 1;
        }
        else if(stemina > maxStemina){
            stemina = 100;
        }
    }
    public void StatControll(){
        if(strength >= 1200){
            strength = 1200;
        }
        else if(strength <= 0){
            strength = 1;
        }

        if(defense >= 1200){
            defense = 1200;
        }
        else if(defense <= 0){
            defense = 1;
        }

        if(tough >= 1200){
            tough = 1200;
        }
        else if(tough <= 0){
            tough = 1;
        }

        if(ability >= 1200){
            ability = 1200;
        }
        else if(ability <= 0){
            ability = 1;
        }

        if(piety >= 1200){
            piety = 1200;
        }
        else if(piety <= 0){
            piety = 1;
        }
    }

    public void Stay()
    {
        //스테미너 20퍼센트확률로 30 증가 60퍼센트확률로 50증가 20퍼센트확률로 70증가
        int random = Random.Range(0, 100);
        if (random <= 20){
            stemina += 30;
            var steminaUp = Instantiate(statusTextPrefab, transform.position, Quaternion.identity);
            steminaUp.transform.SetParent(GameObject.Find("Viewport").transform);
            steminaUp.text = "스테미너 30 증가";
        }
        else if (random > 20 && random <= 80){
            stemina += 50;
            var steminaUp = Instantiate(statusTextPrefab, transform.position, Quaternion.identity);
            steminaUp.transform.SetParent(GameObject.Find("Viewport").transform);
            steminaUp.text = "스테미너 50 증가";
        }
        else if (random > 80){
            stemina += 70;
            var steminaUp = Instantiate(statusTextPrefab, transform.position, Quaternion.identity);
            steminaUp.transform.SetParent(GameObject.Find("Viewport").transform);
            steminaUp.text = "스테미너 70 증가";
        }
        SteminaControll();

    }

    public void Strength()
    {
        if (strength >= 200)
        {
            StrengthLevel = 2;
        }
        if(strength >= 400)
        {
            StrengthLevel = 3;
        }
        if(strength >= 600){
            StrengthLevel = 4;
        }
        if(strength >= 800){
            StrengthLevel = 5;
        }

        
        
        player.GetComponent<Player>().Anim.SetBool("isRun", true);
        StaminaCheck(1);
        SteminaControll();
        StatControll();
        Invoke("resetAnim", 2f);
    }

    void resetAnim()
    {
        player.GetComponent<Player>().ResetAnimation();
    }

    public void Defense()
    {
        if (defense >= 200)
        {
            DefenseLevel = 2;
        }
        if(defense >= 400)
        {
            DefenseLevel = 3;
        }
        if(defense >= 600){
            DefenseLevel = 4;
        }
        if(defense >= 800){
            DefenseLevel = 5;
        }
        

        
        
        player.GetComponent<Player>().Anim.SetBool("isRun", true);
        StaminaCheck(2);
        SteminaControll();
        StatControll();
        Invoke("resetAnim", 2f);
    }

    public void Tough()
    {
        if(tough >= 50){
            maxHealth = 200;
            health = maxHealth;
        }
        if(tough >= 100){
            maxHealth = 250;
            health = maxHealth;
        }
        if(tough >= 150){
            maxHealth = 300;
            health = maxHealth;
        }
        if(tough >= 200)
        {
            ToughLevel = 2;
            maxHealth = 350;
            health = maxHealth;
        }
        if(tough >= 250){
            maxHealth = 400;
            health = maxHealth;
        }
        if(tough >= 400)
        {
            ToughLevel = 3;
        }
        if(tough >= 600){
            ToughLevel = 4;
        }
        if(tough >= 800){
            ToughLevel = 5;
        } 
        
        player.GetComponent<Player>().Anim.SetBool("isRun", true);
        StaminaCheck(3);
        SteminaControll();
        StatControll();
        Invoke("resetAnim", 2f);
    }

    public void Ability()
    {
        if (ability >= 200)
        {
            AbilityLevel = 2;
        }
        if(ability >= 400)
        {
            AbilityLevel = 3;
        }
        if(ability >= 600)
        {
            AbilityLevel = 4;
        }
        if(ability >= 800){
            AbilityLevel = 5;
        }
        
        
        player.GetComponent<Player>().Anim.SetBool("isRun", true);
        StaminaCheck(4);
        SteminaControll();
        StatControll();
        Invoke("resetAnim", 2f);
    }

    public void Piety()
    {
        
        if (piety >= 200)
        {
            PietyLevel = 2;
        }
        if (piety >= 400)
        {
            PietyLevel = 3;
        }
        if (piety >= 600)
        {
            PietyLevel = 4;
        }
        if (piety >= 800)
        {
            PietyLevel = 5;
        }

        
        player.GetComponent<Player>().Anim.SetBool("isRun", true);
        StaminaCheck(5);
        SteminaControll();
        StatControll();
        Invoke("resetAnim", 2f);
    }
    
    private void OnApplicationQuit()
    {
        if(remainTurn == 0 && isfighting == true){
            
        }else if (health > 1){
            QuitSave();
        }

    }

    // private void OnDisable()
    // {
    //     if(health > 1){
    //         QuitSave();
    //     }
    // }

    public void QuitSave(){
        DataSaver.SaveData saveData = new DataSaver.SaveData(currentCharacter,currentItem,strength,ability,piety,defense,tough,stemina,StrengthLevel,DefenseLevel,ToughLevel,AbilityLevel,PietyLevel,maxStemina,turn,remainTurn,eventCount,health,maxHealth);
        saveData.currentStrength = strength;
        saveData.currentDefense = defense;
        saveData.currentTough = tough;
        saveData.currentAbility = ability;
        saveData.currentPiety = piety;
        saveData.currentStemina = stemina;
        saveData.currentStrengthLevel = StrengthLevel;
        saveData.currentDefenseLevel = DefenseLevel;
        saveData.currentToughLevel = ToughLevel;
        saveData.currentAbilityLevel = AbilityLevel;
        saveData.currentPietyLevel = PietyLevel;
        saveData.currentremainTurn = remainTurn;
        saveData.currenteventCount = eventCount;
        saveData.currentmaxStemina = maxStemina;
        saveData.currentCharacter = currentCharacter;
        saveData.currentItem = currentItem;
        saveData.currentTurn = turn;
        saveData.currentremainTurn = remainTurn;
        saveData.currenteventCount = eventCount;
        saveData.currentHealth = health;
        saveData.currentmaxHealth = maxHealth;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, json);
    }

    public void LoadSave(){
        string json = File.ReadAllText(filePath);
        DataSaver.SaveData saveData = JsonUtility.FromJson<DataSaver.SaveData>(json);
        strength = saveData.currentStrength;
        defense = saveData.currentDefense;
        tough = saveData.currentTough;
        ability = saveData.currentAbility;
        piety = saveData.currentPiety;
        stemina = saveData.currentStemina;
        StrengthLevel = saveData.currentStrengthLevel;
        DefenseLevel = saveData.currentDefenseLevel;
        ToughLevel = saveData.currentToughLevel;
        AbilityLevel = saveData.currentAbilityLevel;
        PietyLevel = saveData.currentPietyLevel;
        maxStemina = saveData.currentmaxStemina;
        turn = saveData.currentTurn;
        remainTurn = saveData.currentremainTurn;
        eventCount = saveData.currenteventCount;
        currentCharacter = saveData.currentCharacter;
        currentItem = saveData.currentItem;
        turn = saveData.currentTurn;
        remainTurn = saveData.currentremainTurn;
        eventCount = saveData.currenteventCount;
        health = saveData.currentHealth;
        maxHealth = saveData.currentmaxHealth;
    }

    public void DeleteSave(){
        File.Delete(filePath);
    }

    public Text SpeedText;


    //2배속
    public void Speed_2()
    {
        Time.timeScale = 2f;
        SpeedText.text = "X2";
    }

    public void Speed_1()
    {
        Time.timeScale = 1f;
        SpeedText.text = "X1";
    }

    public void SpeedControl(){
        if(Time.timeScale == 1f){
            Speed_2();
        }
        else if(Time.timeScale == 2f){
            Speed_1();
        }
    }
}
