using UnityEngine;

namespace SmartFPController
{
	[RequireComponent(typeof(BoxCollider))]
	public class Ladder : MonoBehaviour
	{
		public AudioClip[] footstepSounds;

		private AudioSource m_Audio;

		private AudioClip lastClip;

		public Transform m_Transform { get; private set; }

		private void Awake()
		{
			m_Transform = base.transform;
			Collider component = GetComponent<Collider>();
			component.enabled = true;
			component.isTrigger = true;
		}

		public void AssignAudioSource(AudioSource audioSource)
		{
			m_Audio = audioSource;
			if (m_Audio != null)
			{
				m_Audio.outputAudioMixerGroup = GameSettings.SFXOutput;
			}
		}

		public void PlayLadderFootstepSound()
		{
			if (!(m_Audio == null))
			{
				m_Audio.pitch = Time.timeScale;
				int num = Random.Range(1, footstepSounds.Length);
				lastClip = footstepSounds[num];
				m_Audio.PlayOneShot(lastClip);
				footstepSounds[num] = footstepSounds[0];
				footstepSounds[0] = lastClip;
			}
		}
	}
}
