using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using UIExpansionKit.API;
using Harmony;
using MelonLoader;
using UnityEngine;
using UnityEditor;
using VRC;
using System.Collections.Generic;

[assembly:MelonInfo(typeof(SmallerEmojis.SmallerEmojis), "SmallerEmojis", "0.1.0", "nonce-twice", "https://github.com/nonce-twice/SmallerEmojis")]
[assembly:MelonGame("VRChat", "VRChat")]

namespace SmallerEmojis 
{

    public class SmallerEmojis : MelonMod
    {

        public const string Pref_CategoryName = "Smaller Emojis";
        public bool Pref_DebugOutput = false;
        public float Pref_EmojiSize = 0.1f;

        public override void OnApplicationStart()
        {
            MelonPreferences.CreateCategory(Pref_CategoryName);
            MelonPreferences.CreateEntry(Pref_CategoryName, nameof(Pref_EmojiSize), 0.1f, "Emoji Max Size [0-1]");
            MelonPreferences.CreateEntry(Pref_CategoryName, nameof(Pref_DebugOutput),   false,  "Show Debug Output");
//            UIExpansionKit.API.ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("HideEmojis", HideEmojis);
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            switch (buildIndex) {
                case 0: break; 
                case 1: break;  
                case 2: break;  
                default:
                    ApplyAllSettings();
                    break;
            }
        }
        public override void OnPreferencesSaved()
        {
            ApplyAllSettings();
        }

        private void HideEmojis()
        {
            var particleRenderers = Resources.FindObjectsOfTypeAll<ParticleSystemRenderer>();

            IEnumerable<ParticleSystemRenderer> particleSystems =
                from renderer in particleRenderers
                where renderer.gameObject.name.StartsWith("Emoji")
                select renderer;

            LogDebugMsg($"Found {particleSystems.ToArray().Length.ToString()} possible emoji objects."); 
            LogDebugMsg($"Setting emoji sizes");

            foreach(var ps in particleSystems)
            {
                if (ps != null)
                {
//                    LogDebugMsg($"Found {ps.name}");
                    ps.maxParticleSize = Pref_EmojiSize;
                }
            }
        }

        private void ApplyAllSettings()
        {
            UpdatePreferences();
            HideEmojis();
        }

        private void UpdatePreferences()
        {
            Pref_DebugOutput = MelonPreferences.GetEntryValue<bool>(Pref_CategoryName, nameof(Pref_DebugOutput));
            Pref_EmojiSize   = Mathf.Clamp(MelonPreferences.GetEntryValue<float>(Pref_CategoryName, nameof(Pref_EmojiSize)), 0f, 1f);
        }

        private void LogDebugMsg(string msg)
        {
            if (!Pref_DebugOutput)
                return; 
            MelonLogger.Msg(msg);
        }
    }
}