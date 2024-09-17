using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSceneManager : GameSceneManager
{
    public void StartScene(FightData _fightData)
    {
        base.StartScene();
        FightManager.I?.EnterFight(_fightData);
    }
}
