using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// GraphAlgos.cs  - This file contains Graph algorithems that we need. It is fairly self-contained. 
/// </summary>

namespace GraphAlgos
{
    public enum textureMethE { none,material, bitmap }
    public enum textureGeomE { none,plane, mesh }
    [Serializable]
    public class graphtex
    {
        public textureMethE texMeth;
        public textureGeomE texGeom;
        public string bitmapName = "";
        public string materialName = "";
        public int bitmapXpix = 100;
        public int bitmapYpix = 100;
        public float aspectXoY = 1f;
        public Vector3 scale;
        public Vector3 rotate;
        public Vector3 translate;

        public graphtex()
        {
            texMeth = textureMethE.none;
            texGeom = textureGeomE.none;
            scale = Vector3.zero;
            rotate = Vector3.zero;
            translate = Vector3.zero;
        }
        public void SetMaterialPlane(string matname, int xpix, int ypix, Vector3 sca,Vector3 rot,Vector3 trn)
        {
            // here we store the bitmap values for possible future reference, but we do not use them
            this.texMeth = textureMethE.material;
            this.texGeom = textureGeomE.plane;
            this.materialName = matname;
            this.bitmapXpix = xpix;
            this.bitmapYpix = ypix;
            this.aspectXoY = xpix * 1f / ypix;
            this.scale = sca;
            this.rotate = rot;
            this.translate = trn;
        }
        public void SetMaterialPlane(string matname, int xpix, int ypix, Vector3 rot, Vector3 trn)
        {
            // here we calculate the scale factor from the aspect ratio
            this.texMeth = textureMethE.material;
            this.texGeom = textureGeomE.plane;
            this.materialName = matname;
            this.bitmapXpix = xpix;
            this.bitmapYpix = ypix;
            this.aspectXoY = xpix * 1f / ypix;
            var sca = new Vector3(10*aspectXoY, 1, 10);
            this.scale = sca;
            this.rotate = rot;
            this.translate = trn;
        }
        public graphtex(graphtex gt)
        {
            this.texMeth = gt.texMeth;
            this.texGeom = gt.texGeom;
            this.bitmapName = gt.bitmapName;
            this.materialName = gt.materialName;
            this.bitmapXpix = gt.bitmapXpix;
            this.bitmapYpix = gt.bitmapYpix;
            this.aspectXoY = gt.aspectXoY;
            this.scale = gt.scale;
            this.rotate = gt.rotate;
            this.translate = gt.translate;
        }
    }
    public class graphmods
    {
        public string mod_name_pfx = "";
        public float mod_x_fak = 1.0f;
        public float mod_x_off = 0.0f;
        public float mod_y_fak = 1.0f;
        public float mod_y_off = 0.0f;
        public float mod_z_fak = 1.0f;
        public float mod_z_off = 0.0f;
        public graphmods()
        {
            initmods();
        }
        public void initmods()
        {
            mod_name_pfx = "";
            mod_x_fak = 1.0f;
            mod_x_off = 0.0f;
            mod_y_fak = 1.0f;
            mod_y_off = 0.0f;
            mod_z_fak = 1.0f;
            mod_z_off = 0.0f;
        }
        public Vector3 modv(Vector3 v)
        {
            var rv = new Vector3(
                v.x * mod_x_fak - mod_x_off,
                v.y * mod_y_fak - mod_y_off,
                v.z * mod_z_fak - mod_z_off
                );
            return (rv);
        }
        public Vector3 unmodv(Vector3 v)
        {
            var rv = new Vector3(
                (v.x + mod_x_off) / mod_x_fak,
                (v.y + mod_y_off) / mod_y_fak,
                (v.z + mod_z_off) / mod_z_fak
                );
            return (rv);
        }
        public string addprefix(string basestring)
        {
            if (basestring.IndexOf(mod_name_pfx) != 0)
            {
                return mod_name_pfx + basestring;
            }
            return basestring;
        }
    }
    public class LinkCloud
    {
        //        public static LinkCloud linkcloud = null;

        public List<string> nodenamelist = null;
        Dictionary<string, LcNode> nodedict = null;
        public Dictionary<string, string> nodekeywords = null;

