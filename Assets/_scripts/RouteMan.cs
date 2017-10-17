#define UNITYSIM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using GraphAlgos;
using UnityEngine.VR;

namespace BirdRouter
{
    public enum RouterGarnishE {  none, names, coords, all }

    public class RouteMan : MonoBehaviour
    {
        static RouteMan theone = null;

        public LinkedList<string> loglist = null;

        public bool unitySim = false;
        public string logfilenameroot = "brlog";

        public GameObject rgo;
        public float rgoScale = 1.0f;
        public float rgoRotate = 0f;
        public Vector3 rgoTranslate = Vector3.zero;

        public string strans0;
        public string strans1;
        public string strans2;
        public string strans3;

        public float home_height = 1.7f;
        float home_rgoScale = 1.0f;
        float home_rgoRotate = 0f;
        Vector3 home_rgoTranslate = Vector3.zero;

        public float scaleIncFak = 1.1f;
        public float rotationIncDeg = 2.0f;
        public float tranlsationIncMeter = 1.0f;
        public float gridsize = 2.0f;
        public float linknodescale = 3.0f;
        public RouterGarnishE garnish = RouterGarnishE.none;

        public bool showlookat = false;

        public bool movecamera = false;

        public bool droperrormarkers = false;

        public bool autoerrorcorrect = false;
        private bool needrefresh = false;

        public string routeDataDir = "d:/local/routedata";


        public GameObject rmango;

        public LinkCloudCtrl linkcloudctrl = null;
        public PathCtrl pathctrl = null;
        public BirdCtrl birdctrl = null;
        public StatusCtrl statusctrl = null;
        public ErrorMarkerCtrl errmarkctrl = null;
        public FloorPlanCtrl floorplanctrl = null;
        public KeywordMan keyman = null;

        public int keywordLoadInc = 100;
        public int keywordLimit = 80;
        public int keycount = 0;
        public SpatialMapperMan smm = null;


        private Vector4 trans0;
        private Vector4 trans1;
        private Vector4 trans2;
        private Vector4 trans3;
        public Transform rgoTransform;
        public int rgoTransformSetCount=0;
        public int lastRgoTransformSetCount=-1;

        void initVars()
        {
            theone = this;
            loglist = new LinkedList<string>();
            GraphUtil.loglist = loglist;

            // Create game object to hold ctrl inspectors
            rmango = GameObject.Find("RouteMan");

            linkcloudctrl = rmango.AddComponent<LinkCloudCtrl>();
            pathctrl = rmango.AddComponent<PathCtrl>();
            birdctrl = rmango.AddComponent<BirdCtrl>();
            statusctrl = rmango.AddComponent<StatusCtrl>();
            errmarkctrl = rmango.AddComponent<ErrorMarkerCtrl>();
            floorplanctrl = rmango.AddComponent<FloorPlanCtrl>();
            linkcloudctrl.setRouteMan(this);
            pathctrl.setRouteMan(this);
            birdctrl.setRouteMan(this);
            statusctrl.setRouteMan(this);
            errmarkctrl.setRouteMan(this);
            floorplanctrl.setRouteMan(this);

            keyman = new KeywordMan(this);


            // Create game object to hold actual game objects
            rgo = new GameObject("Rgo");
            rgoTransformSetCount += 1;


            statusctrl.outMode = StatusCtrl.outModeE.help;
            statusctrl.RealizeMode();
            keycount = keyman.totalKeywordCount();

            smm = FindObjectOfType<SpatialMapperMan>();

            DisableSpatialMapping();

        }

