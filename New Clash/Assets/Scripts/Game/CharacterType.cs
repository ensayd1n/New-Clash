using UnityEngine;

[CreateAssetMenu(fileName = "New Character Type",menuName = "Character Type")]
public class CharacterType : ScriptableObject
{
    public string Name;
    public string Desciption;
    public float MaxHealth;
    public float GivenDamage;
    public float DamageDuration;
    public float Cost;
    public float MoveSpeed;
    public float TargetDisctance;
    public Sprite CharacterImage;
    public GameObject ThisGameObject;
    public GameObject ThisHologramParentGameObject;
}