        public List<string> linknamelist = null;
        Dictionary<string, LcLink> linkdict = null;
        //List<Link> lklistlist = null;

        System.Random ranman = null;
        public float maxRanHeight = 0.0f;
        public float minRanHeight = 0.0f;
        public static int ranSeed = 1;

        public graphtex floorMan = new graphtex();

        public int linkcount()
        {
            return linkdict.Count;
        }
        public int nodecount()
        {
            return nodenamelist.Count;
        }
        private int newnodenamenumber = 0;
        public string genuniquenodename()
        {
            var maxiter = nodecount() + 100;
            var niter = 0;
            while (niter < maxiter)
            {
                var newname = "uniqnodename-" + newnodenamenumber;
                newnodenamenumber++;
                if (!isnodename(newname)) return newname;
            }
            return null;
        }
        private int newlinknamenumber = 0;
        public string genuniquelinkname()
        {
            var maxiter = nodecount() + 100;
            var niter = 0;
            while (niter < maxiter)
            {
                var newname = "uniqlinkname-" + newlinknamenumber;
                newlinknamenumber++;
                if (!islinkname(newname)) return newname;
            }
            return null;
        }

        public LinkCloud()
        {
            ranman = new System.Random(ranSeed);
            ranSeed++;
            nodenamelist = new List<string>();
            nodedict = new Dictionary<string, LcNode>();
            nodekeywords = new Dictionary<string, string>();
            linknamelist = new List<string>();
            //lklistlist = new List<Link>();
            linkdict = new Dictionary<string, LcLink>();
        }
        public void noiseUpNodes(float maxdistx, float maxdisty, float maxdistz)
        {
            var rnd = new System.Random();
            foreach (var ptname in nodenamelist)
            {
                var lpt = nodedict[ptname];
                var incx = (float)(2 * maxdistx * rnd.NextDouble() - maxdistx);
                var incy = (float)(2 * maxdisty * rnd.NextDouble() - maxdisty);
                var incz = (float)(2 * maxdistz * rnd.NextDouble() - maxdistz);
                lpt.pt = new Vector3(lpt.pt.x + incx, lpt.pt.y + incy, lpt.pt.z + incz);
            }
        }
        public bool voiceEnabled(string roomname)
        {
            return nodekeywords.Values.Contains<string>(roomname);
        }
        #region linkcloud construction
        public graphmods gm = new graphmods();


        public LcNode AddNode(string name, Vector3 v)
        {
            name = gm.addprefix( name );
            if (nodedict.ContainsKey(name))
            {
                throw new UnityException("Duplicate Point name");
            }
            v = gm.modv(v);
            nodenamelist.Add(name);
            var lpt = new LcNode(this,name, v);
            nodedict.Add(name, lpt);
            anchorpt = lpt;
            return (lpt);
        }
        public float yfloor = 0;
        float ranHeight()
        {
            var dlt = maxRanHeight - minRanHeight;
            var rv = yfloor + (float)ranman.NextDouble() * dlt + minRanHeight;
            return (rv);
        }
        public LcNode AddNodePtxy(string name, double x, double z)
        {
            var xf = (float)x;
            var yf = ranHeight();
            var zf = (float)z;
            return (AddNode(name, new Vector3(xf, yf, zf)));
        }
        public LcNode GetNewNode(string name, Vector3 v)
        {
            if (nodedict.ContainsKey(name))
            {
                return (nodedict[name]);
            }
            // otherwise we need to make it
            nodenamelist.Add(name);
            var node = new LcNode(this,name, v);
            nodedict.Add(name, node);
            return (node);
        }
        public LcLink AddLink(string lname, LcNode lp1, LcNode lp2)
        {
            lname = gm.addprefix( lname );
            if (linkdict.ContainsKey(lname))
            {
                throw new UnityException("AddLink: Duplicate Link name:" + lname);
            }
            linknamelist.Add(lname);
            var lnk = new LcLink(this,lname, lp1, lp2);
            linkdict.Add(lname, lnk);
            //lklistlist.Add(lnk);
            return (lnk);
        }
        public LcLink AddLinkByNodeName(string lp1name, string lp2name, string lname = "")
        {
            lp1name = gm.addprefix( lp1name );
            lp2name = gm.addprefix( lp2name );
            verifyNodeExists("AddLink", lp1name);
            verifyNodeExists("AddLink", lp2name);
            var lp1 = nodedict[lp1name];
            var lp2 = nodedict[lp2name];
            if (lname == "")
            {
                lname = gm.addprefix(lp1.name + ":" + lp2.name);
            }
            else
            {
                lname = gm.addprefix(lname);
            }

            if (linkdict.ContainsKey(lname))
            {
                throw new UnityException("AddLinkByNodeName: Duplicate Link name:" + lname);
            }
            linknamelist.Add(lname);
            var lnk = new LcLink(this,lname, lp1, lp2);
            linkdict.Add(lname, lnk);
            //lklistlist.Add(lnk);
            return (lnk);
        }
        static LcNode anchorpt = null;
        public LcLink LinkTo(string lptname, Vector3 v, string lname = "")
        {
            var lp0 = anchorpt;
            if (lp0 == null)
            {
                throw new UnityException("Anchorpt null");
            }
            lptname = gm.addprefix( lptname );
            v = gm.modv(v);
            var lp1 = GetNewNode(lptname, v);
            if (lname == "")
            {
                lname = lp0.name + ":" + lptname;
            }
            else
            {
                lname = gm.addprefix( lname );
            }
            if (linkdict.ContainsKey(lname))
            {
                throw new UnityException("LinkTo: Duplicate Link name:" + lname);
            }
            linknamelist.Add(lname);

            var lnk = new LcLink(this,lname, lp0, lp1);
            linkdict.Add(lname, lnk);
            //lklistlist.Add(lnk);
            anchorpt = lp1;
            return (lnk);
        }