        public void RequestRefresh()
        {
            needrefresh = true;
        }
        public void RequestRefresh(string highobjname)
        {
            needrefresh = true;
            //this.highobjnames.Add(highobjname);
        }
        public void ToggleDropErrorMarkers()
        {
            droperrormarkers = !droperrormarkers;
        }
        public void CorrectOnErrorMarkers()
        {
           // Debug.Log("CorrectOnErrorMarkers called");
            errmarkctrl.FinishMarking();
            var foundCorrection = errmarkctrl.CalculateOptimalTransformation();
            if (foundCorrection)
            {
                CorrectAngle(errmarkctrl.rotvek_deg.y);
                CorrectPositionDiff(errmarkctrl.trnvek_met);
            }
        }
        public void StartErrorMarking(int n=5)
        {
            errmarkctrl.startMarking(n);
        }
        public void FinishErrorMarking()
        {
            errmarkctrl.CalculateOptimalTransformation();
        }
        public void EnableSpatialMapping()
        {
            if (smm != null)
            {
                smm.SetSpatialMapping(true);
            }
            else
            {
                RouteMan.Log("smm is null");
            }
        }
        public void DisableSpatialMapping()
        {
            if (smm != null)
            {
                smm.SetSpatialMapping(false);
            }
            else
            {
                RouteMan.Log("smm is null");
            }
        }
        public void IncSpatialExtent()
        {
            if (smm != null)
            {
                smm.ChangeSpatialExtent(2.0f);
            }
        }
        public void DecSpatialExtent()
        {
            if (smm != null)
            {
                smm.ChangeSpatialExtent(-2.0f);
            }
        }
        public void IncSpatialDetail()
        {
            if (smm != null)
            {
                smm.ChangeSpatialDetail(1);
            }
        }
        public void DecSpatialDetail()
        {
            if (smm != null)
            {
                smm.ChangeSpatialDetail(-1);
            }
        }
        public static void Log(string msg)
        {
            if (theone!=null)
            {
                if (theone.loglist!=null)
                {
                    theone.loglist.AddFirst(msg);
                }
            }
        }

        #region linkcloud commands
        public void SetFov(float fov)
        {
            var maincam = Camera.main; // only works with one camera
            maincam.fieldOfView = fov;
        }
        #endregion


        #region linkcloud commands

        private Vector3 adjustDiff(Vector3 diff)
        {
            var angrad = Mathf.PI * rgoRotate / 180;
            var x = Mathf.Cos(angrad) * diff.x - Mathf.Sin(angrad) * diff.z;
            var y = diff.y;
            var z = Mathf.Sin(angrad) * diff.x + Mathf.Cos(angrad) * diff.z;
            return new Vector3(x, y, z);
        }

        private void CorrectPositionDiff(Vector3 diff)
        {
            rgoTranslate = rgoTranslate - diff;
            //StopBird();
            RefreshRouteManGos();
        }

        private void CorrectPosition(Vector3 ptwc)
        {
            var maincam = Camera.main; // only works with one camera
            var campt = maincam.transform.position;
            //campt.y = 0;
            var diff = ptwc - campt;
            diff.y += home_height;
            var adiff = adjustDiff(diff);
            //var adiff = diff;
            RouteMan.Log("Campt:" + campt + "  ptwc:" + ptwc +" hh:"+home_height + "  diff:" + diff + "  adiff:" + adiff);
            rgoTranslate = rgoTranslate - adiff;
            //StopBird();
            RefreshRouteManGos();
        }

