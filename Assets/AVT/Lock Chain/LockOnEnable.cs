using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AVT
{
	public class LockOnEnable : MonoBehaviour
	{
		public LockChain lockChain;
		public string lockKey;

		private void OnEnable()
		{
			if (lockChain != null)
				lockChain.Lock(lockKey);
		}

		private void OnDisable()
		{
			if (lockChain != null) {
				VoidObject.Instance.StartCoroutine(UnlockNextFrame());
			}
		}

		protected IEnumerator UnlockNextFrame()
		{
			yield return new WaitForEndOfFrame();
			lockChain.Unlock(lockKey);
		}
	}
}
