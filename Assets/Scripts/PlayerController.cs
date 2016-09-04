﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Limit
{
    public float min, max;
}

[System.Serializable]
public struct PlayerWeapons
{
    public PlayerWeapon Bolt;
    public PlayerWeapon Sphere;
    public PlayerWeapon Laser;
}

public class PlayerController : Damageable {

    public float moveSpeed;
    public float tiltFactor;
    public Limit boundaryX;
    public Limit boundaryZ;
    public PlayerWeapons weapons;
    public HealthBar healthCircle;
    public WeaponBar weaponCircle;

    private PlayerWeapon _currentWeapon;

    new void Start()
    {
        // from Damageable
        base.Start();

        // initial properties
        loadWeapon(weapons.Bolt);
    }

    void Update()
    {
        // fire the weapon
        if (Input.GetButton("Fire1"))
            _currentWeapon.fire();

        // handle weapon switching
        handleWeaponSelect();
    }

	void FixedUpdate()
    {
        // handle the movements
        move();
    }

    // add functions to update the UI of health
    public override void applyDamage(float damage)
    {
        // base function
        base.applyDamage(damage);
        
        // update UI of health
        healthCircle.update(_health, maxHealth);
    }

    public void addWeaponExp(int exp)
    {
        // add exp to current weapon
        _currentWeapon.addExperience(exp);

        // update UI
        weaponCircle.update(_currentWeapon);
    }

    // handle the movement of the plane
    private void move()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        float axisHorizontal = Input.GetAxis("Horizontal");
        float axisVertical = Input.GetAxis("Vertical");

        // update velocity
        Vector3 movement = new Vector3(axisHorizontal, 0.0f, axisVertical);
        rigidbody.velocity = moveSpeed * movement;

        // update position
        rigidbody.position = new Vector3
        (
            Mathf.Clamp(rigidbody.position.x, boundaryX.min, boundaryX.max),
            rigidbody.position.y,
            Mathf.Clamp(rigidbody.position.z, boundaryZ.min, boundaryZ.max)
        );

        // update rotation
        rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tiltFactor);
    }

    // handle the buttons to select weapon
    private void handleWeaponSelect()
    {
        if (Input.GetButtonDown("Weapon1"))
            loadWeapon(weapons.Bolt);

        else if (Input.GetButtonDown("Weapon2"))
            loadWeapon(weapons.Bolt);

        else if (Input.GetButtonDown("Weapon3"))
            loadWeapon(weapons.Bolt);
    }

    private void loadWeapon(PlayerWeapon weapon)
    {
        _currentWeapon = weapon;

        // update UI
        weaponCircle.update(weapon);
    }
}
