using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMain : MonoBehaviour
{
    
    public CharacterListPresenter characterListPresenter;
    // Start is called before the first frame update
    void Start()
    {
        UserModel player = PlayerData.GetRandom();
        player.isOwn = true;
        characterListPresenter.Generate(new Vector2Int(0,0),0, player);
    }

    // Update is called once per frame
    void Update()
    {
        var characterPresenter = characterListPresenter.GetOwnCharacterPresenter();
        InputAxis axis = InputAxis.GetInputAxis();
        characterPresenter.SetDirection(new Vector3(axis.I.x, 0, axis.I.y));
        characterPresenter.Move(axis.F.x, axis.F.y);
    }
}
