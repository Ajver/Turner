using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private enum State
    {
        LookingInto,
        Interacting,
    }

    private State state;
    private float stateTimer;

    private IInteractable interactable;
    
    private int interactDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.LookingInto:
                SlowlyLookAt(interactable.GetTransform().position);
                break;
            case State.Interacting:
                break;
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.LookingInto:
                state = State.Interacting;
                interactable.Interact(OnInteractComplete);

                // Don't need any state timer - the interaction time is controlled by the Interactable object 
                isActive = false;
                break;
            case State.Interacting:
                break;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        
        state = State.LookingInto;
        float lookingIntoTime = 1f;
        stateTimer = lookingIntoTime;
        
        ActionStart(callback);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition gridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        for (int x = -interactDistance; x <= interactDistance; x++)
        {
            for (int z = -interactDistance; z <= interactDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasInteractableAtGridPosition(testPos))
                {
                    continue;
                }

                validPosList.Add(testPos);
            }
        }
        
        return validPosList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
