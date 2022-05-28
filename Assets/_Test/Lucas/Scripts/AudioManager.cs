using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static bool onCooldown;
    
    [SerializeField] private AudioConfigurationSO audioConfigurationSO;
    [SerializeField] private AudioSource AmbientAudioPlayer;
    [SerializeField] private Vector2 AmbientAudioCooldown;

    private void Awake()
    {
        onCooldown = false;
    }

    private void Update()
    {
        if (!onCooldown)
        {
            StartCoroutine(AmbientSoundTimer());
        }
    }

    private IEnumerator AmbientSoundTimer()
    {
        onCooldown = true;
        yield return new WaitForSeconds(Random.Range(AmbientAudioCooldown.x, AmbientAudioCooldown.y));
        PlayAnAmbientSound(false);
        onCooldown = false;
        yield return new WaitForSeconds(AmbientAudioCooldown.x);
    }
    
    public void PlayAnAmbientSound(bool usePlayersLocation)
    {
        AmbientAudioPlayer.clip = audioConfigurationSO.GetRandomAmbientAudio();
        AmbientAudioPlayer.transform.position = usePlayersLocation ? FindObjectOfType<PlayerController>().gameObject.transform.position : GetRandomAmbientLocation();
        AmbientAudioPlayer.Play();
    }

    private Vector3 GetRandomAmbientLocation()
    {
        if (audioConfigurationSO.ambientAudioLocations == null || audioConfigurationSO.ambientAudioLocations.Count < 1)
        {
            return this.transform.position;
        }

        return audioConfigurationSO.ambientAudioLocations[
            Random.Range(0, audioConfigurationSO.ambientAudioLocations.Count)].position;
    }
}
