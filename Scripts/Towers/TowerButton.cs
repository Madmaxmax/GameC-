using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public GameObject TowerObject { get; private set; }
    public Sprite DragSprite { get; private set; }
    public int TowerPrice { get; private set; }

    [SerializeField] private GameObject towerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int towerPrice;

    private void Awake()
    {
        TowerObject = towerObject;
        DragSprite = dragSprite;
        TowerPrice = towerPrice;
    }
}
