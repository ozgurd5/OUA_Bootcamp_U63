using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public static bool isHostCoder;

    public static void HostSelectCoder()
    {
        isHostCoder = true;
    }

    public static void HostSelectArtist()
    {
        isHostCoder = false;
    }
}
