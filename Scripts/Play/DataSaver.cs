using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static DataSaver instance;
    public List<SaveData> savedata = new List<SaveData>();
    [System.Serializable]
    public class SaveData
    {
        public string currentCharacter;
        public int currentItem;
        public int currentStrength;
        public int currentAbility;
        public int currentPiety;
        public int currentDefense;
        public int currentTough;
        public float currentStemina;
        public int currentStrengthLevel;
        public int currentDefenseLevel;
        public int currentToughLevel;
        public int currentAbilityLevel;
        public int currentPietyLevel;
        public float currentmaxStemina;
        public int currentTurn;
        public int currentremainTurn;
        public int currenteventCount;
        public float currentHealth;
        public float currentmaxHealth;

        public SaveData(string currentCharacter, int currentItem, int currentStrength, int currentAbility, int currentPiety, int currentDefense, int currentTough, float currentStemina, int currentStrengthLevel, int currentDefenseLevel, int currentToughLevel, int currentAbilityLevel, int currentPietyLevel, float currentmaxStemina, int currentTurn, int currentremainTurn, int currenteventCount,float currentHealth,float currentmaxHealth)
        {
            this.currentCharacter = currentCharacter;
            this.currentItem = currentItem;
            this.currentStrength = currentStrength;
            this.currentAbility = currentAbility;
            this.currentPiety = currentPiety;
            this.currentDefense = currentDefense;
            this.currentTough = currentTough;
            this.currentStemina = currentStemina;
            this.currentStrengthLevel = currentStrengthLevel;
            this.currentDefenseLevel = currentDefenseLevel;
            this.currentToughLevel = currentToughLevel;
            this.currentAbilityLevel = currentAbilityLevel;
            this.currentPietyLevel = currentPietyLevel;
            this.currentmaxStemina = currentmaxStemina;
            this.currentTurn = currentTurn;
            this.currentremainTurn = currentremainTurn;
            this.currenteventCount = currenteventCount;
            this.currentHealth = currentHealth;
            this.currentmaxHealth = currentmaxHealth;
        }

    }


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }




    //������ ���� �� ������ ������
    private void OnApplicationQuit()
    {

    }
}
