using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterPresenter : MonoBehaviour {

    // Use this for initialization
    public UserModel status;
    
    [SerializeField]
    public CharacterView characterView;
    public float speed = 4.0f;
    public bool isMove = false;
    public Vector3 targetPosition;        
    public List<ItemModel> itemModels;
   
    void Update () {
        if (status.isOwn)
        {
            if (isMove && status.isAction == false)
            {
                Moving();
            }
        }
        else if (isMove)
        {            
            Moving();
        }
        
    }
    
    public void Initialize(UserModel model, int guid)
    {
        //絶対にnewしないと参照が変になる。
        status = new UserModel(model) {guid = guid, isAction = false};
        characterView.Initialize();
        DummyItem();
    }
        
    public void DummyItem()
    {        
        //itemModels.Add(new ItemModel(ItemData.GetRandom()));
    }
    
    public void DummyInitialize(List<int> pos)
    {        
        int posx = pos[0];
        int posz = pos[1];
        SetPosition(new Vector3(posx, 0, posz));
        SetDirection(new Vector3(0, 0, -1));
    }
    
    //攻撃処理
    public void Attack(MapPresenter m, CharacterListPresenter characterListPresenter)
    {
        Vector3 pos = status.position + status.direction;
        if (pos.x < 0 || pos.z < 0){
            return;
        }

        var vector2Int = new Vector2Int((int) pos.x, (int) pos.z);
        if (m.GetTileModel(vector2Int).charaType == TileModel.CharaType.Player ||
            m.GetTileModel(vector2Int).charaType == TileModel.CharaType.Enemy)
        {
            CharacterPresenter characterPresenter = 
                characterListPresenter.characterListPresenter
                    .FirstOrDefault(presenter => presenter.Key == m.GetTileModel(vector2Int).guid).Value;
            
            int damage = CalcAction.CalcAttack(status, characterPresenter.status);
            
            Debug.Log("Attack Damage : "+ damage);            
            characterPresenter.CalcHp(damage);
            Debug.Log("After Hp : "+ characterPresenter.status.hp);       
            if (characterPresenter.status.hp <= 0)
            {
                characterListPresenter.Delete(characterPresenter);
            }
        }
        SetIsAction(true);

        characterView.Attack();
    }

    /// <summary>
	/// Force	その質量を使用して、rigidbodyへの継続的な力を追加します。
	/// Acceleration	その質量を無視して、rigidbodyへの継続的な加速を追加します。
	/// Impulse	その質量を使用して、rigidbodyに瞬時に速度変化を追加します。
	/// VelocityChange	その質量を無視して、rigidbodyに瞬時に速度変化を追加します。
    /// transform.AddForce(x, 0, z, ForceMode.Acceleration);
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
	public void Move (float x,float z) {
        isMove = true;
		Rigidbody transform = characterView.GetTransForm();
		Vector3 nowPosition = transform.position;

        targetPosition.x = x/200 + nowPosition.x;
        targetPosition.z = z/200 + nowPosition.z;
        
        characterView.Run(true);
	}

    void Moving(){
        Rigidbody transform = characterView.GetTransForm();
        Vector3 nowPosition = transform.position;

        float step = 0.04f;//speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, 0, targetPosition.z), step);
        if (targetPosition.x == nowPosition.x && targetPosition.z == nowPosition.z)
        {
            transform.velocity = Vector3.zero;
            isMove = false;
            SetIsAction(true);
            characterView.Run(false);            
        }
    }
    public void CalcMove (float x,float z, MapPresenter mapPresenter) {
        isMove = true;
        Rigidbody transform = characterView.GetTransForm();
        Vector3 nowPosition = transform.position;

        targetPosition.x = x/200 + nowPosition.x;
        targetPosition.z = z/200 + nowPosition.z;
        
        characterView.Run(true);
    }
    
    /// user.status.position に座標保存
    public void SetPosition(Vector3 position)
    {
        status.position = position;
        characterView.SetPosition(position);
    }

    /// <summary>
    /// <code>
    /// transform.eulerAnglesで向きを変更
    /// user.status.directionに向き保存
    /// </code>
    /// </summary>
    public void SetDirection(Vector3 position)
    {
        int nowDirection = TransFormUtil.GetChangeRotate(status.direction);
        int targetDirection = TransFormUtil.GetChangeRotate(position);

        float angle = Mathf.LerpAngle(nowDirection, targetDirection, Time.time);
        
        characterView.SetAngles(new Vector3(0, angle, 0));

        status.direction = new Vector3(position.x, position.y, position.z);
    }
    
    public bool SetItem(ItemModel itemModel)
    {
        if (GameConfig.ItemMaxLimit <= itemModels.Count)
        {
            Debug.Log("NOTICE : Over Has Item Limit");
            return false;
        }
            
        itemModels.Add(itemModel);
        return true;
    }
    
    
    void OnWillRenderObject()
    {
        //カメラに表示されている時のみ
        if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")
        {
            // 処理
        }
    }
    public void CalcHp(int damage)
    {
        status.hp -= damage;
        
        float per = (float)status.hp / (float)status.maxHp;
        Debug.Log("Per : " + per);
        characterView.UpdateHud((int)(per * 100));
    }
    public void SetFloorId(int floorId)
    {
        status.floorId = floorId;
    }
    
    /// <summary>
    /// SetFloorId
    /// SetPosition
    /// SetFloorId
    /// を内包している
    /// </summary>
    /// <param name="floorId"></param>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    public void SetMapData(int floorId, Vector3 position, Vector3 direction)
    {
        SetFloorId(floorId);
        SetPosition(position);
        SetDirection(direction);
    }
                
    public int GetAction()
    {
        return Random.Range(1, 2);
    }
    
    //行動済み設定
    public void SetIsAction(bool isAction)
    {
        status.isAction = isAction;
    }
}
