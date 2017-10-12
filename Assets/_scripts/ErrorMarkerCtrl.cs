using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphAlgos;

namespace BirdRouter
{

    public class ErrorMarkerCtrl : MonoBehaviour
    {

        private RouteMan rman;
        public void setRouteMan(RouteMan rman)
        {
            // this is attached as a component, thus we cannot set it in a contructor
            this.rman = rman;
        }

        public float errmarkIntervalSecs = 2.0f;
        public int nErrmarkIntervalsInSet = 5;

        public int nMarksInList = 0;
        public float markElaped = 0f;

        public enum markingStateE { unused, resting, marking }
        public markingStateE markingState = markingStateE.unused;


        public Vector3 rotvek_deg = Vector3.zero;
        public Vector3 trnvek_met = Vector3.zero;


        LinkedList<optimAnchorPoint> emlist = null;

        Path path;
        public void SetErrorMarkPath(Path path)
        {
            this.path = path;
        }
        // Use this for initialization
        void Start()
        {

        }

        public void startMarking(int n=5)
        {
            emlist = new LinkedList<optimAnchorPoint>();
            nErrmarkIntervalsInSet = n;
            nMarksInList = 0;
            markingState = markingStateE.marking;
            markstopwatch = new System.Diagnostics.Stopwatch();
            markstopwatch.Start();
        }
        public void FinishMarking(int n = 5)
        {
            markingState = markingStateE.resting;
        }

        public bool CalculateOptimalTransformation()
        {
            var opo = new oapOptimizer(optTypeSelectorE.rotYtransXYZ);
            opo.verbosity = oapOptimizer.verbosityE.info;
            opo.addOapList(emlist);
            opo.init();
            opo.optimize();
            rotvek_deg = opo.rotvek * 180 / Mathf.PI;
            trnvek_met = opo.trnvek;

            var plausible = true;
            if (Mathf.Abs(rotvek_deg.y)>10)
            {
                plausible = false;
            } 
            if (Vector3.Magnitude(trnvek_met)>1.0)
            {
                plausible = false;
            }
 
            RouteMan.Log("Opo status:" + opo.status+"  bstval:"+opo.bstval+"  bstidx:"+opo.bstiter+" Plausible:"+plausible);
            RouteMan.Log("Optimized rotvek:" +rotvek_deg.ToString("f3"));
            RouteMan.Log("Optimized trnvek:" +trnvek_met.ToString("f3"));
            Debug.Log("Opo status:" + opo.status + "  bstval:" + opo.bstval + "  bstidx:" + opo.bstiter + " Plausible:" + plausible);
            Debug.Log("Optimized rotvek:" + rotvek_deg.ToString("f3"));
            Debug.Log("Optimized trnvek:" + trnvek_met.ToString("f3"));
            return (plausible);
        }


        System.Diagnostics.Stopwatch markstopwatch = null;

        public void markMark()
        {
            float elap = markstopwatch.ElapsedMilliseconds/1000.0f;
            markElaped = elap;
            if (elap > errmarkIntervalSecs)
            {
                var maincam = Camera.main; // only works with one camera
                var campt = maincam.transform.position;
                campt.y -= rman.home_height;
                //campt.y = 0;
                float pathlen = 0;
                var pathcampt = rman.pathctrl.FindClosestPointOnPath(campt,out pathlen);

                int i = emlist.Count;
                var empt = new optimAnchorPoint("pt-" + i, campt, pathcampt,pathlen);
                emlist.AddLast(empt);
                nMarksInList += 1;
                RefreshGos();
                if (emlist.Count >= nErrmarkIntervalsInSet)
                {
                    markingState = markingStateE.resting;
                    markstopwatch = null;
                }
                else
                {
                    markstopwatch.Reset();
                    markstopwatch.Start();
                }
            }
        }


        GameObject ermarkgo = null;

        public float emsize = 0.2f;

        void CreateGos()
        {
            if (emlist == null) return; // nothing to do
            DeleteGos(); // delete any old ones before we redraw the new ones

            if (ermarkgo == null)
            {
                ermarkgo = new GameObject("ErrMarkers" );
                ermarkgo.transform.parent = rman.rgo.transform;
            }

            int i = 0;
            var scname = "green";
            var tcname = "red";
            foreach ( var em in emlist)
            {
                var sname = "campt" + i;
                var tname = "pathpt" + i;
                var ssph = GraphUtil.CreateMarkerSphere(sname, em.source, size: 1.1f * emsize, clr: scname);
                ssph.transform.parent = ermarkgo.transform;
                var tsph = GraphUtil.CreateMarkerSphere(tname, em.target, size: 1.1f * emsize, clr: tcname);
                tsph.transform.parent = ermarkgo.transform;
                i += 1;
            }

        }
        void DeleteGos()
        {
            if (ermarkgo != null)
            {
                Destroy(ermarkgo);
                ermarkgo = null;
            }
        }
        public void RefreshGos()
        {
            DeleteGos();
            CreateGos();
        }
        // Update is called once per frame
        void Update()
        {
           // Debug.Log("Updating errmark");
            if (markingState == markingStateE.marking)
            {
                markMark();
            }
        }
    }
}