﻿using UnityEngine;
using Mirror;

namespace Complete
{
    public class TankMovement : NetworkBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager
        public float m_Speed = 12f;                 // How fast the tank moves forward and back
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving
        public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary

        private Rigidbody m_Rigidbody;              // Reference used to move the tank
        //private float m_MovementInputValue;         // The current value of the movement input
        //private float m_TurnInputValue;             // The current value of the turn input
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks
        //private InputAction m_MoveAction;           // Move Action reference (Unity 2020 New Input System)
        //private InputAction m_TurnAction;           // Turn Action reference (Unity 2020 New Input System)
        private bool isDisabled = false;            // To avoid enabling / disabling Input System when tank is destroyed


        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }


        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic
            m_Rigidbody.isKinematic = false;

            // Also reset the input values
            //m_MovementInputValue = 0f;
            //m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();

            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }

            isDisabled = false;
        }


        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }

            isDisabled = true;
        }


        private void Start()
        {
            // Store the original pitch of the audio source
            m_OriginalPitch = m_MovementAudio.pitch;

            // Unity 2020 New Input System
            // Get a reference to the MultiplayerEventSystem for this player
            //EventSystem ev = GameObject.Find ("EventSystem").GetComponent<EventSystem>();

            // Find the Action Map for the Tank actions and enable it
            //InputActionMap playerActionMap = ev.GetComponent<PlayerInput>().actions.FindActionMap ("Tank");
            //playerActionMap.Enable();

            // Find the 'Move' action
            //m_MoveAction = playerActionMap.FindAction ("MoveTank");

            // Find the 'Turn' action
            //m_TurnAction = playerActionMap.FindAction ("TurnTank");

            // Enable and hook up the events
            //m_MoveAction.Enable();
            //m_TurnAction.Enable();
            //m_MoveAction.performed += OnTankMove;
            //m_TurnAction.performed += OnTankTurn;
        }


        private void Update()
        {
            if (!isLocalPlayer)
                return;

            EngineAudio();
        }
        private void FixedUpdate()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate
            if (!isLocalPlayer)
                return;
            Move();
            Turn();
        }

        private void EngineAudio()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.1f && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        private void Move()
        {
            Vector3 movement = Input.GetAxis("Vertical") * transform.forward * m_Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);  
        }


        private void Turn()
        {
            Quaternion rotation = Quaternion.Euler(Input.GetAxis("Horizontal") * new Vector3(0,1,0) * m_TurnSpeed * Time.deltaTime);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
        }
    }
}