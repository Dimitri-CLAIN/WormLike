using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaiiImplementation
{
    public enum SlimeAnimationState
    {
        Idle,
        Walk,
        Jump,
        Attack,
        Damage
    }

    /// <summary>
    /// Copy of EnemyAi script to animate the assigned slime 
    /// </summary>
    public class AnimateSlime : MonoBehaviour
    {
        public KawaiiSlimeSelector.KawaiiSlime slimeType;
        public Face faces;
        public GameObject SmileBody;
        public SlimeAnimationState currentState;

        public Animator animator;
        public int damType;

        private int m_CurrentWaypointIndex;

        private bool move;
        private Material faceMaterial;
        private Vector3 originPos;
        [SerializeField]
        private float animSpeed; // agent.velocity.magnitude
        

        public enum WalkType
        {
            Patroll,
            ToOrigin
        }

        private WalkType walkType;

        void Start()
        {
            originPos = transform.position;
            faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
            walkType = WalkType.Patroll;
        }

        void SetFace(Texture tex)
        {
            faceMaterial.SetTexture("_MainTex", tex);
        }

        void Update()
        {


            switch (currentState)
            {
                case SlimeAnimationState.Idle:

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                    StopAgent();
                    SetFace(faces.Idleface);
                    break;

                case SlimeAnimationState.Walk:

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;
                    
                    if (walkType == WalkType.ToOrigin)
                    {
                        SetFace(faces.WalkFace);
                    }

                    // set Speed parameter synchronized with agent root motion moverment
                    animator.SetFloat("Speed", animSpeed);


                    break;

                case SlimeAnimationState.Jump:

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;

                    StopAgent();
                    SetFace(faces.jumpFace);
                    animator.SetTrigger("Jump");

                    //Debug.Log("Jumping");
                    break;

                case SlimeAnimationState.Attack:

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                    StopAgent();
                    SetFace(faces.attackFace);
                    animator.SetTrigger("Attack");

                    // Debug.Log("Attacking");

                    break;
                case SlimeAnimationState.Damage:

                    // Do nothing when animtion is playing
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                        || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                        || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;

                    StopAgent();
                    animator.SetTrigger("Damage");
                    animator.SetInteger("DamageType", damType);
                    SetFace(faces.damageFace);

                    //Debug.Log("Take Damage");
                    break;

            }

        }


        private void StopAgent()
        {
            animator.SetFloat("Speed", 0);
        }
    }

}