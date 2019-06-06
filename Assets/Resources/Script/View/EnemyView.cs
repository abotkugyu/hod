using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour {
	
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    Animator animator;
    [SerializeField]
    public Rigidbody trans;
    
    public void Initialize()
    {
    }
    
    public void Run(bool is_run)
    {
    }
    
    public void Attack()
    {
    }

    public void SetPosition(int x, int z)
    {
    }
    
    public void SetAngles(Vector3 angles)
    {
        enemy.transform.eulerAngles = angles;
    }
    public Rigidbody GetTransForm()
    {
        return trans;
    }
}
