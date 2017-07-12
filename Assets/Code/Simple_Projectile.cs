using UnityEngine;

public class Simple_Projectile : Projectile , ITake_Damage
{
    public int Damage;
    public GameObject DestroyEffect;
    public int PointValue;
    public float TTL;

    public void Update()
    {
        if ((TTL -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }

        transform.Translate((Direction + new Vector2(InitialVelocity.x, 0)) * speed * Time.deltaTime, Space.World);

    }

    protected override void OnCollideOther(Collider2D other)
    {
        DestroyProjectile();
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITake_Damage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointValue != 0) //change this if you want to award a negative score to a player
        {
            var projectile = instigator.GetComponent<Projectile>();

            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                Game_Manager.Instance.AddPoints(PointValue);
                Floating_Text.Show(string.Format("+{0}!", PointValue), "Points Item", new From_WorldPoint_Text_Positioner(Camera.main, transform.position, 1.5f, 50f));

                DestroyProjectile();
            }

        }
    }

  
    private void DestroyProjectile()
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);


        Destroy(gameObject);
    }
	
}
