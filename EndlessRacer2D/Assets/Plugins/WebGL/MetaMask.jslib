mergeInto(LibraryManager.library, {
    ConnectWallet: function () {
        console.log("✅ ConnectWallet() JS function called."); // Debug: this must show up in console

        if (typeof window.ethereum !== 'undefined') {
            window.ethereum.request({ method: 'eth_requestAccounts' })
                .then(function (accounts) {
                    if (accounts.length > 0) {
                        var wallet = accounts[0];
                        console.log("✅ Wallet retrieved: " + wallet); // Debug
                        SendMessage('WalletBridge', 'SetWalletAddress', wallet);
                    }
                })
                .catch(function (error) {
                    console.error("❌ MetaMask error:", error);
                });
        } else {
            alert("❌ MetaMask not detected. Please install it.");
        }
    }
});
