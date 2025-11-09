using UnityEngine;

public class DragUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 startpos;
    public GameObject[] slots = new GameObject[9];

    void Start()
    {
        startpos = transform.position;
    }

    public void DragMethod()
    {
        transform.position = Input.mousePosition; // overlay screen accepted
    }

    public void EndDragMethod()
    {
        float dist;
        float mindist = 10000f;
        GameObject goalslot = null;

        foreach (var slot in slots)
        {
            dist = Vector3.Distance(transform.position, slot.transform.position);
            if (dist < mindist)
            {
                mindist = dist;
                goalslot = slot;
            }
        }
        if (mindist < 100f)
            transform.position = goalslot.transform.position;
        else
            transform.position = startpos;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.mousePosition.x + " , " + Input.mousePosition.y);
        //if (isSelected)
        //{

        //}

    }
}
