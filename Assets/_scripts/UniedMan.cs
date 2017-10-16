using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using GraphAlgos;

/// <summary>
/// UniedMan - this class containes Unity3D editor specific code - code that cannot run on the HoloLens
/// </summary>
/// 
namespace BirdRouter
{
#if UNITY_EDITOR
    public class UniedMan : MonoBehaviour
    {
        // This class holds static methods that invoke RouteMan commands from the classic windows menu
        // these menu commands only work on the static methods and we didn't want to clutter RouteMan 
        // with them, although that may be the best option in the long run

        static RouteMan rman;

        public GUIText gt;

        public GameObject ground;
        public GameObject rmango;
        // Use this for initialization
        void Start()
        {
            // Create ground
            //ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //ground.name = "Ground-Plane";
            //ground.Transforms.localScale = new Vector3(50, 50, 25);
            //ground.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);

            // Create routeman
            //rmango = new GameObject("RouteMan");
            //rman = rmango.AddComponent<RouteMan>();
            //rman.rmango = rmango;

            gt = GetComponent<GUIText>();

            // Find routeman
            rman = FindObjectsOfType<RouteMan>()[0];
            RouteMan.Log("Assigned rman");
            rman.unitySim = true;
            rman.logfilenameroot = "c:\\transfer\\" + rman.logfilenameroot;

            var fsh = FindObjectsOfType<FollowShere>()[0];
            fsh.setRouteMan(rman);

            // Move light back a bit
            var dlight = GameObject.Find("Directional Light");           
            dlight.transform.localPosition = new Vector3(-1,0,-1);
        }


        #region global commands
        [MenuItem("--Glob/Refresh")]
        static void Refresh()
        {
            rman.Regen();
        }
        [MenuItem("--Glob/Toggle LinkCloud Visibility")]
        static void ToggleLinkCloudVisibily()
        {
            rman.ToggleLinkCloudVisibily();
        }
        [MenuItem("--Glob/Show Route")]
        static void ShowRoute()
        {
            rman.ShowRoute();
        }
        [MenuItem("--Glob/Hide Route")]
        static void HideRoute()
        {
            rman.HideRoute();
        }
        [MenuItem("--Glob/Show Links")]
        static void ShowLinks()
        {
            rman.ShowLinks();
        }
        [MenuItem("--Glob/Hide Links")]
        static void HideLinks()
        {
            rman.HideLinks();
        }
        [MenuItem("--Glob/Garnish")]
        static void Garnish()
        {
            rman.NextGarnish();
        }
        [MenuItem("--Glob/Toggle Floor Plan")]
        static void ShowFloorPlan()
        {
            rman.ToggleFloorPlan();
        }
        #endregion
        #region Transforms
        [MenuItem("--Trans/Set Home Height")]
        static void SetHomeHeight()
        {
            rman.ResetHomeHeight();
        }
        [MenuItem("--Trans/Grow")]
        static void Grow()
        {
            rman.Grow();
        }
        [MenuItem("--Trans/Shrink")]
        static void Shrink()
        {
            rman.Shrink();
        }
        [MenuItem("--Trans/Trans Left")]
        static void TranslateLeft()
        {
            rman.TranslateLeft();
        }
        [MenuItem("--Trans/Trans Right")]
        static void TranslateRight()
        {
            rman.TranslateRight();
        }
        [MenuItem("--Trans/Trans Up")]
        static void TranslateUp()
        {
            rman.TranslateUp();
        }
        [MenuItem("--Trans/Trans Down")]
        static void TranslateDown()
        {
            rman.TranslateDown();
        }
        [MenuItem("--Trans/Trans Forward")]
        static void TranslateForward()
        {
            rman.TranslateForward();
        }
        [MenuItem("--Trans/Trans Back")]
        static void TranslateBack()
        {
            rman.TranslateBack();
        }
        [MenuItem("--Trans/RotateCw")]
        static void RotateCw()
        {
            rman.RotateCw();
        }
        [MenuItem("--Trans/RotateCcw")]
        static void RotateCcw()
        {
            rman.RotateCcw();
        }
        [MenuItem("--Trans/RotateCw 30")]
        static void RotateCw30()
        {
            rman.RotateEverything(30);
        }
        [MenuItem("--Trans/RotateCcw 30")]
        static void RotateCcw30()
        {
            rman.RotateEverything(-30);
        }
        [MenuItem("--Trans/Grow 1")]
        static void Grow01()
        {
            rman.Grow01();
        }
        [MenuItem("--Trans/Grow 10")]
        static void Grow10()
        {
            rman.Grow10();
        }
        [MenuItem("--Trans/Grow 50")]
        static void Grow50()
        {
            rman.Grow50();
        }
        [MenuItem("--Trans/Shrink 1")]
        static void Shrink01()
        {
            rman.Shrink01();
        }
        [MenuItem("--Trans/Shrink 10")]
        static void Shrink10()
        {
            rman.Shrink10();
        }
        [MenuItem("--Trans/Shrink 50")]
        static void Shrink50()
        {
            rman.Shrink50();
        }
        #endregion

