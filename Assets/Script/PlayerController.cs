﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float m_Speed = 0.2f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    
    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_state;
    private Animator m_animator;
    public int m_max_stamina;
    private int m_stamina;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }

    private void Start()
    {
        // The axes names are based on player number.
        m_MovementAxisName = "Vertical";
        m_TurnAxisName = "Horizontal";
        m_animator = GetComponent<Animator>();
        m_stamina = m_max_stamina;
    }


    private void Update()
    {
        // Store the value of both input axes.

    }
    private void LateUpdate()
    {

        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        if (m_animator.GetBool("Correr")) m_stamina --;
        Debug.Log(m_stamina);

        if (Input.GetButton("Fire1") && (m_stamina > 0))
        {
            if (!(m_MovementInputValue == 0.0f) || !(m_TurnInputValue == 0.0f)) m_animator.SetBool("Correr", true);
        }
        else
        {
            if ((m_MovementInputValue == 0.0f) && (m_TurnInputValue == 0.0f)) m_animator.SetBool("Gatejar", false);
            else m_animator.SetBool("Gatejar", true);
            m_animator.SetBool("Correr", false);
            if (m_stamina < m_max_stamina) m_stamina ++;
        }
        
    }
    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
    }


    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement;
        movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        if (m_animator.GetBool("Correr") && (m_stamina > 1)) movement *= 10;


            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
    
}

