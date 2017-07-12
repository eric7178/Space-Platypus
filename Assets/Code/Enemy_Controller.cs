using UnityEngine;
using System.Collections;
using Spine;
using System;
using Spine.Unity;

public class Enemy_Controller : MonoBehaviour 
{
    SkeletonAnimation PlayerAnim;
    public bool isSprite;

    private bool _isFacingRight;
    private Character_Controller_2D _Controller;
    private float _normalizedHorizontalSpeed;

    public float maxSpeed = 10f;
    public float speedAccelerationGround = 10f;
    public float speedAccelerationAir = 5f;

    public float speedAccelerationWater = 3f;
    public float speedAccelerationSpace = 10f;
    public float maxhealth = 100;
    public GameObject DamageEffect;


    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    public void Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        Debug.Log(trackIndex + " " + state.GetCurrent(trackIndex) + ": event " + e + ", " + e.Int);
    }

    public void Start()
    {
        PlayerAnim = GetComponent<SkeletonAnimation>();
    }

    public void Awake()
    {
        _Controller = GetComponent<Character_Controller_2D>();
        _isFacingRight = transform.localScale.x > 0;

        Health = maxhealth;

       
    }

    public void Update()
    {
        if (!IsDead)
            HandleInput();

        PlayerAnim.state.SetAnimation(0, "FunnyIdle", true);
       
        var movementFactor = _Controller.State.isGrounded ? speedAccelerationGround : speedAccelerationAir;
        
        if (IsDead)
            _Controller.SetHorizontalForce(0);
        else
        //    _Controller.SetHorizontalForce(Mathf.Lerp(_Controller.Velocity.x, movementFactor * maxSpeed, Time.deltaTime * movementFactor));
            _Controller.SetHorizontalForce(Mathf.Lerp(_Controller.Velocity.x, _normalizedHorizontalSpeed * maxSpeed, Time.deltaTime * movementFactor));

    }

    public void Kill()
    {
        //this is if you want the player to fall through the world when he dies
        _Controller.HandleCollisions = false;
        GetComponent<Collider>().enabled = false;

        IsDead = true;

        _Controller.SetForce(new Vector2(0,20));

        Health = 0;

    }

    public void RespawnAt(Transform SpawnPoint)
    {
        if (!_isFacingRight)
            Flip();

        IsDead = false;
        GetComponent<Collider>().enabled = true;
        _Controller.HandleCollisions = true;

        transform.position = SpawnPoint.position;
        Health = maxhealth;

    }

    public void TakeDamage(float Damage)
    {
        Floating_Text.Show(string.Format("-{0}!", Damage), "DamageText", new From_WorldPoint_Text_Positioner(Camera.main, transform.position, 25f, 60f));

        Instantiate(DamageEffect, transform.position, transform.rotation);
        Health -= Damage;
        if (Health <= 0)
            Level_Manager.Instance.KillPlayer();

    }


    private void HandleInput()
    {
       
        if(Input.GetKey(KeyCode.D))
        {
           
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
            
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
            
            
        }

        if (_Controller.canJump && Input.GetKeyDown(KeyCode.Space))
        {
            //_Controller.Jump();//UPDATE THIS FUNCTION
            PlayerAnim.state.AddAnimation(0, "Jump", false, 2);
            
        }
        

    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }



	
}


