using System;
using UnityEngine;

public class PathAI : SimpleAI{
    public Waypoint[] waypoints;

    /* // Start is called before the first frame update
    void Start(){
        
    } */

    // Update is called once per frame
    public void Update(){
        
    }

    public override void NextPath() {
        throw new System.NotImplementedException();
    }

    [Serializable]
    public class Waypoint {
        public Vector3 targetLocation { get { return location; } set { location = value; } }
        public Vector3 location = new Vector3(1f, 0f, 0f);
        public int rotation, duration;
        public bool visible = true;
    }
}
