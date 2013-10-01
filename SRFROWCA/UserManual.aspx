<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManual.aspx.cs" Inherits="SRFROWCA.UserManual" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <title>User Manual</title>
    <style>
<!--
 /* Font Definitions */
 @font-face
	{font-family:Wingdings;
	panose-1:5 0 0 0 0 0 0 0 0 0;
	mso-font-charset:2;
	mso-generic-font-family:auto;
	mso-font-pitch:variable;
	mso-font-signature:0 268435456 0 0 -2147483648 0;}
@font-face
	{font-family:"Cambria Math";
	panose-1:2 4 5 3 5 4 6 3 2 4;
	mso-font-charset:1;
	mso-generic-font-family:roman;
	mso-font-format:other;
	mso-font-pitch:variable;
	mso-font-signature:0 0 0 0 0 0;}
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;
	mso-font-charset:0;
	mso-generic-font-family:swiss;
	mso-font-pitch:variable;
	mso-font-signature:-536870145 1073786111 1 0 415 0;}
@font-face
	{font-family:Tahoma;
	panose-1:2 11 6 4 3 5 4 4 2 4;
	mso-font-charset:0;
	mso-generic-font-family:swiss;
	mso-font-format:other;
	mso-font-pitch:variable;
	mso-font-signature:3 0 0 0 1 0;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{mso-style-unhide:no;
	mso-style-qformat:yes;
	mso-style-parent:"";
	margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:0in;
	line-height:115%;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:"Times New Roman";
	mso-bidi-theme-font:minor-bidi;}
p.MsoAcetate, li.MsoAcetate, div.MsoAcetate
	{mso-style-noshow:yes;
	mso-style-priority:99;
	mso-style-link:"Balloon Text Char";
	margin:0in;
	margin-bottom:.0001pt;
	mso-pagination:widow-orphan;
	font-size:8.0pt;
	font-family:"Tahoma","sans-serif";
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-bidi-font-family:Tahoma;}
p.MsoListParagraph, li.MsoListParagraph, div.MsoListParagraph
	{mso-style-priority:34;
	mso-style-unhide:no;
	mso-style-qformat:yes;
	margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:.5in;
	mso-add-space:auto;
	line-height:115%;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:"Times New Roman";
	mso-bidi-theme-font:minor-bidi;}
p.MsoListParagraphCxSpFirst, li.MsoListParagraphCxSpFirst, div.MsoListParagraphCxSpFirst
	{mso-style-priority:34;
	mso-style-unhide:no;
	mso-style-qformat:yes;
	mso-style-type:export-only;
	margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.5in;
	margin-bottom:.0001pt;
	mso-add-space:auto;
	line-height:115%;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:"Times New Roman";
	mso-bidi-theme-font:minor-bidi;}
p.MsoListParagraphCxSpMiddle, li.MsoListParagraphCxSpMiddle, div.MsoListParagraphCxSpMiddle
	{mso-style-priority:34;
	mso-style-unhide:no;
	mso-style-qformat:yes;
	mso-style-type:export-only;
	margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.5in;
	margin-bottom:.0001pt;
	mso-add-space:auto;
	line-height:115%;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:"Times New Roman";
	mso-bidi-theme-font:minor-bidi;}
p.MsoListParagraphCxSpLast, li.MsoListParagraphCxSpLast, div.MsoListParagraphCxSpLast
	{mso-style-priority:34;
	mso-style-unhide:no;
	mso-style-qformat:yes;
	mso-style-type:export-only;
	margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:.5in;
	mso-add-space:auto;
	line-height:115%;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:"Times New Roman";
	mso-bidi-theme-font:minor-bidi;}
span.BalloonTextChar
	{mso-style-name:"Balloon Text Char";
	mso-style-noshow:yes;
	mso-style-priority:99;
	mso-style-unhide:no;
	mso-style-locked:yes;
	mso-style-link:"Balloon Text";
	mso-ansi-font-size:8.0pt;
	mso-bidi-font-size:8.0pt;
	font-family:"Tahoma","sans-serif";
	mso-ascii-font-family:Tahoma;
	mso-hansi-font-family:Tahoma;
	mso-bidi-font-family:Tahoma;}
span.GramE
	{mso-style-name:"";
	mso-gram-e:yes;}
.MsoChpDefault
	{mso-style-type:export-only;
	mso-default-props:yes;
	font-family:"Calibri","sans-serif";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:"Times New Roman";
	mso-bidi-theme-font:minor-bidi;}
.MsoPapDefault
	{mso-style-type:export-only;
	margin-bottom:10.0pt;
	line-height:115%;}
@page WordSection1
	{size:8.5in 11.0in;
	margin:.5in .5in .5in .5in;
	mso-header-margin:.5in;
	mso-footer-margin:.5in;
	mso-paper-source:0;}
div.WordSection1
	{page:WordSection1;}
 /* List Definitions */
 @list l0
	{mso-list-id:520360713;
	mso-list-type:hybrid;
	mso-list-template-ids:522371372 1500392834 67698713 67698715 67698703 67698713 67698715 67698703 67698713 67698715;}
@list l0:level1
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	margin-left:.75in;
	text-indent:-.25in;
	mso-ansi-font-weight:normal;}
