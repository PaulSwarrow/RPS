using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace DefaultNamespace
{
    public class GameItemPool : MonoBehaviour
    {
        private static GameItemPool pool;

        public static GameItemPool instance
        {
            get
            {
                if (!pool)
                {
                    pool = FindObjectOfType<GameItemPool>();
                }

                return pool;
            }
        }

        



    }
}