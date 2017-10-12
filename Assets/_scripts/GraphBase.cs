using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// GraphAlgos.cs  - This file contains Graph algorithems that we need. It is fairly self-contained. 
/// </summary>

namespace GraphAlgos
{

    public class StatusMsg
    {
        public bool ok;
        public string status;
        public StatusMsg(bool ok,string status)
        {
            this.ok = ok;
            this.status = status;
        }
        public string fullstatus()
        {
            var msg = "ok:" + ok + " " + status;
            return msg;
        }
    }
    public class Range
    {
        public float min;
        public float max;

        public Range(float min = 0, float max = 0)
        {
            this.min = min;
            this.max = max;
        }
        public float clamp(float val)
        {
            return (System.Math.Min(max, System.Math.Max(min, val)));
        }
    }

    #region basics - Weg,LcNode,LcLink
  
    public class LcNode
    {
        public LinkCloud lc;
        public string name { get; set; }
        public Vector3 pt { get; set; }
        public Transform transform { get; set; }
        public int transformSetCount = 0;
        public HashSet<Weg> wegtos { get; set; }

        public LcNode cameFrom { get; set; }
        public float fScore { get; set; }
        public float gScore { get; set; }

        public GameObject go { get; set; }

        //private static int newnamenumber = 0;
        //public static string genuniquename()
        //{
        //    var newname = "uniqnodename-" + newnamenumber;
        //    newnamenumber++;
        //    return newname;
        //}


        public LcNode(LinkCloud lc,string name, Vector3 v)
        {
            this.lc = lc;
            this.name = name;
            this.pt = v;
            wegtos = null;
            astarInit();
        }
        public void resetWegs()
        {
            wegtos = null;
        }
        public float heuristic_cost_estimate(LcNode topt)
        {
            var f = Vector3.Distance(pt, topt.pt);
            return (f);
        }
        public void AddWeg(Weg weg)
        {
            if (wegtos == null)
            {
                wegtos = new HashSet<Weg>();
            }
            wegtos.Add(weg);
        }
        public void DelWeg(Weg weg)
        {
            if (wegtos == null)
            {
                wegtos = new HashSet<Weg>();
            }
            wegtos.Remove(weg);
        }
        public void astarInit()
        {
            cameFrom = null;
            gScore = 9e30f;
            fScore = 9e30f;
        }
        public bool wasMoved()
        {
            var rv = false;
            if (go != null)
            {
                var gpt = go.transform.position;
                if (gpt.x != pt.x || gpt.y != pt.y || gpt.z != pt.z)
                {
                    GraphUtil.Log(name + " moved from " + pt + " to " + gpt);
                    rv = true;
                }
            }
            return rv;
        }
        public void syncToGo()
        {
            if (go != null)
            {
                var gpt = go.transform.position;
                pt = gpt;
            }
        }
    }
    public class LcLink
    {
        public LinkCloud lc;
        public string name { get; set; }
        public LcNode node1 { get; set; }
        public LcNode node2 { get; set; }
        public float len { get; set; }
        public Weg w1 { get; set; }
        public Weg w2 { get; set; }

        //private static int newnamenumber = 0;
        //public static string genuniquename()
        //{
        //    var newname = "uniqlinkname-" + newnamenumber;
        //    newnamenumber++;
        //    return newname;
        //}
        public LcLink(LinkCloud lc,string name, LcNode node1, LcNode node2)
        {
            this.lc = lc;
            this.name = name;
            this.node1 = node1;
            this.node2 = node2;
            len = Vector3.Distance(node1.pt, node2.pt);
            linkLink();
        }
        public void linkLink()
        {
            w1 = new GraphAlgos.Weg(this, this.node2, this.node1);
            w2 = new GraphAlgos.Weg(this, this.node1, this.node2);
            node1.AddWeg(w1);
            node2.AddWeg(w2);
        }
        public void UnlinkLink()
        {
            node1.DelWeg(w1);
            node2.DelWeg(w2);
        }
        public Vector3 LambPt(float l)
        {
            var p1wc = node1.pt; // we use this to build our grids before the transforms have been defined
            var p2wc = node2.pt;
            if (node1.transform != null)
            {
                p1wc = node1.transform.TransformPoint(node1.pt);
                p2wc = node2.transform.TransformPoint(node2.pt);
            }
            //var rpt = (lp2.pt-lp1.pt)*l + lp1.pt;
            var rpt = (p2wc - p1wc) * l + p1wc;
            return (rpt);
        }
        public float FindClosestLamb(Vector3 pt)
        {
            var p1wc = node1.pt;
            var p2wc = node2.pt;
            if (node1.transform != null)
            {
                p1wc = node1.transform.TransformPoint(node1.pt);
                p2wc = node2.transform.TransformPoint(node2.pt);
            }
            return (GraphUtil.FindClosestLambClampedTo01(pt, p1wc, p2wc));
        }
        public float FindClosestLambBruteForce(Vector3 pt)
        {
            float mindist = 9e30f;
            float minlamb = 0;
            var nres = 16;
            float nresf = nres;
            Vector3 dlt = (node2.pt - node1.pt);
            for (int i = 0; i <= nres; i++)
            {
                float lamb = i / nresf;
                var lpt = dlt * lamb + node1.pt;
                float dist = Vector3.Distance(pt, lpt);
                if (dist < mindist)
                {
                    mindist = dist;
                    minlamb = lamb;
                }
            }
            return (minlamb);
        }
        public Vector3 FindClosestPointOnLink(Vector3 pt)
        {
            var l = FindClosestLamb(pt);
            //       GraphUtil.Log(" minlamb:" + l);
            return (LambPt(l));
        }
    }
    public class Weg
    {

