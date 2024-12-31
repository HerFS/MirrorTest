using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNick : NetworkBehaviour
{
    public TextMesh PlayerNickNameText;
    public GameObject FloatingInfo;

    private Material _playerMaterialClone;
    private SceneScript _sceneScript;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string PlayerNickName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color PlayerColor;

    private void Awake()
    {
        _sceneScript = GameObject.FindObjectOfType<SceneScript>();
    }

    private void OnNameChanged(string _old, string _new)
    {
        PlayerNickNameText.text = PlayerNickName;
    }

    private void OnColorChanged(Color _old, Color _new)
    {
        PlayerNickNameText.color = _new;
        _playerMaterialClone = new Material(GetComponent<Renderer>().material);
        _playerMaterialClone.color = _new;
        GetComponent<Renderer>().material = _playerMaterialClone;
    }

    public override void OnStartLocalPlayer()
    {
        FloatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f); // 자기 닉네임 설정 user info
        FloatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string name, Color color) // server에서 실행
    {
        PlayerNickName = name;
        PlayerColor = color;
        _sceneScript.StatusText = $"{PlayerNickName} joined";
        _sceneScript.TextChangedClientRpc(_sceneScript.StatusText);
    }
}
