using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using NMGC.Core;
using UnityEngine;

namespace NMGC;

[BepInPlugin(Constants.PluginGuid, Constants.PluginName, Constants.PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public static AssetBundle NMGCBundle;

    private void Start()
    {
        new Harmony(Constants.PluginGuid).PatchAll(Assembly.GetExecutingAssembly());
        GorillaTagger.OnPlayerSpawned(OnGameInitialized);
    }

    private void Update()
    {
        if (UnityInput.Current.GetKeyDown(KeyCode.F4))
            GUIController.MainPanel.gameObject.SetActive(!GUIController.MainPanel.gameObject.activeSelf);
    }

    private void OnGameInitialized()
    {
        Stream bundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NMGC.Resources.nmgc");
        NMGCBundle = AssetBundle.LoadFromStream(bundleStream);
        bundleStream?.Close();

        if (NMGCBundle == null)
        {
            Debug.LogError("[NMGC] Bundle could not be loaded!");

            return;
        }

        GameObject canvasPrefab = NMGCBundle.LoadAsset<GameObject>("NoMoreGettingChecked");
        GameObject canvas       = Instantiate(canvasPrefab);
        canvas.AddComponent<GUIController>();
    }
}