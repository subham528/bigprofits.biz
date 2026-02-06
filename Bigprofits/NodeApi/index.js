const express = require('express');
/*const { TronWeb } = require('tronweb');*/
const { Web3 } = require('web3');
const axios = require('axios');
require('dotenv').config()

const app = express();
app.use(express.json())

//const tronWeb = new TronWeb({
//    fullNode: process.env.TRON_API_URL,
//    solidityNode: process.env.TRON_API_URL,
//    eventServer: process.env.TRON_API_URL,
//    privateKey: process.env.WAL_PRIVATE_KEY,
//});
const websiteUrl = process.env.WEBSITE_URL
/*const trc20ContractAddress = process.env.WAL_PRIVATE_KEY;*/

app.get("/", (req, resp) => {
    resp.send("App is running")
})

app.get("/withdrawal", (req, resp) => {
    resp.json({ success: true, message: "ready" });
})

app.post('/withdrawal', async (req, resp) => {
    try {
        var currency = "USDT";

        //Calling API to check if withdrawal request exists and valid
        var widInfo = await callApi(websiteUrl + 'account/withdrawal-info', {
            userId: req.body.userId,
            orderId: req.body.orderId
        })
        console.log("withdrawal", widInfo)
        if (!widInfo.success) {
            resp.json({ success: false, message: widInfo.message });
            return;
        }

        //Send TRX/TRON to provided address
        //const txn = await Send_Trx(widInfo.address, widInfo.amount);

        //Send TRC-20 to provided address
        //const txn = await Send_TRC20_Token(widInfo.address, widInfo.amount);

        //Send BEP-20 to provided address
        const txn = await Send_BEP20_Token(widInfo.address, widInfo.amount, currency);
        console.log("withdrawal", txn)

        //Calling API to update withdrawal request according to the result
        var widUpdate = await callApi(websiteUrl + 'account/update-withdrawal', {
            userId: widInfo.userId,
            orderId: req.body.orderId,
            hashId: txn.txid,
            status: txn.result ? "1" : "2",
            message: txn.result ? "Transaction Successfull!" : txn.message
        })
        console.log("withdrawal", widUpdate)
        if (!widUpdate.success) {
            resp.json({ success: false, message: widUpdate.message });
            return;
        }

        //Sending response to member
        if (txn.result && txn.result == true) resp.json({ success: true, message: "Success! " + widInfo.amount + " " + currency + " successfully sent to your wallet. Thank You!" });
        else resp.json({ success: false, message: "Sorry! We are not able to process your request right now. Please try after sometime." });

    } catch (error) {
        console.error('Error:', error);
        resp.status(500).json({ success: false, error: error.message });
    }
});

app.post('/send-token', async (req, resp) => {
    try {
        var currency = "OCSK";

        //Calling API to check if token request exists and valid
        var widInfo = await callApi(websiteUrl + 'account/send-token-info', {
            userId: req.body.userId,
            orderId: req.body.orderId
        })
        console.log("sendtoken", widInfo)
        if (!widInfo.success) {
            resp.json({ success: false, message: widInfo.message });
            return;
        }

        //Send TRX/TRON to provided address
        //const txn = await Send_Trx(widInfo.address, widInfo.amount);

        //Send TRC-20 to provided address
        //const txn = await Send_TRC20_Token(widInfo.address, widInfo.amount);

        //Send BEP-20 to provided address
        const txn = await Send_BEP20_Token(widInfo.address, widInfo.amount, currency);
        console.log("sendtoken", txn)
        if (!txn.result) {
            resp.json({ success: txn.result, message: txn.message });
            return;
        }

        //Calling API to update token request according to the result
        var widUpdate = await callApi(websiteUrl + 'account/update-token', {
            userId: widInfo.userId,
            orderId: req.body.orderId,
            hashId: txn.txid,
            status: txn.result ? 1 : 2,
            message: txn.result ? "Transaction Successfull!" : txn.message
        })
        console.log("sendtoken", widUpdate)
        if (!widUpdate.success) {
            resp.json({ success: false, message: widUpdate.message });
            return;
        }

        //Sending response to member
        if (txn.result && txn.result == true) resp.json({ success: true, message: "Success! " + widInfo.amount + " " + currency + " successfully sent to your wallet. Thank You!" });
        else resp.json({ success: false, message: "Sorry! We are not able to process your request right now. Please try after sometime." });

    } catch (error) {
        console.error('Error:', error);
        resp.status(500).json({ success: false, error: error.message });
    }
});

app.post('/fetch-balance', async (req, resp) => {
    try {
        const tokenName = req.body.tokenName
        const bal = await Balance_BEP20_Token(tokenName);
        console.log("fetchBalance", bal)

        resp.json({ success: bal.result, message: bal.message, balance: bal.balance.toString() });
        return;
    } catch (error) {
        console.error('Error:', error);
        resp.status(500).json({ success: false, error: error.message });
    }
});

