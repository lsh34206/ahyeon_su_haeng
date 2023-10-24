using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        
        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spwan(Define.WorldObject.Player, "UnityChan");
     Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        Managers.Game.Spwan(Define.WorldObject.Monster, "Knight");
    }

    public override void Clear()
    {
        
    }
}