        public void CorrectPosition()
        {
            var maincam = Camera.main; // only works with one camera
            var campt = maincam.transform.position;
            //campt.y = 0;
            var pathcampt = pathctrl.FindClosestPointOnPath(campt);
            //pathcampt.y = 0;
            //var ppwc = pathctrl.pathgo.transform.TransformPoint(pathcampt);
            var ppwc = pathcampt;
            RouteMan.Log("CP pathcampt:" + pathcampt + "  ppwc:" + ppwc);
            CorrectPosition(ppwc);
        }
        public void CorrectAngle(float dang, bool refresh = true)
        {
            rgoRotate = rgoRotate - dang;
            if (refresh)
            {
                //StopBird();
                RefreshRouteManGos();
            }
        }
        public void CorrectAngleOnVectors(Vector3 vk1, Vector3 vk2,bool refresh=true)
        {
            vk1 = Vector3.Normalize(vk1);
            vk2 = Vector3.Normalize(vk2);
            var ang1 = 180*Mathf.Atan2(vk1.z,vk1.x)/Mathf.PI;
            var ang2 = 180*Mathf.Atan2(vk2.z,vk2.x)/Mathf.PI;
            RouteMan.Log("vk2:" + vk2 + "  ang2:" + ang2 + "  vk1:" + vk1 + " ang1:"+ang1);
            var dang = ang2 - ang1;
            rgoRotate = rgoRotate - dang;
            if (refresh)
            {
                //StopBird();
                RefreshRouteManGos();
            }
        }
        public float CorrectAngle(bool refresh = true)
        {
            var maincam = Camera.main; // only works with one camera
            var camfor = maincam.transform.forward;
            // find closet link and get its direction
            var campt = maincam.transform.position;
            campt.y = 0;
            float nearptpathdist = 0;
            var pathweg = pathctrl.FindClosestWegOnPath(campt, out nearptpathdist);
            var wegdir = pathweg.GetWegDirection(normalized: true);
            var wegdirwc = rgo.transform.TransformVector(wegdir);
            CorrectAngleOnVectors(wegdirwc, camfor, refresh:refresh);
            return (nearptpathdist);
        }
        public void CorrectPositionAndAngle()
        {
            var pdist = CorrectAngle();
            var pp = pathctrl.path.MovePositionAlongPath(pdist);
           // var ppwc = pathctrl.pathgo.transform.TransformPoint(pp.pt); // why do we have to do this? Should be wc already?
            var ptwc = rgo.transform.TransformPoint(pp.pt);
            RouteMan.Log("Both pdist:" + pdist + "  pp.pt:" + pp.pt + "  ptwc:" + ptwc);
            CorrectPosition(ptwc);
        }
        public void RevertToHome()
        {
            rgoRotate = home_rgoRotate;
            rgoScale = home_rgoScale;
            rgoTranslate = home_rgoTranslate;
            rgoTransformSetCount += 1;

            //StopBird();
            RefreshRouteManGos();
        }
        public void OrientToEndNode(string newenodename)
        {
            RouteMan.Log("OrientToEndNode:" + newenodename);
            home_rgoRotate = rgoRotate;
            home_rgoScale = rgoScale;
            home_rgoTranslate = rgoTranslate;
            home_height = Camera.main.transform.position.y;

            var lpt = linkcloudctrl.GetNode(newenodename);
            if (lpt.wegtos.Count == 0) return;
            var wa = lpt.wegtos.First(); // get the first one, it should suffice
            var lnk = wa.link;
            //var p1wc = lnk.lp1.transform.TransformPoint(lnk.lp1.pt);
            //var p2wc = lnk.lp2.transform.TransformPoint(lnk.lp2.pt);
            //var p1wc = lnk.lp1.ptwc;
            //var p2wc = lnk.lp2.ptwc;
            //var lnkdir = p2wc - p1wc;
            var wfr = wa.frNode.pt;
            var wto = wa.toNode.pt;
            var wfrwc = rgo.transform.TransformPoint(wfr);
            var wtowc = rgo.transform.TransformPoint(wto);
            var lnkdir = wfr - wto;
            lnkdir = rgo.transform.TransformVector(lnkdir);
            RouteMan.Log("CorrectAngle");
            RouteMan.Log("lpt.name:"+lpt.name+"  wfr:"+wfr+" wto:"+wto);
            RouteMan.Log("      wc:"+lpt.name+"  wfrwc:"+wfrwc+" wtowc:"+wtowc);
            var camfor = Camera.main.transform.forward;
            CorrectAngleOnVectors(lnkdir, camfor);

            // Position
            lpt = linkcloudctrl.GetNode(newenodename);
            wa = lpt.wegtos.First();
            var wto1 = wa.toNode.pt;
            var wto1wc = rgo.transform.TransformPoint(wto1);
            CorrectPosition(wto1wc);
            RouteMan.Log("CorrectPosition");
            RouteMan.Log("Node:" + lpt.name + "  wto1:" + wto1 + " wto1wc:" + wto1wc);
        }
        public void writeLogToFile()
        {
            var fname = this.logfilenameroot;
            fname += System.DateTime.Now.ToString("yyyyMMddTHHmmss")+".log";
            GraphUtil.writeLinkedListToFile(loglist, fname);
            RouteMan.Log("Wrote " + loglist.Count + " lines to file " + fname);
            Debug.Log("Wrote " + loglist.Count + " lines to file " + fname);
        }
        public void writeLogToAzureBlob()
        {
#if NETFX_CORE
            var fname = "hlbirdlog";
#else
            var fname = "unbirdlog";
#endif

            GraphUtil.writeLinkedListToAzureBlob(loglist, fname);
            RouteMan.Log("Wrote " + loglist.Count + " lines to blob " + fname);
            Debug.Log("Wrote " + loglist.Count + " lines to blob " + fname);
        }

