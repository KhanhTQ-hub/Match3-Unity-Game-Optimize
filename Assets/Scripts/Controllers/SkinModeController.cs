using Enums;
using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(menuName = "Controllers/SkinModeController", fileName = "SkinModeController")]
    public class SkinModeController : ScriptableObject
    {
        public SkinType SkinType = SkinType.FOOD;
    }
}