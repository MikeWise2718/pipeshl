using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BirdRouter
{
    // To use this class create a 3d object at the root called "NearSphere" and associate this script with it via drag and drop
    //
    public class NearSphere : MonoBehaviour
    {

        public RouteMan rman = null;

        public PathCtrl pathctrl = null;
        public LinkCloudCtrl linkcloudctrl = null;

        // Use this for initialization
        void Start()
        {
            lastpos = transform.position;
        }

        Vector3 lastpos = new Vector3(5, 0, 5);
        // Update is called once per frame
        void Update()
        {
            if (rman != null)
            {
                pathctrl = rman.pathctrl;
                linkcloudctrl = rman.linkcloudctrl;
                if (pathctrl != null)
                {
                    pathctrl.showNearestPathPoint = true;
                    pathctrl.showNearestWegPoints = true;
                    pathctrl.nearestPointRef = lastpos;
                }
                if (linkcloudctrl != null)
                {
                    linkcloudctrl.showNearestPoint = true;
                    linkcloudctrl.nearestPointRef = lastpos;
                }
            }
            var newpos = transform.position;
            var movedist = Vector3.Distance(newpos, lastpos);
            if (movedist>0)
            {
                if (rman != null)
                {
                    rman.Regen();
                }
            }
            lastpos = newpos;
        }
    }
}
