/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
  // Define changes to default configuration here. For example:
  config.language = 'ar';
  // config.uiColor = '#AADC6E';
  config.skin = 'office2013';
  config.extraPlugins = 'lineheight,tab,paging,arabicnumeric,extraStyleSheets'; //divarea
  config.line_height = "1em;1.5em;2em;3em;4em;5em";
  config.tabSpaces = 6;
  config.resize_enabled = false;

  //upload local files configrations ...
  config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
  config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?type=Images';
  config.filebrowserUploadUrl = '/ckfinder/connector?command=QuickUpload&type=Files';
  config.filebrowserImageUploadUrl = '/ckfinder/connector?command=QuickUpload&type=Images';
  config.filebrowserWindowWidth = '700';
  config.filebrowserWindowHeight = '500';

  // config.extraAllowedContent = '*[*]{*}(*)';
  config.extraAllowedContent = 'header; section[*];  footer; span(*)';
  // () => for class attributes ....
  // [] => for custom or directive attributes ...
  // {} => with attribues {!} or without {} ...
  var customFonts = [
    'AL-Mohanad/"AL-Mohanad"',
    'AL-Mohanad-Bold/"AL-Mohanad-Bold"',
    'DIN Next LT Ar/"DIN-Next-LT-Ar"',
    'DIN Next LT Ar Light/"DIN-Next-LT-Ar-Light"',
    'Al-Mateen/"Al-Mateen"',
    'Kufyan Ar Bold/"Kufyan-Ar-Bold"',
    'GE SS Two Medium/"GE-SS-Two-Medium"',
    'GE Dinar Two/"GE-Dinar-Two"',
    'GEDinar Two Light/"GE-Dinar-Two-Light"',
    'Swis721 Cn BT/"Swis721-Cn-BT"',
    'Shorooq N1/"Shorooq_N1"',
    'JF Flat Regular/"JF-Flat-Regular"',
    'Sakkal Majalla/"Sakkal-Majalla"',
    'Apex Sans Bold/"Apex-Sans-Bold"',
    'Apex Sans Book/"Apex-Sans-Book"',
    'Apex Sans Medium/"Apex-Sans-Medium"',
    'Frutiger LT Ar 45 Light/"Frutiger-LT-Ar-45-Light"',
    'Frutiger LT Ar 55 Roman/"Frutiger-LT-Ar-55-Roman"',
    'Frutiger LT Ar 65 Bold/"Frutiger-LT-Ar-65-Bold"',
    'Frutiger LT Ar 75 Black/"Frutiger-LT-Ar-75-Black"',
    'Hacen Liner Print-out/"Hacen-Liner-Print-out"',
    'Hacen Liner Print-out Light/"Hacen-Liner-Print-out-Light"',
    'Hacen Liner XL/"Hacen-Liner-XL"',
    'Hacen Liner XXL/"Hacen-Liner-XXL"',
    'Segoe UI/"Segoe-UI"',
    'Segoe UI Semilight/"Segoe-UI-Semilight"',
    'Segoe UI Semibold/"Segoe-UI-Semibold"',
    'Segoe UI Bold/"Segoe-UI-Bold"',
    'AGENCYB/"AGENCYB"',
    'AGENCYR/"AGENCYR"',
    'ahronbd/"ahronbd"',
    'angsaub/"angsaub"',
    'ALGER/"ALGER"',
    'andlso/"andlso"',
    'angsa/"angsa"',
    'angsab/"angsab"',
    'angsai/"angsai"',
    'angsau/"angsau"',
    'angsaui/"angsaui"',
    'angsauz/"angsauz"',
    'angsaz/"angsaz"',
    'ANTQUAB/"ANTQUAB"',
    'ANTQUABI/"ANTQUABI"',
    'ANTQUAI/"ANTQUAI"',
    'Sultan bold/"Sultan bold"',
    'Sultan normal/"Sultan normal"',
    'Tajawal-Bold/"Tajawal-Bold"',
    'Tajawal-Medium/"Tajawal-Medium"',
    'Tajawal-Regular/"Tajawal-Regular"',
    'Cairo-Regular/"Cairo-Regular"',
    'Cairo-SemiBold/"Cairo-SemiBold"',
    'ae_Khalid/"ae_Khalid"',
    'ae_Khalid-Bold/"ae_Khalid-Bold"',
    'ge-ss-2-light/"ge-ss-2-light"'
  ]; // 'Arabia Weather/"Arabia Weather"'

  config.font_names = customFonts.join(';') + ';' + config.font_names;
};
