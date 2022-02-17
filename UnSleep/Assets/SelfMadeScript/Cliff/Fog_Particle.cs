using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Particle : MonoBehaviour //지금은 안쓰는 스크립트, 이걸 켜놓으면 콜라이더에 부딫힌 파티클은 지워진다.
{
    private void OnParticleCollision(GameObject other)
    {
        Destroy(other);
    }
}