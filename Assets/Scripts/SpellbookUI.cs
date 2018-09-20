using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellbookUI : NetworkBehaviour {

    [SerializeField]
    private CanvasGroup spellBook;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        OpenClose(spellBook);
	}

    public void OpenClose(CanvasGroup canvasGroup)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
        }
    }
}
