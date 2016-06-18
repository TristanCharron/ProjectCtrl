using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFindingManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest { get { return pathRequestQueue.Peek(); } }
    static PathFindingManager instance;
    Pathfinding pathFinding;
    bool isPathProcessing;

    public static void onRequestPath(Vector3 start, Vector3 end, Action<Vector3,bool> callback)
    {
        PathRequest newRequest = new PathRequest(start, end, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.onCreatingPathProcess(newRequest);
    }

    void onCreatingPathProcess(PathRequest request)
    {
        if(!isPathProcessing && pathRequestQueue.Count > 0)
        {
            pathRequestQueue.Dequeue();
            isPathProcessing = true;
            pathFinding.onFindPath(request.start, request.end);
        }
    }
  
    struct PathRequest
    {
        public Vector3 start, end;
        public Action<Vector3, bool> callback;
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3, bool> _callback)
        {
            start = _start;
            end = _end;
            callback = _callback;
        }
    }

    void Awake()
    {
        instance = this;
        pathFinding = GetComponent<Pathfinding>();
        isPathProcessing = false;
    }
}
