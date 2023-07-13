using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _match3.Input
{
    public struct InputData : IComponentData
    {
        public float2 mouseWorldPosition;
        public float2 mousePosition;

        public bool lmbWasPressed;
        public bool lmbWasReleased;
    }

    public partial class InputSystem : SystemBase
    {
        private Plane _zPlane;

        protected override void OnCreate()
        {
            _zPlane = new Plane(-Vector3.forward, 0f);

            var inputEntity = EntityManager.CreateEntity();
            EntityManager.AddComponent<InputData>(inputEntity);


            //RequireForUpdate<UnityEngine.Camera>();
        }

        protected override void OnUpdate()
        {
            var inputData = SystemAPI.GetSingletonRW<InputData>();
            var input = inputData.ValueRW;

            var mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            input.mouseWorldPosition = (Vector2)mousePosition;
            input.mousePosition = (Vector2)_zPlane.ClosestPointOnPlane(mousePosition);

            input.lmbWasPressed = UnityEngine.Input.GetMouseButtonDown(0);
            input.lmbWasReleased = UnityEngine.Input.GetMouseButtonUp(0);

            inputData.ValueRW = input;
        }
    }
}