        #region status
        [MenuItem("--Stat/Write Log")]
        static void WriteLog()
        {
            rman.writeLogToFile();
        }
        [MenuItem("--Stat/Hide Status")]
        static void HideStatus()
        {
            rman.SetStatusInfoMode(StatusCtrl.outModeE.none);
        }
        [MenuItem("--Stat/Show Status")]
        static void ShowStatus()
        {
            rman.SetStatusInfoMode(StatusCtrl.outModeE.geninfo);

        }
        [MenuItem("--Stat/Trace")]
        static void ShowTrace()
        {
            rman.SetStatusInfoMode(StatusCtrl.outModeE.trace);

        }
        [MenuItem("--Stat/Voice Cmd History")]
        static void ShowVoiceKeys()
        {
            rman.SetStatusInfoMode(StatusCtrl.outModeE.voiceCmdHistory);

        }
        [MenuItem("--Stat/Help")]
        static void ShowHelp()
        {
            rman.SetStatusInfoMode(StatusCtrl.outModeE.help);

        }
        [MenuItem("--Stat/Next Status Mode")]
        static void NextStatusMode()
        {
            rman.NextInfoMode();
        }
        [MenuItem("--Stat/Scroll Up")]
        static void ScrollUp()
        {
            rman.ScrollStatus(-1);
        }
        [MenuItem("--Stat/Scroll Down")]
        static void ScrollDown()
        {
            rman.ScrollStatus(+1);
        }
        [MenuItem("--Stat/Page Up")]
        static void ScrollPageUp()
        {
            rman.ScrollPage(-1);
        }
        [MenuItem("--Stat/Page Down")]
        static void ScrollPageDown()
        {
            rman.ScrollPage(+1);
        }
        [MenuItem("--Stat/Scroll Top")]
        static void ScrollTop()
        {
            rman.ScrollStatus(-100000);
        }
        [MenuItem("--Stat/Scroll Bottom")]
        static void ScrollBottom()
        {
            rman.ScrollStatus(+100000);
        }
        #endregion


