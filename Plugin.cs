using BepInEx;
using HarmonyLib;
using UnityEngine;
using SoupAPI;

namespace scpvg
{
    [BepInPlugin("com.kirisoup.hff.planescenegen", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Human.exe")]
    public partial class Plugin : BaseUnityPlugin
    {
        readonly Harmony harmony = new("com.kirisoup.hff.planescenegen");

        public static readonly string msgHeader = $"@ {PluginInfo.PLUGIN_NAME}: ";

        static readonly string basePath = "BepInEx/plugins/";

        void Start()
        {
            harmony.PatchAll();

            if (Game.currentLevel != null) VisSp();
        }

        void OnDestroy()
        {
            Destroy(spv);
            Destroy(planeCam);
        }

        [HarmonyPatch(typeof(Game), "AfterLoad")]
        class AfterLoad
        {
            [HarmonyPostfix]
            static void InitSpVis() => VisSp();
        }

        static GameObject spv;

        Camera planeCam;

        RenderTexture rtex;

        static float camheight = 60;
        static float camx = 0;
        static float camz = 0;

        static bool enlarged;

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl)) if (Input.GetKeyDown(KeyCode.S)) Save();

            if (Input.GetKeyDown(KeyCode.M)) enlarged = !enlarged;

            if (!enlarged) return;

            float spd = Input.GetKey(KeyCode.LeftShift) ? 0.5f : Input.GetKey(KeyCode.LeftControl) ? 10 : 1;


            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0f) camheight += Input.GetAxis("Mouse ScrollWheel") * spd;

                if (Input.GetKey(KeyCode.LeftArrow)) camx -= spd;
                if (Input.GetKey(KeyCode.RightArrow)) camx += spd;

                if (Input.GetKey(KeyCode.UpArrow)) camz += spd;
                if (Input.GetKey(KeyCode.DownArrow)) camz -= spd;
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0f) planeCam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * spd;

                if (Input.GetKeyDown(KeyCode.LeftArrow)) camx -= spd;
                if (Input.GetKeyDown(KeyCode.RightArrow)) camx += spd;

                if (Input.GetKeyDown(KeyCode.UpArrow)) camz += spd;
                if (Input.GetKeyDown(KeyCode.DownArrow)) camz -= spd;
            }


        }

        void OnGUI()
        {
            if (Game.currentLevel == null) return;

            if (planeCam == null)
            {
                planeCam = new GameObject("planecam") { transform = { parent = transform } }.AddComponent<Camera>();
                planeCam.orthographic = true;
                planeCam.orthographicSize = 200;
            }
            
            if (rtex == null)
            {
                rtex = new RenderTexture(Screen.height, Screen.height, 16, RenderTextureFormat.ARGB32);
                if (!rtex.IsCreated()) rtex.Create();
            }
            
            if (planeCam != null)
            {
                planeCam.transform.position = new Vector3(camx, Human.Localplayer.transform.position.y + camheight, camz);

                planeCam.transform.rotation = Quaternion.Euler(90f, Mathf.Round(Human.Localplayer.controls.cameraYawAngle / 90f) * 90f, 0f);

                planeCam.targetTexture = rtex;

                if (!enlarged) GUI.Box(new Rect(Screen.height*3/4 + (Screen.width - Screen.height), (float)Screen.height*3/4, (float)Screen.height/4, (float)Screen.height/4), rtex, GUIStyle.none);
                else GUI.Box(new Rect((Screen.width - Screen.height)/2, 0f, Screen.height, Screen.height), rtex, GUIStyle.none);
            }
        }

        static void VisSp()
        {
            Destroy(spv);
            spv = ObjMajic.CreateDummySphere("spv", 0.5f, Game.currentLevel.checkpoints[0].transform.position + Vector3.up * 50f, Color.magenta);
        }

        static void Save()
        {
            Camera planecam_ = FindObjectOfType<Plugin>().planeCam;

            RenderTexture ortex = new(7680, 7680, 24, RenderTextureFormat.ARGB32);

            planecam_.targetTexture = ortex;

            Texture2D otex = new(7680, 7680, TextureFormat.RGB24, false);   

            planecam_.Render();

            RenderTexture.active = ortex;

            otex.ReadPixels(new Rect(0, 0, ortex.width, ortex.height), 0, 0);

            otex.Apply();

            byte[] bytes = otex.EncodeToPNG();

            System.IO.File.WriteAllBytes(basePath + "/levelplane.png", bytes);

            RenderTexture.active = null;
        }
    }
}