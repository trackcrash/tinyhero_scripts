using UnityEngine;

[ CreateAssetMenu( fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData" )]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private string CharacterName;
    public string characterName
    {
        get { return CharacterName; }
    }
    [SerializeField]
    private GameObject CharacterPrefab;
    public GameObject characterPrefab
    {
        get { return CharacterPrefab; }
    }
    [SerializeField]
    private int Health;
    public int health
    {
        get { return Health; }
    }
    [SerializeField]
    private int MaxHealth;
    public int maxHealth
    {
        get { return MaxHealth; }
    }
    [SerializeField]
    private int Stemina;
    public int stemina
    {
        get { return Stemina; }
    }
    [SerializeField]
    private int MaxStemina;
    public int maxStemina
    {
        get { return MaxStemina; }
    }
    [SerializeField]
    private int Strength;
    public int strength
    {
        get { return Strength; }
    }
    [SerializeField]
    private int Defense;
    public int defense
    {
        get { return Defense; }
    }
    [SerializeField]
    private int Tough;
    public int tough
    {
        get { return Tough; }
    }
    [SerializeField]
    private int Ability;
    public int ability
    {
        get { return Ability; }
    }
    [SerializeField]
    private int Piety;
    public int piety
    {
        get { return Piety; }
    }

}