        public void NextGarnish()
        {
            switch (garnish)
            {
                case RouterGarnishE.none: garnish = RouterGarnishE.names; break;
                case RouterGarnishE.names: garnish = RouterGarnishE.none; break;
                case RouterGarnishE.coords: garnish = RouterGarnishE.all; break;
                case RouterGarnishE.all: garnish = RouterGarnishE.none; break;
            }
            RefreshRouteManGos();
        }
        public void SetBallColor(string color)
        {
            linkcloudnodecolor = color;
            RefreshRouteManGos();
        }
        public void SetPipeColor(string color)
        {
            linkcloudlinkcolor = color;
            RefreshRouteManGos();
        }

        public void IncInc()
        {
            scaleIncFak = scaleIncFak * 1.1f;
            rotationIncDeg = rotationIncDeg * 2f;
            tranlsationIncMeter = tranlsationIncMeter * 2f;
            keywordLoadInc *= 2;
            RouteMan.Log( "ScaInfFak " + scaleIncFak + "  rotIncDeg:" + rotationIncDeg +
                          "transIncM " + tranlsationIncMeter + "  keyInc:" + keywordLoadInc );
        }
        public void DecInc()
        {
            scaleIncFak = scaleIncFak / 1.1f;
            rotationIncDeg = rotationIncDeg / 2f;
            tranlsationIncMeter = tranlsationIncMeter / 2f;
            keywordLoadInc /= 2;
            RouteMan.Log("ScaInfFak " + scaleIncFak + "  rotIncDeg:" + rotationIncDeg +
                          "transIncM " + tranlsationIncMeter + "  keyInc:" + keywordLoadInc);
        }
        public void IncKeyLimit()
        {
            keywordLimit += keywordLoadInc;
            linkcloudctrl.SetKeyWordLimit(keywordLimit);
            RouteMan.Log("Keywordlimit " + keywordLimit + "  keyInc:" + keywordLoadInc);
        }
        public void DecKeyLimit()
        {
            keywordLimit -= keywordLoadInc;
            linkcloudctrl.SetKeyWordLimit(keywordLimit);
            RouteMan.Log("Keywordlimit " + keywordLimit + "  keyInc:" + keywordLoadInc);
        }
        public void Grow()
        {
            ScaleEverything(scaleIncFak);
        }
        public void Shrink()
        {
            ScaleEverything(1/scaleIncFak);
        }
        public void TranslateLeft()
        {
            var tinc = Vector3.left * tranlsationIncMeter;
            TranslateEverything(tinc);
        }
        public void TranslateRight()
        {
            var tinc = Vector3.right * tranlsationIncMeter;
            TranslateEverything(tinc);
        }
        public void TranslateUp()
        {
            var tinc = Vector3.up * tranlsationIncMeter;
            TranslateEverything(tinc);
        }
        public void TranslateDown()
        {
            var tinc = Vector3.down * tranlsationIncMeter;
            TranslateEverything(tinc);
        }
        public void TranslateForward()
        {
            var tinc = Vector3.forward * tranlsationIncMeter;
            TranslateEverything(tinc);
        }
        public void TranslateBack()
        {
            var tinc = Vector3.back * tranlsationIncMeter;
            TranslateEverything(tinc);
        }
        public void RotateCw()
        {
            RotateEverything(rotationIncDeg);
        }
        public void RotateCcw()
        {
            RotateEverything(-rotationIncDeg);
        }
        public void Rotate()
        {
            ScaleEverything(1 / scaleIncFak);
        }
        public void Grow01()
        {
            ScaleEverything(1.01f);
        }
        public void Shrink01()
        {
            ScaleEverything(1 / 1.01f);
        }
        public void Grow10()
        {
            ScaleEverything(1.1f);
        }
        public void Shrink10()
        {
            ScaleEverything(1/1.1f);
        }
        public void Grow50()
        {
            ScaleEverything(2.0f);
        }
        public void Shrink50()
        {
            ScaleEverything(0.5f);
        }
        public void ScaleEverything(float sfak)
        {
            rgoScale = sfak * rgoScale;
            rgoTransformSetCount += 1;

            //const float rgoScaleMin = 0.01375f;
            //  rgoScale = Mathf.Max(rgoScale, rgoScaleMin);
            //Debug.Log("ScaleEverything sfak:" + sfak + "  rgoScale:" + rgoScale);
            RefreshRouteManGos();
        }
        public void GridOff()
        {

        }
        public void GridOn()
        {

        }
        public void GridBigger()
        {
            gridsize *= 2;
        }
        public void GridSmaller()
        {
            gridsize /= 2;
        }
        public void Regen()
        {
            RefreshRouteManGos();
        }
        public void ResetHomeHeight()
        {
            home_height = 1.7f;
            RouteMan.Log("Reset HomeHeight:" + home_height);
        }
        public void RotateEverything(float rinc)
        {
            rgoRotate = rgoRotate + rinc;
            RouteMan.Log("RotateEverything rinc:" + rinc + "  rgoRotate:" + rgoRotate);
            RefreshRouteManGos();
        }
        public void ToggleMoveCamera()
        {
            movecamera = !movecamera;
            RouteMan.Log("Movecamera now:" + movecamera);
        }
        public void ToggleFloorPlan()
        {
            floorplanctrl.visible = !floorplanctrl.visible;
            RealizeFloorPlanStatus();
            RefreshRouteManGos();
        }
        public void RealizeFloorPlanStatus()
        {
            if (floorplanctrl.visible)
            {
                var lcld = linkcloudctrl.getLinkCloud();
                floorplanctrl.setGraphtex(lcld.floorMan);
            }
            RouteMan.Log("Show floor plan:" + floorplanctrl.visible);
        }
        public void TranslateEverything(Vector3 tinc)
        {
            if (movecamera)
            {
                Camera.main.transform.position +=  tinc;
                RouteMan.Log("TranslateEverything tinc:" + tinc + "  Moved Camera");          
            }
            else
            {
                rgoTranslate = rgoTranslate + tinc;
                RouteMan.Log("TranslateEverything tinc:" + tinc + "  rgoTranslate:" + rgoTranslate);
                RefreshRouteManGos();
            }
        }
        public void Gen43_1()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_b43_1);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void Gen43_2()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_b43_2);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void Gen43_3()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_b43_3);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void Gen43_4()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_b43_4);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void GenBHO()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_bho);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void GenSphere()
        {
            linkcloudctrl.genLinkCloud(genmodeE.gen_sphere);
            RefreshRouteManGos();
        }
        public void GenCirc()
        {

            linkcloudctrl.genLinkCloud(genmodeE.gen_circ);
            RefreshRouteManGos();
        }
        public void Gen431p2()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_b43_1p2);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
            ScaleEverything(0.1375f/2.0f);
        }

        public void GenRedwb3simple()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_redwb_3_simple);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void GenRedwb3()
        {
            DeleteLinkCloud();
            linkcloudctrl.genLinkCloud(genmodeE.gen_redwb_3);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
            keycount = keyman.totalKeywordCount();
        }
        public void GenFromJsonFile(string fname)
        {
            DeleteLinkCloud();
            linkcloudctrl.mappars.filepar.fullfilename = fname;
            linkcloudctrl.genLinkCloud(genmodeE.gen_json_file);
            floorplanctrl.visible = true;
            RealizeFloorPlanStatus();
            CreateLinkCloud();
        }
        public void SaveToJsonFile(string fname)
        {
            linkcloudctrl.SaveToJsonFile(fname);
        }
        public void NoiseUpNodes(float maxdist)
        {
            linkcloudctrl.NoiseUpNodes(maxdist);
            RefreshRouteManGos();
        }
        public void CleanStart()
        {
             DeleteLinkCloud();
        }
        public void CreateLinkCloud()
        {
            pathctrl.path = null;
            //DeleteLinkCloud();
            RefreshRouteManGos();
            RouteMan.Log("Calling astar");
            Astar();
//            SetEndNode(pathctrl.endnodename);
            keyman.initKeywordsWithRooms();
        }
        public void DeleteLinkCloud()
        {
            floorplanctrl.visible = false;
            birdctrl.DeleteBird();
            pathctrl.DeletePath();
            linkcloudctrl.DelLinkCloud();
        }
        public void DeleteLink(string name)
        {
            linkcloudctrl.DeleteLink(name);
        }
        public void SplitLink(string name)
        {
            linkcloudctrl.SplitLink(name);
        }
        public void StartStretchMode(string name)
        {
            linkcloudctrl.StartStretchMode(name);
        }
        public void DeleteNode(string name)
        {
            linkcloudctrl.DeleteNode(name);
        }

        public void ToggleLinkCloudVisibily()
        {
            linkcloudctrl.nodesvisible = !linkcloudctrl.nodesvisible;
            linkcloudctrl.RefreshGos();
        }
        public void ShowRoute()
        {
            linkcloudctrl.nodesvisible = true;
            linkcloudctrl.linksvisible = true;
            pathctrl.visible = true;
            RefreshRouteManGos();
        }
        public void HideRoute()
        {
            linkcloudctrl.nodesvisible = false;
            linkcloudctrl.linksvisible = false;
            pathctrl.visible = false;
            RefreshRouteManGos();
        }
        public void HideLinks()
        {
            linkcloudctrl.nodesvisible = true;
            linkcloudctrl.linksvisible = false;
            pathctrl.visible = true;
            RefreshRouteManGos();
        }
        public void ShowLinks()
        {
            linkcloudctrl.nodesvisible = true;
            linkcloudctrl.linksvisible = true;
            pathctrl.visible = true;
            RefreshRouteManGos();
        }
