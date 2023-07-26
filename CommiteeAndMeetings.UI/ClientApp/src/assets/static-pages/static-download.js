
var model = (function () {
    var token = "{accesstokenvalue}"
    baseURL = 'http://tafeel-srv:25252';
    
        let queyParams = {
            meetingId: '',
            refId: '',
        }
    
        let meeting;
    
        return {
    
            encryptKey: CryptoJS.enc.Utf8.parse('4512631236589784'),
            meeting: meeting,
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
    async function getMeetingByMeetingId() {

        let meetingId = model.getQueryParameters().meetingId;
        let url = model.baseURL + "/api/Meetings/GetMeetingSummary?";
        if (meetingId !== undefined)
            url += "meetingId=" + encodeURIComponent("" + meetingId) + "&"; 
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
        let meeting = await http;
        if (meeting)
            model.meeting = meeting;
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
    function formatAMPM(date) {
        var targetTime = new Date(date),
        tzDifference = targetTime.getTimezoneOffset(),
        offsetTime = new Date(targetTime.getTime() + tzDifference * 60 * 1000)
        var hours = offsetTime.getHours();
        var minutes = offsetTime.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0'+minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
      }
      function formatDate(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0'+minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return (date.getMonth()+1) + "/" + date.getDate() + "/" + date.getFullYear();
      }
    async function drawMeetingsTable(){
        let meetingDate = model.meeting.date
        document.getElementById("meetingDate").innerHTML = formatDate(new Date(meetingDate));
        let fromDate = model.meeting.meetingFromTime;
        document.getElementById("fromDate").innerHTML = `من : ${formatAMPM(new Date(fromDate))}  -  `;
        let toDate = model.meeting.meetingToTime;
        document.getElementById("toDate").innerHTML = `إلي :  ${formatAMPM(new Date(toDate))}` ;
        let location = model.meeting.physicalLocation;
        document.getElementById("physicalLocation").innerHTML = location;
        let title = model.meeting.title;
        document.getElementById("meetingTitle").innerHTML = title;
        let meetingSchedule = 
        `<tr>
          <th>الرقم</th>
          <th>الموضوع</th>
          <th>النتائج و المناقشات</th>
         </tr>`;
    
         for (let [index,r] of model.meeting.momSummaries.entries()){
            
            meetingSchedule += `<tr> 
            <td>${index + 1} </td>
            <td>${r.title}</td>
            <td>${r.description}</td>        
        </tr>`;
         }
        document.getElementById("meetingSchedule").innerHTML = meetingSchedule;
        /////////////////////////////////////////////////
        let meetingTopics = 
        `<tr>
          <th>الرقم</th>
          <th>الموضوع</th>
         </tr>`;
    
         for (let [index,r] of model.meeting.meetingTopicDTOs.entries()){
            
            meetingTopics += `<tr> 
            <td>${index + 1} </td>
            <td>${r.topicTitle}</td>      
        </tr>`;
         }
        document.getElementById("meetingTopics").innerHTML = meetingTopics;
        /////////////////////////////////////////////////
        let meetingAttendees = 
        `<tr>
          <th class="attende-Name">الأسم</th>
          <th class="attende-joptitle">الوظيفة</th>
          <th class="attende-phone">التليفون</th>
          <th class="attende-email">البريد الإلكتروني</th>
         </tr>`;
         for (let r of model.meeting.meetingCoordinators){
            meetingAttendees += `<tr> 
            <td>${r.coordinator.fullNameAr} </td>
            <td>${r.coordinator.jobTitleName}</td>
            <td>${r.coordinator.mobile}</td>
            <td>${r.coordinator.email}</td>   
        </tr>`;
         }
         for (let r of model.meeting.meetingAttendees){
            meetingAttendees += `<tr> 
            <td>${r.attendee.fullNameAr} </td>
            <td>${r.attendee.jobTitleName}</td>
            <td>${r.attendee.mobile}</td>
            <td>${r.attendee.email}</td>   
        </tr>`;
         }
         document.getElementById("meetingAttendes").innerHTML = meetingAttendees;
         let meetingRecommendation =
         `<tr>
           <th style="width: 5%;">الرقم</th>
           <th style="width: 95%;">التوصيات</th>
         </tr>`;
         for (let [index,r] of model.meeting.momComment.entries()){
            meetingRecommendation += `<tr> 
            <td>${index +  1} </td>
            <td>${r.comment.text}</td>
        </tr>`;
         }
         document.getElementById("meetingRecommendations").innerHTML = model.meeting.momComment ? meetingRecommendation : 'لا يوجد توصيات';
    }
    return {
        loadQuryParameters: fetchQueryParameters,
        getMeetingByMeetingId: getMeetingByMeetingId,
        drawMeetingsTable: drawMeetingsTable
    }
})()
var controller = (async function () {
    service.loadQuryParameters()
    await service.getMeetingByMeetingId()
    service.drawMeetingsTable();
})()