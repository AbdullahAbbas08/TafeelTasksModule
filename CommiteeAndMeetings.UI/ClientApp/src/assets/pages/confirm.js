
var model = (function () {
         baseURL = window.location.origin;
        
            let queyParams = {
                meetingId: '',
                userId: '',
                userType:'',
                state:'',
            }
            return {
                setQueryParameters: function (paramtrs) {
                    queyParams = { ...paramtrs }
                },
                getQueryParameters: function () {
                    return queyParams
                },
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
        async function confirmOrReject() {
    
            let meetingId = model.getQueryParameters().meetingId;
            let userId    =    model.getQueryParameters().userId;
            let userType  = model.getQueryParameters().userType;
            var state =     model.getQueryParameters().state;
            let meetingIdEncode =  encodeURIComponent(meetingId);
            let userIdEncode = encodeURIComponent(userId)
                let url_ =    model.baseURL + "/api/Meetings/ConfirmMeetingAttendeesOrCoordinatorState?";
                if (userId !== undefined)
                  url_ += "userId=" + encodeURIComponent("" + userIdEncode) + "&"; 
                if (meetingId !== undefined)
                  url_ += "meetingId=" + encodeURIComponent("" + meetingIdEncode) + "&"; 
                if (userType !== undefined)
                  url_ += "userType=" + encodeURIComponent("" + userType) + "&"; 
                if (state !== undefined)
                  url_ += "state=" + encodeURIComponent("" + state) + "&"; 
                  url_ = url_.replace(/[?&]$/, "");
              var xhr = new XMLHttpRequest();
              xhr.open("POST", url_)
              xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
              xhr.setRequestHeader('Cache-Control', 'no-cache');
              xhr.setRequestHeader('Pragma', 'no-cache');
              xhr.setRequestHeader('Authorization', `Bearer `);
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
        }
        async function drawCommitteData(){
                    if (model.getQueryParameters().state == 3) {
                        document.getElementById("confirmation").style.display  = "block";
                        document.getElementById("refuesd").style.display ="none"
                    } else {
                        document.getElementById("confirmation").style.display  = "none";
                        document.getElementById("refuesd").style.display ="block"
                    }
                
        }
        return {
            loadQuryParameters: fetchQueryParameters,
            confirmOrReject: confirmOrReject,
            drawCommitteData: drawCommitteData
        }
    })()
    var controller = (async function () {
        service.loadQuryParameters()
        await service.confirmOrReject()
        service.drawCommitteData();
    })()
    