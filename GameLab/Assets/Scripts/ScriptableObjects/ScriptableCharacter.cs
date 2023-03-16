using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character", menuName = "CharacterCreator", order = 1)]
public class ScriptableCharacter : ScriptableObject
{
    //Name of the character
    public string characterName;
    //ID of the character (for reference)
    public int characterID;
    //Enum of the character
    public Character characterEnum;
    [TextArea(3, 10)]
    public string description;
    public Image abilityIcon;
    public Image characterImage;
    public Mesh characterMesh;
    public float abilityCooldown;
    public float moveSpeed;
    public float jumpHeight;

    // Start is called before the first frame update
    public void Awake()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
