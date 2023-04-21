using UnityEngine;

using Player;

using FMOD.Studio;

public class WaterfallSFX : MonoBehaviour
{
    [SerializeField] private float maxDistHeard;

    //L: This might be better than event emitter since we just want to call the event in code.
    [SerializeField] private FMODUnity.EventReference waterfallSFXEvent;
    private FMOD.Studio.EventInstance sfxInstance;

    private Collider2D[] _colliders;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider2D>();
    }

    private void OnEnable()
    {
        if (!sfxInstance.isValid())
        {
            sfxInstance = FMODUnity.RuntimeManager.CreateInstance(waterfallSFXEvent);
        }

        sfxInstance.start();
    }

    private void OnDisable()
    {
        sfxInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    // Update is called once per frame
    void Update()
    {
        PLAYBACK_STATE playbackState;
        sfxInstance.getPlaybackState(out playbackState);
        //Debug.Log(playbackState);

        float sqDistToPlayer = GetSqDistToPlayer();

        float fade = Mathf.Clamp01(sqDistToPlayer / (maxDistHeard*maxDistHeard));
        //Debug.Log($"Fade: {fade}");
        sfxInstance.setParameterByName("Fade", fade);
        sfxInstance.release();
    }

    private float GetSqDistToPlayer()
    {
        float minDist = float.MaxValue;
        foreach (Collider2D col in _colliders)
        {
            Vector2 closestPt = col.ClosestPoint(PlayerCore.Actor.transform.position);
            float sqDist = Vector2.SqrMagnitude((Vector2)PlayerCore.Actor.transform.position - closestPt);
            if (sqDist < minDist)
            {
                minDist = sqDist;
            }
        }

        return minDist;

        //Uncomment this instead if the implementation is too slow.
        //return Vector2.SqrMagnitude(PlayerCore.Actor.transform.position - this.transform.position);
    }
}
