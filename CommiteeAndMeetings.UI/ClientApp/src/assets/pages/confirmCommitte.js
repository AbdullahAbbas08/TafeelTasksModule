
var model = (function () {
    baseURL = window.location.origin;
   
       let queyParams = {
        commiteeMemberId: '',
        memberState: '',
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

       let commiteeMemberId = model.getQueryParameters().commiteeMemberId;
       let encode = encodeURIComponent(commiteeMemberId);

       let memberState = model.getQueryParameters().memberState;
       let url_ =  model.baseURL + "/api/CommiteeUsers/ConfirmChangeMemberState?";
       if (commiteeMemberId !== undefined)
           url_ += "commiteeMemberId=" + encodeURIComponent("" + encode) + "&"; 
       if (memberState !== undefined)
           url_ += "memberState=" + encodeURIComponent("" + memberState) + "&"; 
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
               if (model.getQueryParameters().memberState == 2) {
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