        public LcLink LinkTooPtxz(string lptname, double x, double z, string lname = "")
        {
            var xf = (float)x;
            var yf = ranHeight();
            var zf = (float)z;
            return (LinkTo(lptname, new Vector3(xf, yf, zf), lname));
        }
        public LcLink LinkTo(string newanchor, string lptname, Vector3 v, string lname = "")
        {
            if (!isnodename(newanchor))
            {
                throw new UnityException("LinkTo: Unknown anchor:" + newanchor);
            }
            anchorpt = nodedict[newanchor];
            return (LinkTo(lptname, v, lname));
        }
        public LcLink LinkTo(string newanchor, string lptname, double x, double y, double z, string lname = "")
        {
            var xf = (float)x;
            var yf = (float)y;
            var zf = (float)z;
            return (LinkTo(newanchor, lptname, new Vector3(xf, yf, zf), lname));
        }
        public LcLink LinkToxz(string newanchor, string lptname, double x, double z, string lname = "")
        {
            var xf = (float)x;
            var yf = ranHeight();
            var zf = (float)z;
            return (LinkTo(newanchor, lptname, new Vector3(xf, yf, zf), lname));
        }

        static int aclcount = 0;
        public void AddCrosLink(string lname, float x, float z, string lname1, string lname2)
        {
            lname = gm.addprefix( lname);
            lname1 = gm.addprefix(lname1 );
            lname2 = gm.addprefix( lname2 );
            var pt = new Vector3(x, yfloor, z);
            pt = gm.modv(pt);
            // find closest filtered link 1 and 2
            var lnk1 = FindClosestLinkOnLineCloudFiltered(lname1, pt);
            if (lnk1==null)
            {
                lnk1 = FindClosestLinkOnLineCloudFiltered(lname1, pt);
                throw new UnityException("AddCrossLink: Could not find filtered link for filter:" + lname1);
            }
            var lnk2 = FindClosestLinkOnLineCloudFiltered(lname2, pt);
            if (lnk2 == null)
            {
                throw new UnityException("AddCrossLink: Could not find filtered link for filter:" + lname2);
            }
            //GraphUtil.Log("ACL pt:" + pt+" lnk1:"+lnk1+" lnk2:"+lnk2);
            var pt1 = lnk1.FindClosestPointOnLink(pt);
            var newlname1 = gm.addprefix(lname1 + aclcount);
            var newpname1 = gm.addprefix(lname + "-0");
            var lpt1 = PunchNewLinkPt(lnk1, pt1, newptname: newpname1, newlinkrootname: newlname1, deleteparentlink: true);
            var pt2 = lnk2.FindClosestPointOnLink(pt);
            var newlname2 = gm.addprefix(lname2 + aclcount);
            var newpname2 = gm.addprefix(lname + "-1");
            var lpt2 = PunchNewLinkPt(lnk2, pt2, newptname: newpname2, newlinkrootname: newlname2, deleteparentlink: true);
            //GraphUtil.Log("ACL pt:" + pt + " pt1:" + pt1 + " pt2:" + pt2);
            AddLink(lname,lpt1, lpt2 );
            aclcount += 1;
        }
        public void addRoomLink(string roomnumber, float x, float z, string occupantnames = "")
        {
            GraphUtil.Log("Add Room Link:" + roomnumber);
            var rmname = "rm" + roomnumber;
            var lpt1 = AddNodePtxy(rmname, x, z);

            var pt = new Vector3(x, yfloor, z);
            pt = gm.modv(pt);
            var filter = gm.addprefix("c");
            var lnk = FindClosestLinkOnLineCloudFiltered(filter, pt, deb: false);
            if (lnk == null)
            {
                throw new UnityException("AddRoomLink: Could not find filtered link for filter:" + filter);
            }
            var punchpoint = lnk.FindClosestPointOnLink(pt);
            var lpt2 = PunchNewLinkPt(lnk, punchpoint, newptname: "cor" + roomnumber, newlinkrootname: lnk.name, deleteparentlink: true);
            AddLink("l" + rmname, lpt1, lpt2);
        }
        #endregion construction
        public LcNode GetNode(int n)
        {
            if (n < 0)
            {
                n = nodenamelist.Count + n;
            }
            if (n < 0 || nodenamelist.Count <= n)
            {
                throw new UnityException("GetNode: index out of range:" + n);
            }
            var pname = nodenamelist[n];
            return (nodedict[pname]);
        }
        public LcNode GetNode(string lptname)
        {
            if (!isnodename(lptname))
            {
                throw new UnityException("GetNode: No link point with this name:" + lptname);
            }
            return (nodedict[lptname]);
        }
        public LcNode GetRanLinkPt()
        {
            int n = nodenamelist.Count;
            var idx = ranman.Next(n);
            var pname = nodenamelist[idx];
            return (nodedict[pname]);
        }
        public bool islinkname(string lname)
        {
            return (linkdict.ContainsKey(lname));
        }
        public bool isnodename(string pname)
        {
            return (nodedict.ContainsKey(pname));
        }
        public StatusMsg ChangeNodeName(string oldnodename,string newnodename)
        {
            if (!isnodename(oldnodename)) return new StatusMsg(false,"Old nodename "+oldnodename+" does not exist");
            if (isnodename(newnodename)) return new StatusMsg(false,"New nodename "+newnodename+"exists already");
            var node = nodedict[oldnodename];
            node.name = newnodename;
            nodedict.Remove(oldnodename);
            nodedict[newnodename] = node;
            nodenamelist.Remove(oldnodename);
            nodenamelist.Add(newnodename);
            return new StatusMsg(true,"changed name from "+oldnodename+" to "+newnodename);
        }
        public StatusMsg ChangeLinkName(string oldlinkname, string newlinkname)
        {
            if (!islinkname(oldlinkname)) return new StatusMsg(false,"Old nodename "+oldlinkname+" does not exist");
            if (islinkname(newlinkname)) return new StatusMsg(false,"New nodename "+newlinkname+"exists already");
            var link = linkdict[oldlinkname];
            link.name = newlinkname;
            linkdict.Remove(oldlinkname);
            linkdict[newlinkname] = link;
            linknamelist.Remove(oldlinkname);
            linknamelist.Add(newlinkname);
            return new StatusMsg(true,"changed name from "+ oldlinkname + " to "+ newlinkname);
        }
        #region linkcloud editing
        static int newnodecount = 0;

