using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour {
	
    [SerializeField]
    public GameObject model;
    [SerializeField]
    Animator animator;
    [SerializeField]
    public Rigidbody trans;
    [SerializeField]
    public GameObject hud;
    [SerializeField]
    public GameObject _arm;
    [SerializeField]
    public GameObject _weapon;
    
    public void Initialize()
    {
	    animator = model.GetComponent<Animator>();
    }
    
    public void Wait()
    {
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
    public void Death()
    {
	    GameObject effect = Resources.Load("Object/Effect/Death") as GameObject;	    
	    GameObject obj = Object.Instantiate(effect, model.transform.position, Quaternion.identity) as GameObject;
	    obj.layer = 9;
	    var particle = effect.GetComponent<ParticleSystem>();
	    particle.Play();
	    Destroy(obj, 2);
    }    
    public void Equip(string modelName)
    {
	    // 一度持っているものを削除
	    UnEquip();
	    GameObject arm = Resources.Load("Object/Item/001") as GameObject;	    
	    _weapon = Object.Instantiate(arm, _arm.transform.position, _arm.transform.rotation) as GameObject;
	    _weapon.transform.parent = _arm.transform;
    }
    
    public void UnEquip()
    {
	    if (_weapon != null)
	    {
		    Destroy(_weapon);
		    _weapon = null;
	    }
    }
    
    public void SetPosition(Vector3 position)
    {
	    model.transform.position = position;
    }
    
    public void SetAngles(Vector3 angles)
    {
	    model.transform.eulerAngles = angles;
    }
    public Rigidbody GetTransForm()
    {
	    return trans;
    }
        
    public void UpdateHud(int per)
    {
	    HudView hudView = hud.GetComponent<HudView>();
	    hudView.updateHealth(per);
    }
}

