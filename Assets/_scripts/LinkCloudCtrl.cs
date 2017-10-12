using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraphAlgos;

namespace BirdRouter
{

    public class LinkCloudCtrl : MonoBehaviour
    {
        private RouteMan rman;
        public void setRouteMan(RouteMan rman)
        {
            // this is attached as a component, thus we cannot set it in a contructor
            this.rman = rman;
        }

        public genmodeE Genmode = genmodeE.gen_b43_1;
        public Vector2 stats_nodes_links = Vector2.zero;
        public Range LinkFLoor;
        public float linkNodeSize;
        public float linkRadius;
        public bool nodesvisible = true;
        public bool linksvisible = true;
        public MapGenParameters mappars;
        public LinkCloud linkcloud = null;

        public bool showNearestPoint = false;
        public Vector3 nearestPointRef = new Vector3(5, 0, 5);
        public int maxVoiceKeywords = 100;
        public int nVoiceKeywords = 0;



        public enum nodeMoveModeE { stretch,move };

        public nodeMoveModeE nodeMoveMode = nodeMoveModeE.move;

        void initVals()
        {
            Genmode = genmodeE.gen_b43_1;
            linkRadius = 0.08f;
            linkNodeSize = 3*linkRadius;
            mappars = new MapGenParameters();
            LinkFLoor = new Range(0,0);
            nodesvisible = true;
            nodeMoveMode = nodeMoveModeE.move;
        }

        void Start()
        {
            initVals();
        }
        // Update is called once per frame
        int updateCount = 0;
        int needRefreshUpdateCount = 0;
        void Update()
        {
            updateCount += 1;
            var lcld = getLinkCloud();
            var moved = lcld.checkForLinkptMovement();
            if (moved)
            {
                if (nodeMoveMode == nodeMoveModeE.move)
                {
                    lcld.syncNodeMovement();
                }
                needRefreshUpdateCount = updateCount;
            }
            if (needRefreshUpdateCount>0 && ((updateCount-needRefreshUpdateCount)>15))
            {
                rman.RequestRefresh();
                needRefreshUpdateCount = 0;
            }

        }

        // LinkCloud Stuff
        public genmodeE lastgenmodel = genmodeE.gen_none;

        public void genLinkCloud(genmodeE genmode=genmodeE.gen_circ)
        {
            this.Genmode = genmode;
            linkcloud = getLinkCloud();
            var mm = new MapMaker(linkcloud,mappars);
            linkcloud.maxRanHeight = LinkFLoor.max;
            linkcloud.minRanHeight = LinkFLoor.min;

            mm.maxVoiceKeywords = this.maxVoiceKeywords;
            mm.AddGraphToLinkCloud(Genmode);
            nVoiceKeywords = mm.nVoiceKeywords;
            lastgenmodel = Genmode;
        }

        public LinkCloud getLinkCloud()
        {
            if (linkcloud == null)
            {
                linkcloud = new LinkCloud();
            }
            return (linkcloud);
        }
        public void SetKeyWordLimit(int maxVoiceKeywords)
        {
            this.maxVoiceKeywords = maxVoiceKeywords;
        }
        public void SaveToJsonFile(string fname)
        {
            var lcld = getLinkCloud();
            if (lcld!=null)
            {
                MapMaker.SaveToFile(lcld, fname);
            }
        }


        static int gogencount = 0;
        void CreateLinkCloudGos()
        {
            var lcld = getLinkCloud();
            if (linkcloudgos == null)
            {
                linkcloudgos = new GameObject();
                linkcloudgos.name = "LinkCloud-" + gogencount;
                linkcloudgos.transform.parent = rman.rgo.transform;
                gogencount++;
            }
            if (linksvisible)
            {
                foreach (var lnkname in lcld.linknamelist)
                {
                    var lnk = lcld.GetLink(lnkname);
                    var cname = linkcolor(lnk.name);
                    var go = LinkGo.MakeNewLinkGo(rman,lnk,linkRadius, cname);
                    go.transform.parent = linkcloudgos.transform;
                }
            }
            if (nodesvisible)
            { 
                foreach (string lptname in lcld.linkpoints())
                {
                    var lpt = lcld.GetNode(lptname);
                    var cname = nodecolor(lpt.name);
                    var go = NodeGo.MakeNewNodeGo(rman,lpt,linkNodeSize,cname);
                    go.transform.parent = linkcloudgos.transform;
                }
                if (showNearestPoint)
                {
                    var npt = FindClosestPointOnLineCloud(nearestPointRef);
                    var nname = "linknearsph-";
                    var pnsph = GraphUtil.CreateMarkerSphere(nname, npt, size: 2.5f* linkNodeSize, clr: "red");
                    pnsph.transform.parent = linkcloudgos.transform;
                }
            }
            stats_nodes_links.x = lcld.nodecount();
            stats_nodes_links.y = lcld.linkcount();
        }
        void DeleteLinkCloudGos()
        {
            if (linkcloudgos != null)
            {
                Destroy(linkcloudgos);
                linkcloudgos = null;
            }
        }
        #region public methods
        public void InitLinkCloud(bool forcenew = true)
        {
            if (forcenew)
            {
                linkcloud = null;
                lastgenmodel = genmodeE.gen_none;
            }
            CreateLinkCloudGos();
        }
        public void DelLinkCloud()
        {
            DeleteLinkCloudGos();
            linkcloud = null;
        }

