using System;
using System.Collections;
using UnityEngine;


public class DashCommand : Command
    {
    public float dashForce;
    public float dashLength;
    public bool canDashAgain;
    
    private InputMoveCommand thisInputMoveCommand;
    private CharacterInput thisCharacterInput;
    private Transform thisTransform;
    //private Animator _animator;
    //private static readonly int Dashing = Animator.StringToHash("Dashing");

    public void Awake()
    {
        thisInputMoveCommand = GetComponent<InputMoveCommand>();
        thisTransform = GetComponent<Transform>();
        thisCharacterInput = GetComponent<CharacterInput>();
        canDashAgain = true;
    }
    public override void Execute()
    {
        //start courotine where we dash
        if (canDashAgain == false)
        {
            return;
        }
        else
        {
            StartCoroutine(Dash());

        }
        //_animator.SetBool(Dashing, true);
        
        //_animator.SetBool(Dashing, true);
        //Invoke(nameof(SetAnimFalse), dashLength);
    }

    IEnumerator Dash()
    {
        thisCharacterInput.thisDashing = true;
        canDashAgain = false;
        Debug.Log("character is trying to dasg");
        thisInputMoveCommand.AddForce(thisTransform.forward, dashForce);
        yield return new WaitForSeconds(dashLength);
        thisInputMoveCommand.ResetImpact();
        thisCharacterInput.thisDashing = false;
        yield return new WaitForSeconds(dashLength);
        canDashAgain = true;
    }


        //private void SetAnimFalse()
        //{
        //_animator.SetBool(Dashing, false);
        //}
}

