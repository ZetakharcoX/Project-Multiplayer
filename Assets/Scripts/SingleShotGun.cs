using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView pV;

    
    void Awake()
    {
         pV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>() ?.TakeDamage(((GunInfo)itemInfo).damage);
            pV.RPC("RPC_Shoot",RpcTarget.All,hit.point,hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition,0.3f);
        
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletimpactprefab,hitPosition,Quaternion.LookRotation(hitNormal,Vector3.up));
            Destroy(bulletImpactObj,4f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

}
