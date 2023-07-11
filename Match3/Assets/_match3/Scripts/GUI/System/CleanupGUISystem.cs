using Unity.Entities;
using UnityEngine;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class CleanupGUISystem : SystemBase
    {
        private EntityQuery _cleanupQuery;

        protected override void OnCreate()
        {
            _cleanupQuery = SystemAPI.QueryBuilder()
                .WithAll<GUIReference>()
                .WithNone<GUIManager, GUIPrefab>()
                .Build();

            RequireForUpdate(_cleanupQuery);
        }

        protected override void OnUpdate()
        {
            var guiReferenceEntity = _cleanupQuery.GetSingletonEntity();
            var guiReference = _cleanupQuery.GetSingleton<GUIReference>();

            Object.Destroy(guiReference.guiReference);
            EntityManager.RemoveComponent<GUIReference>(guiReferenceEntity);
        }
    }
}