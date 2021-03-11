using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AVT
{
	/*
	 * Usage Example:
	 * Lock Chain can be used as a Pause game variable:
	 * For example, game can be paused because of these reason:
	 * - Open Setting menu
	 * - Show cutscene
	 * - Player selecting item in inventory
	 * - ... etc
	 * 
	 * Whenever those event occur, we add a lock to lock chain 
	 * and unlock it if the event ended.
	 * For action that depend on pause game variable, we check
	 * if Pause Lock Chain isLock == true? (true if have more 
	 * than 0 locks)
	 */
	public class LockChain : MonoBehaviour
	{
		#region Properties
		public bool IsLock {
			get {
				return lockChain.Count > 0;
			}
		}
		protected HashSet<string> lockChain = new HashSet<string>();
		#endregion

		#region FUNCTION
		public void Lock(string key)
		{
			if (lockChain.Contains(key) == false)
				lockChain.Add(key);
		}

		public void Unlock(string key)
		{
			if (lockChain.Contains(key))
				lockChain.Remove(key);
		}

		#endregion
	}
}
