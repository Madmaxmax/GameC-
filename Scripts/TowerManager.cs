using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    public TowerButton towerButtonPrest{get; set;}
    SpriteRenderer spriteRenderer;

    void Start()
    {   
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))   
        {
            var mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (hit.collider.tag == "TowerBase")
            {
                PlaceTower(hit);
            }
        }
        if (spriteRenderer.enabled) 
        {
            FollowMouse();
        }
    }

    public void FollowMouse() 
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    public void StartDrag(Sprite sprite) 
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void EndDrag() 
    {
        spriteRenderer.enabled = false;
    }

    public void BuyTower(int price)
    {
        Manager.instance.RemoveMoney(price);
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPrest != null)
        {
            var towerPrice = towerButtonPrest.TowerPrice;
            if (towerPrice <= Manager.instance.TotalMoney)
            {
                var newTower = Instantiate(towerButtonPrest.TowerObject);
                var towerPos = hit.transform.InverseTransformPoint(hit.point);
                newTower.transform.position = hit.transform.TransformPoint(towerPos);
                BuyTower(towerPrice);
                EndDrag();
            }
        }
    }

    public void SelectedTower(TowerButton towerSelected) 
    {
        if(towerSelected.TowerPrice <= Manager.instance.TotalMoney)
        {
            towerButtonPrest = towerSelected;
            StartDrag(towerButtonPrest.DragSprite);
        }
        
    }
}