using UnityEngine;
using System.Collections;

public class GatePlacer : MonoBehaviour 
{

    public GameObject SpawnWindow;

    bool placingMode = false;
    bool removeMode = false;

    bool enable = false;
    GameObject gate;

    Quaternion rotation = Quaternion.identity;

    void OnEnable()
    {
        GameState.OnStateChanged += StateChanged;
        GameState.OnCircuitSubstateChanged += SubstateChanged;
    }

    void OnDisable()
    {
        GameState.OnStateChanged -= StateChanged;
        GameState.OnCircuitSubstateChanged -= SubstateChanged;
    }

    void StateChanged()
    {

    }

    void SubstateChanged()
    {
        if (GameState.Instance.CurrentCircuitSubstate != GameState.CircuitEditSubstate.GatePlacing)
        {
           placingMode = false;
        }
        else
        {
            placingMode = true;
        }

        if (GameState.Instance.CurrentCircuitSubstate != GameState.CircuitEditSubstate.Removing)
        {
            removeMode = false;
        }
        else
        {
            removeMode = true;
        }
    }

    void Update()
    {
            if (placingMode)
            {
                if (gate == null)
                {
                    return;
                }

                float rot = Input.GetAxis("Mouse ScrollWheel");
                rotation *= Quaternion.Euler(0, 0, rot * 50f);

                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 40f))
                {
                    if (hit.transform.gameObject.GetComponent<ElectronicsBoard>() != null)
                    {
                        gate.transform.position = hit.point;
                        gate.transform.rotation = Quaternion.LookRotation(hit.normal) * rotation;

                        if (Input.GetMouseButtonDown(0))
                        {
                            gate.transform.parent = hit.transform;
                            gate.GetComponent<Collider>().enabled = true;
                            gate = null;

                            GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.Wiring);

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
                        GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.Wiring);

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
        GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.GatePlacing);
    }

    public void CloseSpawnWindow()
    {
        SpawnWindow.SetActive(false);
        GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.Wiring);
    }

    public void RemoveMode()
    {
        //removeMode = !removeMode;

        if (GameState.Instance.CurrentCircuitSubstate == GameState.CircuitEditSubstate.Removing)
        {
            GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.Wiring);
        }
        else
        {
            GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.Removing);
        }

    }

    public void SpawnGate(string gName)
    {
        gate = Resources.Load("Gates/" + gName) as GameObject;
        gate = Instantiate(gate) as GameObject;
        gate.GetComponent<Collider>().enabled = false;

        SpawnWindow.SetActive(false);
        placingMode = true;
        GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.GatePlacing);
    }
	
}
