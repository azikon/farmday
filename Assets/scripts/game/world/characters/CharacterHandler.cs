using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterHandler : HandlerBase<CharacterHandler>
{
    public Character CurrentCharacter;

    public override void LoadData()
    {
        base.LoadData();
    }

    public void DoUpdateFrame()
    {
        if (CurrentCharacter != null)
        {
            CurrentCharacter.DoUpdateFrame();
        }
    }
}