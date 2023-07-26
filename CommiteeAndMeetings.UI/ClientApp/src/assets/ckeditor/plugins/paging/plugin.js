(function () {
  CKEDITOR.plugins.add('paging', {
    init: function (editor) {
      var template;
      var loaded = false;
      var onDeletePage = false;
      editor.setData('<section></section>');

      editor.on('instanceReady', function () {
        generatePagesNumbers();
      });

      editor.on('change', function (e) {
        generatePagesNumbers();
        loaded = false;
        getTemplate(editor);
      });

      editor.on('contentDom', function (e) {
        onDeletePage = false;
        autoPaging(editor);
      });

      //on move to another row
      editor.on('selectionChange', function () {
        var startNode = editor.getSelection().getStartElement();
        var pageNumber, previousSibling;
        if (startNode.$.tagName === 'SECTION') {
          if (!startNode.$.previousSibling) {
            pageNumber = 1;
          } else {
            pageNumber = 1;
            previousSibling = startNode.$.previousSibling;
            while (previousSibling) {
              pageNumber++;
              previousSibling = previousSibling.previousSibling;
            }
          }
        } else {
          var ancestor = startNode.$.parentNode;
          while (ancestor) {
            if (ancestor.tagName === 'SECTION') {
              if (!ancestor.previousSibling) {
                pageNumber = 1;
              } else {
                pageNumber = 1;
                previousSibling = ancestor.previousSibling;
                while (previousSibling) {
                  pageNumber++;
                  previousSibling = previousSibling.previousSibling;
                }
              }
              break;
            }
            ancestor = ancestor.parentNode;
          }
        }
      });

      editor.on( 'paste', function( e ) {
        onDeletePage = false;
        autoPaging(editor);
        const PasteData = e.data.dataValue + '<p>&nbsp;</p>';
        e.data.dataValue = PasteData;
        //console.log('this is the paste code' + PasteData)
        var selection = e.editor.getSelection();
        if (selection) {
          var ranges = selection.getRanges();
          if (ranges != null) {
            var range = ranges[0];
            if (range != null) {
              range = range.clone();

              var startNode = range.startContainer;
              var endNode = range.endContainer;

              var cancelEvent = false;

              var pos = startNode.getPosition(endNode);
              var ancestor;
              switch (pos) {
                case CKEDITOR.POSITION_IDENTICAL: {
                  switch (e.data.keyCode) {
                    case 8: { //BACKSPACE
                      if (range.startOffset === 1) {
                        var newAncestor = startNode.$.parentNode;
                        var firstHeader = $(editor.document.$).contents().find('body > section > header')[0];
                        var firstHeaderHeight = firstHeader.offsetHeight + firstHeader.offsetTop;
                        var elemOffsetHeight = newAncestor.offsetTop;

                        if (firstHeader && firstHeaderHeight === elemOffsetHeight) {
                          newAncestor.innerHTML = "بداية الخطاب";
                          // newAncestor.firstChild.insertData(0,"بداية الخطاب");
                          var selection= CKEDITOR.instances.editor.getSelection();
                          var range = selection.getRanges()[0];
                          var pCon = range.startContainer.getAscendant('p',true);
                          var newRange = new CKEDITOR.dom.range(range.document);
                          newRange.setStart(pCon.firstChild ,range.startOffset+"بداية الخطاب".length);
                          newRange.setEnd(pCon.firstChild ,range.startOffset+"بداية الخطاب".length);
                          newRange.select();
                          fireChange();
                        }
                      }
                      if (range.startOffset === 0) {
                        if (startNode.$.tagName === 'SECTION' && !startNode.$.previousSibling) {
                          cancelEvent = true;
                          break;
                        }
                        ancestor = startNode.$.parentNode;
                        if (ancestor && ancestor.tagName === 'SECTION') {
                          if (!startNode.$.previousSibling && !ancestor.previousSibling) {
                            cancelEvent = true;
                            break;
                          }
                        }
                      }
                      break;
                    }
                    default: {
                      return true;
                    }
                  }
                  break;
                }
                case CKEDITOR.POSITION_DISCONNECTED: {
                  cancelEvent = true;
                  break;
                }
                default: {
                  break;
                }
              }

              if (cancelEvent) {
                //Cancel the event
                e.cancelBubble = true;
                e.returnValue = false;
                e.cancel();
                e.stop();
                return false;
              }
            }
          }
        }
        return true;
      });

      editor.on('key', function (e) {
        onDeletePage = false;
        var selection = e.editor.getSelection();
        if (selection) {
          var ranges = selection.getRanges();
          if (ranges != null) {
            var range = ranges[0];
            if (range != null) {
              range = range.clone();

              var startNode = range.startContainer;
              var endNode = range.endContainer;

              var cancelEvent = false;

              var pos = startNode.getPosition(endNode);
              var ancestor;
              switch (pos) {
                case CKEDITOR.POSITION_IDENTICAL: {
                  switch (e.data.keyCode) {
                    case 13:
                      autoPaging(editor);
                      break;
                    case 8: { //BACKSPACE
                      if (range.startOffset === 1) {
                        var newAncestor = startNode.$.parentNode;
                        var firstHeader = $(editor.document.$).contents().find('body > section > header')[0];
                        var firstHeaderHeight = firstHeader.offsetHeight + firstHeader.offsetTop;
                        var elemOffsetHeight = newAncestor.offsetTop;

                        if (firstHeader && firstHeaderHeight === elemOffsetHeight) {
                          newAncestor.innerHTML = "بداية الخطاب";
                          // newAncestor.firstChild.insertData(0,"بداية الخطاب");
                          var selection= CKEDITOR.instances.editor.getSelection();
                          var range = selection.getRanges()[0];
                          var pCon = range.startContainer.getAscendant('p',true);
                          var newRange = new CKEDITOR.dom.range(range.document);
                          newRange.setStart(pCon.firstChild ,range.startOffset+"بداية الخطاب".length);
                          newRange.setEnd(pCon.firstChild ,range.startOffset+"بداية الخطاب".length);
                          newRange.select();
                          fireChange();
                        }
                      }
                      if (range.startOffset === 0) {
                        if (startNode.$.tagName === 'SECTION' && !startNode.$.previousSibling) {
                          cancelEvent = true;
                          break;
                        }
                        ancestor = startNode.$.parentNode;
                        if (ancestor && ancestor.tagName === 'SECTION') {
                          if (!startNode.$.previousSibling && !ancestor.previousSibling) {
                            cancelEvent = true;
                            break;
                          }
                        }
                      }
                      break;
                    }
                    default: {
                      return true;
                    }
                  }
                  break;
                }
                case CKEDITOR.POSITION_DISCONNECTED: {
                  cancelEvent = true;
                  break;
                }
                default: {
                  break;
                }
              }

              if (cancelEvent) {
                //Cancel the event
                e.cancelBubble = true;
                e.returnValue = false;
                e.cancel();
                e.stop();
                return false;
              }
            }
          }
        }
        return true;

      });

      function getTemplate(editor) {
        if (!loaded) {
          const data = editor.getData();
          const sectionHasClass = localStorage['ckeditorClasses'] === '1' ? true : false;
          if (data.includes('<header>') || data.includes('<footer>')) {
            loaded = true;
            let header = $(editor.document.$).contents().find('body > section > header').length > 0 ? $(editor.document.$).contents().find('body > section > header')[0].outerHTML : '';
            let footer = $(editor.document.$).contents().find('body > section > footer').length > 0 ? $(editor.document.$).contents().find('body > section > footer')[0].outerHTML : '';
            //get header and footer and set it's attributes to false ...
            head = $(editor.document.$).contents().find('body > section > header')[0];
            foot = $(editor.document.$).contents().find('body > section > footer')[0];
            if (head && foot) {
              head.setAttribute('contenteditable', false);
              foot.setAttribute('contenteditable', false);
            }
            template = (header && footer) ? {
              header: header,
              footer: footer
            } : null;
          }
          let newSection = $(editor.document.$).contents().find('body > section').length > 0 ? $(editor.document.$).contents().find('body > section')[0] : '';
          if (newSection && newSection.getAttribute('id') === 'gaddaUni') newSection.setAttribute('class', 'gaddaUni');
          if (sectionHasClass) {
            let newSection = $(editor.document.$).contents().find('body > section').length > 0 ? $(editor.document.$).contents().find('body > section')[0] : '';
            if (newSection && !newSection.classList.contains('noBorderClass')) newSection.setAttribute('class', 'noBorderClass');
          }
        }
      }

      function autoPaging(editor) {
        var editorContents = $(editor.document.$).contents();
        var sections = editorContents.find('body > section');
        if (!sections.length) {
          var bodyContents = editorContents.find('body >');
          if (bodyContents.length) {
            bodyContents.wrapAll('<section></section>');
          } else {
            editorContents.find('body').html('<section></section>');
          }
          return;
        }
        var sectionHeight = sections.eq(0).height();
        var footerHeight = sections.eq(0).find('footer').outerHeight();
        footerHeight = footerHeight ? footerHeight : 0;
        sections.each(sectionCheck);

        function sectionCheck(index, section) {
          if (onDeletePage) {
            return;
          }
          var exceededElementsParents = $(section).find('> :not(footer)').filter(function (i, ele) {
            return ($(ele).position().top + $(ele).outerHeight()) > (sectionHeight - footerHeight) && ($(ele).outerHeight() > (sectionHeight - footerHeight))
          });

          if (exceededElementsParents.length) {
            exceededElementsParents.replaceWith(exceededElementsParents.html());
            sectionCheck(index, section);
            return;
          }

          var exceededElements = $(section).find('> :not(footer)').filter(function (i, ele) {
            return ($(ele).position().top + $(ele).outerHeight()) > (sectionHeight - footerHeight) && !($(ele).outerHeight() > (sectionHeight - footerHeight))
          });
          var nextSection = sections[index + 1];

          if (exceededElements.length) {
            var ranges = editor.getSelection().getRanges();
            if (!nextSection ) {
              const headerexist = sections.eq(0)[0].innerHTML.includes('header');
              const footerexist = sections.eq(0)[0].innerHTML.includes('footer');
              const isSectionHasClassNoBorderClass = $(section).hasClass('noBorderClass');
              // gedda university water mark
              const isSectionHasClassGaddaUni = $(section).parent().find('section#gaddaUni').length;
              if(template && !headerexist && !footerexist) {
                template.header = '';
                template.footer = '';
              }
              const tmplt = (template)  ? (template.header + template.footer) : '';
              if (tmplt) {
                $(section).after($('<section data-page="' + (index + 2) + '" class="' + (isSectionHasClassNoBorderClass ? 'noBorderClass' : '')+(isSectionHasClassGaddaUni ? ' gaddaUni' : '')+'">' + tmplt + '<span contenteditable="false" class="remove-page">x</span></section>')).next('section').find('header').after(exceededElements.detach());
              } else {
                $(section).after($('<section data-page="' + (index + 2) + '" class="' + (isSectionHasClassNoBorderClass ? 'noBorderClass' : '')+(isSectionHasClassGaddaUni ? ' gaddaUni' : '')+'"><span contenteditable="false" class="remove-page">x</span></section>'))
                  .append(exceededElements.detach());
              }
            } else {
              $(nextSection).find('header').after(exceededElements.detach());
            }
            editor.getSelection().selectRanges(ranges);
            fireChange();
          }

          if (nextSection) {
            var lastElementInSection = $(section).find('> :not(footer)').last();
            if (lastElementInSection) {
              var emptySpaceHeight = (sectionHeight - footerHeight) - (lastElementInSection.position().top + lastElementInSection.outerHeight());
              var firstElementInNextSection = $(nextSection).find('> :not(header)').first();
              var firstElementInNextSectionHeight = firstElementInNextSection ? firstElementInNextSection.outerHeight() : 0;
              if (emptySpaceHeight > firstElementInNextSectionHeight) {
                ranges = editor.getSelection().getRanges();
                var footer = $(section).find('> footer');
                footer.eq(0) ? footer.eq(0).before(firstElementInNextSection.detach()) : $(section).append(firstElementInNextSection.detach());
                editor.getSelection().selectRanges(ranges);
                fireChange();
              }
            }
            //get header and footer for new section and set it's attributes to false ...
            var nextHeader = $(nextSection).find('header')[0];
            var nextFooter = $(nextSection).find('footer')[0];
            (nextHeader && !nextHeader.getAttribute('contenteditable')) ? nextHeader.setAttribute('contenteditable', false): '';
            (nextFooter && !nextFooter.getAttribute('contenteditable')) ? nextFooter.setAttribute('contenteditable', false): '';
            // after add delete icon >> add eventListeners ....
            deleteLetterPage();
          }
        }
      }

      function deleteLetterPage() {
        const bodyElm = $(editor.document.$).contents().find('body')[0];
        if (bodyElm) {
          const spanElms = bodyElm.querySelectorAll('.remove-page') ? Array.prototype.slice.call(bodyElm.querySelectorAll('.remove-page')) : null;
          if (spanElms && spanElms.length) {
            for (var index in spanElms) {
              if (!spanElms[index].getAttribute('data-event-click')) {
                spanElms[index].setAttribute('data-event-click', true);
                spanElms[index].addEventListener('click', function (event) {
                  event.stopPropagation();
                  const elementCliked = event.target;
                  bodyElm.removeChild(elementCliked.parentElement);
                  onDeletePage = true;
                }, false);
              }
            }
          }
        }
      }

      function generatePagesNumbers() {
        $(editor.document.$).contents().find('body > section').each(function (i) {
          $(this).attr('data-page', i + 1);
        });
      }

      function fireChange() {
        setTimeout(function () {
          editor.fire('change');
        }, 50);
      }
    }
  });
})();
