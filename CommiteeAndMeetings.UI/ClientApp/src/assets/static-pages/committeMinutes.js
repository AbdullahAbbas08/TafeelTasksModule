
var model = (function () {
    var token = "{accesstokenvalue}"
    baseURL = 'http://localhost:5000';
    
        let queyParams = {
            id: '',
        }
    
        let committe;
        let committeAttachStats
    
        return {
    
            encryptKey: CryptoJS.enc.Utf8.parse('4512631236589784'),
            committe: committe,
            committeAttachStats:committeAttachStats,
            setQueryParameters: function (paramtrs) {
                queyParams = { ...paramtrs }
            },
            getQueryParameters: function () {
                return queyParams
            },
            token: token,
            baseURL: baseURL
        }
    
})()
var service = (function () {

    function fetchQueryParameters() {
        let queryString = {};
        location.search.substr(1).split("&").forEach(function (pair) {
            if (pair === "") return;
            var parts = pair.split("=");
            queryString[parts[0]] = parts[1] &&
                decodeURIComponent(parts[1].replace(/\+/g, " "));
        });
        model.setQueryParameters(queryString);
    }
    async function getCommitteByCommitteId() {

         let committeId = decryptString(model.getQueryParameters().id);
        let id = encrypteString(model.getQueryParameters().id)
        let url = model.baseURL + "/api/Commitees/GetCommitteeDetailsWithValidityPeriod?";
        if (id !== undefined)
            url += "id=" + encodeURIComponent("" + id) + "&"; 
            url = url.replace(/[?&]$/, "");
        url = url.replace(/[?&]$/, "");
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url)
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        xhr.setRequestHeader('Cache-Control', 'no-cache');
        xhr.setRequestHeader('Pragma', 'no-cache');
        xhr.setRequestHeader('Authorization', `Bearer ` + model.token);
       
        // xhr.responseType = 'blob';
        xhr.send();
        let http = new Promise((resolve, reject) => {
            xhr.onload = () => {
                if (xhr.status >= 400) {
                    reject(xhr.response);
                } else {
                    resolve(JSON.parse(xhr.response));
                }
            }
        })
        let committe = await http;
        if (committe)
            model.committe = committe;
    }
    async function getCommitteAttachmentStats() {

        // let committeId = decryptString(model.getQueryParameters().id);
        let CommitteId = encrypteString(model.getQueryParameters().id)
        let url = model.baseURL + "/api/Commitees/CommitteStatistic?";
        if (CommitteId !== undefined)
            url += "CommitteId=" + encodeURIComponent("" + CommitteId) + "&"; 
            url = url.replace(/[?&]$/, "");
        url = url.replace(/[?&]$/, "");
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url)
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        xhr.setRequestHeader('Cache-Control', 'no-cache');
        xhr.setRequestHeader('Pragma', 'no-cache');
        xhr.setRequestHeader('Authorization', `Bearer ` + model.token);
       
        // xhr.responseType = 'blob';
        xhr.send();
        let http = new Promise((resolve, reject) => {
            xhr.onload = () => {
                if (xhr.status >= 400) {
                    reject(xhr.response);
                } else {
                    resolve(JSON.parse(xhr.response));
                }
            }
        })
        let committeAttachStats = await http;
        if (committeAttachStats)
            model.committeAttachStats = committeAttachStats;
    }

    function encrypteString(string = null) {
        if (!string) return
        var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(string), model.encryptKey,
            {
                keySize: 128 / 8,
                iv: model.encryptKey,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });

        return encrypted.toString();
    }
    function decryptString(string) {
        var decrypted = CryptoJS.AES.decrypt(string, model.encryptKey, {
            keySize: 128 / 8,
            iv: model.encryptKey,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

        try {
            let x = decrypted.toString()
            return decrypted.toString(CryptoJS.enc.Utf8);
        } catch (err) {
            return false;
        }
    }
    async function drawMeetingsTable(){
        let commiteName = model.committe.name;
        document.getElementById("committeName").innerHTML = commiteName
        let committeTile = model.committe.title;
        document.getElementById("committeTile").innerHTML = committeTile;
        let committeCategory = model.committe.category.categoryNameAr;
        document.getElementById("committeCategory").innerHTML = committeCategory;
        let committeType = model.committe.commiteeType.commiteeTypeNameAr;
        document.getElementById("committeType").innerHTML = committeType;
        let committeHeadUnit = model.committe.currenHeadUnit.fullNameAr;
        document.getElementById("committeHeadUnit").innerHTML = committeHeadUnit;
        let committeStatus = model.committe.currentStatus.currentStatusNameAr;
        document.getElementById("committeStatus").innerHTML = committeStatus;
        let departmentUnit = model.committe.departmentLink.organizationNameAr;
        document.getElementById("departmentUnit").innerHTML = departmentUnit;
        let committeDesc = model.committe.description;
        document.getElementById("committeDesc").innerHTML = committeDesc;

       let attachNum = model.committeAttachStats[0].count;
       document.getElementById('attachNum').innerHTML = attachNum

       let voteNum = model.committeAttachStats[1].count;
       document.getElementById('voteNum').innerHTML = voteNum;

       let taskNum = model.committeAttachStats[2].count;
       document.getElementById('taskNum').innerHTML = taskNum;
       
       let meetingsNum = model.committeAttachStats[3].count;
       document.getElementById('meetingsNum').innerHTML = meetingsNum;
              
       let incomingTrans = model.committeAttachStats[4].count;
       document.getElementById('incomingTrans').innerHTML = incomingTrans;
                     
       let outcomingTrans = model.committeAttachStats[5].count;
       document.getElementById('outcomingTrans').innerHTML = outcomingTrans

        let committeMembers = 
        `<tr>
          <th class="attende-Name">الأسم</th>
          <th class="attende-joptitle">البريد</th>
          <th class="attende-phone">الهاتف</th>
         </tr>`;
         for (let r of model.committe.members){
            committeMembers += `<tr> 
            <td>${r.user.fullNameAr} </td>
            <td>${r.user.email ?  r.user.email : 'لا يوجد'}</td>
            <td>${r.user.mobile ? r.user.mobile : 'لايوجد'}</td>
        </tr>`;
         }
         document.getElementById("committeMembers").innerHTML = committeMembers;

        
      
    }
    return {
        loadQuryParameters: fetchQueryParameters,
        getCommitteByCommitteId: getCommitteByCommitteId,
        getCommitteAttachmentStats:getCommitteAttachmentStats,
        drawMeetingsTable: drawMeetingsTable
    }
})()
var controller = (async function () {
    service.loadQuryParameters()
    await service.getCommitteByCommitteId()
    await service.getCommitteAttachmentStats()
    service.drawMeetingsTable();
})()