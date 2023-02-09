using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{   
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float looksentivity, speed, Runspeed, jumpheight, smoothTime;
    [SerializeField] bool grounded;
    [SerializeField] Item[] items;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject ui;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] FixedJoystick directionstick;
    [SerializeField] Button Jumpbutton;

    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    Rigidbody rigid;
    PhotonView pV;
    int itemindex;
    int previousitemindex = -1;
    public int copyitemindex =0;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playermanager;

    void Awake()
    {
           rigid = GetComponent<Rigidbody>(); 
           pV = GetComponent<PhotonView>();

           playermanager = PhotonView.Find((int)pV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if(pV.IsMine)
        {
            EquipItem(0);
        }

        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rigid);
            Destroy(ui);
        }
    }

    void Update()
    {
        if(!pV.IsMine)
            return;

        Look();
        Move();
        Jump();

        for(int i= 0; i<items.Length; i++)
        {
            if(Input.GetKeyDown((i+1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(itemindex >= items.Length - 1)
                EquipItem(0);
            
            else
                EquipItem(itemindex + 1);
            
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(itemindex <= 0 )
                EquipItem(items.Length - 1);
            else
                EquipItem(itemindex -1);
        }

      if (Input.GetMouseButtonDown(0))
        {
            items[itemindex].Use();
        }
    

        if(transform.position.y < -10)
        {
            Die();
        }

        
    }

    public void Fire()
    {
        items[itemindex].Use();
    }

    public void Jump()
    {
        if ( Input.GetKey(KeyCode.Space) /* Input.GetMouseButtonDown(1) */  &&  grounded)
        {
            rigid.AddForce(transform.up * jumpheight);
        }
    }

    private void Move()
    {
        Vector3 move = new Vector3( Input.GetAxisRaw("Horizontal") /* joystick.Horizontal */ , 0, Input.GetAxisRaw("Vertical") /* joystick.Vertical */).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, move * (Input.GetKey(KeyCode.LeftShift) ? Runspeed : speed), ref smoothMoveVelocity, smoothTime);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up *  Input.GetAxisRaw("Mouse X") /* directionstick.Horizontal */ * looksentivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") /* directionstick.Vertical */* looksentivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void EquipItem (int _index)
    {
        if(_index == previousitemindex)
            return;

        itemindex = _index;
        items[itemindex].itemGameObject.SetActive(true);

        if(previousitemindex != -1)
        {
            items[previousitemindex].itemGameObject.SetActive(false);
        }

        previousitemindex = itemindex;

        if(pV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("ItemIndex",itemindex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
       if(changedProps.ContainsKey("ItemIndex") && !pV.IsMine && targetPlayer == pV.Owner)
       {
        EquipItem((int)changedProps["ItemIndex"]);
       } 
    }


    void FixedUpdate()
    {
        if(!pV.IsMine)
            return;

        rigid.MovePosition(rigid.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        pV.RPC(nameof(RPC_TakeDamage),pV.Owner,damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth/maxHealth;
        if(currentHealth <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
        
    }

    void Die()
    {
        playermanager.Die();
    }
}
