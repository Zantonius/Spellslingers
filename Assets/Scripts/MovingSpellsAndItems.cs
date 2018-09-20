using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MovingSpellsAndItems : NetworkBehaviour {

    private static MovingSpellsAndItems instance;

    public static MovingSpellsAndItems MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MovingSpellsAndItems>();
            }

            return instance;
        }
    }

    public IMoveable MyMoveable { get; set; }

    private Image icon;

	// Use this for initialization
	void Start () {
        icon = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        icon.transform.position = Input.mousePosition;
	}

    public void TakeMoveableObject(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }
}
