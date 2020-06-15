using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    // Start is called before the first frame update

    public ParticleSystem magicfirer;
    public ParticleSystem Exploxion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Togglemagicfirer()
    {
        if( magicfirer.isPlaying)
        {
            magicfirer.Stop();
        }
        else
        {
            magicfirer.Play();
        }
        
    }

    public void ToggleExploxion()
    {
        if (Exploxion.isPlaying)
        {
            Exploxion.Stop();
        }
        else
        {
            Exploxion.Play();
        }

    }
}
