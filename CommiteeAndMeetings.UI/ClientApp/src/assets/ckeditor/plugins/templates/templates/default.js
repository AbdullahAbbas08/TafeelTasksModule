// moved the code to initSample in ck-editor coponent to call it each initalization
//CKEDITOR.addTemplates("default", {
//  imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath("templates") + "templates/images/"),
//  templates: getTemplates()
//});

//function getTemplates() {
//  var templates = [];
//  var user = ;
//  const accessToken = JSON.parse(localStorage['AccessToken']);
//  var authorizationHeader = 'Authorization';
//  var xhr = new XMLHttpRequest();
//  xhr.open('GET', '/api/LetterTemplate/GetLetterTemplateForUser?organizationId=' + user.organizationId + '', false);
//  xhr.setRequestHeader(authorizationHeader, 'Bearer ' + accessToken);
//  xhr.onload = function () {

//    var res = JSON.parse(xhr.responseText);
//    for (var i = 0; i < res.length; i++) {
//      if (localStorage.getItem('ckeditorClasses') === "1") {
//        let pars = new DOMParser().parseFromString(res[i].text, 'text/html');
//        if (pars.querySelector('section')) {
//          if (!pars.querySelector('section').classList.contains('noBorderClass')) {
//            pars.querySelector('section').classList.add('noBorderClass');
//            res[i].text = pars.querySelector('section').outerHTML;
//          }
//        }
//      }
//      templates.push({
//        title: res[i].letterTemplateName,
//        html: res[i].text,
//        image: /*'template1.gif'*/'',
//        description: ''
//      });
//    }
//    //xhr.abort();
//  };
//  xhr.send();
//  return templates;

//}
