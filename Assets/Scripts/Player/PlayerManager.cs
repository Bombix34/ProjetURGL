using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ObjectManager
{
    [SerializeField]
    protected PlayerSettings settings;
    public Animator Animator { get; private set; }
    private CharacterRenderer characterRenderer;

    protected void Start()
    {
        characterRenderer = GetComponentInChildren<CharacterRenderer>();
        if (!isLocalPlayer)
            return;
        Animator = GetComponent<Animator>();
        ChangeState(new PlayerIdleState(this));
    }

    protected override void Update()
    {
        if (!isLocalPlayer)
            return;
        if (currentState == null)
            return;
        currentState.Execute();
    }

    public Vector2 MovementInput
    {
        get
        {
            if (!isLocalPlayer)
            {
                // exit from update if this is not the local player
                return Vector2.zero;
            }

            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(settings.MovementSpeed * inputX, settings.MovementSpeed * inputY, 0);
            if (inputX < 0)
            {
                characterRenderer.IsRendererFlip = true;
            }
            else if (inputX != 0)
            {
                characterRenderer.IsRendererFlip = false;
            }
            movement *= Time.deltaTime;
            return movement;
        }
    }
}