#endregion

#region birdcommands
        public void StartBird()
        {
            if (birdctrl.isAtGoal())
            {
                ReversePath();
            }
            birdctrl.StartBird();
        }
        public void PauseBird()
        {
            birdctrl.PauseBird();
        }
        public void UnPauseBird()
        {
            birdctrl.UnPauseBird();
        }
        public void StopBird()
        {
            if (pathctrl.path != null)
            {
                var bpos = birdctrl.GetBirdPos();
                var lpt = linkcloudctrl.PunchNewLinkPt(bpos,deleteparentlink:true);
                pathctrl.startnodename = lpt.name;
                pathctrl.GenAstarPath();
                PropagatePath();
                RefreshRouteManGos();
            }
        }
        public void ResetCalled()
        {
            if (pathctrl.path == null)
            {
                pathctrl.GenAstarPath();
            }
            birdctrl.ResetBirdToStartOfPath();
        }
        public void FasterBird()
        {
            birdctrl.AdjustSpeed(1.7f, 0.5f);
        }
        public void SlowerBird()
        {
            birdctrl.AdjustSpeed(1/1.7f, 0.5f);
        }
        public void SetSpeed(float newvel)
        {
            if (birdctrl.isAtStart())
            {
                StartBird();
            }
            birdctrl.SetSpeed(newvel);
        }
        public void DeleteBird()
        {
            birdctrl.DeleteBird();
        }
        public void NextBirdForm()
        {
            birdctrl.NextForm();
            birdctrl.RefreshGos();
        }
        public void FlyBirdHigher()
        {
            birdctrl.AdjustBirdHeight(1.25f);
        }
        public void FlyBirdLower()
        {
            birdctrl.AdjustBirdHeight( 1/1.25f );
        }