        GameObject linkcloudgos = null;

        private string nodecolor(string nodename)
        {
            if (rman==null)
            {
                return ("steelblue");
            }
            var nl = nodename.Length;

            var colselecter = RouteMan.RmColorModeE.linkcloudnode;
            var lcld = getLinkCloud();
            if (lcld.voiceEnabled(nodename))
            {
                colselecter = RouteMan.RmColorModeE.linkcloudnodex;
            }
            return(rman.getcolorname(colselecter, nodename));
        }

        private string linkcolor(string linkname)
        {
            if (rman == null)
            {
                return ("orange");
            }
            return(rman.getcolorname(RouteMan.RmColorModeE.linkcloudlink, linkname));
        }
        public LcLink GetLink(string name)
        {
            var lcld = getLinkCloud();
            if (lcld.islinkname(name))
            {
                return(lcld.GetLink(name));
            }
            return (null);
        }
        public void DeleteLink(string name)
        {
            var lcld = getLinkCloud();
            if (lcld.islinkname(name))
            {
                lcld.DelLink(name);
                RefreshGos();
            }
        }
        public void SplitLink(string name)
        {
            var lcld = getLinkCloud();
            if (lcld.islinkname(name))
            {
                lcld.SplitLink(name);
                RefreshGos();
            }
        }
        public void StartStretchMode(string selname)
        {
            var lcld = getLinkCloud();
            if (lcld.isnodename(selname))
            {
                if (nodeMoveMode == nodeMoveModeE.move)
                {
                   // turn purple and start stretching
                    this.nodeMoveMode = nodeMoveModeE.stretch;
                    var lpt = lcld.GetNode(selname);
                    var csph = GraphUtil.CreateMarkerSphere(name, lpt.pt, linkNodeSize * 2, "purple");
                    csph.transform.parent = lpt.go.transform;
                    lcld.StartStretchNode(selname);
                }
                else
                {
                    // now stop stretching
                    lcld.finishStretchMovement(selname);
                    nodeMoveMode = nodeMoveModeE.move;
                    rman.RequestRefresh(selname);
                }
            }
        }
        public void DeleteNode(string name)
        {
            var lcld = getLinkCloud();
            if (lcld.isnodename(name))
            {
                lcld.DelNode(name);
                RefreshGos();
            }
        }
        public LcNode PunchNewLinkPt(PathPos pp, string newptname = "", bool deleteparentlink = false)
        {
            var lcld = getLinkCloud();
            var lpt = lcld.PunchNewLinkPt(pp,newptname,deleteparentlink);
            return (lpt);
        }
        public LcNode GetLinkPt(int idx)
        {
            var lcld = getLinkCloud();
            var lpt = lcld.GetNode(idx);
            return (lpt);
        }
        public LcNode GetLinkPt(string name)
        {
            var lcld = getLinkCloud();
            var lpt = lcld.GetNode(name);
            return (lpt);
        }
        public void RefreshGos()
        {
            linkNodeSize = rman.linknodescale * linkRadius;
            DeleteLinkCloudGos();
            CreateLinkCloudGos();
        }
        public LcNode GetRandomNode()
        {
            var lcld = getLinkCloud();
            var lpt = lcld.GetRanLinkPt();
            return (lpt);
        }
        public Path GenAstar(string startnodename, string endnodename)
        {
            var lcld = getLinkCloud();
            var path = lcld.GenAstar(startnodename, endnodename);
            return (path);
        }
        public Path GenRanPath(string startnodename,int n)
        {
            var lcld = getLinkCloud();
            var path = lcld.GenRanPath(startnodename,n);
            return (path);
        }
        public LinkedList<string> GetKeywordKeys()
        {
            var lcld = getLinkCloud();
            return (lcld.GetKeywordKeys());
        }
        public LinkedList<string> GetKeywordValues()
        {
            var lcld = getLinkCloud();
            return (lcld.GetKeywordValues());
        }
        public string GetKeywordValue(string key)
        {
            var lcld = getLinkCloud();
            return (lcld.GetKeywordValue(key));
        }
        public bool islinkname(string lname)
        {
            var lcld = getLinkCloud();
            return (lcld.islinkname(lname));
        }
        public bool islinkptname(string pname)
        {
            var lcld = getLinkCloud();
            return (lcld.isnodename(pname));
        }
        public Vector3 FindClosestPointOnLineCloud(Vector3 pt)
        {
            var lcld = getLinkCloud();
            return (lcld.FindClosestPointOnLineCloud(pt));
        }
        public LcLink FindClosestLinkOnLineCloudFiltered(string filter,Vector3 pt)
        {
            return (FindClosestLinkOnLineCloudFiltered(filter, pt));
        }
        public void NoiseUpNodes(float maxdist)
        {
            var lcld = getLinkCloud();
            lcld.noiseUpNodes(maxdist,maxdist,maxdist);
        }
        #endregion
    }
}