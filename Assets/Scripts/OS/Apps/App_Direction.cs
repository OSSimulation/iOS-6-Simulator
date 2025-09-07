using UnityEngine;

public class App_Direction : MonoBehaviour
{
    public int newIndex { get; private set; }

    [SerializeField] private Direction Direction;
    public Direction direction => Direction;

    private void Update()
    {
        if (direction == Direction.LEFT)
        {
            if (transform.parent.GetSiblingIndex() != 0)
            {
                newIndex = transform.parent.GetSiblingIndex();
            }
            else
            {
                newIndex = 0;
            }
        }
        else if (direction == Direction.RIGHT)
        {
            newIndex = transform.parent.GetSiblingIndex() + 1;
        }
        else if (direction == Direction.BG)
        {
            newIndex = transform.childCount;
        }
    }
}

public enum Direction
{
    LEFT,
    RIGHT,
    BG
}