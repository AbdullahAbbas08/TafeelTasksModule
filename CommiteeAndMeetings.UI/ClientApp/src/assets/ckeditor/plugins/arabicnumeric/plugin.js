(function () {
  CKEDITOR.plugins.add('arabicnumeric', {
    lang: ['ar', 'en'],
    icons: 'converttoarabic,converttoenglish',
    init: function (editor) {
      editor.addCommand('converttoarabicCMD', {
        exec: function () {
          var selectedNode = $(editor.getSelection().getNative().anchorNode)[0];
          if (selectedNode.nodeType === Node.TEXT_NODE) {
            var selectedText = editor.getSelection().getSelectedText();
            selectedNode.textContent = selectedNode.textContent.replace(selectedText, parseEnglish(selectedText));
          }
        }
      });

      editor.ui.addButton('converttoarabic', {
        label: editor.lang.arabicnumeric.convertToArabicNumeric,
        title: editor.lang.arabicnumeric.convertToArabicNumeric,
        command: 'converttoarabicCMD',
        toolbar: 'tools',
        icon: 'converttoarabic'
      });

      editor.addCommand('converttoenglishCMD', {
        exec: function () {
          var selectedNode = $(editor.getSelection().getNative().anchorNode)[0];
          if (selectedNode.nodeType === Node.TEXT_NODE) {
            var selectedText = editor.getSelection().getSelectedText();
            selectedNode.textContent = selectedNode.textContent.replace(selectedText, parseArabic(selectedText));
          }
        }
      });

      editor.ui.addButton('converttoenglish', {
        label: editor.lang.arabicnumeric.convertToEnglishNumeric,
        title: editor.lang.arabicnumeric.convertToEnglishNumeric,
        command: 'converttoenglishCMD',
        toolbar: 'tools',
        icon: 'converttoenglish'
      });

      function parseEnglish(str) {
        str = str.toString();
        return str.replace(/[0-9]/g, function (d) {
          return String.fromCharCode(d.charCodeAt(0) + 1584) // Convert To Arabic Numbers
        });
      }

      function parseArabic(str) {
        return str.replace(/[٠١٢٣٤٥٦٧٨٩]/g, function (d) {
          return d.charCodeAt(0) - 1632; // Convert Arabic numbers
        }).replace(/[۰۱۲۳۴۵۶۷۸۹]/g, function (d) {
          return d.charCodeAt(0) - 1776; // Convert Persian numbers
        });
      }
    }
  });
})();