#endregion

        public void PropagatePath()
        {
            var path = pathctrl.path;
            birdctrl.SetBirdPath(path);
            errmarkctrl.SetErrorMarkPath(path);
        }

#region pipecommands
        public void Astar()
        {
            pathctrl.GenAstarPath();
            PropagatePath();
            RefreshRouteManGos();
        }
        public void SetStartNode(string newenodename)
        {
            if (birdctrl.isRunning())
            {
                StopBird();  // If we reset the endnode during running we need to stop and set a new node there
            }
            pathctrl.startnodename = newenodename;
            pathctrl.GenAstarPath();
            PropagatePath();
            RefreshRouteManGos();
        }
        public enum RoomActionE { makeDestination, orientOn, makeStart, makeHome }
        public RoomActionE roomAction = RoomActionE.makeDestination;

        public bool loadVoiceKeysForAllRooms=false;

        public void togglLoadVoiceKeysForAllRooms()
        {
            loadVoiceKeysForAllRooms = !loadVoiceKeysForAllRooms;
            RouteMan.Log("loadVoiceKeysForAllRooms set to:" + loadVoiceKeysForAllRooms);
        }

#region status info commands
    public void NextInfoMode()
        {
            statusctrl.NextInfoMode();
        }
        public void SetStatusInfoMode(StatusCtrl.outModeE mode)
        {
            statusctrl.outMode = mode;
            statusctrl.RealizeMode();
        }
        public void ScrollStatus(int n)
        {
            statusctrl.scroll(n);
        }
        public void ScrollPage(int n)
        {
            statusctrl.scrollpage(n);
        }
