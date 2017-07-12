using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{

    public float speed;
    public LayerMask CollisionMask;

    public GameObject Owner { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector2 InitialVelocity { get; private set; }

    public void initialize(GameObject owner, Vector2 direction, Vector2 initialvelocity)
    {

        Owner = owner;
        Direction = direction;
        InitialVelocity = initialvelocity;
        OnInitialized();
    }

    protected virtual void OnInitialized()
    { }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
       if((CollisionMask.value & (1 << other.gameObject.layer)) == 0)
        {
            OnNotCollideWith(other);
            return;
        }

        var isOwner = other.gameObject == Owner;
        if(isOwner)
        {
            OnCollidOwner();
            return;
        }

        var takeDamage = (ITake_Damage)other.GetComponent(typeof(ITake_Damage));
        if(takeDamage != null)
        {
            OnCollideTakeDamage(other, takeDamage);
            return;
        }

        OnCollideOther(other);
    }

    protected virtual void OnNotCollideWith(Collider2D other)
    {

    }

    protected virtual void OnCollidOwner()
    {

    }

    protected virtual void OnCollideTakeDamage(Collider2D other, ITake_Damage takeDamage)
    {

    }

    protected virtual void OnCollideOther(Collider2D other)
    {

    }

}


