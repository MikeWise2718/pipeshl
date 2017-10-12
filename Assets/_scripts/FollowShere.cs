using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BirdRouter;
using GraphAlgos;

public class FollowShere : MonoBehaviour {


    RouteMan rman;
    public void setRouteMan(RouteMan rman)
    {
        // this is attached as a component, thus we cannot set it in a contructor
        this.rman = rman;

        var startpos = transform.position;
        startpos.y = rman.home_height;
        transform.position = startpos;
    }

    GameObject bird = null;
    Vector3 vel = Vector3.zero;
    //Vector3 startpos;
    Vector3 lastbirdpos = Vector3.zero;
    Vector3 birdpos = Vector3.zero;
    public float maxvel = 2.5f;
//    public float decelFak = 0.8f;
  //  public float attractFak = 0.01f;
    public float decelFak = 0.8f;
    public float attractFak = 8.0f;
    public bool followBird = false;
    public float CamClosestPathDist = 0;
    public Vector3 nearPathPosLocal = Vector3.zero;
    public Vector3 nearPathPosWc = Vector3.zero;
    public Vector3 nearSpherePosition = Vector3.zero;
    public int nearPathIdx = 0;
    public float nearPathLambda = 0f;
    public string nearPathWegname = "";

    public bool noiseUpPath = false;
    public float noiseRange = 0.1f;
    public Vector3 noiseBias = Vector3.zero;
    public float noiseBiasResetInterval = 2.0f;
    public float noiseBiasResetElapsed = 0f;

    System.Random ranman = new System.Random();

    // Use this for initialization
    void Start () {

    }
	
    void Follow(float deltat)
    {
        birdpos = bird.transform.position;
        var pdelt = bird.transform.position - transform.position;
        var pdeltbef = pdelt.y;
        pdelt.y = 0; // don't try and adjust the height
        var dist = Vector3.Distance(bird.transform.position, transform.position);
        //Debug.Log("Pdelt:" + pdeltbef);
        //            if (dist < 1.0 && vel.magnitude < 0.3)
        if (dist < 1.0)
        {
            vel = Vector3.zero;
            pdelt = Vector3.zero;
        }
        else if (dist < 2.0)
        {
            vel = vel.normalized * decelFak * decelFak;
            pdelt = pdelt.normalized * decelFak * decelFak;
        }
        else
        {
            vel = vel.normalized * decelFak;
            pdelt = pdelt.normalized * decelFak;
        }
        var bmovedist = Vector3.Distance(lastbirdpos, birdpos);
        if (bmovedist == 0)
        {
            vel = vel.normalized * 0.2f; // put the brakes on
        }

        vel = vel + attractFak * pdelt * Time.deltaTime;
        var moveit = vel * Time.deltaTime;

        if (vel.magnitude > maxvel)
        {
            vel = vel.normalized * maxvel; // clamp at maxvel
        }
        // var dfk = 1 / Time.deltaTime;
        //RouteMan.Log("Found bird vel:"+vel+" speed:"+vel.magnitude + " dt:"+Time.deltaTime+" dist:"+dist);
        var newpos = transform.position + moveit;
        // newpos.y = startpos.y;  // pin the y axis
        if (noiseUpPath)
        {
            var rx = (float)ranman.NextDouble() - 0.5f;
            var ry = (float)ranman.NextDouble() - 0.5f;
            var rz = (float)ranman.NextDouble() - 0.5f;
            var vkoff = new Vector3(rx, ry, rz);
            noiseBiasResetElapsed += deltat;
            if (noiseBiasResetElapsed > noiseBiasResetInterval)
            {
                var rbx = (float)ranman.NextDouble() - 0.5f;
                var rby = (float)ranman.NextDouble() - 0.5f;
                var rbz = (float)ranman.NextDouble() - 0.5f;
                noiseBias = new Vector3(rbx/60, rby/60, rbz/60);
                noiseBiasResetElapsed = 0;
            }
            newpos += noiseBias + noiseRange*vkoff;
        }
        transform.position = newpos;
        var lookpos = bird.transform.position;
        lookpos.y *= 0.9f; // look down a bit so we can see the ground too
        transform.LookAt(lookpos);

        lastbirdpos = birdpos;
    }

    // Update is called once per frame
    void Update()
    {
        if (followBird)
        {
            bird = GameObject.Find("Bird");
            if (bird != null)
            {
                Follow(Time.deltaTime);
            }
        }
        var npsph = GameObject.Find("nearpathsph-go");
        if (npsph != null)
        {
            var pathdst = -1f;
            var nearestPointRef = Camera.main.transform.position;
            rman.pathctrl.FindClosestWegOnPath(nearestPointRef, out pathdst);
            PathPos pp = rman.pathctrl.path.MovePositionAlongPath(pathdst);
            CamClosestPathDist = pathdst;
            if (pp != null)
            {
                nearPathPosLocal = pp.pt;
                var ptwc = rman.transform.TransformPoint(pp.pt); // we don't want the followsphere to scale! 
                                                                 // Thus rman.transform and not rman.rgo.transform
                nearPathPosWc = ptwc;
                nearSpherePosition = npsph.transform.position; 
                nearPathIdx = pp.index;
                nearPathLambda = pp.lambda;
                var lnk = pp.weg.link;
                nearPathWegname = lnk.name;
            }
        }
    }
}
