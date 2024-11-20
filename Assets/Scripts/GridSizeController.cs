using UnityEngine;
using UnityEngine.UI;

public class GridSizeController : MonoBehaviour
{
    private GridLayoutGroup grid;
    private readonly float canvasX = 480;
    private float coefficient;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    private void Update()
    {
        coefficient = Screen.height / canvasX;
        var x = ((float)Screen.width / Screen.height) * Screen.height / (4 * coefficient);
        grid.cellSize = new Vector2(x, grid.cellSize.y);
    }
}
