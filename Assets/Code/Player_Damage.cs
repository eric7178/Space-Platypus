using UnityEngine;
using System.Collections;

public class Player_Damage : MonoBehaviour 
{
    public int damageToGive = 10;

    private Vector3 _LastPos;
    private Vector3 _velocity;

    public void LateUpdate()
    {
        _velocity = (_LastPos - (Vector3)transform.position) / Time.deltaTime;
        _LastPos = transform.position;

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var p = other.GetComponent<Player>();
        if (p == null)
            return;

        p.TakeDamage(damageToGive, gameObject);
        var controller = p.GetComponent<Character_Controller_2D>();
        var totalVelocity = controller.Velocity + _velocity;
        controller.SetForce(new Vector2(//tweek these numbers
            -1 * Mathf.Sign(totalVelocity.x)  * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 5, 10,20),
            -1 * Mathf.Sign(totalVelocity.y ) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 2, 0, 15)));

    }
}
