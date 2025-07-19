using UnityEngine;

public class SheepDraggedState : ISheepState
{
    public SheepDraggedState(float dragDuration = 4f)
    {
        SheepManager.Instance.SheepDrag.StartDragging(dragDuration);
    }

    public void Enter()
    {
    }

    public void Update()
    {
     
    }

    public void Exit() 
    { 
        SheepStateController.Instance.IsSheepBeingDragged = false;
    }
}
