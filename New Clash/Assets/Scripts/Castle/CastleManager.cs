using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    public CharacterType CastleType;
    public bool TeamBlue, TeamRed;
    [HideInInspector] public string TargetCharacterTag="UnTagged";

    private void Awake()
    {
        if (TeamBlue)
        {
            TargetCharacterTag = "TeamRedCharacter";
        }
        else if (TeamRed)
        {
            TargetCharacterTag = "TeamBlueCharacter";
        }
    }
}
