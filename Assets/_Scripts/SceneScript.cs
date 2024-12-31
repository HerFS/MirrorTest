using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneScript : NetworkBehaviour
{
    public int TestInt;

    public TextMeshProUGUI CanvasStatusText;
    public MirrorMovement MirrorMovement;

    //[SyncVar(hook = nameof(OnStatusTextChanged))]
    public string StatusText;

    private void OnStatusTextChanged(string _old, string _new)
    {
        CanvasStatusText.text = StatusText;
    }

    [ClientRpc]
    public void TextChangedClientRpc(string statusText)
    {
        StatusText = statusText;
        CanvasStatusText.text = StatusText;
    }

    public void ButtonSendMessage()
    {
        if (MirrorMovement != null)
        {
            MirrorMovement.CmdSendPlayerMessage();
        }
    }
}