//async function Send_Trx(address, amount) {
//    try {
//        const sunAmount = tronWeb.toSun(amount);
//        const transaction = await tronWeb.transactionBuilder.sendTrx(address, sunAmount);
//        const signedTransaction = await tronWeb.trx.sign(transaction);
//        const txn = await tronWeb.trx.sendRawTransaction(signedTransaction);
//        return txn;
//    } catch (error) {
//        return error;
//    }
//}

//async function Send_TRC20_Token(address, amount) {
//    let result, txid, message;
//    try {
//        const sunAmount = tronWeb.toSun(amount);
//        let contract = await tronWeb.contract().at(trc20ContractAddress);
//        await contract.transfer(
//            address,
//            sunAmount
//        ).send().then(res => {
//            txid = res;
//            result = true;
//            message = "Success! Transaction submitted successfully."
//        })
//    } catch (error) {
//        result = false
//        message = "Sorry! Blockchain API is not working right now. Please try again"
//    }
//    return { result: result, txid: txid, message: message };
//}

async function Balance_BEP20_Token(tokenName) {
    let result, message, balance;
    try {
        const privateKey = tokenName == "USDT" ? process.env.USDT_BEP20_PK : process.env.TOKEN_BEP20_PK
        const web3 = new Web3(process.env.BINANCE_NODE_URL);
        const wallet = web3.eth.accounts.privateKeyToAccount(web3.utils.toHex(privateKey));

        const contractAddress = tokenName == "USDT" ? process.env.USDT_BEP20_CONTRACT : process.env.TOKEN_BEP20_CONTRACT;
        const contractAbi = JSON.parse(tokenName == "USDT" ? process.env.USDT_BEP20_ABI : process.env.TOKEN_BEP20_ABI);
        const contract = new web3.eth.Contract(contractAbi, contractAddress);

        const bal = await contract.methods.balanceOf(wallet.address).call();
        console.log("Balance:", bal);

        result = true;
        message = "Balance fetched successfully!";
        balance = Number(bal) / (10 ** 18)
    } catch (error) {
        result = false
        message = error
        balance = 0
    }
    return { result: result, message: message, balance: balance };
}

async function Send_BEP20_Token(address, amount, tokenName) {
    let result, txid, message;
    try {
        const privateKey = tokenName == "USDT" ? process.env.USDT_BEP20_PK : process.env.TOKEN_BEP20_PK
        const web3 = new Web3(process.env.BINANCE_NODE_URL);
        const wallet = web3.eth.accounts.privateKeyToAccount(web3.utils.toHex(privateKey));

        const contractAddress = tokenName == "USDT" ? process.env.USDT_BEP20_CONTRACT : process.env.TOKEN_BEP20_CONTRACT;
        const contractAbi = JSON.parse(tokenName == "USDT" ? process.env.USDT_BEP20_ABI : process.env.TOKEN_BEP20_ABI);
        const contract = new web3.eth.Contract(contractAbi, contractAddress);

        if (!web3.utils.isAddress(address)) {
            resp.json({ success: false, message: "Invalid address! Please provide a valid address" });
            return;
        }

        if (isNaN(amount) || amount <= 0) {
            resp.json({ success: false, message: "Invalid amount! Amount must be a positive number." });
            return;
        }

        const weiValue = web3.utils.toWei(amount.toString(), 'ether');
        const data = contract.methods.transfer(address, weiValue).encodeABI();

        const gasPrice = await web3.eth.getGasPrice();
        const gasLimit = await contract.methods.transfer(address, weiValue).estimateGas({ from: wallet.address });

        const tx = {
            nonce: await web3.eth.getTransactionCount(wallet.address),
            from: wallet.address,
            to: contractAddress,
            gasPrice: gasPrice,
            gasLimit: gasLimit,
            data: data,
        };

        const signedTx = await web3.eth.accounts.signTransaction(tx, privateKey);
        const txReceipt = await web3.eth.sendSignedTransaction(signedTx.rawTransaction);

        if (txReceipt.status === 1n) {
            txid = txReceipt.transactionHash;
            result = true;
            message = "Success! Transaction submitted successfully."
        }
        else {
            txid = txReceipt.transactionHash;
            result = false;
            message = "Failed! We could not complete your transacton right now. Please try after sometime."
        }
    } catch (error) {
        result = false
        message = error
        //message = "Sorry! Blockchain API is not working right now. Please try again"
    }
    return { result: result, txid: txid, message: message };
}


//process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'; //Uncomment this line only in localhost to bypass self sign certificate error

async function callApi(apiUrl, requestData) {
    try {
        const response = await axios.post(apiUrl, requestData);
        return response.data;
    } catch (error) {
        return error;
    }
}

const port = 5001; // Specify the desired port number
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

