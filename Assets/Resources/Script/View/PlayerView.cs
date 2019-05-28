using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour {
	
    [SerializeField]
    public GameObject player;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody trans;
    
    public void Initialize()
    {
	    animator = player.GetComponent<Animator>();
	    animator.Play("Wait");
    }
    
    public void Run(bool is_run)
    {
	    animator.SetBool("is_run", is_run);
    }
    
    public void Attack()
    {
	    GetComponent<Animator>().SetTrigger("attack");
    }

    public void SetPosition(int x, int z)
    {
	    
    }
    
    public void SetAngles(Vector3 angles)
    {
	    player.transform.eulerAngles = angles;
    }
    public Rigidbody GetTransForm()
    {
	    return trans;
    }
}

