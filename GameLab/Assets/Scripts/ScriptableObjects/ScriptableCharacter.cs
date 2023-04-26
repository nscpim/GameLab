using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character", menuName = "CharacterCreator", order = 1)]
public class ScriptableCharacter : ScriptableObject
{
    //Name of the character.
    public string characterName;
    //ID of the character (for reference).
    public int characterID;
    //Enum of the character
    public Character characterEnum;
    //Short description of the character.
    [TextArea(3, 10)]
    public string description;
    //Image for the ability icon.
    public Image abilityIcon;
    //Image for the character.
    public Image characterImage;
    //Mesh of the character.
    public Mesh characterMesh;
    //Cooldown of the ability.
    public float abilityCooldown;
    //Movement speed of the character.
    public float speed;
    //Jump height of the character.
    public float jumpSpeed;

    public float turnSmoothTime = 0.1f;

    public float gravity;

    public float maxSpeed;

    public float acceleration;

    //public float startingSpeed;


    // Start is called before the first frame update
    public void Awake()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
