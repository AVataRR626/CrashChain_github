using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class QSIPlatformCharAnimator : MonoBehaviour 
{
	public bool inAction = false;

	[System.Serializable]
	public class AnimationParams
	{
		public float moveSpeed;
		public bool hit;
		public bool jumpTrigger;
		public bool isJumping;
		public bool meditateTrigger;
		public bool isMeditating;
		public bool [] actionTriggers;

	}
	public AnimationParams animationParams;

	[System.Serializable]
	public class ParameterStrings
	{
		public string moveSpeed = "MoveSpeed";
		public string hit = "Hit";
		public string jumpTrigger = "jump";
		public string isJumping = "isJumping";
		public string meditateTrigger = "meditate";
		public string isMeditating = "isMeditating";
		public string [] actionStrings;

	}
	public ParameterStrings paramStrings;

	public Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () 
	{
		CheckTriggers();
		CheckInAction();

	}

	public bool CheckInAction()
	{
		inAction = false;

		for(int i = 0; i < animationParams.actionTriggers.Length; i++)
		{
			if(animationParams.actionTriggers[i])
			{	inAction = true;
				break;
			}
		}

		return inAction;
	}

	void CheckTriggers()
	{
		//link speed for walking and running
		animator.SetFloat(paramStrings.moveSpeed,animationParams.moveSpeed);

		HandleMeditate();

		HandleJumps();

		//now do the other actions.
		if(animationParams.actionTriggers.Length == paramStrings.actionStrings.Length)
		{
			for(int i = 0; i < animationParams.actionTriggers.Length;i ++)
			{

				if(animationParams.actionTriggers[i])
				{
					animator.SetTrigger(paramStrings.actionStrings[i]);
					animationParams.actionTriggers[i] = false;
				}
			}
		}
	}

	void HandleJumps()
	{
		//link stuff for jumping
		animator.SetBool(paramStrings.isJumping,animationParams.isJumping);
		
		//if(!animationParams.isJumping)
		//animationParams.jumpTrigger = false;
		
		
		if(animationParams.jumpTrigger)
		{	animator.SetTrigger(paramStrings.jumpTrigger);
			animationParams.jumpTrigger = false;
		}
	}

	void HandleMeditate()
	{
		//link stuff for meditation
		animator.SetBool(paramStrings.isMeditating,  animationParams.isMeditating);

		if(!animationParams.isMeditating)
			animationParams.meditateTrigger = false;

		if(animationParams.meditateTrigger)
		{
			animator.SetTrigger (paramStrings.meditateTrigger);
			animationParams.meditateTrigger = false;
		}
	}

	public void Meditate()
	{
		animationParams.meditateTrigger = true;
		animationParams.isMeditating = true;
	}

	public void Damage(float dmg)
	{
		Debug.Log ("GotHitAnim:" + paramStrings.hit);
		animator.SetTrigger(paramStrings.hit);
	}
}
