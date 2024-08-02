
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using System.IO;

class JenkinsBuildHelper
{

    static readonly string[] SCENES = FindEnabledEditorScenes();

    // ============ Production CD Credentials =====================
    static readonly string ANDROID_CD_NAME_PROD = "slider_games";
    static readonly string ANDROID_CD_ID_PROD = "4e59a8ab-0111-45d9-a55d-7237f6695a06";
    // ============ Stage CD Credentials ==========================
    static readonly string ANDROID_CD_NAME_STAGE = "slider_games_stage";
    static readonly string ANDROID_CD_ID_STAGE = "e11d483d-0cd1-4278-bdd6-6048cc97c0fe";
    // ============================================================

    private static string brandID = "us";
    private static string bucketName = "testAssetBundles";

    [MenuItem("IvyJenkins/PerformAndroidIOSBuild")]
    static void PerformAndroidIOSBuild()
    {
      
        bool isDebugBuild = false;
        bool isAssetBundleFeatureEnabled = false;

        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            UnityEngine.Debug.Log("PerformAndroidIOSBuild GetCommandLineArgs args: " + "i val: " + i + "  : " + args[i]);

            if (args[i].Contains(JenkinsBuildConstants.ARGS_BUILD_NUM))
            {
                ABVersionInfo.BuildNumber = args[i].Substring(0, args[i].IndexOf("."));
            }
            else if (args[i].Contains(JenkinsBuildConstants.ARGS_JOB_NAME))
            {
                ABVersionInfo.JobName = args[i].Substring(0, args[i].IndexOf("."));
            }
            else if (args[i].Contains(JenkinsBuildConstants.ARGS_BRAND_ID_NAME))
            {
                brandID = args[i].Substring(0, args[i].IndexOf("."));
            }
            else if (args[i].Contains(JenkinsBuildConstants.ARGS_BUCKET_NAME))
            {
                bucketName = args[i].Substring(0, args[i].IndexOf("."));
            }
            else if (args[i].Contains(JenkinsBuildConstants.ARGS_LABEL_NAME))
            {
                DisableAllScenes();
              
                Debug.Log("JenkinsBuildHelper : PerformAndroidIOSBuild : Selected Scens " + args[i]);

                try
                {
                    string[] labelName = args[i].Split('_');
                    AddRequiredGamesScenes(labelName[1]);

                }
                catch (Exception e)
                {
                    Debug.Log("JenkinsBuildHelper : PerformAndroidIOSBuild : split failed " + e.StackTrace);
                }
            }
            else
            {
                switch (args[i])
                {
                    case JenkinsBuildConstants.ARGS_BUILD_TYPE_DEBUG:
                        isDebugBuild = true;
                        break;
                    case JenkinsBuildConstants.ARGS_AB_ENABLED:
                        isAssetBundleFeatureEnabled = true;
                        break;
                    case JenkinsBuildConstants.BUNDLE_TYPE_ASSET_BUNDLES:
                        if (isAssetBundleFeatureEnabled)
                            CreateAssetBundles();
                        break;
                    case JenkinsBuildConstants.BUNDLE_TYPE_ADDRESSABLES:
                        //if (isAssetBundleFeatureEnabled)
                           // CreateAddressables();
                        break;
                }
            }

        }
        
        //RunTestCases();
        //WriteVersionNumberToFile();

        //SetCDCredentials(isDebugBuild);

        Debug.Log("Job_name : " + ABVersionInfo.JobName);
        string export_folder_name = ABVersionInfo.JobName + "_Export";
#if UNITY_ANDROID
 Debug.Log("UNITY_ANDROID : called...");
       //GenericBuild(SCENES, "../../" + export_folder_name, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
       GenericBuild(SCENES, "cicd_unity_sample_export_android", BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
#else
        PerformiOSBuild();
#endif
    }

