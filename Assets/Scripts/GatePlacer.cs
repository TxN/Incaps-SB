using UnityEngine;
using System.Collections;

public class GatePlacer : MonoBehaviour 
{

    public GameObject SpawnWindow;

    bool placingMode = false;
    bool removeMode = false;

    GameObject gate;

    Quaternion rotation = Quaternion.identity;

    void Update()
    {
        if (placingMode)
        {

           
           float rot = Input.GetAxis("Mouse ScrollWheel");
           rotation *= Quaternion.Euler(0, 0, rot * 50f);
            
            RaycastHit hit;
			Ray ray = GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 40f))
            {
                if(hit.transform.gameObject.GetComponent<ElectronicsBoard>() != null)
                {
                    gate.transform.position = hit.point;
                    gate.transform.rotation = Quaternion.LookRotation(hit.normal) * rotation;

                    if (Input.GetMouseButtonDown(0))
                    {
                        gate.transform.parent = hit.transform;
                        gate.GetComponent<Collider>().enabled = true;
                        gate = null;
                        placingMode = false;
                        rotation = Quaternion.identity;
                        GetComponent<Linker>().enabled = true;
                    }
                }

            }
        }
        else if (removeMode)
        {
            
             if (Input.GetMouseButtonDown(0))
             {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 40f))
                    {
                        removeMode = false;
                        if (hit.transform.gameObject.GetComponent<Gate>() != null)
                        {
                            Destroy(hit.transform.gameObject);
                        }
                    }
            }
        }
    }

    public void OpenSpawnWindow()
    {
        SpawnWindow.SetActive(true);
        GetComponent<Linker>().enabled = false;
    }

    public void CloseSpawnWindow()
    {
        SpawnWindow.SetActive(false);
        GetComponent<Linker>().enabled = true;
    }

    public void RemoveMode()
    {
        removeMode = !removeMode;
        if (removeMode)
        {
            GetComponent<Linker>().enabled = false;
        }
        else
        {
            GetComponent<Linker>().enabled = true;
        }
    }

    public void SpawnGate(string gName)
    {
        gate = Resources.Load("Gates/" + gName) as GameObject;
        gate = Instantiate(gate) as GameObject;
        gate.GetComponent<Collider>().enabled = false;

        CloseSpawnWindow();
        placingMode = true;
        GetComponent<Linker>().enabled = false;

    }
	
}
