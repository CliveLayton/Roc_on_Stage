using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float despawnTime = 5;
    //[SerializeField] private int damage = 1;

    public MoverBase shooter;

    private Vector2 direction;

    /// <summary>
    /// sets the shooter to the new shooter
    /// sets the direction of the bullet to the targetedPosition subtract with the position of the shooter
    /// Ignore the collision with the shooter
    /// adding force to the bullet to move it
    /// starts coroutine to despawn the bullet after few seconds
    /// </summary>
    /// <param name="shooter">Script MoverBase</param>
    /// <param name="targetedPosition">Vector2</param>
    public void Shoot(MoverBase shooter, Vector2 targetedPosition)
    {
        this.shooter = shooter;

        direction = targetedPosition - shooter.GetPosition();
        direction.Normalize();
        
        Physics.IgnoreCollision(shooter.GetComponent<Collider>(), GetComponent<Collider>());
        
        GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }

    /// <summary>
    /// ignores the collision of the bullet with the same shooter
    /// </summary>
    /// <param name="other">Collision</param>
    private void OnCollisionEnter(Collision other)
    {
        Bullet otherBullet = other.gameObject.GetComponent<Bullet>();
        if (otherBullet != null && otherBullet.shooter == shooter)
        {
            Physics.IgnoreCollision(shooter.GetComponent<Collider>(), other.collider);
            return;
        }
    }

    /// <summary>
    /// waits for the despawntime and then destroys the gameobject
    /// </summary>
    /// <returns>waits for the despawntime in seconds</returns>
    private IEnumerator DespawnAfterTimeCoroutine()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