    [MenuItem("IvyJenkins/PerformiOSBuild")]
    static void PerformiOSBuild()
    {
        Debug.Log("PerformiOSBuild : called...");
        //GenericBuild(SCENES, "../../"+ ABVersionInfo.JobName, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
        GenericBuild(SCENES, "cicd_unity_sample_export_iOS", BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    [MenuItem("IvyJenkins/DisableAllScenes")]
    static void DisableAllScenes() {

        Debug.Log("JenkinsBuildHelper : DisableAllScenes : called...");

        EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;

        for (int i = 0; i < existingScenes.Length; i++)
        {
            UnityEngine.Debug.Log("JenkinsBuildHelper DisableAllScenes: name & path : " + existingScenes[i].path + " Enabled : " + existingScenes[i].enabled);

            bool isSplashScene = existingScenes[i].path.Contains(JenkinsBuildConstants.SPLASH_SCENE);

            if (!isSplashScene)
            {
                existingScenes[i].enabled = false;
            }

        }

        EditorBuildSettingsScene[] newScenes = existingScenes;
        EditorBuildSettings.scenes = newScenes;
    }

    [MenuItem("IvyJenkins/SimulateScenesEnableAsPerLabel")]
    static void SimulateScenesEnableAsPerLabel() {
        DisableAllScenes();
        string argStr = JenkinsBuildConstants.ARGS_LABEL_NAME + "_" + JenkinsBuildConstants.LABEL_BWIN_BE;
        string[] sceneNames = argStr.Split('_');
        AddRequiredGamesScenes(sceneNames[1]);
    }

    static void AddRequiredGamesScenes(string labelName) {

        Debug.Log("JenkinsBuildHelper : AddRequiredGamesScenes : lableName : " + labelName);

        Type enumName = null;

        switch (labelName) {
            
            case JenkinsBuildConstants.LABEL_US:
                enumName = typeof(JenkinsBuildConstants.USGames);
                break;
            case JenkinsBuildConstants.LABEL_ROW:
                enumName = typeof(JenkinsBuildConstants.ROWGames);
                break;
            case JenkinsBuildConstants.LABEL_BWIN_ES:
                enumName = typeof(JenkinsBuildConstants.BwinESGames);
                break;
            case JenkinsBuildConstants.LABEL_SBZA:
                enumName = typeof(JenkinsBuildConstants.SBZAGames);
                break;
            case JenkinsBuildConstants.LABEL_BWIN_BE:
                enumName = typeof(JenkinsBuildConstants.BwinBEGames);
                break;
            case JenkinsBuildConstants.LABEL_BWIN_COM:
                enumName = typeof(JenkinsBuildConstants.BwinComGames);
                break;
            case JenkinsBuildConstants.LABEL_SB_COM:
                enumName = typeof(JenkinsBuildConstants.SBComGames);
                break;
            case JenkinsBuildConstants.LABEL_ONTARIO:
                enumName = typeof(JenkinsBuildConstants.OntarioGames);
                break;
            case JenkinsBuildConstants.LABEL_BET_MGM:
                enumName = typeof(JenkinsBuildConstants.BetMgmGames);
                break;
            case JenkinsBuildConstants.LABEL_SB_UK:
                enumName = typeof(JenkinsBuildConstants.SBUKGames);
                break;
            case JenkinsBuildConstants.LABEL_GAMEBOOKERS:
                enumName = typeof(JenkinsBuildConstants.GameBookersGames);
                break;
            case JenkinsBuildConstants.LABEL_PARTYPOKER_COM:
                enumName = typeof(JenkinsBuildConstants.PartyPokerComGames);
                break;
            case JenkinsBuildConstants.LABEL_PREMIUM:
                enumName = typeof(JenkinsBuildConstants.PremiumGames);
                break;
        }
        AddScenesToTheBuild(enumName);
    }

    static void AddScenesToTheBuild(Type label)
    {
        foreach (string sName in Enum.GetNames(label))
        {
            Debug.Log("JenkinsBuildHelper : AddScenesToTheBuild : sceneName : " + sName);
            AddSceneWithName(sName);
        }
    }

    static void AddSceneWithName(string sceneName) {

        EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;

        for (int i = 0; i < existingScenes.Length; i++)
        {
            UnityEngine.Debug.Log("AddSceneWithName existingScenes: " + existingScenes[i].path + " Enabled: " + existingScenes[i].enabled);

            if (existingScenes[i].path.Contains(sceneName))
            {
                Debug.Log("JenkinsBuildHelper : AddSceneWithName : sceneName : " + sceneName+" got added");
                existingScenes[i].enabled = true;
            }
            else
            {
                Debug.Log("JenkinsBuildHelper : AddSceneWithName : no scene exist with give name " + sceneName);
            }

        }
        EditorBuildSettingsScene[] newScenes = existingScenes;
        EditorBuildSettings.scenes = newScenes;
    }

    //[MenuItem("IvyJenkins/EnableSelectedScenes")]
    static void EnableSelectedScenes(string sceneName)
    {

        Debug.Log("JenkinsBuildHelper : EnableSelectedScenes : called...");

        EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;

        for (int i = 0; i < existingScenes.Length; i++)
        {
            UnityEngine.Debug.Log("AddCarromScenes existingScenes: " + existingScenes[i].path + " Enabled: " + existingScenes[i].enabled);

            if (existingScenes[i].path.Contains(sceneName))
            {
                existingScenes[i].enabled = true;
            }
            else {
                Debug.Log("JenkinsBuildHelper : EnableSelectedScenes : no scene exist with give name "+sceneName);
            }

        }
        EditorBuildSettingsScene[] newScenes = existingScenes;
        EditorBuildSettings.scenes = newScenes;
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        Debug.Log("JenkinsBuildHelper : GenericBuild : " + target_dir + " " + build_target + " " + build_options);
        //BuildCache.PruneCache();
        //BuildCache.PurgeCache(false);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        
#if UNITY_IOS
                buildPlayerOptions.options = build_options;
#endif

        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = target_dir;
        buildPlayerOptions.target = build_target;

#if UNITY_ANDROID
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
#endif
        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;
        //buildPlayerOptions.options = BuildOptions.CompressWithLz4;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            UnityEngine.Debug.Log("Build failed");
        }
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    public static void SetCDCredentials(string projectID, string projectName)
    {
        const string ProjectSettingsAssetPath = "ProjectSettings/ProjectSettings.asset";
        SerializedObject projectSettings = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(ProjectSettingsAssetPath)[0]);
        SerializedProperty projectIDProp = projectSettings.FindProperty("cloudProjectId");
        UnityEngine.Debug.Log("SetCDCredentials : projectIDProp : " + projectIDProp);
        SerializedProperty projectNameProp = projectSettings.FindProperty("projectName");
        UnityEngine.Debug.Log("SetCDCredentials : projectNameProp : " + projectNameProp);
        projectIDProp.stringValue = projectID;
        projectNameProp.stringValue = projectName;
        bool status = projectSettings.ApplyModifiedProperties();
        if (status)
            AssetDatabase.SaveAssets();
    }

    static void SetCDCredentials(bool isDebug)
    {
        if (isDebug)
        {
            SetCDCredentials(ANDROID_CD_ID_STAGE, ANDROID_CD_NAME_STAGE);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "DEBUG_BUILD;");
        }
        else
        {
            SetCDCredentials(ANDROID_CD_ID_PROD, ANDROID_CD_NAME_PROD);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "RELEASE_BUILD;");
        }
    }

    [MenuItem("IvyJenkins/RunTestCases")]
    static void RunTestCases() {

        Debug.Log("JenkinsBuildHelper : RunTestCases : called... ");

        TestRunnerApi testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
        Debug.Log("JenkinsBuildHelper : RunTestCases : testRunnerApi : " + testRunnerApi);
        testRunnerApi.RegisterCallbacks(new UnitTestCallbacks(), 0);
        Filter filter = new Filter()
        {
            testMode = TestMode.PlayMode
        };

        testRunnerApi.Execute(new ExecutionSettings(filter));

        /*string testPlatform = "editmode";
        string resultFilePath = "Assets/Reports/report.xml";
        string[] testFilters = { };
        string[] testCategories = { };

        var assembly = Assembly.Load("UnityEditor.TestRunner");
        var batchType = assembly.GetType("UnityEditor.TestTools.TestRunner.Batch");
        Debug.Log("batchType : " + batchType);
        var runTestsMethod = batchType.GetMethod("RunTests", BindingFlags.Static | BindingFlags.NonPublic);
        runTestsMethod.Invoke(null, new object[] { testPlatform, resultFilePath, testFilters, testCategories });*/ 
    }

    [MenuItem("IvyJenkins/CreateAssetBundles")]
    public static void CreateAssetBundles() {

        string root = "AssetBundles";

        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + root);

        AssetBundleBuild[] buildMap = new AssetBundleBuild[fileEntries.Length];

        int count = 0;

        string path = "";

        foreach (string fileName in fileEntries)
        {
            string ss = fileName.Substring(0, fileName.LastIndexOf('/') + 1);
           
            string newFileName = fileName.Replace(ss, "");

            newFileName = newFileName.Replace(".meta", "");

            if (newFileName != ".DS_Store")
            {
                buildMap[count].assetBundleName = newFileName;
                string[] enemyAssets = new string[1];
                enemyAssets[0] = "Assets/AssetBundles/" + newFileName;
                buildMap[count].assetNames = enemyAssets;

                path = Application.dataPath + "Assets../../../AssetBundles/";

            }

            count++;
        }

        //string outPutPath = Application.dataPath + "/../../../../Unitygamebundles/testAssetBundles/" + EditorUserBuildSettings.activeBuildTarget +"/betmgm";
        //string outPutPath = Application.dataPath + "/../../../../Unitygamebundles/testAssetBundles/" + EditorUserBuildSettings.activeBuildTarget + "/" + brandID;
        string outPutPath = Application.dataPath + "/../../../../Unitygamebundles/" + bucketName + "/" + EditorUserBuildSettings.activeBuildTarget + "/" + brandID;


        BuildPipeline.BuildAssetBundles(outPutPath, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();

        //RemoveUnwantedFilesAfterBundle();

    }

    [MenuItem("IvyJenkins/WriteVersionNumberToFile")]
    static void WriteVersionNumberToFile() {
        string path = "Assets/Resources/VersionNumber.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(Application.version);
        writer.Close();
    }
}
