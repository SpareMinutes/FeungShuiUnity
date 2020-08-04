using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class MusicNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;
    public string SourceName;
    public float Volume;
    public AudioClip Clip;

    public override void Execute() {
        AudioSource source = GameObject.Find(SourceName).GetComponent<AudioSource>();
        if (source != null) {
            source.volume = Volume / 100f;
            if (source.clip == null || !source.clip.Equals(Clip)) {
                source.clip = Clip == null ? null : Clip;
                source.Play();
            }
        }
        ExecuteNext(GetOutputPort("next"));
    }
}