#endregion

        public void SetRoomAction(RoomActionE action)
        {
            roomAction = action;
        }
        public void NodeAction(string nodename)
        {
            //Debug.Log("NodeAction node:" + nodename + " action:" + roomAction);
            switch( roomAction )
            {
                case RoomActionE.orientOn:
                    {
                        OrientToEndNode(nodename);
                        break;
                    }
                case RoomActionE.makeDestination:
                    {
                        SetEndNode(nodename);
                        break;
                    }
                case RoomActionE.makeStart:
                    {
                        SetStartNode(nodename);
                        break;
                    }
                case RoomActionE.makeHome:
                    {
                        OrientToEndNode(nodename);
                        SetStartNode(nodename);
                        break;
                    }
            }
        }
        public void SetEndNode(string newenodename)
        {
            bool restartbird = false;
            if (!linkcloudctrl.islinkptname(newenodename)) return;
            if (birdctrl.isAtGoal())
            {
                //var tmp = pathctrl.startnodename;
                pathctrl.startnodename = pathctrl.endnodename;
                 // this keeps the path from hopping away to the start point
                 // when we change the end node when we are finished and want to go somewhere else
                 // note that calling ReversePath leads to a stackoverflow
            }
            if (birdctrl.isRunning())
            {
                StopBird();  // If we reset the endnode during running we need to stop and set a new node there
                restartbird = true;
            }
            pathctrl.endnodename = newenodename;
            pathctrl.GenAstarPath();
            PropagatePath();
            RefreshRouteManGos();
            if (restartbird)
            {
                StartBird(); 
            }
        }
        public void SetRandomEndNode()
        {
            var lpt = linkcloudctrl.GetRandomNode();
            SetEndNode( lpt.name );
        }
        public void ReversePath()
        {
            var tmp = pathctrl.startnodename;
            pathctrl.startnodename = pathctrl.endnodename;
            SetEndNode(tmp);
        }
        public void RandomPath()
        {
            pathctrl.GenRanPath();
            PropagatePath();
            RefreshRouteManGos();
        }
        public void DeletePath()
        {
            pathctrl.DeletePath();
        }
        public void TogglePathVisibily()
        {
            pathctrl.visible = !pathctrl.visible;
            pathctrl.RefreshGos();
        }
#endregion

        private List<string> highobjnames = new List<string>();
        private string highobactselname = "";
        public void saveHighobs()
        {
#if UNITY_EDITOR
            var selgos = UnityEditor.Selection.gameObjects;
           // string selojbnames = "";
            foreach (var selgo in selgos)
            {
                highobjnames.Add(selgo.name);
               // selojbnames += "|" + selgo.name;
            }
            var actgo = UnityEditor.Selection.activeGameObject;
            if (actgo != null)
            {
                highobactselname = actgo.name;
               // Debug.Log("Saved " + highobactselname);
            }
            else
            {
               // Debug.Log("Nothing selected");
            }
            //Debug.Log("Saved"+selojbnames);
#endif
        }
        void restoreHighobs()
        {
#if UNITY_EDITOR
            var actgo = GameObject.Find(highobactselname);
            if (actgo != null)
            {
                UnityEditor.Selection.activeGameObject = actgo;
               // Debug.Log("Restored:" + actgo.name);
            }
            else
            {
               // Debug.Log("Not Restored (actogo is null):" + highobactselname);
            }
            if (highobjnames.Count > 0)
            {
                List<GameObject> listgos = new List<GameObject>();
              //  string selogjnames = "";
                foreach (var selname in highobjnames)
                {
                    GameObject selgo = GameObject.Find(selname);
                    if (selgo != null)
                    {
                        listgos.Add(selgo);
                       // selogjnames += "|" + selgo.name;
                    }
                }
                var selgos = listgos.ToArray();
                UnityEditor.Selection.objects = selgos;
               // Debug.Log("Restored:" + selogjnames);
            }
#endif
        }
        void clearhighobs()
        {
            highobjnames.Clear();
            highobactselname = "";
        }

#region gameobject management
        public void RefreshRouteManGos()
        {
            // Cleanse the transform :)
            saveHighobs();
            rgo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            rgo.transform.localScale = Vector3.one;

// This is Unity wierdness - it should work doing things before
//            rgo.transform.localScale = new Vector3(rgoScale, rgoScale, rgoScale);
//            rgo.transform.Rotate(0, rgoRotate, 0);
//            rgo.transform.Translate(rgoTranslate);

            linkcloudctrl.RefreshGos();
            pathctrl.RefreshGos();
            birdctrl.RefreshGos();
            errmarkctrl.RefreshGos();
            floorplanctrl.RefreshGos();

// We have to do it afterwards - that is the trick :)
            rgo.transform.localScale = new Vector3(rgoScale, rgoScale, rgoScale);
            rgo.transform.Rotate(0, rgoRotate, 0);
            rgo.transform.Translate(rgoTranslate);
            rgoTransformSetCount += 1;

            RouteMan.Log("Refresh rgo - Scale:" + rgoScale + "  Rotate:" + rgoRotate + " Translate:" + rgoTranslate);
            keycount = keyman.totalKeywordCount();
            //restoreHighobs();

        }