        public LcNode PunchNewLinkPt(PathPos pp, string newptname = "", bool deleteparentlink = false)
        {
            return (PunchNewLinkPt(pp.weg.link, pp.pt, newptname: newptname, deleteparentlink: deleteparentlink));
        }
        public LcNode PunchNewLinkPt(LcLink lnk, Vector3 pt, string newptname = "", string newlinkrootname = "", bool deleteparentlink = false, bool alwayspunch = false)
        {
            if (!alwayspunch)
            {
                // we don't punch a new link point if we are close enough to one of the end points
                // 5e-2 is 5 cm
                if (Vector3.Distance(pt, lnk.node1.pt) < 5e-2)
                {
                    //  GraphUtil.Log("Punched to lp1:"+lnk.lp1.pt);
                    return lnk.node1;
                }
                if (Vector3.Distance(pt, lnk.node2.pt) < 5e-2)
                {
                    //  GraphUtil.Log("Punched to lp2:" + lnk.lp2.pt);
                    return lnk.node2;
                }
            }
            if (newptname == "")
            {
                newptname = "tmp-x-" + newnodecount;
            }
            newnodecount += 1;
            pt = gm.unmodv(pt);
            //GraphUtil.Log("Adding point:" + pt);
            var newlpt = AddNode(newptname, pt);
            var newlinkname0 = "";
            var newlinkname1 = "";
            if (newlinkrootname != "")
            {
                newlinkname0 = newlinkrootname + "0";
                newlinkname1 = newlinkrootname + "1";
            }
            AddLink(newlinkname0, newlpt, lnk.node1);
            //GraphUtil.Log("Adding lnk:" + newlinkname0);
            AddLink(newlinkname1, newlpt, lnk.node2);
            //GraphUtil.Log("Adding lnk:" + newlinkname1);
            if (deleteparentlink)
            {
                //GraphUtil.Log("Deleting lnk:" + lnk.name);
                DelLink(lnk.name);
            }
            return (newlpt);
        }
        public bool checkForLinkptMovement()
        {
            int nmoved = 0;
            foreach( var lptname in nodenamelist )
            {
                var lpt = GetNode(lptname);
                if (lpt.wasMoved())
                {
                    nmoved += 1;
                }
            }
            //GraphUtil.Log("nmoved:" + nmoved);
            return nmoved!=0;
        }
        public void finishStretchMovement(string lptname)
        {
            //GraphUtil.Log("stretchLinkptMovement");
            var node = GetNode(lptname);
            var closeest = FindClosestNodeToNodeGo(node);
            if (Vector3.Distance(closeest.go.transform.position, node.go.transform.position) > 0.2)
            {
                var newname = genuniquenodename();
                var newlpt = GetNewNode(newname, node.go.transform.position);
                var newlinkname = genuniquelinkname();
                AddLink(newlinkname, node, newlpt);
            } else
            {
                var newlinkname = genuniquelinkname();
                AddLink(newlinkname, node, closeest);
            }
        }
        public bool syncNodeMovement()
        {
            int nmoved = 0;
            foreach (var lptname in nodenamelist)
            {
                var lpt = GetNode(lptname);
                if (lpt.wasMoved())
                {
                    lpt.syncToGo();
                }
            }
            //GraphUtil.Log("moved and synced:" + nmoved);
            return nmoved == 0;
        }
        public void DelLink(string lname)
        {
            if (!linkdict.ContainsKey(lname))
            {
                throw new UnityException("DelLink: Unknown Link name:" + lname);
            }
            var lnk = linkdict[lname];
            lnk.UnlinkLink();
            //lklistlist.Remove(lnk);
            linkdict.Remove(lname);
            linknamelist.Remove(lname);
        }
        public void SplitLink(string lname)
        {
            if (!linkdict.ContainsKey(lname))
            {
                throw new UnityException("SplitLink: Unknown Link name:" + lname);
            }
            var lnk = linkdict[lname];
            var npt = (lnk.node1.pt + lnk.node2.pt) * 0.5f;
            var nnodename = genuniquenodename();
            PunchNewLinkPt(lnk, npt, newptname: nnodename, newlinkrootname:lnk.name, deleteparentlink: true, alwayspunch: true);
        }
        public void StartStretchNode(string lptname)
        {
            if (!linkdict.ContainsKey(lptname))
            {
                throw new UnityException("StartStretchNode: Unknown node name:" + lptname);
            }

            var lpt = GetNode(lptname);

        }
        public void DelNode(string nname)
        {
            if (!nodedict.ContainsKey(nname))
            {
                throw new UnityException("DelNodeL Unknown Node name:" + nname);
            }
            var pt = nodedict[nname];
            // first find the names of all connected links
            var linknamelist = new HashSet<string>();
            foreach (var w in pt.wegtos)
            {
                linknamelist.Add(w.link.name);
            }
            // now delete them all
            foreach (var lname in linknamelist)
            {
                DelLink(lname);
            }
            // now remove the point
            nodedict.Remove(nname);
            nodenamelist.Remove(nname);
        }
        #endregion
        public List<string> linkpoints()
        {
            return (nodenamelist);
        }

