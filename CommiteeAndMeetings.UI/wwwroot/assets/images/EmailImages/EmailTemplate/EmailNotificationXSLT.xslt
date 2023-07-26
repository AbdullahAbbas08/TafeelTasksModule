<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html"/>
  <xsl:template match="/">    
    <html>
      <head>
        <title>.:: وزارة الحج ::.</title>        
      </head>
       <body style="margin: 0;" >
        <table align="center" cellpadding="0" cellspacing="0" margin="0" dir="rtl"  width="680px;">
          <tr>
            <td >
              <img src="cid:top" style="" />
            </td>
          </tr>
          <tr>
            <td style="padding-bottom: 50px;
    padding-right: 14px;
    padding-top: 17px !important;">
              <img src="cid:top1" />              
              <p style="direction: rtl; font-family: tahoma; font-size: 13px; line-height: 26px;padding-right: 18px;padding-left: 26px;
                    text-align: justify; color: #2e302d">
                <xsl:value-of select="EmailTemplates/notification/msg"/> 
                <span style="direction: ltr;font-family: arial; font-size: 13px; font-weight: bold; color: #56924c;">
                  <xsl:value-of select="EmailTemplates/notification/transNumber"/>
                </span>
              </p>
            </td>
          </tr>
          <tr>
            <td>
              <img src="cid:top2" />
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>

<!--This control belongs to General_Headers (Id: 30) Category for Language Settings-->