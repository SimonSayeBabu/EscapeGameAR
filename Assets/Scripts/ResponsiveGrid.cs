using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGrid : MonoBehaviour
{
    public int columns = 4;
    public int rows = 2;
    public Vector2 spacing = new Vector2(10, 10);
    public Vector2 padding = new Vector2(10, 10); // padding horizontal et vertical

    private RectTransform rt;
    private GridLayoutGroup grid;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        ResizeGrid();
    }

    void ResizeGrid()
    {
        float totalSpacingX = spacing.x * (columns - 1) + padding.x * 2;

        float cellSize = (rt.rect.width - totalSpacingX) / columns;

        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = spacing;
        grid.padding = new RectOffset((int)padding.x, (int)padding.x, (int)padding.y, (int)padding.y);
    }
}