        public void verifyNodeExists(string callname, string ptname)
        {
            if (!nodedict.ContainsKey(ptname))
            {
                throw new UnityException("verifyPointExists: Node '" + ptname + "' not defined in " + callname);
            }
        }
        public LcLink GetLink(string name)
        {
            if (islinkname(name))
            {
                var lnk = linkdict[name];
                return (lnk);
            }
            return (null);
        }
        public LcNode FindClosestNode(Vector3 pt)
        {
            float mindist = 9e30f;
            LcNode minnode = null;
            foreach (var nname in nodenamelist)
            {
                var node = GetNode(nname);
                var dist = Vector3.Distance(pt, node.pt);
                if (dist < mindist)
                {
                    mindist = dist;
                    minnode = node;
                }
            }
            return (minnode);
        }
        public LcNode FindClosestNodeToNodeGo(LcNode targetnode)
        {
            float mindist = 9e30f;
            LcNode minnode = null;
            var gopt = targetnode.go.transform.position;
            foreach (var nname in nodenamelist)
            {
                if (nname != targetnode.name)
                {
                    var node = GetNode(nname);
                    var dist = Vector3.Distance(gopt, node.pt);
                    if (dist < mindist)
                    {
                        mindist = dist;
                        minnode = node;
                    }
                }
            }
            return (minnode);
        }
        public Vector3 FindClosestPointOnLineCloud(Vector3 pt)
        {
            var rpt = Vector3.zero;
            float mindist = 9e30f;
            foreach (var lnkname in this.linknamelist)
            {
                var lnk = GetLink(lnkname);
                var npt = lnk.FindClosestPointOnLink(pt);
                var dist = Vector3.Distance(pt, npt);
                if (dist < mindist)
                {
                    rpt = npt;
                    mindist = dist;
                }
            }
            return rpt;
        }
        public LcLink FindClosestLinkOnLineCloudFiltered(string filter, Vector3 pt, bool deb = false)
        {
            if (deb)
            {
                GraphUtil.Log("FCLOLCF filter:" + filter + " pt:" + pt);
            }
            var rpt = Vector3.zero;
            LcLink rlnk = null;
            float mindist = 9e30f;
            foreach (var lnkname in linknamelist)
            {
                var lnk = GetLink(lnkname);
                if (filter == "" || lnk.name.IndexOf(filter) == 0)
                {
                    var npt = lnk.FindClosestPointOnLink(pt);
                    var dist = Vector3.Distance(pt, npt);
                    if (dist < mindist)
                    {
                        rlnk = lnk;
                        rpt = npt;
                        mindist = dist;
                        if (deb)
                        {
                            var lamb = lnk.FindClosestLamb(pt);
                            GraphUtil.Log("    lnk:" + lnk.name + " npt:" + npt + " dist:" + dist + " lamb:" + lamb);
                            GraphUtil.Log("    lnk.p1:" + lnk.node1.pt + " p2:" + lnk.node2.pt);
                        }
                    }
                }
            }
            if (deb)
            {
                GraphUtil.Log("FCLOLCF found:" + rlnk.name);
            }
            return rlnk;
        }
        public LinkedList<string> GetKeywordKeys()
        {
            var l = new LinkedList<string>(nodekeywords.Keys);
            return (l);
        }
        public LinkedList<string> GetKeywordValues()
        {
            var l = new LinkedList<string>(nodekeywords.Values);
            return (l);
        }
        public string GetKeywordValue(string key)
        {
            if (!nodekeywords.ContainsKey(key)) return ("");
            return (nodekeywords[key]);
        }
        #region pathfinding
        LcNode findminfscore(HashSet<LcNode> openset)
        {
            var minf = 99e30f;
            LcNode minlpt = null;
            foreach (var lpt in openset)
            {
                if (lpt.fScore < minf)
                {
                    minf = lpt.fScore;
                    minlpt = lpt;
                }
            }
            return (minlpt);
        }
        Path reconstructPath(LcNode spt, LcNode ept)
        {
            var cflist = new LinkedList<LcNode>();
            cflist.AddLast(ept);
            var curpt = ept;
            int nloop = 0;
            while (curpt != spt)
            {
                nloop = nloop + 1;
                if (nloop > (4 * nodenamelist.Count))
                {
                    GraphUtil.Log("Iter count exceeded 1 in reconstructPath");
                    return (null);
                }

                cflist.AddLast(curpt);
                curpt = curpt.cameFrom;
            }
            cflist.AddLast(curpt);
            nloop = 0;
            var path = new Path(spt);
            var cur = cflist.Last;
            while (cur != null)
            {
                nloop = nloop + 1;
                if (nloop > (4 * nodenamelist.Count))
                {
                    GraphUtil.Log("Iter count exceeded 2 in reconstructPath");
                    return (null);
                }
                var next = cur.Previous;
                if (next != null)
                {
                    var nxpt = next.Value.pt;
                    foreach (var w in cur.Value.wegtos)
                    {
                        if (nxpt == w.toNode.pt)
                        {
                            path.AddWaypt(w);
                            continue;
                        }
                    }
                }
                cur = next;
            }
            path.Finish();
            return (path);
        }
        public Path GenAstar(string sptname, string eptname)
        {
            verifyNodeExists("GenAstar", sptname);
            verifyNodeExists("GenAstar", eptname);
            var spt = nodedict[sptname];
            var ept = nodedict[eptname];

            foreach (var ptname in nodenamelist)
            {
                var lpt = nodedict[ptname];
                lpt.astarInit();
            }

            var closedSet = new HashSet<LcNode>();
            var openSet = new HashSet<LcNode>();
            openSet.Add(spt);
            spt.gScore = 0;
            spt.fScore = spt.heuristic_cost_estimate(ept);

            int nloop = 0;

            while (openSet.Count > 0)
            {
                //   GraphUtil.Log("openSet count:" + openSet.Count + " closedSet count:" + closedSet.Count);
                nloop = nloop + 1;
                if (nloop > (4 * nodenamelist.Count))
                {
                    GraphUtil.Log("Iter count exceeded in genAstar");
                    return (null);
                }

                var current = findminfscore(openSet);
                if (current == ept)
                {
                    return (reconstructPath(spt, ept));
                }
                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var w in current.wegtos)
                {
                    if (closedSet.Contains(w.toNode)) continue; // already evaluated

                    if (!openSet.Contains(w.toNode))            // discover a new node 
                    {
                        openSet.Add(w.toNode);
                    }

                    var tentative_gScore = current.gScore + Vector3.Distance(current.pt, w.toNode.pt);
                    if (tentative_gScore > w.toNode.gScore) continue; // this is not a better path

                    w.toNode.cameFrom = current;
                    w.toNode.gScore = tentative_gScore;
                    w.toNode.fScore = w.toNode.gScore + ept.heuristic_cost_estimate(w.toNode);
                }
            }
            return (null);
        }
        public Path GenRanPath(string sptname, int npts)
        {
            verifyNodeExists("GenRanPath", sptname);
            var pt = nodedict[sptname];
            var path = new Path(pt);
            var rnd = new System.Random();
            for (int i = 0; i < npts; i++)
            {
                var wegar = pt.wegtos.ToArray();
                var w = wegar[rnd.Next(wegar.Length)];
                path.AddWaypt(w);
                pt = w.toNode;
            }
            path.Finish();
            return (path);
        }
        #endregion
    }
}

