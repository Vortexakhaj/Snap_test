using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchandSnap : MonoBehaviour
{
    [SerializeField] Camera camSecond;
    FPS fpsScript;
    public string _placeholderName;
    public enum TriggerState { none, triggerEnter, triggerStay, tiggerExit };
    public TriggerState triggerState;
    public bool dragging = false;
    public bool placed = false;
    private Vector3 initialPos;
    private bool moveBack = false;


    // Start is called before the first frame update
    void Start()
    {
        triggerState = TriggerState.none;
        fpsScript = GetComponent<FPS>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        triggerState = TriggerState.triggerEnter;
        fpsScript.enabled = false;
        Camera.main.gameObject.SetActive(false);
        camSecond.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        triggerState = TriggerState.triggerStay;
        Debug.Log(other.gameObject.name);
        if (!placed)
        {
            if (other.gameObject.name == _placeholderName && !dragging)
            {
                placed = true;
            }
            if (placed)
                StartCoroutine(SmoothDock(other.gameObject.transform.position));
        }

    }

    private void OnTriggerExit(Collider other)
    {
        triggerState = TriggerState.tiggerExit;
        placed = false;
        fpsScript.enabled = true;
    }

    IEnumerator SmoothDock(Vector3 pos)
    {
        float timeElapsed = 0f;
        float duration = 3f;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(transform.position, pos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
    }
}