        #region generate commands
        [MenuItem("--Gen/Clean Start")]
        static void CleanStart()
        {
            rman.CleanStart();
        }
        [MenuItem("--Gen/Generate &g")]
        static void CreateGenerate()
        {
            rman.Gen43_1();
        }
        [MenuItem("--Gen/Gen BHO")]
        static void GenBHO()
        {
            rman.GenBHO();
        }
        [MenuItem("--Gen/Gen 43-1")]
        static void Gen43_1()
        {
            rman.Gen43_1();
        }
        [MenuItem("--Gen/Gen 43-2")]
        static void Gen43_2()
        {
            rman.Gen43_2();
        }
        [MenuItem("--Gen/Gen 43-3")]
        static void Gen43_3()
        {
            rman.Gen43_3();
        }
        [MenuItem("--Gen/Gen 43-4")]
        static void Gen43_4()
        {
            rman.Gen43_4();
        }
        [MenuItem("--Gen/Gen 43 1+2")]
        static void Gen431p2()
        {
            rman.Gen431p2();
        }
        [MenuItem("--Gen/GenRedwb 3 Simple")]
        static void GenRedwb3s()
        {
            rman.GenRedwb3simple();
        }
        [MenuItem("--Gen/GenRedwb 3")]
        static void GenRedwb3()
        {
            rman.GenRedwb3();
        }
        [MenuItem("--Gen/GenSphere")]
        static void GenSphere()
        {
            rman.GenSphere();
        }
        [MenuItem("--Gen/GenCirc")]
        static void GenCirc()
        {
            rman.GenCirc();
        }
        [MenuItem("--Gen/Load Route Data File")]
        static void LoadRouteData()
        {
            string path = EditorUtility.OpenFilePanel("Load Route Data File", rman.routeDataDir, "json");
            if (path.Length != 0)
            {
                rman.GenFromJsonFile(path);
            }
        }
        [MenuItem("--Gen/Save Route Data File")]
        static void SaveRouteData()
        {
            var path = EditorUtility.SaveFilePanel(
                    "Save Route Data as json",
                    rman.routeDataDir,
                    "data.json",
                    "json");

            if (path.Length != 0)
            {
                rman.SaveToJsonFile(path);
            }
        }
        #endregion
        #region linkcloud commands
        [MenuItem("--LinkCloud/Delete All")]
        static void DelLinkCloud()
        {
            rman.DeleteLinkCloud();
        }

        [MenuItem("--LinkCloud/Delete Selected Link")]
        static void DelSelLink()
        {
            var selobj = UnityEditor.Selection.activeGameObject;
            if (selobj != null)
            {
                rman.DeleteLink(selobj.name);
            }
        }
        [MenuItem("--LinkCloud/Delete Selected Node")]
        static void DelSelNode()
        {
            var selobj = UnityEditor.Selection.activeGameObject;
            if (selobj != null)
            {
                rman.DeleteNode(selobj.name);
            }
        }
        [MenuItem("--LinkCloud/Split Selected Link &s")]
        static void SplitlSelLink()
        {
            var selobj = UnityEditor.Selection.activeGameObject;
            if (selobj != null)
            {
                rman.SplitLink(selobj.name);
            }
        }
        [MenuItem("--LinkCloud/Stretch Node &n")]
        static void StretchNode()
        {
            var selobj = UnityEditor.Selection.activeGameObject;
            if (selobj != null)
            {
                rman.StartStretchMode(selobj.name);
            }
        }
        [MenuItem("--LinkCloud/LinkUp Nodes")]
        static void LinkUpNodes()
        {
            var selobjs = UnityEditor.Selection.objects;
            if (selobjs != null)
            {
//                rman.StartStretchMode(selobj.name);
            }
        }
        [MenuItem("--LinkCloud/Noise Up Nodes")]
        static void NoiseUpLinkCloud()
        {
            var ok = EditorUtility.DisplayDialog("Are you sure you want to noise things up?",
                                                 "Really sure?", "Yes, noise them up", "No thanks");
            if (ok) {
                rman.NoiseUpNodes(1);
            }
        }
        #endregion

