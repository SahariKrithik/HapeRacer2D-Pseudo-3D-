mergeInto(LibraryManager.library, {
    ConnectWallet: function () {
        if (typeof window.ethereum !== 'undefined') {
            window.ethereum
                .request({ method: 'eth_requestAccounts' })
                .then(accounts => {
                    if (accounts.length > 0) {
                        let wallet = accounts[0];
                        SendMessage('WalletBridge', 'SetWalletAddress', wallet);
                    }
                })
                .catch(err => {
                    console.error("MetaMask error:", err);
                });
        } else {
            alert("MetaMask not found. Please install MetaMask.");
        }
    }
});
