using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System;

public class ButtonUI : MonoBehaviour, IPointerClickHandler {

    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    Image frameImage;
    string parentButtonName;

    public Slider cooldownSlider;
    public Image fillImage;
    public float startCooldown = 5f;
    public float currentCooldown;
    public bool decreaseCooldownSpellChosen;
    public bool decreaseCooldownFireMade;
    public bool allowResetOfCooldown;

    [SerializeField]
    private Image spellIcon;

    public Image SpellIcon
    {
        get
        {
            return spellIcon;
        }

        set
        {
            spellIcon = value;
        }
    }

    // Use this for initialization
    void Start () {
        frameImage = GetComponent<Image>();
        parentButtonName = transform.name;
        currentCooldown = startCooldown;
        decreaseCooldownFireMade = false;
        decreaseCooldownSpellChosen = false;
        allowResetOfCooldown = true;
           
    }
	
	// Update is called once per frame
	void Update () {
        if (SpellslingerScript.localSlinger != null) { 
            if (SpellslingerScript.localSlinger.hasAuthority == true)
            {
                ActionBarButtonAnimation();
            }
        }

        StartCooldownSlider();
    }

    // Rotates the action bar. Should probably make this into a function.
    void ActionBarButtonAnimation()
    {
        if (Input.GetKeyDown("1"))
        {
            if (parentButtonName == "ActionButton0")
            {
                frameImage.sprite = selectedSprite;
                cooldownSlider.maxValue = SpellslingerScript.localSlinger.GetPlayerSpellCooldows(0);
                decreaseCooldownSpellChosen = true;
            }
            if (parentButtonName == "ActionButton2" || parentButtonName == "ActionButton1")
            {
                frameImage.sprite = notSelectedSprite;             
            }
        }
        else if (Input.GetKeyDown("2"))
        {
            if (parentButtonName == "ActionButton1")
            {
                frameImage.sprite = selectedSprite;
                cooldownSlider.maxValue = SpellslingerScript.localSlinger.GetPlayerSpellCooldows(1);
                decreaseCooldownSpellChosen = true;
            }
            if (parentButtonName == "ActionButton2" || parentButtonName == "ActionButton0")
            {
                frameImage.sprite = notSelectedSprite;
            }
        }
        else if (Input.GetKeyDown("3"))
        {
            if (parentButtonName == "ActionButton2")
            {
                frameImage.sprite = selectedSprite;
                cooldownSlider.maxValue = SpellslingerScript.localSlinger.GetPlayerSpellCooldows(2);
                decreaseCooldownSpellChosen = true;
            }
            if (parentButtonName == "ActionButton0" || parentButtonName == "ActionButton1")
            {
                frameImage.sprite = notSelectedSprite;
            }
        }

        // Removes choosing border after a spell has been cast
        if (Input.GetButton("Fire1"))
        {
            frameImage.sprite = notSelectedSprite;
            if (decreaseCooldownSpellChosen && allowResetOfCooldown) { 
                decreaseCooldownFireMade = true;               
                cooldownSlider.value = cooldownSlider.maxValue;
                allowResetOfCooldown = false;
            }
        }
    }

    private void StartCooldownSlider()
    {      
        if (cooldownSlider != null && decreaseCooldownFireMade && decreaseCooldownSpellChosen)
        {
            //Debug.Log("Decreasing cooldown");
            //currentCooldown -= Time.deltaTime;
            cooldownSlider.value -= Time.deltaTime; //= currentCooldown;

            if (cooldownSlider.value <= 0)
            if (cooldownSlider.value <= 0)
            {
                decreaseCooldownFireMade = false;
                decreaseCooldownSpellChosen = false;
                allowResetOfCooldown = true;
            }
        }      
    }

    // Updates the actionbar after placement of spell icon
    public void UpdateVisual()
    {

        spellIcon.sprite = MovingSpellsAndItems.MyInstance.Put().MyIcon;
        spellIcon.color = Color.white;

        int actionButtonPlace = Int32.Parse(parentButtonName.Substring(12, 1));

        SpellslingerScript.localSlinger.CmdSetPlayerSpell(SpellBook.MyInstance.GetSpellName(MovingSpellsAndItems.MyInstance.Put().MySpellName), actionButtonPlace);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (MovingSpellsAndItems.MyInstance.MyMoveable != null)
            {
                UpdateVisual();
            }
        }
    }
}
