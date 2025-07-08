using Game.Config;
using Game.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Player
{
    public sealed class PlayerView : UnitView
    {
        [SerializeField] private Image _health;
        [SerializeField] private SkinnedMeshRenderer _full;
        [SerializeField] private SkinnedMeshRenderer _head;
        [SerializeField] private SkinnedMeshRenderer _helmet;
        [SerializeField] private SkinnedMeshRenderer _vest;
        [SerializeField] private SkinnedMeshRenderer _uniform;
        [SerializeField] private SkinnedMeshRenderer _gloves;
        [SerializeField] private SkinnedMeshRenderer _shoes;

        protected override void OnModelChanged(UnitModel model)
        {
            var playerModel = model as PlayerModel;

            _health.fillAmount = (float)playerModel.Health / playerModel.HealthNominal;

            bool hasAllClothMeshes = playerModel.HasAllClothMeshes;
            if(!hasAllClothMeshes)
            {
                _full.sharedMesh = playerModel.FullSkinnedMesh;
                _head.sharedMesh = null;
                return;
            }

            _helmet.sharedMesh = playerModel.ClothMeshMap[ClothElementType.Helmet];
            _vest.sharedMesh = playerModel.ClothMeshMap[ClothElementType.Vest];
            _uniform.sharedMesh = playerModel.ClothMeshMap[ClothElementType.Uniform];
            _gloves.sharedMesh = playerModel.ClothMeshMap[ClothElementType.Gloves];
            _shoes.sharedMesh = playerModel.ClothMeshMap[ClothElementType.Shoes];
        }
    }
}

