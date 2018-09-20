using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SpellbookButtonUI : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string spellName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Pick up the spell
            MovingSpellsAndItems.MyInstance.TakeMoveableObject(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
}
