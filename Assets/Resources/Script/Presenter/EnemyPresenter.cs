using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour {

    public UserModel status = new UserModel();
    public float speed = 4.0f;
    public bool isMove = false;
    public Vector3 targetPosition;

    void Update()
    {
        if (isMove)
        {
            Moving();
        }
    }
        
    public int GetAction()
    {
        return Random.Range(1, 2);
    }

    //移動処理
    public void StartMove(float x, float z)
    {
        isMove = true;
        Rigidbody transform = GetComponent<Rigidbody>();
        Vector3 nowPosition = transform.position;
        transform.AddForce(x, 0, z, ForceMode.Acceleration);
        targetPosition.x = nowPosition.x;
        targetPosition.z = nowPosition.z;
    }

    void Moving()
    {
        Rigidbody transform = GetComponent<Rigidbody>();
        Vector3 nowPosition = transform.position;
        if (targetPosition.x != nowPosition.x)
        {
            if (targetPosition.x + 1 < nowPosition.x)
            {
                transform.velocity = Vector3.zero;
                isMove = false;
                status.isAction = true;
            }
            else if (targetPosition.x - 1 > nowPosition.x)
            {
                transform.velocity = Vector3.zero;
                isMove = false;
                status.isAction = true;
            }
        }
        else if (targetPosition.z != nowPosition.z)
        {
            if (targetPosition.z + 1 < nowPosition.z)
            {
                transform.velocity = Vector3.zero;
                isMove = false;
                status.isAction = true;
            }
            else if (targetPosition.z - 1 > nowPosition.z)
            {
                transform.velocity = Vector3.zero;
                isMove = false;
                status.isAction = true;
            }
        }
    }

    //position設定
    public void SetPosition(Vector3 position)
    {
        status.position = new Vector3(status.position.x + position.x, status.position.y + position.y, status.position.z + position.z);
    }

    //向き設定
    public void SetDirection(Vector3 position)
    {
        status.direction = new Vector3(position.x, position.y, position.z);
    }
        
    //向き設定
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
