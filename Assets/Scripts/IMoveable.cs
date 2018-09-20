using UnityEngine;

public interface IMoveable
{
    Sprite MyIcon
    {
        get;
    }

    string MySpellName
    {
        get;
    }

    int MySpellId
    {
        get;
    }

    float MySpellCooldown
    {
        get;
    }

    float MySpellSpeed
    {
        get;
    }
}