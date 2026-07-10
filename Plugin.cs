using BepInEx;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace info
{
 // made this its own seperate mod because someone wanted it
 // most of this is just taken from one of my very old mods under a different alias 

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {

        private GameObject textObject;
        private TextMesh textMesh;
        private float deltaTime = 0.0f;
        private bool isTextVisible = true;
        private bool pressed = true;
        private float gameStartTime;
        public string hmd = "HMD not found";

        void Start()
        {
            Invoke(nameof(Cooltext), 3f);
        }

        void Update()
        {
            // for some reason start is never called and neither is onenable so im sorry but we are getting the hmd every frame because im lazy
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);

            foreach (var device in inputDevices)
            {
                if ((device.characteristics & InputDeviceCharacteristics.HeadMounted) == InputDeviceCharacteristics.HeadMounted)
                {
                    hmd = device.name;
                    int openParen = hmd.IndexOf('(');
                    int closeParen = hmd.IndexOf(')');
                    if (openParen != -1 && closeParen > openParen)
                    {
                        hmd = hmd.Substring(openParen + 1, closeParen - openParen - 1);
                    }
                }
            }

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
                float fps = 1.0f / deltaTime;
                float serverPing = PhotonNetwork.GetPing();

                textMesh.text = $"FPS: {Mathf.Ceil(fps)}\nPING: {serverPing}ms\nHMD: {hmd}";      
        }

        void Cooltext()
        {

            GameObject leftHand = GameObject.Find("head");
            if (leftHand != null)
            {

                textObject = new GameObject("Stats");

                textMesh = textObject.AddComponent<TextMesh>();
                textMesh.fontSize = 14;
                textMesh.characterSize = 0.01f;
                textMesh.color = Color.white;

                textMesh.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

                textObject.transform.SetParent(leftHand.transform);
                textObject.transform.localPosition = new Vector3(-0.25f, 0.1f, 0.8f);
                textObject.transform.localRotation = Quaternion.identity;


                textObject.SetActive(true);
            }
        }

    }
}
