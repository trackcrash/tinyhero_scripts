using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManage : MonoBehaviour
{
    public GameObject SuperButton;
    public GameObject playerPrefab;
    public GameObject player;
    public GameObject hud;
    public GameObject TutorialUI;
    public Transform hudpos;
    public Text StrengthText;
    public Text DefenseText;
    public Text ToughText;
    public Text AbilityText;
    public Text PietyText;
    public Text TurnText;
    public GameObject DiePanel;
    public Text ObjectText;
    //Buttons
    public Button StrengthButton;
    public Button DefenseButton;
    public Button ToughButton;
    public Button AbilityButton;
    public Button PietyButton;
    public Button TurnButton;
    public GameObject str;
    public GameObject def;
    public GameObject tou;
    public GameObject abi;
    public GameObject pie;
    public GameObject tur;
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
    //playerstrengthtext
    public string playerStrengthText;
    public bool isfighting = false;
    public string currentCharacter = "wizard";
    public int currentItem = 2;
    public bool ending = false;
    public void Awake()
    {
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
    public void Update()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerhpbar.value = stemina / maxStemina;
        hptext.text = stemina + "/" + maxStemina;
        healthbar.value = health / maxHealth;
        //���� statusTextPrefab�� 4�� �̻��̸� ���� ������ statusTextPrefab�� ����
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




        //texts
        StrengthText.text = "힘: " + strength;
        DefenseText.text = "방어: " + defense;
        ToughText.text = "저항력: " + tough;
        AbilityText.text = "마법: " + ability;
        PietyText.text = "신앙: " + piety;
        TurnText.text = "목표까지 앞으로" + remainTurn +"턴";

        //monsterspawner�� hpbar�� �־������
        if (isfighting == true)
        {
            ButtonControlFight();
        }

    }

    public void stopquickclick(){
        SuperButton.SetActive(false);
        Invoke("startquickclick", 2f);
        
    }
    public void startquickclick(){
        SuperButton.SetActive(true);

    }
    //��ǥ�ؽ�Ʈ
    public void SetObjectText()
    {
        //ObjectText.text = text;
    }

    //�ϼ���
    public void SetTurn()
    {
        if (eventCount == 0)
        {
            turn = 10;
        }
        else if (eventCount == 1)
        {
            turn = 8;
        }
        else if (eventCount == 2)
        {
            turn = 10;
        }
        else if (eventCount == 3)
        {
            turn = 15;
        }
        else if (eventCount == 4)
        {
            turn = 8;
        }
        remainTurn = turn;
    }
    //������ ����
    public void SetRemainTurn()
    {
        remainTurn -= 1;
    }
    public void startBattle()
    {
        monsterSpawner.SpawnMonster(eventCount);
        hpbar.SetActive(true);
        //ResetAnimation
        player.GetComponent<Player>().ResetAnimation();
        isfighting = true;
        ButtonControlFight();
        InvokeRepeating("Player_Attack", 1f, 2f);
        InvokeRepeating("attackMotion", 1f, 1f);
        InvokeRepeating("Monster_attack", 3f, 2f);

    }
    //���������� �Ѿ��
    public void NextTurn()
    {
        GameObject monster = GameObject.FindWithTag("Monster");
        SetRemainTurn();
        if (remainTurn == 5)
        {
            if (eventCount < 3)
            {
                ChangeDialogCanvas();
                TestMessage_Selection.GetComponent<TestMessage_Selection>().event_updater(eventCount);
            }
        }
        else if (remainTurn == 0)
        {
            startBattle();
        }
    }
    //��ư ��Ʈ�� 2�ʵ��� (��Ȱ��ȭ) 
    public void ButtonControlOff()
    {
        StrengthButton.interactable = false;
        DefenseButton.interactable = false;
        ToughButton.interactable = false;
        AbilityButton.interactable = false;
        PietyButton.interactable = false;
        TurnButton.interactable = false;
        Invoke("ButtonControlOn", 2f);
    }
    //�����߸��߰�
    public void ButtonControlFight()
    {
        StrengthButton.interactable = false;
        DefenseButton.interactable = false;
        ToughButton.interactable = false;
        AbilityButton.interactable = false;
        PietyButton.interactable = false;
        TurnButton.interactable = false;
    }
    //��ư ��Ʈ��(Ȱ��ȭ)
    public void ButtonControlOn()
    {
        TurnButton.interactable = true;
    }
    //�÷��̾� ���ݸ��
    public void attackMotion()
    {
        player.GetComponent<Player>().Anim.SetTrigger("attack");
    }
    // ĵ���� ��ȯ �Լ�
    public void ChangeDialogCanvas()
    {
        Maincanvas.SetActive(false);
        Dialogcanvas.SetActive(true);
    }
    public void ChangeMainCanvas()
    {
        Maincanvas.SetActive(true);
        Dialogcanvas.SetActive(false);
    }
    //���Ͱ���
    void Monster_attack()
    {
        GameObject monster = GameObject.FindWithTag("Monster");
        health -= monster.GetComponent<Monster>().damage / (defense / 2);
        if (health <= 0)
        {
            //���� �ִϸ��̼�
            player.GetComponent<Player>().Anim.SetTrigger("die");
            DiePanel.SetActive(true);
            CancelInvoke("Player_Attack");
            CancelInvoke("Monster_attack");
            CancelInvoke("attackMotion");
            //���� ����
            //player.GetComponent<Player>().DieSound();
        }
        else
        {
            //�ǰ� �ִϸ��̼�
            player.GetComponent<Player>().Anim.SetTrigger("hurt");
            //�ǰ� ����
            // player.GetComponent<Player>().HitSound();
        }
    }
    //�÷��̾� ����
    public void Player_Attack()
    {
        //����������Ʈ�� ������
        GameObject monster = GameObject.FindWithTag("Monster");
        //calculate the damage to monster
        if (strength > ability)
        {
            //piety ��ġ�� ���� �������� ũ��Ƽ�� �������� ��
            int random = Random.Range(0, 100);
            if (random < piety / 10)
            {
                damage = (strength / monster.GetComponent<Monster>().defense) * 2;
            }
            else
            {
                damage = strength / monster.GetComponent<Monster>().defense;
            }
        }
        else if (ability > strength)
        {
            int random = Random.Range(0, 100);
            if (random < piety / 10)
            {
                damage = (ability / monster.GetComponent<Monster>().defense) * 2;
            }
            else
            {
                damage = ability / monster.GetComponent<Monster>().defense;
            }
        }
        else if (strength == ability)
        {
            int random = Random.Range(0, 100);
            if (random < piety / 10)
            {
                damage = (strength / monster.GetComponent<Monster>().defense) * 2;
            }
            else
            {
                damage = strength / monster.GetComponent<Monster>().defense;
            }
        }


        //������ 0���� ������ 0���� �����÷ο����
        if (damage < 0)
        {
            damage = 0;
        }

        //����
        monster.GetComponent<Monster>().health -= damage;
        GameObject hudText = Instantiate(hud);
        hudText.transform.position = hudpos.position;
        hudText.transform.SetParent(hudpos.transform);
        hud.GetComponent<FloatingDam>().damage = damage;
        monsterhpbar.value = monster.GetComponent<Monster>().health / monster.GetComponent<Monster>().fullhealth;
        //���� ��������
        //invoke���߿� coroutine���� �ٲ����
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
            if (isfighting == false)
            {
                eventCount += 1;
                SetTurn();
                ButtonControlOn();
                endingPanel.SetActive(true);
            }
        }

    }

    //�÷��̾�
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
    public void nextStep(){
        Debug.Log("nextStep");

    }
    private void Start()
    {
        itemKey.Add(1, 3);
        itemKey.Add(2, 20);
        itemKey.Add(3, 3);
        itemKey.Add(4, 20);
        ItemStat(currentItem);
        characterKey.Add("wizard", 0);
        characterKey.Add("fighter", 1);
        characterKey.Add("warrior", 2);
        StartStat(characterKey[currentCharacter]);

        StrengthButton.interactable = false;
        DefenseButton.interactable = false;
        ToughButton.interactable = false;
        PietyButton.interactable = false;
        TurnButton.interactable = false;
        

    }
    public void ItemStat(int key)
    {
        if (key == 1)
        {
            abilitytraining += itemKey[key];
        }
        else if (key == 2)
        {
            abistatbouns += itemKey[key];
        }
        else if (key == 3)
        {
            strtraining += itemKey[key];
        }
        else if (key == 4)
        {
            strstatbouns += itemKey[key];
        }

    }
    // Character Data scriptable object to get player stats
    public void StartStat(int key)
    {
            health = characterData[key].health;
            maxHealth = characterData[key].health;
            stemina = 55;
            maxStemina = characterData[key].stemina;
            strength = characterData[key].strength + strstatbouns;
            defense = characterData[key].defense + defstatbouns;
            tough = characterData[key].tough + toughstatbouns;
            ability = characterData[key].ability + abistatbouns;
            piety = characterData[key].piety + piestatbouns;
    }



    public void StaminaCheck(int what)
    {
        if (stemina < maxStemina / 2)
        {
            float failChance = (maxStemina / 2 - stemina) / (maxStemina / 2) * 100;
            float random = Random.Range(0, 100);
            if (random <= failChance)
            {
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

    //���׹̳��� 100���ϸ� 1������ 100���� ũ�� 100���� ����
    public void SteminaControll()
    {
        if (stemina < 1)
        {
            stemina = 1;
        }
        else if (stemina > maxStemina)
        {
            stemina = 100;
        }
    }
    public void StatControll()
    {
        if (strength >= 1200)
        {
            strength = 1200;
        }
        else if (strength <= 0)
        {
            strength = 1;
        }

        if (defense >= 1200)
        {
            defense = 1200;
        }
        else if (defense <= 0)
        {
            defense = 1;
        }

        if (tough >= 1200)
        {
            tough = 1200;
        }
        else if (tough <= 0)
        {
            tough = 1;
        }

        if (ability >= 1200)
        {
            ability = 1200;
        }
        else if (ability <= 0)
        {
            ability = 1;
        }

        if (piety >= 1200)
        {
            piety = 1200;
        }
        else if (piety <= 0)
        {
            piety = 1;
        }
    }

    public void Stay()
    {
        //���׹̳� 20�ۼ�ƮȮ���� 30 ���� 60�ۼ�ƮȮ���� 50���� 20�ۼ�ƮȮ���� 70����
        int random = Random.Range(0, 100);
        if (random <= 20)
        {
            stemina += 30;
            var steminaUp = Instantiate(statusTextPrefab, transform.position, Quaternion.identity);
            steminaUp.transform.SetParent(GameObject.Find("Viewport").transform);
            steminaUp.text = "스테미너 30 증가";
        }
        else if (random > 20 && random <= 80)
        {
            stemina += 50;
            var steminaUp = Instantiate(statusTextPrefab, transform.position, Quaternion.identity);
            steminaUp.transform.SetParent(GameObject.Find("Viewport").transform);
            steminaUp.text = "스테미너 50 증가";
        }
        else if (random > 80)
        {
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
        if (strength >= 400)
        {
            StrengthLevel = 3;
        }
        if (strength >= 600)
        {
            StrengthLevel = 4;
        }
        if (strength >= 800)
        {
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
        if (defense >= 400)
        {
            DefenseLevel = 3;
        }
        if (defense >= 600)
        {
            DefenseLevel = 4;
        }
        if (defense >= 800)
        {
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
        if (tough >= 50)
        {
            maxHealth = 200;
            health = maxHealth;
        }
        if (tough >= 100)
        {
            maxHealth = 250;
            health = maxHealth;
        }
        if (tough >= 150)
        {
            maxHealth = 300;
            health = maxHealth;
        }
        if (tough >= 200)
        {
            ToughLevel = 2;
            maxHealth = 350;
            health = maxHealth;
        }
        if (tough >= 250)
        {
            maxHealth = 400;
            health = maxHealth;
        }
        if (tough >= 400)
        {
            ToughLevel = 3;
        }
        if (tough >= 600)
        {
            ToughLevel = 4;
        }
        if (tough >= 800)
        {
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
        if (ability >= 400)
        {
            AbilityLevel = 3;
        }
        if (ability >= 600)
        {
            AbilityLevel = 4;
        }
        if (ability >= 800)
        {
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
}
