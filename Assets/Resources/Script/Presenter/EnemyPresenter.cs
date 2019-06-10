using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour {

    public UserModel status;
    public float speed = 4.0f;
    public bool isMove = false;
    public Vector3 targetPosition;
    
    [SerializeField]
    public EnemyView enemyView;

    void Update()
    {
        if (isMove)
        {
            Moving();
        }
    }
        
    public void Initialize(UserModel model, int guid)
    {                
        //絶対にnewしないと参照が変になる。
        status = new UserModel(model) {guid = guid, isAction = false};
    }

        
    public int GetAction()
    {
        return Random.Range(1, 2);
    }

    //移動処理
    public void Move(float x, float z)
    {
        isMove = true;
        Rigidbody transform = enemyView.GetTransForm();
        Vector3 nowPosition = transform.position;

        targetPosition.x = x/200 + nowPosition.x;
        targetPosition.z = z/200 + nowPosition.z;
        
    }

    void Moving()
    {        
        Rigidbody transform = enemyView.GetTransForm();
        Vector3 nowPosition = transform.position;

        float step = 0.04f;//speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, 0, targetPosition.z), step);
        if (targetPosition.x == nowPosition.x && targetPosition.z == nowPosition.z)
        {
            transform.velocity = Vector3.zero;
            isMove = false;
            SetIsAction(true);
        }
    }

    //position設定
    public void SetPosition(Vector3 position)
    {
        status.position = position;
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
        
        enemyView.SetAngles(new Vector3(0, angle, 0));

        status.direction = new Vector3(position.x, position.y, position.z);
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
    
    //行動済み設定
    public void SetIsAction(bool isAction)
    {
        status.isAction = isAction;
    }

    void OnWillRenderObject()
    {
        //カメラに表示されている時のみ
        if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")
        {
            // 処理
        }
    }
}