#endregion

        public string startnodecolor = "green";
        public string endnodecolor = "red";
        public string linkcloudnodecolor = "blue";
        public string linkcloudnodecolorx = "steelblue";
        public string linkcloudlinkcolor = "yellow";
        public string pathnodecolor = "cyan";
        public string pathlinkcolor = "purple";
        public string pathlookatcolor = "steelblue";

        public enum RmColorModeE { startnode,endnode,linkcloudnode,linkcloudnodex,linkcloudlink,pathnode,pathlink,pathlookat }

        public string getcolorname(RmColorModeE mode,string name="")
        {
            var cname = "pink";
            if (mode == RmColorModeE.linkcloudnode || mode == RmColorModeE.linkcloudnodex)
            {
                if (name == pathctrl.startnodename) mode = RmColorModeE.startnode;
                if (name == pathctrl.endnodename) mode = RmColorModeE.endnode;
            }
            switch (mode)
            {
                case RmColorModeE.startnode: cname = startnodecolor; break;
                case RmColorModeE.endnode: cname = endnodecolor; ; break;
                case RmColorModeE.linkcloudnode: cname = linkcloudnodecolor; ; break;
                case RmColorModeE.linkcloudnodex: cname = linkcloudnodecolorx; ; break;
                case RmColorModeE.linkcloudlink: cname = linkcloudlinkcolor; ; break;
                case RmColorModeE.pathnode: cname = pathnodecolor; ; break;
                case RmColorModeE.pathlink: cname = pathlinkcolor ; break;
                case RmColorModeE.pathlookat: cname = pathlinkcolor; break;
            }
            return (cname);
        }
        private void Start()
        {
            initVars();
        }
        public void ToggleAutoErrorCorrect()
        {
            SetErrorCorrect( !autoerrorcorrect );
        }
        public void SetErrorCorrect(bool onoff)
        {
            autoerrorcorrect = onoff;
            RouteMan.Log("autoerrorcorrect now:" + autoerrorcorrect);
            if (autoerrorcorrect && errmarkctrl.markingState != ErrorMarkerCtrl.markingStateE.marking)
            {
                errmarkctrl.startMarking();
            }
        }

        static int updatesSinceRefresh = 0;
        private void Update()
        {
            if (autoerrorcorrect)
            {
                if (errmarkctrl.nMarksInList >= errmarkctrl.nErrmarkIntervalsInSet)
                {
                 //   Debug.Log("CorrectOnErrorMarkers to be called");
                    this.CorrectOnErrorMarkers();
                    errmarkctrl.startMarking();
                }
            }
            if (rgoTransformSetCount != lastRgoTransformSetCount)
            {
                rgoTransform = rgo.transform;
                lastRgoTransformSetCount = rgoTransformSetCount;
                if (rgoTransform != null)
                {
                    var m = rgoTransform.worldToLocalMatrix;
                    trans0 = m.GetRow(0);
                    trans1 = m.GetRow(1);
                    trans2 = m.GetRow(2);
                    trans3 = m.GetRow(3);
                    strans0 = trans0.ToString("F2");
                    strans1 = trans1.ToString("F2");
                    strans2 = trans2.ToString("F2");
                    strans3 = trans3.ToString("F2");
                }
            }
            if (needrefresh)
            {
                RefreshRouteManGos();
                updatesSinceRefresh = 0;
                needrefresh = false;
            }
            if (updatesSinceRefresh==2)
            {
                // this is a silly delay in restoring the selected objects 
                // that causes the selected objects to flash, 
                // but refreshing under 2 updates later provokes the dreaded 
                // "You are pushing more GUIClips than you are popping" message
                // from the Hierarchy View List window in the Unity Editor
                restoreHighobs();
                clearhighobs();
            }
            updatesSinceRefresh += 1;

        }
    }
}