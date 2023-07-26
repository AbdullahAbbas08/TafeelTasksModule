
var model = (function () {
    var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI2NDdmZTRiZC1kYzBhLTQzZTYtOTNkYS1hMjZhODBkMGM2Y2YiLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4MS8iLCJpYXQiOjE2ODU4NzI1MTksImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJvYWo5eFdvQ3Y2ZUxPTW9tQ2ZEZlFJc252QlJFaWhUNmVHN0tMNWpaQUNmREo4T0JmUUlzbnZUNmVHN0tDZkRKOE9CZlFJc252VDZlRzdLIiwiV3IweDRyS1pRRkNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S2xmMVJzTmI3VDV2ZkZDSnl5eDkzcWJYa1ZEa1hnT3VrQ2ZESjhPQmZRSXNudlQ2ZUc3SyI6Ikt1Q2ZEZlFJc252QlJFaWhUNmVHN0sxUVVnVnFqMmhabFpxeHdPRkpDWGRlOHFLSXVsa0lEVVVCMENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S3dXVUxsNVhJemVYemwybHoxREZTdDRacGQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3NlcmlhbG51bWJlciI6ImViZWIzNWI3ZDkwNDRkYzU5OWM3OTgwN2RkOWUyM2RlIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy91c2VyZGF0YSI6IjEiLCJlWDhDZkRKOE9CZlFJc252QlJFaWhUNmVHN0s5N2gwWDJHUlRhVFNWekFVNkdBWVljMzBxZVI2cTdoZEswcWtLbEFDZkRKOE9CZlFJc252VDZlRzdLIjoiS2JGZkMyUWpWRmZxd1pvTFB4MmYwZ0NmREo4T0JmUUlzbnZUNmVHN0tDZkRKOE9CZlFJc252VDZlRzdLIiwiZVg4Q2ZESjhPQmZRSXNudkJSRWloVDZlRzdLOTdoMFgyR1JUYVRTVnpBVTZPdVJwRWlNS1ZyMWRNV1ZKSDlObEhwQW1xNFp2Mk1nWjhQRUtQZXRNVmdkIjoic3k5TW1HRmRPeXc1VkJ4ZFJrYXNETGVlWVdiZmNzbnZRemF5ZVZjZ0RuY0NmREo4T0JmUUlzbnZUNmVHN0siLCJlWDhDZkRKOE9CZlFJc252QlJFaWhUNmVHN0s5N2gwWDJHUlRhVFNWekFVNk93b1FMT3V5VjQ3V3FSam1td2loRE5wYXJsT3VpenlQN0NmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S0t4MlhNWkcycCI6IkNmRGZRSXNudkJSRWloVDZlRzdLM1FTQTdTQUVNVnZ1ZTdtc0phOFpFd3lvWUFJVnFJVjc4cnFQaWl5eTJJQ2ZESjhPQmZRSXNudlQ2ZUc3SyIsInN0TlRVeU1LdFN4VWY0Q2ZESjhPQmZRSXNudkJSRWloVDZlRzdLZGlubTF2R0RpU0pxSjNlVGtvbEdwaTdwNDBwUjZuWjBBUEl2RDUyNEpDZkRmUUlzbnZCUkVpaFQ2ZUc3S0tsYk41SEQiOiIyMTI0Z1o2VWoxcUxVU1BWcTloekNmRGZRSXNudkJSRWloVDZlRzdLZ0NmREo4T0JmUUlzbnZUNmVHN0tDZkRKOE9CZlFJc252VDZlRzdLIiwic3ROVFV5TUt0U3hVZjRDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tkaW5tMXZHRGlTSnFKM2VUa29sR3BpN3A0MHBRY29ZTnNSOEJhOTZDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tyZ3RaMEZTQ2ZEZlFJc252QlJFaWhUNmVHN0taYnRmQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLZmlVT0NmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S0UyNXdJeWQxZWNWRlFDZkRKOE9CZlFJc252VDZlRzdLQ2ZESjhPQmZRSXNudlQ2ZUc3SyI6Im0wOG44SmlCRlFjQ1AzSm1YWWNxZjVTc0xJYXRJemxUOVZZNW1EdERBSXcwNmxGbnVDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tmOTNXSElwNFhQVlVOMSIsIndyT2xlbGRxTVlYcThKbE16Nk1jZXRzQzYzbWhDbm1nV0pNR2tuUW9yOEpNMm9jY1ZRakhwbm5obkdWTmlnYUciOiI2N3VMTWtRT2Z5QVozS2gxSUI3Q2ZESjhPQmZRSXNudkJSRWloVDZlRzdLNUFDZkRKOE9CZlFJc252VDZlRzdLQ2ZESjhPQmZRSXNudlQ2ZUc3SyIsImVhakNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3SzY1MGJ5REJVOU04Z0Nqa3V1NExjVWxhM3hFTW1teHZrR09Femgxc0NmREo4T0JmUUlzbnZUNmVHN0siOlsiS3VDZkRmUUlzbnZCUkVpaFQ2ZUc3SzFRVWdWcWoyaFpsWnF4d09GSkNYZGU4cUtJdWxrSURVVUIwQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLd1dVTGw1WEl6ZVh6bDJsejFERlN0NFpwZCIsIkt1Q2ZEZlFJc252QlJFaWhUNmVHN0sxUVVnVnFqMmhabFpxeHdPRkpDWGRlOHFLSXVsa0lEVVVCMENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S3dXVUxsNVhJemVYemwybHoxREZTdDRacGQiXSwiZWFqQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLNjUwYnlEQlU5TThnQ2prdXUzczlSNG9MM2thVkZRdWFGbUVobHhZQ2ZESjhPQmZRSXNudlQ2ZUc3SyI6WyJiWEZGMzlJWElKc1prQ2ZEZlFJc252QlJFaWhUNmVHN0tDdWFvUzVxWGRsSzA5R285cXlOY085Ynhhb2FDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tuZGVxZEFoRk9sanljTkdMTGtTcVNCIiwiYlhGRjM5SVhJSnNaa0NmRGZRSXNudkJSRWloVDZlRzdLQ3Vhb1M1cVhkbEswOUdvOXF5TmNPOWJ4YW9hQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLbmRlcWRBaEZPbGp5Y05HTExrU3FTQiJdLCJzdE5UVXlNS3RTeFVmNENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S2Rpbm0xdkdEaVNKcUozZVRrb2xHcGk3cDQwcFE0UE05dXh4T0UzR0pDZkRmUUlzbnZCUkVpaFQ2ZUc3SzR2dWNKVHV6Y0F2OTZ6YVljZzg4WVlxQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLc0lxS05nQ2ZESjhPQmZRSXNudlQ2ZUc3S0NmREo4T0JmUUlzbnZUNmVHN0siOiJ0R0NmRGZRSXNudkJSRWloVDZlRzdLY3JodTM1bG5pQ2ZEZlFJc252QlJFaWhUNmVHN0swb1RxSXZqYkNDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tPOHdPakpER1BUTUxUeFZoaFRDZkRmUUlzbnZCUkVpaFQ2ZUc3SzRDZkRKOE9CZlFJc252VDZlRzdLIiwiRG00ZXl6dUxGMTNDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tsbHM1YnBZazU3WTF3a1VYR0JjQkc5b2ZrRkNmRGZRSXNudkJSRWloVDZlRzdLRTVhRUNmREo4T0JmUUlzbnZUNmVHN0siOiJ0bENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S3VrbWRxeEJnQ2ZEZlFJc252QlJFaWhUNmVHN0ttSVdGY0hDNDJ3Q2ZESjhPQmZRSXNudlQ2ZUc3S0NmREo4T0JmUUlzbnZUNmVHN0siLCJuYmYiOjE2ODU4NzI1MTksImV4cCI6MTY4NTg3OTcxOSwiYXVkIjoiQW55In0.y5IxOeBj2iHXLbV4SnpXRoCi85WjHgrnwGD2ILp_2LE"
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