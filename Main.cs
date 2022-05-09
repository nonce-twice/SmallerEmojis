using System;
using System.Reflection;
using System.Collections;
using UIExpansionKit.API;
using Harmony;
using MelonLoader;
using UnityEngine;
using UnityEditor;
using VRC;

[assembly:MelonInfo(typeof(SmallerEmojis.SmallerEmojis), "SmallerEmojis", "0.1", "nonce-twice", "https://github.com/nonce-twice/SmallerEmojis")]
[assembly:MelonGame("VRChat", "VRChat")]

namespace SmallerEmojis 
{

    public class SmallerEmojis : MelonMod
    {

        // Debug for limiting stuff
        public const string Pref_CategoryName = "Smaller Emojis";
        public bool Pref_DebugOutput = false;

        private bool initialized = false;
        private bool avatarMenuOpen = false;
        private bool desktopZoomOut = false;
        private bool grabOn = true;

        private ICustomShowableLayoutedMenu customMenu;

        private VRCVrCameraSteam ourSteamCamera;
        private Transform ourCameraRig;
        private Transform ourCameraTransform;


        public override void OnApplicationStart()
        {
            MelonPreferences.CreateCategory(Pref_CategoryName);
            MelonPreferences.CreateEntry(Pref_CategoryName, nameof(Pref_DebugOutput),   false,  "Show Debug Output");


//            customMenu = UIExpansionKit.API.ExpansionKitApi.CreateCustomFullMenuPopup(LayoutDescription.WideSlimList);
//            customMenu.AddSimpleButton("Move model to floor", OnPageAvatarOpen);

//            var avatarMenu = UIExpansionKit.API.ExpansionKitApi.GetExpandedMenu(ExpandedMenu.AvatarMenu);
//            avatarMenu.AddSimpleButton("Toggle Grab", ToggleGrab);
//            avatarMenu.AddSimpleButton("BetterAvatarPreviewMenu", OpenMenu);

        }

        public void OpenMenu()
        {
            customMenu.Show();
        }
        public void CloseMenu()
        {
            customMenu.Hide();
        }

        // Skip over initial loading of (buildIndex, sceneName): [(0, "app"), (1, "ui")]
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            switch (buildIndex) {
                case 0: 
                    break; 
                case 1: 
                    break;  
                default:
                    ApplyAllSettings();
                    break;
            }
        }

        public override void OnPreferencesSaved()
        {
            ApplyAllSettings();
        }

        private void ApplyAllSettings()
        {
            UpdatePreferences();
        }

        private void UpdatePreferences()
        {
            Pref_DebugOutput = MelonPreferences.GetEntryValue<bool>(Pref_CategoryName, nameof(Pref_DebugOutput));
        }

        private void LogDebugMsg(string msg)
        {
            if (!Pref_DebugOutput)
            {
                return; 
            }
            MelonLogger.Msg(msg);
        }
        
    }
}