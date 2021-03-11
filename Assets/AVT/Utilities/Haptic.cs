#if MOREMOUNTAINS_NICEVIBRATIONS

using UnityEngine;
using MoreMountains.NiceVibrations;

public class Haptic : MonoBehaviour
{
	public void PlayHaptic(int type)
	{
		MMVibrationManager.Haptic((HapticTypes)type);
	}
}

#endif
