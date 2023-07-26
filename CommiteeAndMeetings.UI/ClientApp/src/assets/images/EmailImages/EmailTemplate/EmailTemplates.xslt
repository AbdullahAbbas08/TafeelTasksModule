<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html"/>

  <xsl:template match="/">
    <html>
      <head>
        <title>.:: وزارة الحج ::.</title>
      </head>
      <body style="margin:0; direction:rtl">
        <div  style="margin-bottom: 20px;height: 130px;font-family: Helvetica, Arial, sans-serif;background:url(../../../images/EmailTemplates/temp-headbg.jpg) 0 0 repeat-x;font-size: 1.6em;">
          <img src="cid:image1" style="display: block;    height: 67px;    margin-bottom: 0;    margin-left: auto;    margin-right: auto;    padding-top: 25px;    width: 237px;" />
        </div>
        <div style=" width:647px; margin:0 auto; display:block;">
          <h1 style="margin: 0 0 8px;height: 37px;font-family: Helvetica, Arial, sans-serif;background:url(../../../images/EmailTemplates/temp-tittle.jpg) 0 0 no-repeat;font-size: 21px; color: #435241;padding: 9px 43px 0 0;text-align: right;text-shadow: 1px 1px 0 #c6e2c1;">تفاصيل المعاملة</h1>
          <div>
            <span style="margin-bottom: 3px;height: 16px;font-family: tahoma;background:url(../../../images/EmailTemplates/images/temo-content.jpg) 0 0 repeat-x;font-size:12px; color:#30382f; border:1px solid #f0f0f0;display:block; float:right;width: 137px;width: 137px; text-align:right; padding:14px; -moz-border-radius: 0px 10px 10px 0px;-webkit-border-radius: 0px 10px 10px 0px;-khtml-border-radius: 0px 10px 10px 0px;border-radius:0px 10px 10px 0px;">اسم المعاملة</span>
            <span style="margin-bottom: 3px;height: 16px;font-family: tahoma;background:#f7f7f7;font-size:12px; color:#30382f; margin-right:1px;display: block; float:right;width: 446px;text-align:right; padding:14px; border:1px solid #f7f7f7;-moz-border-radius: 10px 0px 0px 10px;-webkit-border-radius: 10px 0px 0px 10px;-khtml-border-radius: 10px 0px 0px 10px;border-radius:10px 0px 0px 10px;">
              <xsl:value-of select="EmailTemplates/notification/msg"/>
            </span>
          </div>
        </div>
        <div style="clear:both"></div>
        <div  style="margin-top: 62px;height: 69px;font-family: Helvetica, Arial, sans-serif;background:url(../../../images/EmailTemplates/temp-footer.jpg) 0 0 repeat-x;font-size: 1.6em;">
          <span style="color: #838383;    display: block;    font-size: 11px;    height: 42px;    margin-bottom: 0;    margin-left: auto;    margin-right: auto;    padding-top: 25px;    width: 232px;" >Copy rights allrights reserved to NBU @2013</span>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
<!--This control belongs to General_Headers (Id: 30) Category for Language Settings-->