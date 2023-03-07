using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character", menuName = "CharacterCreator", order = 1)]
public class ScriptableCharacter : ScriptableObject
{
    public string characterName;
    public int characterID;
    public Character character;
    [TextArea(3, 10)]
    public string description;
    public Image abilityIcon;
    public Image characterImage;
    public Mesh characterMesh;
    public float abilityCooldown;
    public float moveSpeed;
    public float jumpHeight;
    private float timeStamp;

   
    // Start is called before the first frame update

    public void ExecuteAbility() 
    {
        if (timeStamp <= Time.time)
        {
            switch (character)
            {
                case Character.Test:
                    break;
                case Character.Test1:
                    break;
                case Character.Test2:
                    break;
                case Character.Test3:
                    break;
                default:
                    break;
            }
        }

        timeStamp = Time.time + abilityCooldown;
    }

    public void Awake()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public enum Character 
    {
    Test,
    Test1,
    Test2,
    Test3,
    }

}

