// Copyright (c) Cragon. All rights reserved.

namespace Casinos
{
    using System;
    using System.IO;
    using UnityEngine;

    public class EditorCfgUserSettingsCopy
    {
        //-------------------------------------------------------------------------
        public bool ForceUseDirResourcesLaunch;// �Ƿ�ǿ��ʹ��Ŀ¼ResourcesLaunch
        public bool ForceUseDirDataOss;// �Ƿ�ǿ��ʹ��Ŀ¼DataOss
    }

    public class MbMain : MonoBehaviour
    {
        //---------------------------------------------------------------------
        CasinosContext Context { get; set; }

        string BuglyAppIDAndroid = "0aed5e7e56";
        string BuglyAppIDiOS = "e438009ecd";

        //---------------------------------------------------------------------
        void Awake()
        {
#if DEBUG
            BuglyAgent.ConfigDebugMode(true);
#else
            BuglyAgent.ConfigDebugMode(false);
#endif

            // Config default channel, version, user 
            BuglyAgent.ConfigDefault(null, null, null, 0);
            // Config auto report log level, default is LogSeverity.LogError, so the LogError, LogException log will auto report
            BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogError);
            // Config auto quit the application make sure only the first one c# exception log will be report, please don't set TRUE if you do not known what are you doing.
            BuglyAgent.ConfigAutoQuitApplication(false);
            // If you need register Application.RegisterLogCallback(LogCallback), you can replace it with this method to make sure your function is ok.
            //BuglyAgent.RegisterLogCallback(null);

#if UNITY_IPHONE || UNITY_IOS
            BuglyAgent.InitWithAppId(BuglyAppIDiOS);
#elif UNITY_ANDROID
            BuglyAgent.InitWithAppId(BuglyAppIDAndroid);
#endif

            // TODO Required. If you do not need call 'InitWithAppId(string)' to initialize the sdk(may be you has initialized the sdk it associated Android or iOS project),
            // please call this method to enable c# exception handler only.
            BuglyAgent.EnableExceptionHandler();

            // TODO NOT Required. If you need to report extra data with exception, you can set the extra handler
            // BuglyAgent.SetLogCallbackExtrasHandler (MyLogCallbackExtrasHandler);
            //BuglyAgent.PrintLog(LogSeverity.LogInfo, "Init the bugly sdk");
        }

        //---------------------------------------------------------------------
        void Start()
        {
            //CasinosContext.Instance.AddLog("OpenInstall.RegisterWakeupHandler(_onWakeupFinish)");
            Debug.Log("RegisterWakeupHandler, GetInstall");

            var openinstall = GetComponent<io.openinstall.unity.OpenInstall>();
            openinstall.RegisterWakeupHandler(_onWakeupFinish);
            openinstall.GetInstall(60, _onGetInstallFinish);

            //openinstall.ReportRegister();
            //openinstall.ReportEffectPoint("aaa", 1);

            //Debug.Log("��ȡ������1111");
            //string c = NativeFun.getCountryCode();
            //Debug.Log("��ȡ������2222");
            //Debug.Log("��ȡ������3333  " + c);

            bool force_use_resouceslaunch = false;
            bool force_use_dataoss = false;

#if UNITY_EDITOR
            EditorCfgUserSettingsCopy cfg_usersettings = null;
            string p = Path.Combine(Environment.CurrentDirectory, "./Assets/SettingsUser/");
            var di = new DirectoryInfo(p);
            string path_settingsuser = di.FullName;

            string full_filename = Path.Combine(path_settingsuser, StringDef.FileEditorUserSettings);
            if (File.Exists(full_filename))
            {
                using (StreamReader sr = File.OpenText(full_filename))
                {
                    string s = sr.ReadToEnd();
                    cfg_usersettings = JsonUtility.FromJson<EditorCfgUserSettingsCopy>(s);
                }
            }

            if (cfg_usersettings != null)
            {
                force_use_resouceslaunch = cfg_usersettings.ForceUseDirResourcesLaunch;
                force_use_dataoss = cfg_usersettings.ForceUseDirDataOss;
            }

            string info = string.Format("ForceUseDirResourcesLaunch={0}, ForceUseDirDataOss={1}",
                force_use_resouceslaunch, force_use_dataoss);
            Debug.Log(info);
            Debug.Log("PersistentDataPath=" + Application.persistentDataPath);
#endif

            Context = new CasinosContext(force_use_resouceslaunch, force_use_dataoss);
            Context.Launch();
        }

        //---------------------------------------------------------------------
        void Update()
        {
            if (Context != null)
            {
                Context.Update(Time.deltaTime);
            }
        }

        //---------------------------------------------------------------------
        void OnDestroy()
        {
            if (Context != null)
            {
                Context.Close();
            }
        }

        //-------------------------------------------------------------------------
        void OnApplicationPause(bool pause)
        {
            if (Context != null)
            {
                Context.OnApplicationPause(pause);
            }
        }

        //-------------------------------------------------------------------------
        void OnApplicationFocus(bool focusStatus)
        {
            if (Context != null)
            {
                Context.OnApplicationFocus(focusStatus);
            }
        }

        //---------------------------------------------------------------------
        void _onGetInstallFinish(io.openinstall.unity.OpenInstallData data)
        {
            Debug.Log("_onGetInstallFinish");

            string channel_code = " ";
            if (!string.IsNullOrEmpty(data.channelCode))
            {
                channel_code = data.channelCode;
            }

            string bind_data = " ";
            if (!string.IsNullOrEmpty(data.bindData))
            {
                bind_data = data.bindData;
            }

            var log = string.Format("MbNative._onGetInstallFinish() �������={0}���Զ�������={1}",
                channel_code, bind_data);
            Debug.Log(log);
        }

        //---------------------------------------------------------------------
        void _onWakeupFinish(io.openinstall.unity.OpenInstallData data)
        {
            Debug.Log("_onWakeupFinish");

            string channel_code = " ";
            if (!string.IsNullOrEmpty(data.channelCode))
            {
                channel_code = data.channelCode;
            }

            string bind_data = " ";
            if (!string.IsNullOrEmpty(data.bindData))
            {
                bind_data = data.bindData;
            }

            var log = string.Format("MbNative._onWakeupFinish() �������={0}���Զ�������={1}",
                channel_code, bind_data);
            Debug.Log(log);
        }
    }
}