@list l0:level2
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	margin-left:1.25in;
	text-indent:-.25in;}
@list l0:level3
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	margin-left:1.75in;
	text-indent:-9.0pt;}
@list l0:level4
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	margin-left:2.25in;
	text-indent:-.25in;}
@list l0:level5
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	margin-left:2.75in;
	text-indent:-.25in;}
@list l0:level6
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	margin-left:3.25in;
	text-indent:-9.0pt;}
@list l0:level7
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	margin-left:3.75in;
	text-indent:-.25in;}
@list l0:level8
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	margin-left:4.25in;
	text-indent:-.25in;}
@list l0:level9
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	margin-left:4.75in;
	text-indent:-9.0pt;}
@list l1
	{mso-list-id:724910268;
	mso-list-type:hybrid;
	mso-list-template-ids:120197068 -1601928882 67698713 67698715 67698703 67698713 67698715 67698703 67698713 67698715;}
@list l1:level1
	{mso-level-text:%1-;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l1:level2
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l1:level3
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	text-indent:-9.0pt;}
@list l1:level4
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l1:level5
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l1:level6
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	text-indent:-9.0pt;}
@list l1:level7
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l1:level8
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l1:level9
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	text-indent:-9.0pt;}
@list l2
	{mso-list-id:893783122;
	mso-list-type:hybrid;
	mso-list-template-ids:-1679782058 67698703 67698713 67698715 67698703 67698713 67698715 67698703 67698713 67698715;}
