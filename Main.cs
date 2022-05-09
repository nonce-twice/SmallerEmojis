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

[assembly:MelonInfo(typeof(SmallerEmojis.SmallerEmojis), "SmallerEmojis", "0.1", "nonce-twice", "https://github.com/nonce-twice/SmallerEmojis")]
[assembly:MelonGame("VRChat", "VRChat")]

namespace SmallerEmojis 
{

    public class SmallerEmojis : MelonMod
    {

        // Debug for limiting stuff
        public const string Pref_CategoryName = "Smaller Emojis";
        public bool Pref_DebugOutput = true;
        public float Pref_EmojiSize = 0.5f;

        private readonly static string[] EmojiList =
        {
            "EmojiLike",
            "EmojiTomato",
            "Emoji_Fall_Bats"
        };

        public override void OnApplicationStart()
        {
            MelonPreferences.CreateCategory(Pref_CategoryName);
            MelonPreferences.CreateEntry(Pref_CategoryName, nameof(Pref_DebugOutput),   true,  "Show Debug Output");
            MelonPreferences.CreateEntry(Pref_CategoryName, nameof(Pref_EmojiSize),   0.5f,  "Emoji Size as factor of default");
            UIExpansionKit.API.ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("HideEmojis", HideEmojis);
        }

        private void HideEmojis()
        {
            LogDebugMsg($"Setting emoji sizes");

            var particleRenderers = Resources.FindObjectsOfTypeAll<ParticleSystemRenderer>();

            IEnumerable<ParticleSystemRenderer> particleSystems =
                from renderer in particleRenderers
                where renderer.gameObject.name.StartsWith("Emoji")
                select renderer;

            LogDebugMsg($"Found {particleSystems.ToArray().Length.ToString()} possible emoji objects."); 

            foreach(var ps in particleSystems)
            {
                if (ps != null)
                {
                    LogDebugMsg($"Found {ps.name}");
                    ps.maxParticleSize = Pref_EmojiSize;
                }
            }
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
        private void ApplyAllSettings()
        {
            UpdatePreferences();
        }



        private void UpdatePreferences()
        {
            Pref_DebugOutput = MelonPreferences.GetEntryValue<bool>(Pref_CategoryName, nameof(Pref_DebugOutput));
            Pref_EmojiSize   = MelonPreferences.GetEntryValue<float>(Pref_CategoryName, nameof(Pref_EmojiSize));
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