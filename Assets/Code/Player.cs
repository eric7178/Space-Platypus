using UnityEngine;
using System.Collections;
using Spine.Unity;



public class Player : MonoBehaviour , ITake_Damage
{

   

    private bool _isFacingRight;
    private Character_Controller_2D _Controller;
    private float _normalizedHorizontalSpeed;

    public float maxSpeed = 10f;
    public float speedAccelerationGround = 10f;
    public float speedAccelerationAir = 5f;

    public float speedAccelerationWater = 3f;
    public float speedAccelerationSpace = 10f;
    public int maxhealth = 100;
    public GameObject DamageEffect;


    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    public SkeletonAnimation PlayerAnim;

    public string CurrentAnim = "";

    public float _timeHeld = 0.0f;
    public float _timeForFullJump = 2.0f;
    public float _minJumpForce = 10f;
    public float _maxJumpForce = 200f;
    



    public void Awake()
    {
        
        _Controller = GetComponent<Character_Controller_2D>();
        _isFacingRight = transform.localScale.x > 0;

        Health = maxhealth;

        PlayerAnim = GetComponent<SkeletonAnimation>();
       

    }

    public void SetAnimation(string name, bool loop)
    {
        if (name == CurrentAnim && name != "Jump")
            return;

        PlayerAnim.state.SetAnimation(0, name, loop);
        CurrentAnim = name;
        Debug.Log(name);

    }


    public void Update()
    {
        if (!IsDead)
        HandleInput();

        

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

	public void TakeDamage(int Damage, GameObject instigator)
    {
        Floating_Text.Show(string.Format("-{0}!", Damage), "DamageText", new From_WorldPoint_Text_Positioner(Camera.main, transform.position, 25f, 60f));

        Instantiate(DamageEffect, transform.position, transform.rotation);
        Health -= Damage;
        if (Health <= 0)
            Level_Manager.Instance.KillPlayer();

    }


    private void HandleInput()
    {

        if (Input.GetKey(KeyCode.D))
        {

            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
            SetAnimation("Walk", true);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
            SetAnimation("Walk", true);
        }
        else
        {
            _normalizedHorizontalSpeed = 0;

        }

        if (/*_Controller.canJump &&*/ Input.GetKey(KeyCode.Space))
        {
            _Controller.Jump();
            SetAnimation("Jump", false);

        }
        if()
        {

        }
    }

       
       

    

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }



	
}
