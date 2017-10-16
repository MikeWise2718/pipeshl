using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphAlgos
{
    public enum genmodeE
    {
        gen_none, gen_circ, gen_sphere, gen_b43_1, gen_b43_2, gen_b43_3, gen_b43_4, gen_b43_1p2,
        gen_bho, gen_small, gen_redwb_3_simple, gen_redwb_3, gen_json_file
    };

    public class GenGlobeParameters
    {
        public int nlng;
        public int nlat;
        public float radius;
        public float height;
        public GenGlobeParameters(int nlng = 12, int nlat = 12, float radius = 10, float height = 10)
        {
            this.nlng = nlng;
            this.nlat = nlat;
            this.radius = radius;
            this.height = height;
        }
    }
    public class GenCircParameters
    {
        public int npoints;
        public float radius;
        public float height;

        public GenCircParameters(int npoints = 6, float radius = 10, float height = 0)
        {
            this.npoints = npoints;
            this.radius = radius;
            this.height = height;
        }
    }
    public class GenFileParameters
    {
        public string fullfilename;

        public GenFileParameters()
        {
            this.fullfilename = "";
        }
        public string getFullFileName()
        {
            string rv = fullfilename;
            return rv;
        }
    }
    public class MapGenParameters
    {
        public GenGlobeParameters glopar = new GenGlobeParameters();
        public GenCircParameters circpar = new GenCircParameters();
        public GenFileParameters filepar = new GenFileParameters();
    }
    public class MapMaker
    {
        LinkCloud lc;
        MapGenParameters mgp = new MapGenParameters();
        public MapMaker(LinkCloud lc, MapGenParameters mgp = null)
        {
            if (mgp != null)
            {
                this.mgp = mgp;
            }
            this.lc = lc;
        }
        static Dictionary<string, int> modelcount = new Dictionary<string, int>();
        private string getmodelprefix(string modelprefix, bool forcecount = false)
        {
            if (modelcount.ContainsKey(modelprefix))
            {
                var ncount = modelcount[modelprefix]++;
                return modelprefix + (ncount + 1) + "-";
            }
            else
            {
                modelcount[modelprefix] = 0;
                if (forcecount)
                {
                    return modelprefix + "0-";
                }
                else
                {
                    return modelprefix;
                }
            }
        }
        public int maxVoiceKeywords = 70;
        public int nVoiceKeywords = 0;
        public void AddRedwB3rooms()
        {
            lc.addRoomLink("3261", 42.95f, 158.96f, "NA");
            lc.addRoomLink("3215", 145.14f, 187.71f, "BAPERRY");
            lc.addRoomLink("3377", 196.52f, 217.20f, "KIWATANA");
            lc.addRoomLink("3267", 56.81f, 144.34f, "MNARANJO");
            lc.addRoomLink("3381", 196.52f, 232.14f, "KABYSTRO,ALCARDEN");
            lc.addRoomLink("3375", 196.52f, 209.86f, "AMITAGRA");
            lc.addRoomLink("3359", 232.03f, 166.37f, "ABOCZAR");
            lc.addRoomLink("3353", 232.05f, 144.32f, "PETERYI");
            lc.addRoomLink("3173", 65.29f, 253.45f, "PKHANNA");
            lc.addRoomLink("3169", 65.29f, 275.39f, "BALUS");
            lc.addRoomLink("3374", 210.96f, 209.86f, "BLAIRSH");
            lc.addRoomLink("3257", 43.00f, 144.32f, "KATHLEES,OMASEK");
            lc.addRoomLink("3376", 210.96f, 217.20f, "MPIGGOTT");
            lc.addRoomLink("3129", 144.24f, 264.42f, "MARIANAQ");
            lc.addRoomLink("3205", 145.12f, 232.14f, "MATTPE");
            lc.addRoomLink("3282", 85.74f, 131.11f, "LAUPRES");
            lc.addRoomLink("3184", 107.90f, 240.24f, "GILPETTE");
            lc.addRoomLink("3207", 145.12f, 224.67f, "WENDYJ");
            lc.addRoomLink("3372", 210.96f, 202.51f, "FPACE");
            lc.addRoomLink("3069", 254.08f, 253.45f, "FAYEB");
            lc.addRoomLink("3335", 197.41f, 145.54f, "JEPEARSO,EUNICES");
            lc.addRoomLink("3221", 159.81f, 165.29f, "PHILIBRI");
            lc.addRoomLink("3253", 43.03f, 166.37f, "PAGUNASH");
            lc.addRoomLink("3385", 197.41f, 165.29f, "EVANI");
            lc.addRoomLink("3371", 196.52f, 195.17f, "NINDYHU");
            lc.addRoomLink("3073", 254.08f, 275.39f, "NA");
            lc.addRoomLink("3105", 197.41f, 274.17f, "SENTHILC,MKRANZ");
            lc.addRoomLink("3199", 159.81f, 254.68f, "ROSALYNV");
            lc.addRoomLink("3063", 231.59f, 254.68f, "NA");
            lc.addRoomLink("3378", 210.95f, 224.66f, "TOMFREE");
            lc.addRoomLink("3337", 197.41f, 155.42f, "NA");
            lc.addRoomLink("3103", 197.41f, 264.42f, "ANKURT,SIMRANS");
            lc.addRoomLink("3141", 122.08f, 274.30f, "THOKRAKU,SAWEAVER");
            lc.addRoomLink("3234", 115.24f, 179.47f, "NA");
            lc.addRoomLink("3155", 87.90f, 274.17f, "SACHAA");
            lc.addRoomLink("3179", 87.90f, 254.68f, "ROBERH");
            lc.addRoomLink("3370", 210.96f, 195.17f, "MARKKOTT");
            lc.addRoomLink("3089", 231.59f, 264.42f, "KRMARCHB");
            lc.addRoomLink("3185", 110.05f, 264.55f, "JOEGURA,LANAMAY");
            lc.addRoomLink("3097", 209.40f, 274.21f, "PURNAG");
            lc.addRoomLink("3223", 151.96f, 165.29f, "KAVENK");
            lc.addRoomLink("3087", 231.59f, 274.17f, "NA");
            lc.addRoomLink("3123", 159.81f, 274.17f, "SHTANYA");
            //     lc.addRoomLink("3115", 157.53f, 261.26f, "NA");
            lc.addRoomLink("3236", 107.90f, 179.47f, "EMILYM");
            //      lc.addRoomLink("3115", 160.82f, 261.26f, "NA");
            //       lc.addRoomLink("3115", 160.06f, 267.59f, "NA");
            lc.addRoomLink("3238", 100.56f, 179.47f, "SHYATT");
            //     lc.addRoomLink("3115", 163.35f, 267.59f, "SASO");
            lc.addRoomLink("3244", 78.28f, 179.47f, "LUCYHUR");
            lc.addRoomLink("3140", 116.00f, 264.42f, "NA");
            //      lc.addRoomLink("3115", 156.77f, 267.59f, "NA");
            lc.addRoomLink("3240", 93.21f, 179.47f, "NA");
            lc.addRoomLink("3348", 225.14f, 155.42f, "NA");
            lc.addRoomLink("3321", 163.35f, 152.38f, "ANIDH");
            lc.addRoomLink("3252", 56.25f, 179.47f, "SUSANNEV");
            // lc.addRoomLink("3321", 150.44f, 152.38f, "NA");
            // lc.addRoomLink("3321", 160.06f, 152.38f, "GIJOHN");
            lc.addRoomLink("3248", 63.59f, 179.47f, "TOMURPHY");
            lc.addRoomLink("3296", 116.00f, 155.42f, "NA");
            //  lc.addRoomLink("3115", 166.65f, 267.59f, "NA");
            lc.addRoomLink("3306", 138.16f, 155.42f, "NA");
            lc.addRoomLink("3326", 174.24f, 153.14f, "NA");
            lc.addRoomLink("3112", 174.24f, 266.58f, "NA");
            lc.addRoomLink("3274", 71.66f, 155.48f, "NA");
            lc.addRoomLink("3246", 70.93f, 179.47f, "MODEME");
            lc.addRoomLink("3284", 93.85f, 155.42f, "NA");
            lc.addRoomLink("3264", 49.92f, 155.42f, "NA");
            lc.addRoomLink("3242", 85.74f, 179.47f, "ERIKAH");
            lc.addRoomLink("3360", 226.28f, 179.47f, "NABINK");
            lc.addRoomLink("3168", 70.93f, 288.61f, "RICKOL");
            lc.addRoomLink("3158", 78.28f, 288.61f, "JALLEN");
            lc.addRoomLink("3156", 85.74f, 288.61f, "RLONGDEN");
            lc.addRoomLink("3152", 93.85f, 264.42f, "NA");
            //  lc.addRoomLink("3115", 154.36f, 261.26f, "NA");
            lc.addRoomLink("3148", 93.21f, 288.61f, "AMBROSEW");
            // lc.addRoomLink("3115", 150.06f, 267.59f, "NA");
            //  lc.addRoomLink("3115", 160.69f, 263.85f, "NA");
            lc.addRoomLink("3340", 203.36f, 155.42f, "NA");
            lc.addRoomLink("3146", 100.56f, 288.61f, "ALEXMUK");
            // lc.addRoomLink("3115", 153.48f, 267.59f, "NA");
            lc.addRoomLink("3144", 107.90f, 288.61f, "SHAUNH");
            lc.addRoomLink("3115", 164.11f, 261.26f, "SAIEMA");
            lc.addRoomLink("3090", 225.64f, 264.42f, "NA");
            lc.addRoomLink("3142", 115.24f, 288.61f, "LUISTO");
            lc.addRoomLink("3080", 247.29f, 264.42f, "NA");
            lc.addRoomLink("3102", 203.36f, 264.42f, "NA");
            lc.addRoomLink("3166", 72.20f, 264.42f, "NA");
            lc.addRoomLink("3310", 138.29f, 264.42f, "NA");
            lc.addRoomLink("3391", 179.58f, 177.81f, "NA");
            lc.addRoomLink("3100", 204.12f, 288.61f, "CARLACAS");
            lc.addRoomLink("3134", 137.53f, 288.61f, "LISAOL");
            lc.addRoomLink("3075", 253.62f, 268.10f, "NA");
            lc.addRoomLink("3401", 152.21f, 195.05f, "NA");
            lc.addRoomLink("3136", 130.18f, 288.61f, "BKRAFFT");
            lc.addRoomLink("3138", 122.71f, 288.61f, "DOTTIES");
            lc.addRoomLink("3037", 152.21f, 224.67f, "NA");
            lc.addRoomLink("3167", 65.61f, 268.10f, "KOBELL");
            lc.addRoomLink("3096", 218.81f, 288.61f, "SQUINN");
            lc.addRoomLink("3259", 43.33f, 151.62f, "KERAINES");
            lc.addRoomLink("3351", 231.72f, 151.62f, "GORKEMY");
            lc.addRoomLink("3098", 211.46f, 288.61f, "SCHUMA");
            lc.addRoomLink("3027", 158.24f, 215.20f, "NA");
            lc.addRoomLink("3403", 158.24f, 204.64f, "NA");
            lc.addRoomLink("3108", 181.97f, 287.69f, "NA");
            lc.addRoomLink("3327", 185.26f, 155.42f, "NA");
            lc.addRoomLink("3094", 226.28f, 288.61f, "MARCBAX");
            lc.addRoomLink("3111", 185.26f, 264.42f, "NA");
            lc.addRoomLink("3086", 233.75f, 288.61f, "ADRIENR");
            lc.addRoomLink("3254", 45.05f, 179.40f, "ANANDE");
            lc.addRoomLink("3041", 184.29f, 240.26f, "NA");
            //       lc.addRoomLink("3321", 161.71f, 158.33f, "HOLLIS");
            lc.addRoomLink("3178", 85.74f, 240.24f, "DREWG");
            //       lc.addRoomLink("3321", 153.73f, 152.38f, "ANDDAL");
            lc.addRoomLink("3186", 115.24f, 240.24f, "TIMTHO");
            lc.addRoomLink("3074", 248.43f, 288.61f, "MIKEPAL");
            lc.addRoomLink("3084", 241.09f, 288.61f, "ERICDAI");
            //       lc.addRoomLink("3321", 169.81f, 152.38f, "NA");
            lc.addRoomLink("3174", 70.93f, 240.24f, "MARLAB");
            //     lc.addRoomLink("3321", 155.25f, 158.33f, "CHMCMU");
            lc.addRoomLink("3062", 226.28f, 240.24f, "ANGELACO");
            //   lc.addRoomLink("3321", 164.87f, 158.33f, "NA");
            lc.addRoomLink("3180", 93.21f, 240.24f, "LCOZZENS");
            //    lc.addRoomLink("3321", 158.54f, 158.33f, "NA");
            lc.addRoomLink("3176", 78.28f, 240.24f, "JUANCOL");
            lc.addRoomLink("3043", 186.04f, 255.03f, "NA");
            lc.addRoomLink("3389", 186.09f, 164.93f, "NA");
            lc.addRoomLink("3399", 162.82f, 195.03f, "NA");
            //     lc.addRoomLink("3321", 166.52f, 152.38f, "RYBER");
            //    lc.addRoomLink("3321", 156.90f, 152.38f, "AMLUND");
            lc.addRoomLink("3033", 162.94f, 224.69f, "NA");
            lc.addRoomLink("3064", 233.75f, 240.24f, "RGUSTAFS");
            lc.addRoomLink("3288", 100.56f, 131.11f, "LPAPPS");
            lc.addRoomLink("3270", 63.59f, 131.11f, "DALEW");
            lc.addRoomLink("3258", 48.78f, 131.11f, "BRUJO");
            lc.addRoomLink("3278", 70.93f, 131.11f, "WFONG");
            lc.addRoomLink("3039", 159.81f, 237.67f, "NA");
            lc.addRoomLink("3268", 56.25f, 131.11f, "MMERCURI");
            lc.addRoomLink("3290", 107.90f, 131.11f, "KLEADER");
            lc.addRoomLink("3334", 137.40f, 138.46f, "NA");
            lc.addRoomLink("3314", 152.34f, 131.11f, "KFILE");
            lc.addRoomLink("3143", 159.94f, 281.27f, "NA");
            lc.addRoomLink("3286", 93.21f, 131.11f, "MERTB");
            lc.addRoomLink("3233", 140.80f, 172.22f, "NA");
            // lc.addRoomLink("3185", 160.95f, 247.59f, "NA");
            lc.addRoomLink("3393", 160.37f, 181.19f, "NA");
            lc.addRoomLink("3304", 137.53f, 131.11f, "DMOREH");
            lc.addRoomLink("3313", 159.81f, 145.54f, "HALBER");
            lc.addRoomLink("3312", 144.87f, 131.11f, "MHOISECK");
            lc.addRoomLink("3316", 159.81f, 131.11f, "CORINMAR");
            lc.addRoomLink("3298", 115.24f, 131.11f, "NICOLM");
            lc.addRoomLink("3300", 122.71f, 131.11f, "XAVIERP");
            lc.addRoomLink("3302", 130.18f, 131.11f, "MARKCROF");
            lc.addRoomLink("3068", 248.43f, 240.24f, "CARRIEAM");
            lc.addRoomLink("3066", 241.09f, 240.24f, "MSELIN");
            lc.addRoomLink("3342", 204.12f, 131.11f, "LAURALON");
            lc.addRoomLink("3099", 203.49f, 209.86f, "NA");
            lc.addRoomLink("3308", 138.16f, 209.86f, "NA");
            lc.addRoomLink("3128", 144.87f, 288.61f, "MPEREZ");
            lc.addRoomLink("3354", 226.28f, 131.11f, "TERRIM");
            lc.addRoomLink("3346", 218.81f, 131.11f, "MIKEMOL");
            lc.addRoomLink("3344", 211.46f, 131.11f, "THDRELLE");
            lc.addRoomLink("3153", 87.90f, 264.42f, "MANDLAM,KSBAFNA");
            lc.addRoomLink("3060", 218.82f, 240.25f, "TSCHMIDT");
            lc.addRoomLink("3362", 218.82f, 179.46f, "TSTORCH");
            lc.addRoomLink("3145", 110.05f, 274.30f, "RADEOK");
            lc.addRoomLink("3122", 159.81f, 288.61f, "TODDGAR");
            lc.addRoomLink("3237", 99.92f, 165.29f, "KRISTENQ");
            lc.addRoomLink("3188", 122.70f, 240.26f, "SBUCHAN");
            lc.addRoomLink("3124", 152.34f, 288.61f, "DEREKMO");
            lc.addRoomLink("3101", 209.37f, 264.38f, "PABLOJB");
            lc.addRoomLink("3232", 122.70f, 179.45f, "LUZJARA");
            lc.addRoomLink("3225", 144.24f, 165.29f, "SUSKA,PABHANDA");
            lc.addRoomLink("3231", 122.08f, 165.29f, "EVANW,BIBARF");
            lc.addRoomLink("3247", 65.61f, 165.29f, "JANC");
            lc.addRoomLink("3218", 130.83f, 187.72f, "ALICEC");
            lc.addRoomLink("3204", 130.83f, 232.13f, "JASONLEE");
            lc.addRoomLink("3297", 122.08f, 155.42f, "ROBESM");
            lc.addRoomLink("3309", 144.24f, 155.42f, "AKANGAW,PGURU");
            lc.addRoomLink("3285", 99.92f, 155.42f, "DOHAMI");
            lc.addRoomLink("3206", 130.82f, 224.67f, "GREGGPI");
            lc.addRoomLink("3208", 130.82f, 217.20f, "CSLOTTA");
            lc.addRoomLink("3210", 130.82f, 209.86f, "SPYROS");
            lc.addRoomLink("3273", 65.61f, 155.42f, "NA");
            lc.addRoomLink("3271", 65.62f, 145.55f, "BSMIT");
            lc.addRoomLink("3212", 130.82f, 202.51f, "ALIHOB");
            lc.addRoomLink("3216", 130.82f, 195.17f, "PUVITH");
            lc.addRoomLink("3182", 100.56f, 240.24f, "JMEIER");
            lc.addRoomLink("3280", 78.28f, 131.11f, "SHINOY");
            lc.addRoomLink("3287", 99.92f, 145.54f, "SPATHANI");
            lc.addRoomLink("3301", 122.08f, 145.54f, "RODOLPHD");
            lc.addRoomLink("3311", 144.24f, 145.54f, "TACRIS,BRITTB");
            lc.addRoomLink("3293", 108.79f, 151.62f, "NICONS,JANEENS");
            lc.addRoomLink("3121", 167.66f, 274.17f, "ISABELF");
            lc.addRoomLink("3235", 108.79f, 166.43f, "LANIO");
            lc.addRoomLink("3245", 78.91f, 166.43f, "AHANSON");
            lc.addRoomLink("3291", 108.79f, 144.28f, "RSHARPL");
            lc.addRoomLink("3227", 130.94f, 166.43f, "MLALL");
            lc.addRoomLink("3307", 130.94f, 158.96f, "PKHODAK,JELIPE");
            lc.addRoomLink("3281", 87.81f, 145.51f, "SUSANJA");
            lc.addRoomLink("3305", 130.94f, 151.62f, "VAIBHAVA,JOEMRICK");
            lc.addRoomLink("3295", 108.79f, 158.96f, "STFRANK,JONSAMP");
            lc.addRoomLink("3197", 151.96f, 254.68f, "DDECATUR");
            lc.addRoomLink("3201", 167.66f, 254.68f, "ALEXISC");
            lc.addRoomLink("3181", 101.06f, 253.41f, "CHBARRET,ANDYEUN");
            lc.addRoomLink("3195", 144.24f, 254.68f, "ROBSIMP,BEROMO");
            lc.addRoomLink("3059", 218.43f, 253.41f, "DASCHWIE");
            lc.addRoomLink("3183", 110.05f, 254.68f, "VINELAP");
            lc.addRoomLink("3131", 131.07f, 260.75f, "FCORTES");
            lc.addRoomLink("3133", 131.07f, 268.10f, "PABERNAL");
            lc.addRoomLink("3135", 131.07f, 275.44f, "DILIPSIN");
            lc.addRoomLink("3243", 87.77f, 165.29f, "KEROSH");
            lc.addRoomLink("3367", 210.58f, 166.43f, "FRANKP");
            lc.addRoomLink("3051", 197.41f, 254.68f, "YVISHWA");
            lc.addRoomLink("3191", 131.07f, 253.41f, "JOLLYK,BREULAND");
            lc.addRoomLink("3190", 122.08f, 264.55f, "JOMANNIN,ILOSTFEL");
            lc.addRoomLink("3125", 151.96f, 274.17f, "CALVIND");
            lc.addRoomLink("3187", 122.08f, 254.68f, "JAYANG");
            lc.addRoomLink("3127", 144.24f, 274.17f, "ALESCURE");
            lc.addRoomLink("3163", 79.16f, 260.75f, "ALISONLU");
            lc.addRoomLink("3161", 79.16f, 268.10f, "GARRETTD,PASEHG");
            lc.addRoomLink("3175", 79.16f, 253.41f, "MARKBES");
            lc.addRoomLink("3363", 219.31f, 165.29f, "UFUKT");
            lc.addRoomLink("3256", 41.23f, 131.26f, "RIMESM");
            lc.addRoomLink("3072", 255.98f, 288.47f, "SCOTTCLA");
            lc.addRoomLink("3279", 78.91f, 144.15f, "BRENTSIN");
            lc.addRoomLink("3172", 63.47f, 240.35f, "FRANKPET");
            lc.addRoomLink("3070", 255.98f, 240.38f, "ACORLEY");
            lc.addRoomLink("3055", 209.44f, 254.68f, "YAMAGDI,MAKASIEW");
            lc.addRoomLink("3219", 167.66f, 165.29f, "YELENAK");
            lc.addRoomLink("3275", 78.91f, 159.09f, "JOCLARKE");
            lc.addRoomLink("3380", 210.70f, 232.14f, "LIZD");
            lc.addRoomLink("3170", 63.47f, 288.50f, "RSIVA");
            lc.addRoomLink("3368", 210.70f, 187.70f, "MARCOAL");
            lc.addRoomLink("3356", 233.83f, 131.26f, "JJESTER");
            lc.addRoomLink("3095", 218.43f, 275.57f, "MIRST");
            lc.addRoomLink("3104", 196.63f, 288.48f, "RDIXIT");
            lc.addRoomLink("3343", 210.58f, 144.15f, "GVERSTER");
            lc.addRoomLink("3147", 101.06f, 275.57f, "GREGVAR");
            lc.addRoomLink("3091", 218.43f, 260.63f, "NA");
            lc.addRoomLink("3339", 210.58f, 159.09f, "LISATHOM");
            lc.addRoomLink("3151", 101.06f, 260.63f, "JIMSMI");
            lc.addRoomLink("3283", 87.83f, 155.46f, "VINGU");
            lc.addRoomLink("3336", 196.63f, 131.23f, "SIMONBOO");
            lc.addRoomLink("3318", 167.28f, 131.26f, "MAZENS");
            lc.addRoomLink("3120", 167.28f, 288.46f, "STHENRY,CAROU");
            lc.addRoomLink("3358", 233.83f, 179.32f, "TGERBER");
            lc.addRoomLink("3303", 130.94f, 144.28f, "SKOSTED");
            lc.addRoomLink("3379", 196.52f, 224.67f, "GRARCHIB,KABABBAR");
            lc.addRoomLink("3369", 196.52f, 187.70f, "JESAM");
            lc.addRoomLink("3373", 196.52f, 202.51f, "MARVINQ,DASCHMID");
            lc.addRoomLink("3083", 240.39f, 275.38f, "DMANI");
            lc.addRoomLink("3107", 189.43f, 274.17f, "SANAIR");
            lc.addRoomLink("3213", 145.25f, 195.17f, "WAELA");
            //       lc.addRoomLink("3321", 159.32f, 154.28f, "NA");
            lc.addRoomLink("3077", 254.13f, 260.75f, "NA");
            lc.addRoomLink("3211", 144.11f, 204.41f, "GUANGW");
            lc.addRoomLink("3209", 144.11f, 215.43f, "MANIR");
            lc.addRoomLink("3331", 181.08f, 145.54f, "TINALANG");
            lc.addRoomLink("3109", 181.08f, 274.17f, "JIMEPES");
            lc.addRoomLink("3349", 232.10f, 158.96f, "SAMESHS");
            lc.addRoomLink("3333", 189.43f, 145.54f, "STLEIGH");
            lc.addRoomLink("3165", 65.24f, 260.75f, "WIRIVERA,BMJ");
            lc.addRoomLink("3347", 219.38f, 155.46f, "FLORENTR");
            lc.addRoomLink("3263", 56.88f, 158.96f, "RACHELHA");
            lc.addRoomLink("3056", 210.45f, 240.62f, "PABA,VIRAN");
            lc.addRoomLink("3265", 56.88f, 151.62f, "NA");
            lc.addRoomLink("3383", 196.52f, 179.09f, "PRITHAB,TMCCANTS");
            lc.addRoomLink("3251", 56.88f, 166.43f, "SAMFO");
            lc.addRoomLink("3067", 240.33f, 253.41f, "NDEREUCK");
            lc.addRoomLink("3189", 131.20f, 240.62f, "DINAG");
            lc.addRoomLink("3365", 210.45f, 179.09f, "GEETUM,JABELL");
            lc.addRoomLink("3081", 240.33f, 268.10f, "GERMYONG,JAPAG");
            lc.addRoomLink("3395", 184.37f, 184.92f, "NA");
            lc.addRoomLink("3079", 240.33f, 260.75f, "YVETTEW");
            lc.addRoomLink("3345", 219.35f, 145.51f, "TREYFLY,DAKING");
            lc.addRoomLink("3229", 131.20f, 179.09f, "BJORNJ,BRTINK");
            lc.addRoomLink("3217", 145.12f, 179.09f, "NA");
            lc.addRoomLink("3193", 145.12f, 240.62f, "ALVEISEH,JEPOUTON");
            lc.addRoomLink("3049", 196.52f, 240.62f, "CHENMLIU,SABINEM");
        }
        public void CreatePointsForRedwB3()
        {
            lc.gm.initmods();
            lc.yfloor = 0;

            float[] Fz = { 138.45f, 172.25f, 247.65f, 281.45f };
            float[] Fx = { 30.3f, 50.0f, 53.5f, 72.0f, 94.0f, 116.0f, 138.0f, 173.6f, 203.0f, 225.0f, 244.0f, 247.0f, 266.5f };
            lc.gm.mod_name_pfx = getmodelprefix("rwb-f03-");
            lc.gm.mod_x_fak = 1 / 2.6f;
            lc.gm.mod_x_off = lc.gm.mod_x_fak * Fx[1];
            lc.gm.mod_z_fak = -1 / 2.6f;
            lc.gm.mod_z_off = lc.gm.mod_z_fak * Fz[0];
            lc.AddNodePtxy("cv0-s", Fx[00], Fz[0]);
            var pfx = "";
            var cv0n = pfx + "cv0";
            var cv1n = pfx + "cv1";
            var cv2n = pfx + "cv2";
            var cv3n = pfx + "cv3";

            lc.LinkTooPtxz("cv0-e", Fx[10], Fz[0], lname: cv0n);

            lc.AddNodePtxy("cv1-s", Fx[00], Fz[1]);
            lc.LinkTooPtxz("cv1-e", Fx[10], Fz[1], lname: cv1n);

            lc.AddNodePtxy("cv2-s", Fx[02], Fz[2]);
            lc.LinkTooPtxz("cv2-e", Fx[12], Fz[2], lname: cv2n);

            lc.AddNodePtxy("cv3-s", Fx[02], Fz[3]);
            lc.LinkTooPtxz("cv3-e", Fx[12], Fz[3], lname: cv3n);

            var Fzmid = (Fz[0] + Fz[1]) / 2;
            lc.AddCrosLink("ch00", Fx[00], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch01", Fx[01], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch02", Fx[03], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch03", Fx[04], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch04", Fx[05], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch05", Fx[06], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch06", Fx[07], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch07", Fx[08], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch08", Fx[09], Fzmid, cv0n, cv1n);
            lc.AddCrosLink("ch09", Fx[10], Fzmid, cv0n, cv1n);

            lc.AddCrosLink("ch10", Fx[06], Fzmid, cv1n, cv2n);
            lc.AddCrosLink("ch11", Fx[07], Fzmid, cv1n, cv2n);
            lc.AddCrosLink("ch12", Fx[08], Fzmid, cv1n, cv2n);

            lc.AddCrosLink("ch20", Fx[02], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch21", Fx[03], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch22", Fx[04], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch23", Fx[05], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch24", Fx[06], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch25", Fx[07], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch26", Fx[08], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch27", Fx[09], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch28", Fx[11], Fzmid, cv2n, cv3n);
            lc.AddCrosLink("ch29", Fx[12], Fzmid, cv2n, cv3n);

            AddRedwB3rooms();

            var template = lc.gm.addprefix("rm");
            addVoiceKeywords(template);
            addRedwestB3names(template);


            //lc.addRoomLink("3268", 56.25f, 131.11f); // marcemerc
            //lc.addRoomLink("3258", 48.78f, 131.11f); // brujo
            //lc.addRoomLink("3256", 41.23f,131.26f);  // rimes
            //lc.addRoomLink("3210", 130.82f, 209.86f);  // syros
            //lc.addRoomLink("3170", 63.47f, 288.50f );  // rsiva
        }
        public void CreatePointsForRedwB3Simple()
        {
            lc.gm.initmods();
            lc.yfloor = 0;
            lc.AddNodePtxy("f01-dt-st01", 0, 0);
            lc.LinkTooPtxz("f01-wp-c00", 3.0, 0);
            lc.LinkTooPtxz("f01-wp-c01", 5.52, 0);

            lc.LinkTooPtxz("f01-wp-c02", 9.94, 0);
            lc.LinkTooPtxz("f01-wp-c03", 13.84, 0);
            lc.LinkTooPtxz("f01-wp-c04", 14.34, 0);
            lc.LinkTooPtxz("f01-wp-c05", 17.80, 0);
            lc.LinkTooPtxz("f01-wp-c06", 21.53, 0);
            lc.LinkTooPtxz("f01-wp-c07", 23.25, 0);
            lc.LinkTooPtxz("f01-wp-c08", 29.15, 0);
            lc.LinkTooPtxz("f01-wp-c12", 33.22, 0);
            lc.LinkTooPtxz("f01-wp-c13", 33.22, -8.5);
            lc.LinkTooPtxz("f01-wp-c14", 33.22, -13);
            lc.LinkTooPtxz("f01-wp-c16", 38.22, -13);
            lc.LinkToxz("f01-wp-c01", "f01-dt-rm3271", 5.52, -3.47); // rm1001
            lc.LinkToxz("f01-wp-c02", "f01-dt-rm3279", 9.94, -3.47);// rm1002
            lc.LinkToxz("f01-wp-c03", "f01-dt-rm3281", 13.84, -3.47);// rm1003
            lc.LinkToxz("f01-wp-c13", "f01-dt-rm3309", 35.22, -8.5);// rm1004
            lc.LinkToxz("f01-wp-c14", "f01-wp-c17", 28.22, -13.0);
            lc.LinkToxz("f01-wp-c14", "f01-wp-c15", 33.22, -27.5);


            lc.LinkToxz("f01-wp-c04", "f01-dt-rm3282", 14.34, 3.47);// kitchen
            lc.LinkToxz("f01-wp-c17", "f01-dt-rm3231", 28.22, -10.0);// rm1005

            lc.LinkToxz("f01-dt-st01", "f01-wp-c91", -1, 0);
            lc.LinkToxz("f01-wp-c91", "f01-wp-c92", -3, 0);
            lc.LinkToxz("f01-wp-c91", "f01-dt-rm3258", -1, 2);// bruce
            lc.LinkToxz("f01-wp-c92", "f01-dt-rm3256", -3, 2);// rimes

            lc.LinkToxz("f01-wp-c00", "f01-dt-rm3268", 3, 2);// marc merc
            lc.LinkToxz("f01-wp-c15", "f01-dt-rm3210", 31.22, -27.5);// spyros 


            // now add the keywords for the keyword recognizer

            string template = "f01-dt-rm";
            addVoiceKeywords(template);
        }
        public bool voiceEnabled(string roomname)
        {
            return lc.nodekeywords.Values.Contains<string>(roomname);
        }
        public void addRedwestB3names(string template)
        {
            lc.nodekeywords.Add("lobby 1", "f01-dt-st01");
            lc.nodekeywords.Add("brad", template + "3271");
            lc.nodekeywords.Add("brent", template + "3279");
            lc.nodekeywords.Add("sue", template + "3281");
            lc.nodekeywords.Add("laura", template + "3342");
            lc.nodekeywords.Add("simon", template + "3336");
            lc.nodekeywords.Add("travis", template + "3358");
            lc.nodekeywords.Add("scott", template + "3072");
            lc.nodekeywords.Add("J D", template + "3182");
            lc.nodekeywords.Add("dina", template + "3189");
            lc.nodekeywords.Add("pradeep", template + "3309");
            lc.nodekeywords.Add("bill", template + "3231");
            lc.nodekeywords.Add("mark", template + "3370");
            lc.nodekeywords.Add("fred pace", template + "3372");
            lc.nodekeywords.Add("kiyoshi", template + "3377");
            lc.nodekeywords.Add("liz", template + "3380");
            lc.nodekeywords.Add("tyson", template + "3362");
            lc.nodekeywords.Add("brigitte", template + "3191");
            lc.nodekeywords.Add("spyros", template + "3210");
            lc.nodekeywords.Add("bruce", template + "3258");
            lc.nodekeywords.Add("rimes", template + "3256");
            lc.nodekeywords.Add("rhymes", template + "3256");
            lc.nodekeywords.Add("ramesh", template + "3170");
            lc.nodekeywords.Add("anand", template + "3254");
            lc.nodekeywords.Add("peter", template + "3353");
            lc.nodekeywords.Add("marc merc", template + "3268");
            lc.nodekeywords.Add("kitchen", template + "3033");
            lc.nodekeywords.Add("printer", template + "3399");
            lc.nodekeywords.Add("central stairs", template + "3033");
            lc.nodekeywords.Add("elevator", template + "3033");
            lc.nodekeywords.Add("mens room", template + "3393");
            lc.nodekeywords.Add("womans room", template + "3039");
        }
        public void addB43names(string template)
        {
            lc.nodekeywords.Add("lobby 1", "f01-dt-st01");
            lc.nodekeywords.Add("brad", template + "3271");
            lc.nodekeywords.Add("brent", template + "3279");
            lc.nodekeywords.Add("sue", template + "3281");
            lc.nodekeywords.Add("laura", template + "3342");
            lc.nodekeywords.Add("simon", template + "3336");
            lc.nodekeywords.Add("travis", template + "3358");
            lc.nodekeywords.Add("scott", template + "3072");
            lc.nodekeywords.Add("J D", template + "3182");
            lc.nodekeywords.Add("dina", template + "3189");
            lc.nodekeywords.Add("pradeep", template + "3309");
            lc.nodekeywords.Add("bill", template + "3231");
            lc.nodekeywords.Add("mark", template + "3370");
            lc.nodekeywords.Add("fred pace", template + "3372");
            lc.nodekeywords.Add("kiyoshi", template + "3377");
            lc.nodekeywords.Add("liz", template + "3380");
            lc.nodekeywords.Add("tyson", template + "3362");
            lc.nodekeywords.Add("brigitte", template + "3191");
            lc.nodekeywords.Add("spyros", template + "3210");
            lc.nodekeywords.Add("bruce", template + "3258");
            lc.nodekeywords.Add("rimes", template + "3256");
            lc.nodekeywords.Add("rhymes", template + "3256");
            lc.nodekeywords.Add("ramesh", template + "3170");
            lc.nodekeywords.Add("anand", template + "3254");
            lc.nodekeywords.Add("peter", template + "3353");
            lc.nodekeywords.Add("marc merc", template + "3268");
            lc.nodekeywords.Add("kitchen", template + "3033");
            lc.nodekeywords.Add("printer", template + "3399");
            lc.nodekeywords.Add("central stairs", template + "3033");
            lc.nodekeywords.Add("elevator", template + "3033");
            lc.nodekeywords.Add("mens room", template + "3393");
            lc.nodekeywords.Add("womans room", template + "3039");
        }
        public static string voiceEnabledRooms = "3271,3279,3281,3342,3336,3358,3072,3182,3189,3309,3231,3370,3372,3377,3380,3362,3191,3210,3258,3256,3170,3254,3353,3268,3144,3146";
        public void addVoiceKeywords(string template)
        {
            int ntotadded = 0;
            int nwildadded = 0;
            int nmustadd = voiceEnabledRooms.Split(',').Count<string>();
            int wildtoadd = maxVoiceKeywords - nmustadd;
            foreach (var nname in lc.nodenamelist)
            {
                if (nname.StartsWith(template))
                {
                    var snum = nname.Remove(0, template.Length);
                    var key = "room " + snum;
                    bool mustadd = voiceEnabledRooms.IndexOf(snum) >= 0;
                    if (mustadd || nwildadded < wildtoadd)
                    {
                        lc.nodekeywords.Add(key, nname);
                        ntotadded += 1;
                        if (!mustadd) nwildadded += 1;
                    }
                    // GraphUtil.Log("Key:" + key + "  Node:" + nname);
                }
            }
            
            nVoiceKeywords = ntotadded;
            GraphUtil.Log("  wildtoadd:" + wildtoadd + "  maxVoiceKeywords:" + maxVoiceKeywords);
            GraphUtil.Log("Added " + ntotadded + " voice keywords - wildones:" + nwildadded);
        }
        public void CreatePointsForB43RoomsFloor1()
        {
            lc.gm.initmods();
            lc.gm.mod_x_fak = 0.9f;
            lc.gm.mod_x_off = -59.4061f;
            lc.gm.mod_z_fak = 0.9f;
            lc.gm.mod_z_off = -22.2394f;

            lc.yfloor = 0;
            lc.AddNodePtxy("f01-dt-st01", 0, 0);
            lc.LinkTooPtxz("f01-wp-c01", 0, 6.52);

            lc.LinkTooPtxz("f01-wp-c02", 0, 8.74);
            lc.LinkTooPtxz("f01-wp-c03", 0, 10.84);
            lc.LinkTooPtxz("f01-wp-c04", 0, 14.34);
            lc.LinkTooPtxz("f01-wp-c05", 0, 17.80);
            lc.LinkTooPtxz("f01-wp-c06", 0.11, 21.53);
            lc.LinkTooPtxz("f01-wp-c07", -1.92, 23.25);
            lc.LinkTooPtxz("f01-wp-c08", -1.92, 29.15);
            lc.LinkTooPtxz("f01-wp-c09", -1.92, 32.76);
            lc.LinkTooPtxz("f01-wp-c10", -4.43, 33.22);
            lc.LinkTooPtxz("f01-wp-c11", -5.97, 33.22);
            lc.LinkTooPtxz("f01-wp-c12", -8.30, 33.22);
            lc.LinkTooPtxz("f01-wp-c13", -8.63, 29.44);
            lc.LinkTooPtxz("f01-wp-c14", -11.91, 29.44);
            lc.LinkTooPtxz("f01-wp-c15", -11.91, 27.55);
            lc.LinkTooPtxz("f01-wp-c16", -9.04, 27.55);
            lc.LinkToxz("f01-wp-c01", "f01-dt-rm1u17", 3.47, 6.29);
            lc.LinkToxz("f01-wp-c02", "f01-dt-rm1u18", 3.47, 8.47);
            lc.LinkToxz("f01-wp-c03", "f01-dt-rm1u19", 3.47, 10.53);

            lc.LinkToxz("f01-wp-c04", "f01-dt-rm1u21", -5.04, 14.15);
            lc.LinkToxz("f01-wp-c05", "f01-dt-rm1203", -4.31, 17.46);
            lc.LinkToxz("f01-wp-c08", "f01-dt-rm1u30", 1.66, 29.68);
            lc.LinkToxz("f01-wp-c09", "f01-dt-rm1u31", 0.60, 32.76);
            lc.LinkToxz("f01-wp-c10", "f01-dt-rm1u33", -4.43, 30.35);
            lc.LinkToxz("f01-wp-c10", "f01-dt-rm1u32", -4.43, 36.44);
            lc.LinkToxz("f01-wp-c11", "f01-dt-rm1u34", -5.97, 30.20);
            lc.LinkToxz("f01-wp-c16", "f01-dt-rm1012", -9.17, 25.03);
            lc.LinkToxz("f01-wp-c15", "f01-dt-rm1013", -11.91, 24.49);
            lc.LinkToxz("f01-wp-c15", "f01-dt-rm1u36", -14.97, 27.55);

            lc.LinkToxz("f01-wp-c03", "f01-wp-c20", -12.63, 10.84);
            lc.LinkTooPtxz("f01-wp-c21", -12.63, 14.15);
            lc.LinkTooPtxz("f01-wp-c22", -9.66, 14.15);
            lc.LinkTooPtxz("f01-wp-c23", -9.66, 17.74);
            lc.LinkTooPtxz("f01-wp-c24", -8.73, 21.73);
            lc.LinkTooPtxz("f01-wp-c25", -4.68, 21.73);
            lc.LinkToxz("f01-wp-c23", "f01-dt-rm1015", -12.37, 18.27);

            lc.AddLinkByNodeName("f01-dt-rm1203", "f01-wp-c25");
            lc.AddLinkByNodeName("f01-dt-rm1012", "f01-wp-c24");

            // now add the keywords for the keyword recognizer
            string template = "f01-dt-rm";
            foreach (var nname in lc.nodenamelist)
            {
                if (nname.StartsWith(template))
                {
                    var key = "room " + nname.Remove(0, template.Length);
                    lc.nodekeywords.Add(key, nname);
                    // RouteMan.Log("Key:" + key + "  Node:" + nname);
                }
            }
            lc.nodekeywords.Add("lobby 1", "f01-dt-st01");
            lc.nodekeywords.Add("kitchen 1", "f01-dt-k01");
            lc.nodekeywords.Add("1u17", "f01-dt-rm1u17");
            lc.nodekeywords.Add("1u18", "f01-dt-rm1u18");
            lc.nodekeywords.Add("1u19", "f01-dt-rm1u19");
            lc.nodekeywords.Add("1u21", "f01-dt-rm1u21");
            lc.nodekeywords.Add("1023", "f01-dt-rm1023");
            lc.nodekeywords.Add("1u30", "f01-dt-rm1u30");
            lc.nodekeywords.Add("1u31", "f01-dt-rm1u31");
            lc.nodekeywords.Add("1u32", "f01-dt-rm1u32");
            lc.nodekeywords.Add("1u33", "f01-dt-rm1u33");
            lc.nodekeywords.Add("1u34", "f01-dt-rm1u34");
        }
        //public void CreatePointsForB43RoomsFloor1()
        //{
        //    lc.gm.initmods();
        //    lc.gm.mod_x_fak = 1.0f;
        //    lc.gm.mod_y_fak = 1.0f;

        //    lc.yfloor = 0;
        //    lc.AddNodePtxy("f01-dt-st01", 0, 0);
        //    lc.LinkTooPtxz("f01-wp-c01", 6.52, 0);

        //    lc.LinkTooPtxz("f01-wp-c02", 8.74, 0);
        //    lc.LinkTooPtxz("f01-wp-c03", 10.84, 0);
        //    lc.LinkTooPtxz("f01-wp-c04", 14.34, 0);
        //    lc.LinkTooPtxz("f01-wp-c05", 17.80, 0);
        //    lc.LinkTooPtxz("f01-wp-c06", 21.53, -0.11);
        //    lc.LinkTooPtxz("f01-wp-c07", 23.25, 1.92);
        //    lc.LinkTooPtxz("f01-wp-c08", 29.15, 1.92);
        //    lc.LinkTooPtxz("f01-wp-c09", 32.76, 1.92);
        //    lc.LinkTooPtxz("f01-wp-c10", 33.22, 4.43);
        //    lc.LinkTooPtxz("f01-wp-c11", 33.22, 5.97);
        //    lc.LinkTooPtxz("f01-wp-c12", 33.22, 8.3);
        //    lc.LinkTooPtxz("f01-wp-c13", 29.44, 8.63);
        //    lc.LinkTooPtxz("f01-wp-c14", 29.44, 11.91);
        //    lc.LinkTooPtxz("f01-wp-c15", 27.55, 11.91);
        //    lc.LinkTooPtxz("f01-wp-c16", 27.55, 9.04);
        //    lc.LinkToxz("f01-wp-c01", "f01-dt-rm1001", 6.29, -3.47);
        //    lc.LinkToxz("f01-wp-c02", "f01-dt-rm1002", 8.47, -3.47);
        //    lc.LinkToxz("f01-wp-c03", "f01-dt-rm1003", 10.53, -3.47);

        //    lc.LinkToxz("f01-wp-c04", "f01-dt-k01", 14.15, 5.04);
        //    lc.LinkToxz("f01-wp-c05", "f01-dt-rm1004", 17.46, 4.31);
        //    lc.LinkToxz("f01-wp-c08", "f01-dt-rm1005", 29.68, -1.66);
        //    lc.LinkToxz("f01-wp-c09", "f01-dt-rm1006", 32.76, -0.60);
        //    lc.LinkToxz("f01-wp-c10", "f01-dt-rm1007", 30.35, 4.43);
        //    lc.LinkToxz("f01-wp-c10", "f01-dt-rm1008", 36.44, 4.43);
        //    lc.LinkToxz("f01-wp-c11", "f01-dt-rm1009", 30.20, 5.97);
        //    lc.LinkToxz("f01-wp-c16", "f01-dt-rm1012", 25.03, 9.17);
        //    lc.LinkToxz("f01-wp-c15", "f01-dt-rm1013", 24.49, 11.91);
        //    lc.LinkToxz("f01-wp-c15", "f01-dt-rm1014", 27.55, 14.97);

        //    lc.LinkToxz("f01-wp-c03", "f01-wp-c20", 10.84, 12.63);
        //    lc.LinkTooPtxz("f01-wp-c21", 14.15, 12.63);
        //    lc.LinkTooPtxz("f01-wp-c22", 14.15, 9.66);
        //    lc.LinkTooPtxz("f01-wp-c23", 17.74, 9.66);
        //    lc.LinkTooPtxz("f01-wp-c24", 21.73, 8.73);
        //    lc.LinkTooPtxz("f01-wp-c25", 21.73, 4.68);
        //    lc.LinkToxz("f01-wp-c23", "f01-dt-rm1015", 18.27, 12.37);

        //    lc.AddLinkByNodeName("f01-dt-rm1004", "f01-wp-c25");
        //    lc.AddLinkByNodeName("f01-dt-rm1012", "f01-wp-c24");

        //    // now add the keywords for the keyword recognizer
        //    string template = "f01-dt-rm";
        //    foreach (var nname in lc.nodenamelist)
        //    {
        //        if (nname.StartsWith(template))
        //        {
        //            var key = "room " + nname.Remove(0, template.Length);
        //            lc.nodekeywords.Add(key, nname);
        //            // RouteMan.Log("Key:" + key + "  Node:" + nname);
        //        }
        //    }
        //    lc.nodekeywords.Add("lobby 1", "f01-dt-st01");
        //    lc.nodekeywords.Add("kitchen 1", "f01-dt-k01");
        //    lc.nodekeywords.Add("1u17", "f01-dt-rm1001");
        //    lc.nodekeywords.Add("1u18", "f01-dt-rm1002");
        //    lc.nodekeywords.Add("1u19", "f01-dt-rm1003");
        //    lc.nodekeywords.Add("1u21", "f01-dt-k01");
        //    lc.nodekeywords.Add("1023", "f01-dt-rm1004");
        //    lc.nodekeywords.Add("1u30", "f01-dt-rm1005");
        //    lc.nodekeywords.Add("1u31", "f01-dt-rm1006");
        //    lc.nodekeywords.Add("1u32", "f01-dt-rm1008");
        //    lc.nodekeywords.Add("1u33", "f01-dt-rm1007");
        //    lc.nodekeywords.Add("1u34", "f01-dt-rm1009");
        //}
        public void CreatePointsForB43RoomsFloor2()
        {
            lc.gm.initmods();
            lc.yfloor = 4;
            lc.AddNodePtxy("f02-dt-st02", 0, 0);
            //var wp1 = lc.AddLinkPtxz("wp-c01", 6.52, 0);
            lc.LinkTooPtxz("f02-wp-c01", 6.52, 0);

            lc.LinkTooPtxz("f02-wp-c02", 8.74, 0);
            lc.LinkTooPtxz("f02-wp-c03", 10.84, 0);
            lc.LinkTooPtxz("f02-wp-c04", 14.34, 0);
            lc.LinkTooPtxz("f02-wp-c05", 17.80, 0);
            lc.LinkTooPtxz("f02-wp-c06", 21.53, -0.11);
            lc.LinkTooPtxz("f02-wp-c07", 23.25, 1.92);
            lc.LinkTooPtxz("f02-wp-c08", 29.15, 1.92);
            lc.LinkTooPtxz("f02-wp-c09", 32.76, 1.92);
            lc.LinkTooPtxz("f02-wp-c10", 33.22, 4.43);
            lc.LinkTooPtxz("f02-wp-c11", 33.22, 5.97);
            lc.LinkTooPtxz("f02-wp-c12", 33.22, 8.3);
            lc.LinkTooPtxz("f02-wp-c13", 29.44, 8.63);
            lc.LinkTooPtxz("f02-wp-c14", 29.44, 11.91);
            lc.LinkTooPtxz("f02-wp-c15", 27.55, 11.91);
            lc.LinkTooPtxz("f02-wp-c16", 27.55, 9.04);
            lc.LinkToxz("f02-wp-c01", "f02-dt-rm2001", 6.29, -3.47);
            lc.LinkToxz("f02-wp-c02", "f02-dt-rm2002", 8.47, -3.47);
            lc.LinkToxz("f02-wp-c03", "f02-dt-rm2003", 10.53, -3.47);

            lc.LinkToxz("f02-wp-c04", "f02-dt-k02", 14.15, 5.04);
            lc.LinkToxz("f02-wp-c05", "f02-dt-rm2004", 17.46, 4.31);
            lc.LinkToxz("f02-wp-c08", "f02-dt-rm2005", 29.68, -1.66);
            lc.LinkToxz("f02-wp-c09", "f02-dt-rm2006", 32.76, -0.60);
            lc.LinkToxz("f02-wp-c10", "f02-dt-rm2007", 30.35, 4.43);
            lc.LinkToxz("f02-wp-c10", "f02-dt-rm2008", 36.44, 4.43);
            lc.LinkToxz("f02-wp-c11", "f02-dt-rm2009", 30.20, 5.97);
            lc.LinkToxz("f02-wp-c16", "f02-dt-rm2012", 25.03, 9.17);
            lc.LinkToxz("f02-wp-c15", "f02-dt-rm2013", 24.49, 11.91);
            lc.LinkToxz("f02-wp-c15", "f02-dt-rm2014", 27.55, 14.97);

            lc.LinkToxz("f02-wp-c03", "f02-wp-c20", 10.84, 12.63);
            lc.LinkTooPtxz("f02-wp-c21", 14.15, 12.63);
            lc.LinkTooPtxz("f02-wp-c22", 14.15, 9.66);
            lc.LinkTooPtxz("f02-wp-c23", 17.74, 9.66);
            lc.LinkTooPtxz("f02-wp-c24", 21.73, 8.73);
            lc.LinkTooPtxz("f02-wp-c25", 21.73, 4.68);
            lc.LinkToxz("f02-wp-c23", "f02-dt-rm2015", 18.27, 12.37);

            lc.AddLinkByNodeName("f02-dt-rm2004", "f02-wp-c25");
            lc.AddLinkByNodeName("f02-dt-rm2012", "f02-wp-c24");

            // now add the keywords for the keyword recognizer
            string template = "f02-dt-rm";
            foreach (var nname in lc.nodenamelist)
            {
                if (nname.StartsWith(template))
                {
                    var key = "room " + nname.Remove(0, template.Length);
                    lc.nodekeywords.Add(key, nname);
                    // RouteMan.Log("Key:" + key + "  Node:" + nname);
                }
            }
            lc.nodekeywords.Add("lobby 2", "f02-dt-st02");
            lc.nodekeywords.Add("kitchen 2", "f02-dt-k02");
        }
        public void CreateCircPoints(int npoints = 10, float rad = 5.0f, float heit = 0)
        {
            lc.gm.initmods();
            lc.gm.mod_name_pfx = getmodelprefix("circ-", forcecount: true);
            var ooname = "Node-0";
            var oname = "";
            for (int i = 0; i < npoints; i++)
            {
                var nname = "Node-" + i;
                float ang = Mathf.PI * (360f * i / npoints) / 180;
                var z = rad * Mathf.Sin(ang);
                var x = rad * Mathf.Cos(ang);
                var y = heit;
                var pt = new Vector3(x, y, z);
                lc.AddNode(nname, pt);
                if (i > 0)
                {
                    var lname = "Link-" + i;
                    lc.AddLinkByNodeName(oname, nname, lname);
                }
                oname = nname;
            }
            lc.AddLinkByNodeName(oname, ooname, "Link-0");
        }
        public void CreateSpherePoints(int nlng = 10, int nlat = 10, float rad = 5.0f, float heit = 0)
        {
            lc.gm.initmods();
            lc.gm.mod_name_pfx = getmodelprefix("sph-", forcecount: true);
            var optar = new LcNode[nlng, nlat];
            for (int i = 0; i < nlat; i++)
            {
                float alat = Mathf.PI * (90 - (i * 180f / (nlat - 1))) / 180;
                float y = rad * Mathf.Sin(alat) + heit;
                float crad = rad * Mathf.Cos(alat);
                for (int j = 0; j < nlng; j++)
                {
                    float alng = Mathf.PI * (j * 360f / nlng) / 180;
                    float z = crad * Mathf.Sin(alng);
                    float x = crad * Mathf.Cos(alng);
                    var pt = new Vector3(x, y, z);
                    var pname = "pt." + i.ToString() + "." + j.ToString();
                    LcNode npt = lc.AddNode(pname, pt);
                    optar[i, j] = npt;
                }
            }
            for (int i = 0; i < nlat; i++)
            {
                for (int j = 0; j < nlng; j++)
                {
                    var jto = (j + 1) % nlng;
                    var lname = "lk.h." + i + "." + j;
                    lc.AddLink(lname, optar[i, j], optar[i, jto]);
                    if (i > 0)
                    {
                        var lname1 = "lk.v." + i + "." + j;
                        lc.AddLink(lname1, optar[i - 1, j], optar[i, j]);
                    }
                }
            }
        }
        public void AddRedwBhoRooms()
        {
            lc.addRoomLink("4092", 27.6f, -11.4f, "NA");
            lc.addRoomLink("4093", 55.3f, -24.8f, "NA");
            lc.addRoomLink("4094", 30.0f, 22.8f, "NA");
            lc.addRoomLink("4095", 26.7f, 3.3f, "NA");

            lc.addRoomLink("FrontDesk", 33.76f, -6.5f, "NA");
            lc.addRoomLink("Elevator 1", 40.1f, -13.3f, "Elevator");
            lc.addRoomLink("Elevator 2", 42.2f, -13.3f, "NA");
            lc.addRoomLink("Stairs401", 38.0f, -10.0f, "NA");
            lc.addRoomLink("Stairs402", 28.4f, -8.3f, "NA");
            lc.addRoomLink("Stairs404", 36.0f, 24.3f, "NA");


            lc.addRoomLink("4003", 32.0f, -14.0f, "NA");
            lc.addRoomLink("4004", 32.0f, -15.6f, "NA");
            lc.addRoomLink("4005", 34.0f, -25.2f, "NA");
            lc.addRoomLink("4006", 40.2f, -28.7f, "NA");
            lc.addRoomLink("4007", 46.6f, -32.1f, "NA");
            lc.addRoomLink("4008", 50.6f, -32.7f, "NA");
            lc.addRoomLink("4009", 50.1f, -25.6f, "NA");
            lc.addRoomLink("4010", 48.3f, -23.7f, "NA");
            lc.addRoomLink("4011", 53.8f, -21.1f, "NA");
            lc.addRoomLink("4012", 49.2f, -22.4f, "NA");
            lc.addRoomLink("4014", 54.0f, -16.6f, "NA");
            lc.addRoomLink("4016", 54.1f, -9.9f, "NA");
            lc.addRoomLink("4017", 47.1f, -9.9f, "NA");
            lc.addRoomLink("4018", 50.7f, -7.3f, "NA");
            lc.addRoomLink("4019", 55.0f, -0.1f, "NA");
            lc.addRoomLink("4020", 50.4f, -2.1f, "NA");
            lc.addRoomLink("4021", 47.8f, -2.1f, "NA");
            lc.addRoomLink("4022", 43.6f, -4.0f, "NA");
            lc.addRoomLink("4023", 39.3f, -2.2f, "NA");
            lc.addRoomLink("4024", 37.7f, -2.2f, "NA");
            lc.addRoomLink("4026", 35.2f, -2.0f, "Mens room");
            lc.addRoomLink("4027", 33.2f, -2.0f, "NA");
            lc.addRoomLink("4028", 31.2f, -1.5f, "Womens room");
            lc.addRoomLink("4029", 28.2f, -6.0f, "NA");
            lc.addRoomLink("4030", 28.2f, -4.0f, "NA");
            lc.addRoomLink("4031", 37.4f, 3.5f, "NA");
            lc.addRoomLink("4032", 40.6f, 1.6f, "NA");
            lc.addRoomLink("4033", 43.7f, 1.7f, "NA");
            lc.addRoomLink("4034", 41.9f, 4.4f, "NA");
            lc.addRoomLink("4035", 45.7f, 4.4f, "NA");
            lc.addRoomLink("4036", 48.0f, 1.1f, "NA");
            lc.addRoomLink("4037", 51.3f, 1.1f, "NA");
            lc.addRoomLink("4038", 51.3f, 5.2f, "NA");
            lc.addRoomLink("4039", 49.6f, 6.5f, "NA");
            lc.addRoomLink("4040", 49.6f, 10.6f, "NA");
            lc.addRoomLink("4042", 53.9f, 13.3f, "NA");
            lc.addRoomLink("4043", 49.6f, 12.0f, "NA");
            lc.addRoomLink("4044", 48.0f, 15.0f, "NA");
            lc.addRoomLink("4045", 47.4f, 17.2f, "NA");
            lc.addRoomLink("4046", 55.0f, 23.7f, "NA");
            lc.addRoomLink("4047", 50.9f, 22.2f, "Mens Room 2");
            lc.addRoomLink("4048", 49.0f, 21.4f, "NA");
            lc.addRoomLink("4049", 47.1f, 22.2f, "Womans Room 2");
            lc.addRoomLink("4050", 44.6f, 21.4f, "NA");
            lc.addRoomLink("4051", 41.1f, 21.4f, "NA");
            lc.addRoomLink("4052", 43.2f, 18.8f, "NA");
            lc.addRoomLink("4053", 39.0f, 18.8f, "NA");
            lc.addRoomLink("4054", 38.0f, 21.4f, "NA");
            lc.addRoomLink("4055", 35.0f, 21.4f, "NA");
            lc.addRoomLink("4056", 33.1f, 25.8f, "NA");
            lc.addRoomLink("4057", 33.7f, 28.4f, "NA");
            lc.addRoomLink("4058", 41.6f, 25.3f, "NA");
            lc.addRoomLink("4059", 47.0f, 25.7f, "NA");
            lc.addRoomLink("4060", 51.3f, 25.7f, "NA");



        }
        public void CreatePointsForBho()
        {
            lc.gm.initmods();
            lc.yfloor = 0;

            float[] Fz = { -30.23f, -27.81f, -23.21f, -11.4f, -7.81f, -0.4f, 23.2f };
            float[] Fx = { 30.0f, 33.9f, 35.0f, 48.5f, 52.3f };
            lc.gm.mod_name_pfx = getmodelprefix("bho-f05-");
            lc.AddNodePtxy("cv1-s", Fx[0], Fz[5]);
            lc.LinkTooPtxz("cv1-e", Fx[0], Fz[4], lname: "cv1");
            lc.LinkTooPtxz("cv1-d", Fx[2], Fz[3], lname: "cd1");
            lc.LinkTooPtxz("cv2-e", Fx[2], Fz[2], lname: "cv2");
            lc.LinkTooPtxz("ch1-e", Fx[3], Fz[0], lname: "ch1");
            lc.LinkTooPtxz("cd2-3", Fx[4], Fz[1], lname: "cd2");
            lc.LinkTooPtxz("cv3-e", Fx[4], Fz[3], lname: "cv3");
            lc.LinkTooPtxz("cv4-e", Fx[4], Fz[5], lname: "cv4");
            lc.LinkTooPtxz("cv5-e", Fx[4], Fz[6], lname: "cv5");
            lc.LinkTooPtxz("ch4-e", Fx[1], Fz[6], lname: "ch4");

            lc.AddLinkByNodeName("cv1-d", "cv3-e", "ch2");
            lc.AddLinkByNodeName("cv1-s", "cv4-e", "ch3");

            lc.addSpurLink("out92", 31.3f, -10.8f);
            lc.addSpurLink("out93", 55.0f, -22.6f);
            lc.addSpurLink("out91a", 36.8f, 2.7f);
            lc.addSpurLink("out91b", 34.1f, 3.7f);
            lc.addSpurLink("out91c", 30.1f, 3.7f);

            lc.addSpurLink("4044", 48.4f, 16.0f);
            lc.addSpurLink("4050", 43.2f, 21.4f);
            lc.addSpurLink("4054", 39.0f, 21.4f);



            AddRedwBhoRooms();

            var template = lc.gm.addprefix("rm");
            //addVoiceKeywords(template);
            //addRedwestB3names(template);


            //lc.addRoomLink("3268", 56.25f, 131.11f); // marcemerc
            //lc.addRoomLink("3258", 48.78f, 131.11f); // brujo
            //lc.addRoomLink("3256", 41.23f,131.26f);  // rimes
            //lc.addRoomLink("3210", 130.82f, 209.86f);  // syros
            //lc.addRoomLink("3170", 63.47f, 288.50f );  // rsiva
        }
        public void CreatePointsSmall()
        {
            lc.gm.initmods();
            lc.gm.mod_name_pfx = getmodelprefix("small-", forcecount: true);
            var p1 = lc.AddNode("pt1", new Vector3(0, 0, 0));
            var p2 = lc.AddNode("pt2", new Vector3(-4, 10, 20));
            var p3 = lc.AddNode("pt3", new Vector3(10, 10, 0));
            lc.AddLink("l1", p1, p2);
            lc.AddLink("l2", p2, p3);
        }
        public GenGlobeParameters globpar = new GenGlobeParameters(12, 12, 10, 10);
        public GenCircParameters circpar = new GenCircParameters(12, 10, 0);
        public Range LinkFloor = new Range(0, 0);
        public void AddGraphToLinkCloud(genmodeE Genmode)
        {
            lc.maxRanHeight = LinkFloor.max;
            lc.minRanHeight = LinkFloor.min;
            switch (Genmode)
            {
                case genmodeE.gen_circ:
                    {
                        //Debug.Log("globpar " + glopar.nlat + " " + glopar.nlng + " _" + glopar.height);
                        CreateCircPoints(npoints: circpar.npoints, rad: circpar.radius, heit: circpar.height);
                        break;
                    }
                case genmodeE.gen_sphere:
                    {
                        Debug.Log("globpar " + globpar.nlat + " " + globpar.nlng + " _" + globpar.height);
                        CreateSpherePoints(nlng: globpar.nlat, nlat: globpar.nlng, rad: globpar.height, heit: globpar.height);
                        break;
                    }
                case genmodeE.gen_b43_1:
                    {
                        CreatePointsForB43RoomsFloor1();
                        var rot = new Vector3(0, -90, 0);
                        var trn = new Vector3(35.42f, 0, -8.72f);
                        lc.floorMan.SetMaterialPlane("US-043-1", 7266, 3599, rot, trn);
                        break;
                    }
                case genmodeE.gen_b43_2:
                    {
                        CreatePointsForB43RoomsFloor1();
                        var rot = new Vector3(0, -90, 0);
                        var trn = new Vector3(35.42f, 0, -8.72f);
                        lc.floorMan.SetMaterialPlane("US-043-2", 7266, 3599, rot, trn);
                        break;
                    }
                case genmodeE.gen_b43_3:
                    {
                        CreatePointsForB43RoomsFloor1();
                        var rot = new Vector3(0, -90, 0);
                        var trn = new Vector3(35.42f, 0, -8.72f);
                        lc.floorMan.SetMaterialPlane("US-043-3", 7266, 3599, rot, trn);
                        break;
                    }
                case genmodeE.gen_b43_4:
                    {
                        CreatePointsForB43RoomsFloor1();
                        var rot = new Vector3(0, -90, 0);
                        var trn = new Vector3(35.42f, 0, -8.72f);
                        lc.floorMan.SetMaterialPlane("US-043-4", 7266, 3599, rot, trn);
                        break;
                    }
                case genmodeE.gen_bho:
                    {
                        CreatePointsForBho();
                        var rot = new Vector3(0, -90, 0);
                        var trn = new Vector3(35.42f, 0, -8.72f);
                        lc.floorMan.SetMaterialPlane("DE-BHO-4", 7266, 4223, rot, trn, scalefak: 1 / 2.086667f);
                        break;
                    }
                case genmodeE.gen_b43_1p2:
                    {
                        CreatePointsForB43RoomsFloor1();
                        CreatePointsForB43RoomsFloor2();
                        lc.AddLinkByNodeName("f01-wp-c01", "f02-wp-c03");
                        lc.AddLinkByNodeName("f01-wp-c12", "f02-wp-c13");
                        break;
                    }
                case genmodeE.gen_redwb_3_simple:
                    {
                        CreatePointsForRedwB3Simple();
                        break;
                    }
                case genmodeE.gen_redwb_3:
                    {
                        CreatePointsForRedwB3();
                        var sca = new Vector3(10.294f, 1, 10.004f);
                        var rot = new Vector3(0, -90, 0);
                        var trn = new Vector3(35.42f, 0, -8.72f);
                        lc.floorMan.SetMaterialPlane("US-RDB-3", 5255, 5099, sca, rot, trn);
                        break;
                    }
                case genmodeE.gen_small:
                    {
                        CreatePointsSmall();
                        break;
                    }
                case genmodeE.gen_json_file:
                    {

                        var jsonlc = JsonLinkCloud.ReadFromFile(mgp.filepar.getFullFileName());
                        AddJsonToLinkCloud(jsonlc, lc);
                        break;
                    }
            }
            var fname = "c:/transfer/" + Genmode.ToString() + ".txt";
            SaveToFile(lc, fname);
        }
        public static void SaveToFile(LinkCloud lc, string fname)
        {
            var jsonlc = MapMaker.MakeJson(lc);
            JsonLinkCloud.WriteToFile(jsonlc, fname);
        }
        public LinkCloud getLinkCloud()
        {
            return lc;
        }
        public static JsonLinkCloud MakeJson(LinkCloud lc)
        {
            int nnodes = 0;
            int nlinks = 0;
            var jsonlc = new JsonLinkCloud(lc.floorMan);
            foreach (var nname in lc.nodenamelist)
            {
                var n = lc.GetNode(nname);
                jsonlc.AddNode(n.name, n.pt);
                nnodes += 1;
            }
            foreach (var lname in lc.linknamelist)
            {
                var lk = lc.GetLink(lname);
                jsonlc.AddLink(lk.name, lk.node1.name, lk.node2.name);
                nlinks += 1;
            }
            GraphUtil.Log("MakeJson nnodes:" + nnodes + " nlinks:" + nlinks);
            Debug.Log("MakeJson nnodes:" + nnodes + " nlinks:" + nlinks);
            return jsonlc;
        }
        public static LinkCloud AddJsonToLinkCloud(JsonLinkCloud jsonlc, LinkCloud lc = null)
        {
            if (lc == null)
            {
                lc = new LinkCloud();
            }
            foreach (var node in jsonlc.nodes.list)
            {
                lc.AddNode(node.name, node.pt);
            }
            foreach (var link in jsonlc.links.list)
            {
                lc.AddLinkByNodeName(link.n1, link.n2, link.name);
            }
            lc.floorMan = jsonlc.floorplan;
            return lc;
        }
    }
}
