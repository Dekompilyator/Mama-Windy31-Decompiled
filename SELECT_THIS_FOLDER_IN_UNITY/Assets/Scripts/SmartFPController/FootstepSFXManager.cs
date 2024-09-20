using System;
using UnityEngine;

namespace SmartFPController
{
	[RequireComponent(typeof(AudioSource))]
	public class FootstepSFXManager : MonoBehaviour
	{
		[Serializable]
		public struct SurfaceData
		{
			public string name;

			public AudioClip jumpingSFX;

			public AudioClip landingSFX;

			public AudioClip[] footstepSounds;
		}

		[SerializeField]
		private SurfaceData generic;

		[SerializeField]
		private SurfaceData[] surfaces;

		private AudioSource m_Audio;

		private void Awake()
		{
			m_Audio = GetComponent<AudioSource>();
			m_Audio.outputAudioMixerGroup = GameSettings.SFXOutput;
			m_Audio.playOnAwake = false;
			m_Audio.loop = false;
			m_Audio.spatialBlend = 1f;
			m_Audio.pitch = Time.timeScale;
		}

		public void PlayJumpingSound(RaycastHit hit)
		{
			m_Audio.PlayOneShot(GetSurfaceByHit(hit).jumpingSFX);
		}

		public void PlayLandingSound(RaycastHit hit)
		{
			m_Audio.PlayOneShot(GetSurfaceByHit(hit).landingSFX);
		}

		public void PlayFootStepSound(RaycastHit hit)
		{
			AudioClip[] footstepSounds = GetSurfaceByHit(hit).footstepSounds;
			int num = UnityEngine.Random.Range(1, footstepSounds.Length);
			m_Audio.clip = footstepSounds[num];
			m_Audio.PlayOneShot(m_Audio.clip);
			footstepSounds[num] = footstepSounds[0];
			footstepSounds[0] = m_Audio.clip;
		}

		private SurfaceData GetSurfaceByHit(RaycastHit hit)
		{
			m_Audio.outputAudioMixerGroup = GameSettings.SFXOutput;
			m_Audio.pitch = Time.timeScale;
			string surface = hit.GetSurface();
			for (int i = 0; i < surfaces.Length; i++)
			{
				if (surfaces[i].name == surface)
				{
					return surfaces[i];
				}
			}
			return generic;
		}
	}
}
