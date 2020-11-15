using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FitTanks : MonoBehaviour
{
    public List<Transform> targets;
    public float smoothTime = .5f;
    public Vector3 velocity;
    public Vector3 offset;

    public float maxZoom = 10f;
    public float minZoom = 50;

    bool setup = false;

    Camera cam;

    private void Start()
    {
        Invoke("Setup", 1);     
    }

    public void Setup() {
        cam = GetComponent<Camera>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            targets.Add(enemy.transform);
        }
        setup = true;
    }

    public void addTarget(Transform target) {
        targets.Add(target);
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) // this is in case we create a server with no tanks.
            return;
        if (setup)
        {
            Move();
            Zoom();
        }
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPos = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance()/50);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
}
