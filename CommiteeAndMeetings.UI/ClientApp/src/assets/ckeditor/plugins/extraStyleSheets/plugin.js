(function () {
  CKEDITOR.plugins.add('extraStyleSheets', {
    init: function (editor) {
      var pluginDirectory = this.path;
      // editor.addContentsCss(pluginDirectory + '../../../css/print-styles.css');
    }
  });
})();
