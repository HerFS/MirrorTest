using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : NetworkBehaviour
{
    private PlayerNick _playerNick;
    private SceneScript _sceneScript;

    private void Awake()
    {
        _playerNick = GetComponent<PlayerNick>();
        _sceneScript = GameObject.FindObjectOfType<SceneScript>();
    }

    [Command]
    public void CmdSendPlayerMessage()
    {
        if (_sceneScript)
        {
            _sceneScript.TestInt++;
            _sceneScript.StatusText = $"{_playerNick.PlayerNickName} says hello {Random.Range(10, 99)}";
            //_sceneScript.TextChangedClientRpc(_sceneScript.StatusText);
        }
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        _sceneScript.MirrorMovement = this;
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            _playerNick.FloatingInfo.transform.rotation = Camera.main.transform.rotation;
            return;
        }

        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);
    }
}
