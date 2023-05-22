using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Engine.Core.Effects;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.Gui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;

        private Camera _camera;
        private Transform _cameraTransform;
        private int _cubeCount = 50;
        private Transform[] _cubeTransforms;
        private float _cubeAngle = 0.0f;

        // Init is called on startup. 
        public override void Init()
        {
            _cubeTransforms = new Transform[_cubeCount];

            // Create a scene tree containing the camera :
            // _scene---+
            //          |
            //          +---cameraNode-----_camera

            // THE CAMERA
            // A node containing one Camera component.
            _camera = new Camera(ProjectionMethod.Perspective, 5, 100, M.PiOver4)
            {
                BackgroundColor = (float4)ColorUint.Greenery,
            };

            _cameraTransform = new Transform()
            {
                Translation = new float3(0, 0, -50)
            };

            var cameraNode = new SceneNode();
            cameraNode.Components.Add(_cameraTransform);
            cameraNode.Components.Add(_camera);

            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);

            // cubes
            for (int i = 0; i < _cubeCount; i++)
            {
                var cubeNode = new SceneNode();

                var transform = new Transform()
                {
                    Translation = new float3(-(_cubeCount / 2) + i, 0, 0),
                    Scale = new float3(1, 1, 1)
                };

                var effect = MakeEffect.FromDiffuse((float4)(i % 2 == 0 ? ColorUint.Black : ColorUint.White));
                var cubeMesh = new CuboidMesh(new float3(1, 1, 1));

                cubeNode.Components.Add(transform);
                cubeNode.Components.Add(effect);
                cubeNode.Components.Add(cubeMesh);

                _scene.Children.Add(cubeNode);

                _cubeTransforms[i] = transform;
            }

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            _cubeAngle += 90.0f * M.Pi/180.0f * DeltaTime;
            for (int i = 0; i < _cubeCount; i++)
            {
                _cubeTransforms[i].Translation = new float3(_cubeTransforms[i].Translation.x, 5 * M.Sin(2 * TimeSinceStart + i), _cubeTransforms[i].Translation.z);
                _cubeTransforms[i].Rotation = new float3(_cubeAngle / 2, _cubeAngle, 2 * _cubeAngle);
                float f = M.Sin(2 * TimeSinceStart + i);
            }

            // Render the scene tree
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

    }
}