@list l2:level1
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l2:level2
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l2:level3
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	text-indent:-9.0pt;}
@list l2:level4
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l2:level5
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l2:level6
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	text-indent:-9.0pt;}
@list l2:level7
	{mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l2:level8
	{mso-level-number-format:alpha-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;}
@list l2:level9
	{mso-level-number-format:roman-lower;
	mso-level-tab-stop:none;
	mso-level-number-position:right;
	text-indent:-9.0pt;}
@list l3
	{mso-list-id:1694380326;
	mso-list-type:hybrid;
	mso-list-template-ids:-1712847514 67698689 67698691 67698693 67698689 67698691 67698693 67698689 67698691 67698693;}
@list l3:level1
	{mso-level-number-format:bullet;
	mso-level-text:\F0B7;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:Symbol;}
@list l3:level2
	{mso-level-number-format:bullet;
	mso-level-text:o;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:"Courier New";}
@list l3:level3
	{mso-level-number-format:bullet;
	mso-level-text:\F0A7;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:Wingdings;}
@list l3:level4
	{mso-level-number-format:bullet;
	mso-level-text:\F0B7;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:Symbol;}
@list l3:level5
	{mso-level-number-format:bullet;
	mso-level-text:o;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:"Courier New";}
@list l3:level6
	{mso-level-number-format:bullet;
	mso-level-text:\F0A7;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:Wingdings;}
@list l3:level7
	{mso-level-number-format:bullet;
	mso-level-text:\F0B7;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:Symbol;}
@list l3:level8
	{mso-level-number-format:bullet;
	mso-level-text:o;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:"Courier New";}
@list l3:level9
	{mso-level-number-format:bullet;
	mso-level-text:\F0A7;
	mso-level-tab-stop:none;
	mso-level-number-position:left;
	text-indent:-.25in;
	font-family:Wingdings;}
ol
	{margin-bottom:0in;}
ul
	{margin-bottom:0in;}
-->
</style>   


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <a href="3WPMUserManual.pdf">Download as PDF</a>
</div>
    <div class="WordSection1">
        <p align='center' style='text-align: center'>
            <b><span style='font-size: 18.0pt; line-height: 115%'>3W Performance Monitoring (3W
                PM)</span></b><b><span style='font-size: 14.0pt; line-height: 115%'></span></b></p>
        <p align='center' style='text-align: center'>
            <b><span style='font-size: 16.0pt; line-height: 115%'>User Guide</span></b><b><span
                style='font-size: 15.0pt; line-height: 115%'></span></b></p>
        <p align='center' style='text-align: center'>
            <b><span style='font-size: 15.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span></b>
        </p>
                    <img src="UserManual_files/image001.png" o:title="" />
                </v:shape>
            </span></b><b><span style='font-size: 14.0pt; line-height: 115%'></span></b>
        </p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>3W Performance Monitoring (3WPM)
                is a web based tool. It has been developed as a reporting tool for all the stakeholders
                to report their activities and share inter organization and intra organization information.
                There is also an Excel dashboard which can be used to extract information from the
                data entered in this tool. Excel dashboard has charts, maps and summary information
                on the basis of data entered in 3WPM.</span></p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>This tool is very simple and only
                has three pages, after registration, to view and enter data. The simple design <span
                    class="GramE">of</span> this tool makes is easy for any user who has a little
                knowledge of computer to enter data into 3WPM.</span></p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>To View data user don’t have to login
                and the home page will show all the data in this online database. User can use different
                criteria to filter data. The data on the page can be export to Excel and there is
                also a link to fetch all data as an XML feed.</span></p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>The user wants to report using this
                tool has to register first. After successfully login user will see two pages, ‘My
                Activities’ and ‘Data Entry’ using these two pages user can enter her/his 3W information
                into 3WPM.</span></p>        
        <p style='text-align: justify'>
            <b><span style='font-size: 14.0pt; mso-bidi-font-size: 12.0pt; line-height: 115%'>Details
                of the home page: The detail of each tag in image is given below</span></b></p>
        <p style='text-align: justify'>
            <b><span style='font-size: 14.0pt; mso-bidi-font-size: 12.0pt; line-height: 115%;
                mso-no-proof: yes'>
                <v:shape id="Picture_x0020_13" o:spid="_x0000_i1034" type="#_x0000_t75" style='width: 539.25pt;
                    height: 219.75pt; visibility: visible; mso-wrap-style: square'>                    
                    <img src="UserManual_files/image002.png" o:title="" />
                </v:shape>
            </span></b><b><span style='font-size: 14.0pt; mso-bidi-font-size: 12.0pt; line-height: 115%'>
            </span></b>
        </p>
        <p class="MsoListParagraphCxSpFirst" style='text-align: justify; text-indent: -.25in;
            mso-list: l2 level1 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>1.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Register:</span></b><span
                    style='font-size: 12.0pt; line-height: 115%'> Click on Register link to register
                    yourself. You will see a very straight forward page. Fields on this page are :</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>a.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>User
                    Name: Enter your user name. If someone already has that username then you will see
                    a message.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>b.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Password:
                    Password should be at least 3 characters.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>c.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Confirm
                    Password: Repeat your password again to confirm.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>d.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Email:
                    Enter your valid email address. This address will be used for correspondence.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>e.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Phone:
                    Enter your phone/cell number.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>f.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Organization:
                    Select your organization from the list. If you organization is not in the list please
                    contact us by using ‘Contact Us’ page, email and/or phone to add your organization.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>g.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Office
                    Country: Select your office country from locations list. If country in which your
                    office resides does not exist in list please contact us.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_14" o:spid="_x0000_i1033" type="#_x0000_t75" style='width: 448.5pt;
                    height: 234.75pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image003.png" o:title="" />
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify; text-indent: -.25in;
            mso-list: l2 level1 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>2.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Login:</span></b><span
                    style='font-size: 12.0pt; line-height: 115%'> Use user-name and password to login.
                    If you don’t have account please click on ‘Register’ link to register yourself.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify; text-indent: -.25in;
            mso-list: l2 level1 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>3.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Filter
                    Data:</span></b><span style='font-size: 12.0pt; line-height: 115%'> <b><span style='mso-spacerun: yes'>
                    </span></b>User can filter data using drop down on top of the page. In most of the drop
                        downs user can select multiple values like in the following image. In following
                        image use selected three clusters to filter there data.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_8" o:spid="_x0000_i1032" type="#_x0000_t75" style='width: 327pt;
                    height: 207.75pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image004.png" o:title="" />
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>User can filter data on Emergency,
                Cluster, Location, Year, Month, User, Organization Type, Organization and Office.
                User can filter data on multiple criteria like Cluster and Location etc.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify; text-indent: -.25in;
            mso-list: l2 level1 lfo2'>
            <![if !supportLists]><b><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>4.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span></b><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>XML
                    Feed Link: </span></b><span style='font-size: 12.0pt; line-height: 115%'>This is very
                        important feature of this tool. This link is to fetch all the data, after filter
                        criteria applied, will be fetched as an XML feed. You can use this xml feed where
                        ever you want but in this tool this feed is especially being generated for the data
                        feed of Excel Dashboard. <b></b></span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>User can get the URL of this feed
                two ways 1) click on this button and it will open a new page, copy URL from browsers
                address bar. 2) Right click on this button and copy link location. After getting
                this URL user can use this URL anywhere s/he wants to.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>To use this in Excel as data feed,
                please follow these steps.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>a.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Get
                    URL of the feed: We have described above two ways to get URL of this feed.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>b.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Open
                    Excel.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>c.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>Click
                    on menu Data -&gt; From Other Sources -&gt; From XML Data Import</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_9" o:spid="_x0000_i1031" type="#_x0000_t75" style='width: 483.75pt;
                    height: 181.5pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image005.png" o:title="" />
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>d.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span class="GramE"><span style='font-size: 12.0pt;
                    line-height: 115%'>Paste copied URL in ‘File Name’ box and click</span></span><span
                        style='font-size: 12.0pt; line-height: 115%'> Open, see following image.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_11" o:spid="_x0000_i1030" type="#_x0000_t75" style='width: 440.25pt;
                    height: 79.5pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image006.png" o:title="" />
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>e.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>You
                    might see few message but just click ‘Yes’ <span class="GramE">Or</span> ‘OK’.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: 1.0in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l2 level2 lfo2'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>f.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><span style='font-size: 12.0pt; line-height: 115%'>You
                    can get latest data just by clicking on ‘Refresh’ button in Excel without going
                    back to 3WPM tool and doing the same exercise again.</span></p>
        <p class="MsoListParagraphCxSpLast" style='text-align: justify; text-indent: -.25in;
            mso-list: l2 level1 lfo2'>
            <![if !supportLists]><b><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>5.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span></b><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Export
                    <span class="GramE">To</span> Excel: </span></b><span style='font-size: 12.0pt; line-height: 115%'>
                        <span style='mso-spacerun: yes'></span>You can export this data to excel by clicking
                        on ‘Export To Excel’ button. All your filtered data will be exported to an Excel
                        Sheet.<b></b></span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%'>Contact US: </span></b><span
                style='font-size: 12.0pt; line-height: 115%'>Use contact us page to send your feedback,
                questions, requests and queries.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_15" o:spid="_x0000_i1029" type="#_x0000_t75" style='width: 392.25pt;
                    height: 346.5pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image007.png" />
                    <link href="UserManual_files/image007.png" />
                    
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span>
        </p>                
      
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>After login user
                will see two new pages. These two pages are interrelated. User has to use both these
                to report his/her 3W activities.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>My Activities
                Page: </span></b><span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                    This is the page from where user will select his/her activitis on which s/he wants
                    to report. This is very simple page. Following is the image and explanation of each
                    item on the page.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_16" o:spid="_x0000_i1028" type="#_x0000_t75" style='width: 539.25pt;
                    height: 263.25pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image008.png" o:title="" />
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>On this page user will see two drop
                downs i.e. ‘Emergency’ &amp; ‘Office’. Emergency drop down will have all the emergencies
                of that country which user has selected at the time of registration. Office drop
                down will have all the emergencies of that country which user has selected at time
                of registration.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>If there is only one emergency and
                only one office then both these will be selected by default otherwise user has to
                select his/her emergency and office from respective drop downs.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>There is one grid on this page with
                heading ‘My Activities’. This grid has all the activities grouped by clusters. User
                can expand/collapse these clusters.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>Add/Remove Activities: If <span
                class="GramE">user want</span> to add one activity at a time then there is ‘+’ icon
                on right of the activity, clicking this icon will add activity in user’s list. If
                user want to remove an activity from his/her list user just have to click on ‘X’
                icon.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>User can also add/remove multiple
                activities at a time. To do this user has to use ‘Select’ checkbox in first column
                of the grid, at left side, and select as many activities as s/he wants from different
                clusters. After selecting activities click on ‘Add Selected’ button on top-right
                of the page to add activities and click on ‘Delete Selected’ button to remove activities
                from his/her list.</span></p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>User can see his/her selected activities
                in ‘Data Entry’.</span></p>
       
        <p style='margin-left: .25in; text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%'>Data Entry Page:</span></b></p>
        <p style='margin-left: .25in; text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_17" o:spid="_x0000_i1027" type="#_x0000_t75" style='width: 539.25pt;
                    height: 130.5pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image009.png" o:title="" />
                </v:shape>
            </span></b><b><span style='font-size: 12.0pt; line-height: 115%'></span></b>
        </p>
        <p style='margin-left: .25in; text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>User can enter target and achieved
                data against their activities in this page. The details of this page are following:</span></p>
        <p class="MsoListParagraphCxSpFirst" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l0 level1 lfo4'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>1.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Emergency:</span></b><span
                    style='font-size: 12.0pt; line-height: 115%'> This is a pull down box having all
                    the emergencies of user’s location. If there is only one emergency then it will
                    be automatically selected but if there is more than one then user has to select
                    the emergency under which s/he wants to report.</span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l0 level1 lfo4'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>2.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Office:
                </span></b><span style='font-size: 12.0pt; line-height: 115%'>This is a pull down box
                    having all the offices of user’s organization. If there is more than one office
                    then user has to select his/her office. If there is only one office then it will
                    be selected automatically.<b></b></span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l0 level1 lfo4'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>3.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Year:
                </span></b><span style='font-size: 12.0pt; line-height: 115%'>This pull down box having
                    years list. Current year will be selected automatically but user can select any
                    other year.<b></b></span></p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l0 level1 lfo4'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>4.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Month:
                </span></b><span style='font-size: 12.0pt; line-height: 115%'>This pull down box having
                    month names. Current month will be selected automatically but user can select any
                    other month.<b></b></span></p>
        <p class="MsoListParagraphCxSpLast" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify; text-indent: -.25in; mso-list: l0 level1 lfo4'>
            <![if !supportLists]><span style='font-size: 12.0pt; line-height: 115%; mso-bidi-font-family: Calibri;
                mso-bidi-theme-font: minor-latin'><span style='mso-list: Ignore'>5.<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span></span></span><![endif]><b><span style='font-size: 12.0pt; line-height: 115%'>Locations
                    (Button): </span></b><span style='font-size: 12.0pt; line-height: 115%'>When user will
                        click this button a window will pop-up. This window will have all the list of admin2
                        locations of user’s admin1 location in left side of the box. User can select one
                        or more than one locations on which s/he wants to report in that particular emergency/year/month.
                        <b></b></span>
        </p>
        <p style='text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_18" o:spid="_x0000_i1026" type="#_x0000_t75" style='width: 539.25pt;
                    height: 180pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image010.png" o:title="" />
                </v:shape>
            </span></b><b><span style='font-size: 12.0pt; line-height: 115%'></span></b>
        </p>
        <p class="MsoListParagraphCxSpFirst" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span></b>
        </p>
        <p class="MsoListParagraphCxSpMiddle" style='margin-left: .75in; mso-add-space: auto;
            text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span></b>
        </p>
        
        
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>After selecting locations user can
                report on the activities.</span></p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%; mso-no-proof: yes'>
                <v:shape id="Picture_x0020_19" o:spid="_x0000_i1025" type="#_x0000_t75" style='width: 540pt;
                    height: 203.25pt; visibility: visible; mso-wrap-style: square'>
                    <img src="UserManual_files/image011.png" o:title="" />
                </v:shape>
            </span><span style='font-size: 12.0pt; line-height: 115%'></span>
        </p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'>User can enter targets and achieved
                for an activity under locations and save. User can also add more locations and remove
                already added locations in the report. If user removes a location and click on save
                button the data of removed locations deleted permanently.</span></p>
        <p class="MsoListParagraph" style='margin-left: .75in; mso-add-space: auto; text-align: justify'>
            <b><span style='font-size: 12.0pt; line-height: 115%'>
                <o:p>&nbsp;</o:p>
            </span></b>
        </p>
        <p style='text-align: justify'>
            <span style='font-size: 12.0pt; line-height: 115%'><span style='mso-spacerun: yes'></span>
            </span>
        </p>
    </div>


</asp:Content>
