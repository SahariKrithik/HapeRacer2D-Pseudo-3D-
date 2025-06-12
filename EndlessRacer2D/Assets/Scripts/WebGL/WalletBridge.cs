using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class WalletBridge : MonoBehaviour
{
    public TMP_Text walletAddressDisplay;

    [DllImport("__Internal")]
    private static extern void ConnectWallet(); // calls JS

    public void RequestWalletConnection()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ConnectWallet();
#else
        Debug.Log("Wallet connection only works in WebGL build.");
        SetWalletAddress("0xTESTWALLET123456"); // for local testing
#endif
    }

    public void SetWalletAddress(string address)
    {
        PlayerPrefs.SetString("WalletAddress", address);
        walletAddressDisplay.text = address;
    }
}
