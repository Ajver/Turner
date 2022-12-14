using System;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public event EventHandler<GridOccupant> OnAnyOccupantMovedGridPosition;
    public event EventHandler OnAnyOccupantClears;
    
    [SerializeField] private GridDebugObject debugObjectPrefab; 
    
    public static LevelGrid Instance { get; private set; }
    
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    
    private GridSystem<GridObject> gridSystem;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one LevelGrid in the scene!");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (position) => new GridObject(position));
    }

    private void Start()
    {
        PathFinding.Instance.Setup(width, height, cellSize);

        if (Testing.IsTestingEnvironment())
        {
            gridSystem.CreateDebugObjects(debugObjectPrefab);
        }
    }

    public void OccupantMovedGridPosition(GridOccupant occupant, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        if (GetOccupantAtGridPosition(fromGridPosition) == occupant)
        {
            ClearOccupantAtGridPosition(fromGridPosition);
        }

        if (!HasOccupantAtGridPosition(toGridPosition))
        {
            SetOccupantAtGridPosition(toGridPosition, occupant);
        }

        OnAnyOccupantMovedGridPosition?.Invoke(this, occupant);
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetInteractable();
    }

    public bool HasInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetInteractable() != null;
    }
    
    public bool HasDamageableOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetDamageable() != null;
    }
    
    public IDamageable GetDamageableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetDamageable();
    }
    
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition)  => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.HasUnit();
    }

    public void SetOccupantAtGridPosition(GridPosition gridPosition, GridOccupant occupant)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        obj.SetOccupant(occupant);
    }

    public void ClearOccupantAtGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        obj.ClearOccupant();
        
        OnAnyOccupantClears?.Invoke(this, EventArgs.Empty);
    }
    
    public GridOccupant GetOccupantAtGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.GetOccupant();
    }

    public bool HasOccupantAtGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.HasOccupant();
    }
    
    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

}
