using UnityEngine;
using System.Collections;

public class Simple_Enemy_AI : MonoBehaviour,ITake_Damage, IPlayer_Respawn_Listener
{

	public float Speed;
	public float FireRate = 1;
	public Projectile Bullet;
	public GameObject DestroyedEffect;
	public int MaxHealth;
	public int PointstoGive;

	private Character_Controller_2D _Controller;
	private Vector2 _Direction;
	private Vector2 _StartPosition;
	private float _CanFireIn;
	private int Health;

	public void Start()
	{
		_Controller = GetComponent<Character_Controller_2D> ();
		_Direction = new Vector2 (-1, 0);
		_StartPosition = transform.position;

		Health = MaxHealth;

	}

	public void Update()
	{
		_Controller.SetHorizontalForce (_Direction.x * Speed);

		if ((_Direction.x < 0 && _Controller.State.isCollidingLeft) || (_Direction.x > 0 && _Controller.State.isCollidingRight))
		{
			_Direction = -_Direction;
			transform.localScale = new Vector3 (-transform.localScale.x,transform.localScale.y,transform.localScale.z);

			if ((_CanFireIn - Time.deltaTime) > 0)
				return;

			//add functionality to allow enemy to detect objects above them later!! EU 2/16/17

			var rayCast = Physics2D.Raycast (transform.position, _Direction, 10, 1 << LayerMask.NameToLayer ("Player"));
			if (!rayCast)
				return;

			var projectile = (Projectile)Instantiate (Bullet, transform.position, transform.rotation);
			projectile.initialize (gameObject, _Direction, _Controller.Velocity);
				//end

			_CanFireIn = FireRate;

		}
	}

	public void TakeDamage(int damage, GameObject instigator)
	{
		Health -= damage;

			if (Health <= 0) 
			{
			Instantiate (DestroyedEffect,transform.position, transform.rotation);
				gameObject.SetActive (false);
			}

		if (PointstoGive != 0) 
		{
			var projectile = instigator.GetComponent<Projectile> ();
			Floating_Text.Show(string.Format("+{0}!",PointstoGive), "PointstarText", new From_WorldPoint_Text_Positioner(Camera.main, transform.position, 1.5f, 50));

		}
	}

	public void OnPlayerRespawn(CheckPoint point, Player p)
	{
		_Direction = new Vector2(-1,0);
		transform.localScale = new Vector3(1,1,1);
		transform.position = _StartPosition;
		gameObject.SetActive(true);
	}

}
