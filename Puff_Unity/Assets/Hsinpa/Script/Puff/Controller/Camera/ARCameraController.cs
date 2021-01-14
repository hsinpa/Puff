using Puff.Model;
using Puff.View;
using Puff.WorldManager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Hsinpa.Utility;
using UnityEngine.Rendering;

namespace Puff.Ctrl
{

    public class ARCameraController : ObserverPattern.Observer
    {
        [SerializeField]
        private ARCameraManager arCamera;

        [SerializeField]
        private ARCameraBackground arCameraBG;

        private DeviceOrientation _deviceOrientation;

        private CommandBuffer commandBuffer;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        Init();
                    }
                    break;

                case EventFlag.Event.LoginSuccessful:
                    {

                    }
                    break;
            }
        }

        private void Init() {
            commandBuffer = new CommandBuffer();
            commandBuffer.name = "AR Camera Background Blit Pass";

            _deviceOrientation = Input.deviceOrientation;
        }

        public Texture TakeScreenShot() {

            if (arCameraBG.material != null && commandBuffer != null)
            {
                //var commandBuffer = new CommandBuffer();
                //commandBuffer.name = "AR Camera Background Blit Pass";
                commandBuffer.Clear();

                var renderTexture = TextureUtility.GetRenderTexture(Screen.width, Screen.height, 24);
                var texture = !arCameraBG.material.HasProperty("_MainTex") ? null : arCameraBG.material.GetTexture("_MainTex");
                Graphics.SetRenderTarget(renderTexture.colorBuffer, renderTexture.depthBuffer);
                commandBuffer.ClearRenderTarget(true, false, Color.clear);
                commandBuffer.Blit(texture, BuiltinRenderTextureType.CurrentActive, arCameraBG.material);
                Graphics.ExecuteCommandBuffer(commandBuffer);

                return renderTexture;
            }

            return null;
        }

    }
}