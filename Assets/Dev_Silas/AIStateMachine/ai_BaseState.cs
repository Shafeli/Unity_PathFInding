using UnityEngine;


// Example Concrete State
/*

public class ai_Example : AiBaseState
{
    private string _leavingStateName = // Name of the state to exit

    Vector2 _vFacingDirection = Vector2.right;

    public AiExampleConfig _exampleConfig = null; // Reference to the scriptableObject asset

    public override void EnterState(AiStateManager aiStateManager)
    {
        // On Enter init values that are the life-cycle of the state
        
        // One way to use the Config system there is also i guess options for getting a few configs and switching them out before
        // changing state. like kinda of a boot leg Hierarchical state machine 

        // Cast to the Config Type before requesting
        _exampleConfig = (AiExampleConfig)aiStateManager.StateMachineMetaData.GetConfigByAssociatedStateName(ToString());

        // If Config is found set it
        if (_exampleConfig)
        {
            // Based on the config
        }
    }

    public override void UpdateState(AiStateManager aiStateManager)
    {

        Vector2 aiMovement = _vFacingDirection;

        // Translate the AI GameObject
        Move(aiStateManager, _vFacingDirection);

        // if ( thing > that )
            {
                // Leave state 
                aiStateManager.SwitchState(LeavingStateName);
            }
    }

    public override void OnEnter(AiStateManager aiStateManager, Collision collision)
    {
        // Stuff that needs to happen on the start of collision
    }

    public override void OnExit(AiStateManager aiStateManager, Collision collision)
    {
        // Stuff that needs to happen on the end of collision
    }

    public override void OnOverlap(AiStateManager aiStateManager, Collision collision)
    {
        // Stuff that needs to happen on the continuation of collision
    }
}

*/
public abstract class AiBaseState
{

    // Called once on switching of states
    public abstract void EnterState(AiStateManager aiStateManager);

    // Called once a frame
    public abstract void UpdateState(AiStateManager aiStateManager);

    // Called once on collision Enter
    public abstract void OnEnter(AiStateManager aiStateManager, Collision2D collision);

    // Called once on collision Exit
    public abstract void OnExit(AiStateManager aiStateManager, Collision2D collision);

    // Called once a frame until Collision is no longer active
    public abstract void OnOverlap(AiStateManager aiStateManager, Collision2D collision);

    public override string ToString()
    {
        return GetType().Name; // This returns the name of the concrete class.
    }

    protected void Move(AiStateManager aiStateManager, Vector2 direction)
    {
        aiStateManager.gameObject.transform.Translate(direction);
    }

    public virtual void OnTriggerEnter(AiStateManager aiStateManager, Collider2D other){ } // optional
}