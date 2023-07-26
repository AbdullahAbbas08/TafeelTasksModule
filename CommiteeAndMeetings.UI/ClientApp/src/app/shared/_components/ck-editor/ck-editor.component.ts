import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { LayoutService } from '../../_services/layout.service';


declare var CKEDITOR;

@Component({
  selector: 'app-ck-editor',
  template: '<div id="editor"></div>',
})
export class CkEditorComponent implements OnInit {

  @Input() config;
  @Output() ready: EventEmitter<any> = new EventEmitter<any>();
  constructor(private layoutService: LayoutService) { }

  ngOnInit(): void {
    if (CKEDITOR.env.ie && CKEDITOR.env.version < 9) {
      CKEDITOR.tools.enableHtml5Elements(document);
    }
    // remove source area in whole app except settings...
    if (!window.location.href.includes('setting')) {
      CKEDITOR.config.removePlugins = 'sourcearea';
    } else {
      CKEDITOR.config.removePlugins = '';
    }
    if (this.config) {
      CKEDITOR.config.height = this.config.height;
      CKEDITOR.config.width = this.config.width;
    } else {
      CKEDITOR.config.height = 500;
      CKEDITOR.config.width = 'auto';
    }
    this.initSample();
  }
  initSample() {
    const wysiwygareaAvailable = isWysiwygareaAvailable(), isBBCodeBuiltIn = !!CKEDITOR.plugins.get('bbcode');
    CKEDITOR.config.removePlugins='font';
    const editorElement = CKEDITOR.document.getById('editor');
    // :(((
    if (isBBCodeBuiltIn) {
      editorElement.setHtml(
        'Hello world!\n\n' +
        'I\'m an instance of [url=https://ckeditor.com]CKEditor[/url].'
      );
    }

    // Depending on the wysiwygarea plugin availability initialize classic or inline editor.
    if (wysiwygareaAvailable) {
      CKEDITOR.replace('editor');
    } else {
      editorElement.setAttribute('contenteditable', 'true');
      CKEDITOR.inline('editor');
      // TODO: we can consider displaying some info box that
      // without wysiwygarea the classic editor may not work.
    }

    function isWysiwygareaAvailable() {
      // If in development mode, then the wysiwygarea must be available.
      // Split REV into two strings so builder does not replace it :D.
      if (CKEDITOR.revision === ('%RE' + 'V%')) {
        return true;
      }
      return !!CKEDITOR.plugins.get('wysiwygarea');
    }

    // if IE apply function to override hasLayout Property
    // If classic or inline editors are created automatically

    CKEDITOR.on('instanceCreated', (event) => {
      event.editor.on('contentDom', (e) => {
        let editable = event.editor.editable(),
          element = editable.$;


        if (element.addEventListener) {
          // IE up to 10.
          element.addEventListener('mscontrolselect', (evt) => {
            evt.preventDefault();
          });
        } else {
          // IE11 and higher.
          element.attachEvent('oncontrolselect', (evt) => {
            evt.returnValue = false;
          });
        }
      });
    });

    CKEDITOR.on('instanceReady', (evt) => {
      this.layoutService.toggleIsLoading(false);
      this.ready.emit(evt.editor);

      evt.editor.on('contentDom', ()=>  {
        evt.editor.document.on('keyup',  (event) => {
          if (event.data.$.key === 'Backspace') {
            
            if (CKEDITOR.instances.editor.document.getBody().$.children[0].childNodes[0].localName !== 'header') {
              CKEDITOR.instances.editor.destroy();
              this.initSample();
            } else if (CKEDITOR.instances.editor.document.getBody().$.children[0].childNodes.length === 3) {
              if (CKEDITOR.instances.editor.document.getBody().$.children[0].childNodes[1].innerHTML === "<br>") {
                CKEDITOR.instances.editor.document.getBody().$.children[0].childNodes[1].innerHTML = 'بداية الخظاب'
              }
            } else if (CKEDITOR.instances.editor.document.getBody().$.children[0].childNodes.length === 2) {
              CKEDITOR.instances.editor.destroy();
              this.initSample();              
            }
          }
        }
        );
      })
    });

  }
}
