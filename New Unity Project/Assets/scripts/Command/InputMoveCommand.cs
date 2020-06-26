using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


public class InputMoveCommand : Command
{
    public float turnSmoothing;
    public float speed;

    private CharacterController thisCharacterController;
    private IMoveInput thisMove;
    private Coroutine thisRotateCoroutine;
    public float thisRotateSpeed;
    public CharacterInput thisCharacterInput;

    //gravity stuff
    public float thisMass;
    public float gravity = Physics.gravity.y;
    public float thisVelocityY;
    public float thisDamping;

    //impact handlers
    public Vector3 thisCurrentImpact;
    public Vector3 velocity;


    private void Awake()
    {
        thisCharacterController = GetComponent<CharacterController>();
        thisMove = GetComponent<IMoveInput>();
        thisCharacterInput = GetComponent<CharacterInput>();
    }

    public override void Execute()
    {

        if (thisRotateCoroutine == null)
            thisRotateCoroutine = StartCoroutine(Rotate());
        
    }

    void Update()
    {
        move();

    }
    private void move()
    {
        //gravity handling

        if (thisCharacterController.isGrounded && thisVelocityY < 0f)
        {
            thisVelocityY = 0f;
        }
        else
        {
            thisVelocityY += gravity * Time.deltaTime * thisMass;
        }
        //moving using the curve
        if (thisCharacterInput.thisDashing != true)
        {
            velocity = speed * thisMove.MoveDirection + Vector3.up * thisVelocityY;
        }
        else
        {
            velocity.y = 0;
        }
        if (thisCurrentImpact.magnitude > 0.2)
        {
            velocity += thisCurrentImpact;
        }

        thisCharacterController.Move(velocity * Time.deltaTime);
        thisCurrentImpact = Vector3.Lerp(thisCurrentImpact, Vector3.zero, thisDamping * Time.deltaTime);
    }
    private IEnumerator Rotate()
        {
        //rotation with coroutine for smooth turn
        while (thisMove.MoveDirection.z != 0 || thisMove.MoveDirection.x != 0)
        {
                Vector3 movement = new Vector3(thisMove.MoveDirection.x, 0.0f, thisMove.MoveDirection.z);
                thisCharacterController.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), thisRotateSpeed * Time.deltaTime);

        yield return null;
        }

        thisRotateCoroutine = null;
    }
    public void AddForce(Vector3 direction, float magnitude)
    {
        thisCurrentImpact += direction.normalized * magnitude / thisMass;
    }
    public void ResetImpact()
    {
        thisCurrentImpact = Vector3.zero;
    }
}
