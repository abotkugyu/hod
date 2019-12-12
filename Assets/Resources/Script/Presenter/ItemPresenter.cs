using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPresenter : MonoBehaviour {

    public ItemModel status;
    
    [SerializeField]
    public ItemView itemView;
    
    public void Initialize(ItemModel model, int guid)
    {
        //絶対にnewしないと参照が変になる。
        status = new ItemModel(model) {guid = guid};
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
    public void SetFloorId(int floorId)
    {
        status.floorId = floorId;
    }    
    
    /// user.status.position に座標保存
    public void SetPosition(Vector3 position)
    {
        status.position = position;
        itemView.SetPosition(position);
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
        
        itemView.SetAngles(new Vector3(0, angle, 0));

        status.direction = new Vector3(position.x, position.y, position.z);
    }
}
