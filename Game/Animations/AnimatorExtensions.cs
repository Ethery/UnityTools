using UnityEngine;

public static class AnimatorExtensions
{
	public static bool TrySetParameter(this Animator animator, int paramID, bool paramValue)
	{
		if (animator != null)
		{
			if (paramID != 0)
			{
				animator.SetBool(paramID, paramValue);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, int paramID, int paramValue)
	{
		if (animator != null)
		{
			if (paramID != 0)
			{
				animator.SetInteger(paramID, paramValue);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, int paramID, float paramValue)
	{
		if (animator != null)
		{
			if (paramID != 0)
			{
				animator.SetFloat(paramID, paramValue);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, int paramID)
	{
		if (animator != null)
		{
			if (paramID != 0)
			{
				animator.SetTrigger(paramID);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, string paramID, bool paramValue)
	{
		if (animator != null)
		{
			if (!string.IsNullOrEmpty(paramID))
			{
				animator.SetBool(paramID, paramValue);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, string paramID, int paramValue)
	{
		if (animator != null)
		{
			if (!string.IsNullOrEmpty(paramID))
			{
				animator.SetInteger(paramID, paramValue);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, string paramID, float paramValue)
	{
		if (animator != null)
		{
			if (!string.IsNullOrEmpty(paramID))
			{
				animator.SetFloat(paramID, paramValue);
				return true;
			}
		}
		return false;
	}

	public static bool TrySetParameter(this Animator animator, string paramID)
	{
		if (animator != null)
		{
			if (!string.IsNullOrEmpty(paramID))
			{
				animator.SetTrigger(paramID);
				return true;
			}
		}
		return false;
	}


}