        public LcLink link { get; set; }
        public LcNode frNode { get; set; }
        public LcNode toNode { get; set; }
        public float distance { get; set; }
        public Weg(LcLink link, LcNode tonode, LcNode frnode)
        {
            this.link = link;
            this.toNode = tonode;
            this.frNode = frnode;
            this.distance = Vector3.Distance(frnode.pt, tonode.pt);
        }
        public Vector3 GetWegDirection(bool normalized = true)
        {
            var p1 = frNode.pt;
            var p2 = toNode.pt;
            var wdir = p2 - p1;
            if (normalized)
            {
                wdir = Vector3.Normalize(wdir);
            }
            return (wdir);
        }
        public Vector3 LambPt(float lamb)
        {
            var dlt = toNode.pt - frNode.pt;
            var rv = lamb * dlt + frNode.pt;
            return (rv);
        }
    }
    public class PathPos
    {
        public Path path { get; set; }
        public Vector3 pt { get; set; }
        public Weg weg { get; set; }
        public int index { get; set; }
        public float lambda { get; set; }
        public bool onpath
        {
            get
            {
                return (0 <= lambda && lambda <= 1);
            }
        }
        public PathPos(Path path, Vector3 pt, Weg weg, int index, float lambda)
        {
            this.path = path;
            this.pt = pt;
            this.weg = weg;
            this.index = index;
            this.lambda = lambda;
        }
    }
    public class Path
    {
        public Vector3 startpt = Vector3.zero;
        public Vector3 endpt = Vector3.zero;
        public Vector3 weg1Pt1 = Vector3.zero;
        public Vector3 weg1Pt2 = Vector3.zero;
        public Vector3 weg1FrPt = Vector3.zero;
        public Vector3 weg1ToPt = Vector3.zero;
        public LcNode start { get; set; }
        public LcNode end { get; set; }
        public List<Weg> waypts = new List<Weg>();
        public PathPos pathPos = null;
        public Path(LcNode start)
        {
            this.start = start;
            startpt = start.pt;
        }
        public void AddWaypt(Weg w)
        {
            // should control to see if additions are consistent
            waypts.Add(w);
            if (waypts.Count == 1)
            {
                weg1Pt1 = w.link.node1.pt;
                weg1Pt2 = w.link.node2.pt;
                weg1FrPt = w.frNode.pt;
                weg1ToPt = w.toNode.pt;
            }
        }
        public void Finish()
        {
            end = waypts[waypts.Count - 1].toNode;
            endpt = end.pt;
        }
        float _plen = -1;
        public float pathLength
        {
            // returns total path length
            get
            {
                if (_plen < 0)
                {
                    _plen = 0;
                    foreach (var w in waypts)
                    {
                        _plen += w.distance;
                    }
                }
                return (_plen);
            }
        }
        public PathPos MovePositionAlongPath(float targetDist)
        {
            // returns the position on the path after moving "target" units along the path
            float dst = 0;
            int i = 0;
            float lmd = 0;
            var p1 = start.pt;
            var p2 = p1;
            foreach (var w in waypts)
            {
                p2 = w.toNode.pt;
                //GraphUtil.Log("mpp i:" + i + "  p1wc:" + p1wc + "  p2wc:" + p2wc);
                var ndst = dst + w.distance;
                if (ndst > targetDist)
                {
                    lmd = (targetDist - dst) / w.distance;
                    var pt = lmd * (p2 - p1) + p1;
                    //GraphUtil.Log("mpp i:" + i + " lmd:" + lmd + "ptwc:"+ptwc);
                    var pp1 = new PathPos(this, pt, w, i, lmd);
                    return (pp1);
                }
                dst = ndst;
                p1 = p2;
                i++;
            }
            // Edge cases - at start and end of path
            PathPos pp2;
            var wcnt = waypts.Count;
            if (wcnt == 0)
            {
                // at the start
                pp2 = new PathPos(this, p2, null, wcnt - 1, 1);
            }
            else
            {
                // at the end
                var ww = waypts[wcnt - 1];
                pp2 = new PathPos(this, p2, ww, wcnt - 1, 1);
            }
            return (pp2);
        }
    }
   
    #endregion
}

