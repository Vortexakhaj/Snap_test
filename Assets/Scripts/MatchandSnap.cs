using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchandSnap : MonoBehaviour
{
    [SerializeField] Camera camSecond;
    FPS fpsScript;
    public GameObject _placeHolder;
    public enum TriggerState { none, triggerEnter, triggerStay, tiggerExit };
    public TriggerState triggerState;
    public bool dragging = false;
    public bool placed = false;
    public Vector3 initialPos;
    public bool moveBack = false;


    // Start is called before the first frame update
    void Start()
    {
        triggerState = TriggerState.none;
        fpsScript = GetComponent<FPS>();
        initialPos = transform.position;
        Invoke("EnablePlaceholder", 1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(initialPos);
        //Debug.Log(!placed && !moveBack);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("5: " + placed);
            transform.position = initialPos;
            moveBack = true;
            placed = false;
            Debug.Log("6: " + placed);
            //fpsScript.enabled = true;
            _placeHolder.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //fpsScript.enabled = false;
        //Camera.main.gameObject.SetActive(false);
        //camSecond.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("1: " + placed);
        if (!placed && !moveBack)
        {
            Debug.Log("2: " + placed);
            if (other.gameObject.name == _placeHolder.name && dragging)
            {
                Debug.Log("3: " + placed);
                placed = true;
                if (placed && !moveBack)
                    StartCoroutine(SmoothDock(other.gameObject.transform.position));
                Debug.Log("4: " + placed);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        placed = false;
        transform.position = initialPos;
        moveBack = true;
        placed = false;
        _placeHolder.SetActive(false);
    }

    IEnumerator SmoothDock(Vector3 pos)
    {
        Debug.Log("1");
        float timeElapsed = 0f;
        float duration = 200f;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(transform.position, pos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
        _placeHolder.SetActive(false);
        CancelInvoke("EnablePlaceholder");
    }

    private void EnablePlaceholder()
    {
        Vector3 pos = transform.position;
        float newDist = SQRT(pos, _placeHolder.transform);
        if (newDist < 10.0f)
        {
            _placeHolder.SetActive(true);
        }
    }

    float SQRT(Vector3 initial, Transform teleport)
    {
        float dist = Vector3.Distance(initial, teleport.transform.position);
        return dist;
    }
}
