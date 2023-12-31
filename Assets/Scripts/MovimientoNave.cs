using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoNave : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object references")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;


    private Rigidbody2D shipRigidbody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    private void Start()
    {
        // Get a reference to the attached RigidBody2D.
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleShipAcceleration();
            HandleShipRotation();
            HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isAccelerating)
        {
            // Increase velocity upto a maximum.
            shipRigidbody.AddForce(shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipAcceleration()
    {
        // Are we accelerating?
        isAccelerating = Input.GetKey(KeyCode.UpArrow);
    }

    private void HandleShipRotation()
    {
        // Ship rotation.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
        }
    }

    private void HandleShooting()
    {
        // Shooting.
        if (Input.GetKeyDown(KeyCode.Space))
        {

            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // Inherit velicity only in the forward direction of ship.
            Vector2 shipVelocity = shipRigidbody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);

            // Don't want to inherit in the opposite direction, else we'll get stationary bullets.
            if (shipForwardSpeed < 0)
            {
                shipForwardSpeed = 0;
            }

            bullet.velocity = shipDirection * shipForwardSpeed;

            // Add force to propel bullet in direction the player is facing.
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
    }
}
