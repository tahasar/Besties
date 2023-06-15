using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;
    public GameObject groundMarker;

    public LayerMask clickable;
    public LayerMask ground;
    
    void Start()
    {
        
        myCam = Camera.main; //<<< MULTIPLAYER'DA SORUN OLURSA BURAYA DİKKAT ET!
    }

    void Update()
    {
        if (UnitSelections.Instance.isMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                    }
                }
                else
                {
                    if(!Input.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelections.Instance.DeselectAll();
                    }
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    groundMarker.transform.position = hit.point;
                    groundMarker.SetActive(false);
                    groundMarker.SetActive(true);
                }
            }
        }
    }
}
