﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ObjectManager
{
    public bool IsVigil = false;
    [SerializeField]
    protected PlayerSettings settings;
    public CharacterRenderer Renderer{ get; private set; }
    public Animator Animator { get; private set; }

    protected void Start()
    {
        Renderer = GetComponentInChildren<CharacterRenderer>();
        if (!hasAuthority)
            return;
        Animator = GetComponent<Animator>();
        Camera.main.GetComponent<CameraManager>().StartCameraFollow(this.transform);
        ChangeState(new PlayerIdleState(this));
    }

    protected override void Update()
    {
        if (!hasAuthority)
            return;
        if (currentState == null)
            return;
        currentState.Execute();
    }

    public void Init()
    {
    }

    public Vector2 MovementInput
    {
        get
        {
            if (!hasAuthority)
            {
                // exit from update if this is not the local player
                return Vector2.zero;
            }
            Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            inputVector *= settings.MovementSpeed;
            if (inputVector.x < 0)
            {
                Renderer.IsRendererFlip = true;
            }
            else if (inputVector.x != 0)
            {
                Renderer.IsRendererFlip = false;
            }
            inputVector *= Time.deltaTime;
            return inputVector;
        }
    }
}