        #region bird commands
        [MenuItem("--Bird/Start")]
        static void StartBird()
        {
            rman.StartBird();
        }
        [MenuItem("--Bird/Pause")]
        static void PauseBird()
        {
            rman.PauseBird();
        }
        [MenuItem("--Bird/UnPause")]
        static void UnPauseBird()
        {
            rman.UnPauseBird();
        }
        [MenuItem("--Bird/Stop")]
        static void StopBird()
        {
            rman.StopBird();
        }
        [MenuItem("--Bird/Reset")]
        static void ResetBird()
        {
            rman.ResetCalled();
        }
        [MenuItem("--Bird/Faster")]
        static void FasterBird()
        {
            rman.FasterBird();
        }
        [MenuItem("--Bird/Slower")]
        static void SlowerBird()
        {
            rman.SlowerBird();
        }
        [MenuItem("--Bird/Change Form")]
        static void ChangeForm()
        {
            rman.NextBirdForm();
        }
        [MenuItem("--Bird/Delete Bird")]
        static void DeleteBird()
        {
            rman.DeleteBird();
        }
        [MenuItem("--Voice/Inc KW Limit")]
        static void IncKwLimit()
        {
            rman.IncKeyLimit();
        }
        [MenuItem("--Voice/Dec KW Limit")]
        static void DecKwLimit()
        {
            rman.DecKeyLimit();
        }

        #endregion

        #region Error Correction
        [MenuItem("--ErrFix/Start Error Marking")]
        static void StartErrorMarking()
        {
            rman.StartErrorMarking();
        }
        [MenuItem("--ErrFix/Finish Error Marking")]
        static void FinishErrorMarking()
        {
            rman.FinishErrorMarking();
        }
        [MenuItem("--ErrFix/Correct On Error Marking")]
        static void CorrectOnErrorMarking()
        {
            rman.CorrectOnErrorMarkers();
        }
        [MenuItem("--ErrFix/Toggle Auto Error Correct")]
        static void ToggleAutoErrorCorrect()
        {
            rman.ToggleAutoErrorCorrect();
        }
        #endregion

        #region pathcommands
        [MenuItem("--Path/Correct Position")]
        static void CorrectPosition()
        {
            rman.CorrectPosition();
        }
        [MenuItem("--Path/Correct Angle")]
        static void CorrectAngle()
        {
            rman.CorrectAngle();
        }
        [MenuItem("--Path/Correct Both")]
        static void CorrectBoth()
        {
            rman.CorrectPositionAndAngle();
        }
        [MenuItem("--Path/A-star Path")]
        static void Astar()
        {
            rman.Astar();
        }
        [MenuItem("--Path/Set Start Node")]
        static void SetStartNode()
        {
            var selobj = UnityEditor.Selection.activeGameObject;
            if (selobj != null)
            {
                rman.SetStartNode(selobj.name);
            }

        }
        [MenuItem("--Path/Set End Node")]
        static void SetEndNode()
        {
            var selobj = UnityEditor.Selection.activeGameObject;
            if (selobj != null)
            {
                rman.SetEndNode(selobj.name);
            }
        }
        [MenuItem("--Path/Random End Node")]
        static void RandomEndNode()
        {
            rman.SetRandomEndNode();
        }
        [MenuItem("--Path/Reverse Path")]
        static void ReversePath()
        {
            rman.ReversePath();
        }
        [MenuItem("--Path/Random Path")]
        static void RandomPath()
        {
            rman.RandomPath();
        }
        [MenuItem("--Path/Delete Path")]
        static void DeletePath()
        {
            rman.DeletePath();
        }
        [MenuItem("--Path/Toggle Path Visibility")]
        static void TogglePathVisibily()
        {
            rman.TogglePathVisibily();
        }
        #endregion

