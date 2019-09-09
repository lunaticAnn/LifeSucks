using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TimelineElement
{
    GameObject _object; // ref to the gameobject that present the timeline element
    float appearTime;
    float movingSpd;

    public TimelineElement(float at, float ms, GameObject obj)
    {
        appearTime = at;
        movingSpd = ms;
        _object = obj;
    }

    // return orphaned
    public bool process(Vector3 tStart, Vector3 tEnd, float timer, float inputTime)
    {
        float t = (timer - appearTime) / movingSpd;
        float tInput = (inputTime - appearTime) / movingSpd;
        if (Mathf.Abs(tInput - 1) < 0.1f)
        {
            // validate by input
            Debug.Log("hit");
            GameObject.Destroy(_object);
            return true;
        }
        if (t > 1.05f)
        {
            // orphaned as past due
            GameObject.Destroy(_object);
            return true;
        }
        _object.transform.position = Vector3.Lerp(tStart, tEnd, t);
        return false;
    }
}

public class Timeline : MonoBehaviour
{
    const float DEFAULT_SPEED = 1;

    public GameObject timeLineElementPrefab;

    public Transform timeLineStart;
    public Transform timeLineEnd;

    public float movingSpd; // timelapse from start to end
    public float[] timePoints;

    private float timer;
    private int currentIndex = 0;
    private float validateInputForTimePoint = -1;
    private Queue<TimelineElement> processQueue = new Queue<TimelineElement>();

    private void createTimelineElementAt(float timePoint)
    {
        GameObject element = Instantiate(timeLineElementPrefab, timeLineStart.position, Quaternion.identity, transform);
        processQueue.Enqueue(new TimelineElement(timePoint, movingSpd > 0.2 ? movingSpd : DEFAULT_SPEED, element));
    }

    private void process()
    {
        int cnt = processQueue.Count;
        for (int i = 0; i < cnt; ++i)
        {
            TimelineElement element = processQueue.Dequeue();
            if (!element.process(timeLineStart.position, timeLineEnd.position, timer, validateInputForTimePoint))
            {
                processQueue.Enqueue(element);
            }
        }

        // create new elements
        while (currentIndex < timePoints.Length && timer - timePoints[currentIndex] > 0)
        {
            createTimelineElementAt(timer);
            currentIndex++;
        }
    }

    private void Reset()
    {
        timer = 0;
        validateInputForTimePoint = -1;
        currentIndex = 0;
        processQueue.Clear();
    }

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        process();

        // currently assigned for testing, will switch to inputhandler
        if (Input.GetKeyDown(KeyCode.Space))
        {
            validateInputForTimePoint = timer;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }
}