        #region room commands
        [MenuItem("--Room/Toggle Load All Rooms")]
        static void ToggleLoadAllRooms()
        {
            rman.togglLoadVoiceKeysForAllRooms();
        }
        [MenuItem("--Room/Make Start")]
        static void MakeStart()
        {
            rman.SetRoomAction(RouteMan.RoomActionE.makeStart);
        }
        [MenuItem("--Room/Orient On")]
        static void OrientOn()
        {
            rman.SetRoomAction(RouteMan.RoomActionE.orientOn);
        }
        [MenuItem("--Room/Make Destination")]
        static void MakeDestination()
        {
            rman.SetRoomAction(RouteMan.RoomActionE.makeDestination);
        }
        [MenuItem("--Room/Home")]
        static void MakeHome()
        {
            rman.SetRoomAction(RouteMan.RoomActionE.makeHome);
        }
        [MenuItem("--Room/Lobby 1")]
        static void DestRoomLobby1()
        {
            rman.NodeAction("f01-dt-st01");
        }
        [MenuItem("--Room/Kitchen 1")]
        static void DestRoomKitchen1()
        {
            rman.NodeAction("f01-dt-k01");
        }
        [MenuItem("--Room/Room 1001")]
        static void DestRoom1001()
        {
            rman.NodeAction("f01-dt-rm1001");
        }
        [MenuItem("--Room/Room 1002")]
        static void DestRoom1002()
        {
            rman.NodeAction("f01-dt-rm1002");
        }
        [MenuItem("--Room/Room 1003")]
        static void DestRoom1003()
        {
            rman.NodeAction("f01-dt-rm1003");
        }
        [MenuItem("--Room/Room 1004")]
        static void DestRoom1004()
        {
            rman.NodeAction("f01-dt-rm1004");
        }
        [MenuItem("--Room/Room 1005")]
        static void DestRoom1005()
        {
            rman.NodeAction("f01-dt-rm1005");
        }
        [MenuItem("--Room/Room 1006")]
        static void DestRoom1006()
        {
            rman.NodeAction("f01-dt-rm1006");
        }
        [MenuItem("--Room/Room 1007")]
        static void DestRoom1007()
        {
            rman.NodeAction("f01-dt-rm1007");
        }
        [MenuItem("--Room/Room 1008")]
        static void DestRoom1008()
        {
            rman.NodeAction("f01-dt-rm1008");
        }
        [MenuItem("--Room/Room 1009")]
        static void DestRoom1009()
        {
            rman.NodeAction("f01-dt-rm1009");
        }
        [MenuItem("--Room/Room 1012")]
        static void DestRoom1012()
        {
            rman.NodeAction("f01-dt-rm1012");
        }
        [MenuItem("--Room/Room 1013")]
        static void DestRoom1013()
        {
            rman.NodeAction("f01-dt-rm1013");
        }
        [MenuItem("--Room/Room 1014")]
        static void DestRoom1014()
        {
            rman.NodeAction("f01-dt-rm1014");
        }
        [MenuItem("--Room/Room 1015")]
        static void DestRoom1015()
        {
            rman.NodeAction("f01-dt-rm1015");
        }
        [MenuItem("--Room/Room 3254 (Anand)")]
        static void DestRoom3254()
        {
            rman.NodeAction("rw--f03-rm3254");
        }
        [MenuItem("--Room/Room 3170 (Ramesh)")]
        static void DestRoom3170()
        {
            rman.NodeAction("rw--f03-rm3170");
        }
        [MenuItem("--Room/Room 3256 (Rimes)")]
        static void DestRoom3256()
        {
            rman.NodeAction("rw--f03-rm3256");
        }
        [MenuItem("--Room/Room 3258 (Bruce)")]
        static void DestRoom3258()
        {
            rman.NodeAction("rw--f03-rm3258");
        }
        [MenuItem("--Room/Room 3268 (Markmerc)")]
        static void DestRoom3268()
        {
            rman.NodeAction("rw--f03-rm3268");
        }

        [MenuItem("--Room/Room 3370 (Mark)")]
        static void DestRoom3370()
        {
            rman.NodeAction("rw--f03-rm3370");
        }
        [MenuItem("--Room/Room 3210 (Spyros)")]
        static void DestRoom3210()
        {
            rman.NodeAction("rw--f03-rm3210");
        }
        #endregion


        // Update is called once per frame
        void Update()
        {
            foreach (char c in Input.inputString)
            {
                RouteMan.Log("hit "+c);
                switch (c)
                {
                    case 's': {
                            SplitlSelLink();
                            break;
                        }
                }
            }
        }
    }
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
