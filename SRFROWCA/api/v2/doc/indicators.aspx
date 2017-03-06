<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="indicators.aspx.cs" Inherits="SRFROWCA.api.v2.doc.indicators" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .wdt {
            width: 260px;
        }

        input[type="text"] {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            background-color: #fff;
            border-bottom-color: #d5d5d5;
            border-bottom-left-radius: 0 !important;
            border-bottom-right-radius: 0 !important;
            border-bottom-style: solid;
            border-bottom-width: 1px;
            border-image-outset: 0 0 0 0;
            border-image-repeat: stretch stretch;
            border-image-slice: 100% 100% 100% 100%;
            border-image-source: none;
            border-image-width: 1 1 1 1;
            border-left-color: #d5d5d5;
            border-left-style: solid;
            border-left-width: 1px;
            border-right-color: #d5d5d5;
            border-right-style: solid;
            border-right-width: 1px;
            border-top-color: #d5d5d5;
            border-top-left-radius: 0 !important;
            border-top-right-radius: 0 !important;
            border-top-style: solid;
            border-top-width: 1px;
            box-shadow: none !important;
            font-family: inherit;
            font-size: 12px;
            line-height: 1;
            padding-bottom: 5px;
            padding-left: 4px;
            padding-right: 4px;
            padding-top: 5px;
            transition-duration: 0.1s;
        }
    </style>
    <link rel="stylesheet" href="../../../assets/css/chosen.css" />
    <script>
        $(function () {
            $('[data-rel=popover]').popover({ html: true });
            $('#btnURL').click(function () {
                var queryString = "";
                var typeQS = MakeCountryQueryString();
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeClusterQueryString();
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeSingleValueQueryString("#selectYear", "year");
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeSingleValueQueryString("#selectLang", "lng");
                queryString = MakeURL(queryString, typeQS);

                var url = $('#hdURL').val();
                url += queryString;
                $('#txtURL').val(url);
                copyToClipboardMsg(document.getElementById("txtURL"), "msg");
            });
        });

        function MakeURL(existingQS, newQS) {
            if (newQS) {
                if (existingQS === "") {
                    existingQS += newQS;
                }
                else {
                    existingQS += "&" + newQS;
                }
            }

            return existingQS;
        }

        function MakeCountryQueryString() {
            var country = new Array();
            $('#selectCountry_chosen').find('.chosen-choices').each(function () {
                $(this).find('li.search-choice').each(function () {
                    var current = $(this);
                    var selectedCountry = $(current).find('span').text();
                    var countryId = GetCountryId(selectedCountry);
                    country.push(countryId);
                });
            });

            var countryQS = "";
            if (country.length > 0) {
                countryQS = "country=" + country.join();
            }
            return countryQS;
        }

        function MakeClusterQueryString() {
            var cluster = new Array();
            $('#selectCluster_chosen').find('.chosen-choices').each(function () {
                $(this).find('li.search-choice').each(function () {
                    var current = $(this);
                    var selectedCluster = $(current).find('span').text();
                    var clusterId = GetClusterId(selectedCluster);
                    cluster.push(clusterId);
                });
            });

            var clusterQS = "";
            if (cluster.length > 0) {
                clusterQS = "cluster=" + cluster.join();
            }
            return clusterQS;
        }

        function MakeSingleValueQueryString(elemId, elemName) {
            var selectedVal = $(elemId).val();

            if (selectedVal != 'undefined' && selectedVal !== "") {
                return elemName + "=" + selectedVal;
            }
            return "";
        }

        function MakeSingleValueQueryStringDDLServer(elemId, elemName) {
            var selectedVal = $(elemId).val();
            if (selectedVal !== 'undefined' && selectedVal !== "" && selectedVal !== "0") {
                return elemName + "=" + selectedVal;
            }
            return "";
        }

        function MakeCheckedValueQueryString(elemId, elemName) {
            var selectedVal = $(elemId).is(':checked');

            if (selectedVal == true) {
                return elemName + "=1";
            }
            return "";
        }

        function GetCountryId(country) {
            var id = 0;
            switch (country) {
                case "BFA":
                    id = 2;
                    break;
                case "CMR":
                    id = 3;
                    break;
                case "TCD":
                    id = 4;
                    break;
                case "GMB":
                    id = 5;
                    break;
                case "MLI":
                    id = 6;
                    break;
                case "MRT":
                    id = 7;
                    break;
                case "NER":
                    id = 8;
                    break;
                case "NGA":
                    id = 9;
                    break;
                case "SEN":
                    id = 10;
                    break;
                case "SAH Reg":
                    id = 567;
                    break;
            }

            return id;
        }

        function GetClusterId(cluster) {
            var id = 0;
            switch (cluster) {
                case "EDU":
                    id = 1;
                    break;
                case "Emg Shelter":
                    id = 2;
                    break;
                case "Health":
                    id = 3;
                    break;
                case "NUT":
                    id = 4;
                    break;
                case "PROT":
                    id = 5;
                    break;
                case "ER":
                    id = 6;
                    break;
                case "COOR":
                    id = 7;
                    break;
                case "FS":
                    id = 8;
                    break;
                case "Logistics":
                    id = 9;
                    break;
                case "WASH":
                    id = 10;
                    break;
                case "Emg Tel":
                    id = 11;
                    break;
                case "MS Ref":
                    id = 12;
                    break;
                case "CCCM":
                    id = 13;
                    break;
            }

            return id;
        }


        function copyToClipboardMsg(elem, msgElem) {
            var succeed = copyToClipboard(elem);
            var msg;
            if (!succeed) {
                msg = "Copy not supported or blocked. Select text and press Ctrl+c to copy."
            } else {
                msg = "Text copied to the clipboard."
            }
            if (typeof msgElem === "string") {
                msgElem = document.getElementById(msgElem);
            }
            msgElem.innerHTML = msg;
            setTimeout(function () {
                msgElem.innerHTML = "";
            }, 2000);
        }

        function copyToClipboard(elem) {
            // create hidden text element, if it doesn't already exist
            var targetId = "_hiddenCopyText_";
            var isInput = elem.tagName === "INPUT" || elem.tagName === "TEXTAREA";
            var origSelectionStart, origSelectionEnd;
            if (isInput) {
                // can just use the original source element for the selection and copy
                target = elem;
                origSelectionStart = elem.selectionStart;
                origSelectionEnd = elem.selectionEnd;
            } else {
                // must use a temporary form element for the selection and copy
                target = document.getElementById(targetId);
                if (!target) {
                    var target = document.createElement("textarea");
                    target.style.position = "absolute";
                    target.style.left = "-9999px";
                    target.style.top = "0";
                    target.id = targetId;
                    document.body.appendChild(target);
                }
                target.textContent = elem.textContent;
            }
            // select the content
            var currentFocus = document.activeElement;
            target.focus();
            target.setSelectionRange(0, target.value.length);

            // copy the selection
            var succeed;
            try {
                succeed = document.execCommand("copy");
            } catch (e) {
                succeed = false;
            }
            // restore original focus
            if (currentFocus && typeof currentFocus.focus === "function") {
                currentFocus.focus();
            }

            if (isInput) {
                // restore prior selection
                elem.setSelectionRange(origSelectionStart, origSelectionEnd);
            } else {
                // clear temporary content
                target.textContent = "";
            }
            return succeed;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg"></div>
        <table style="margin: 0 auto; width: 100%;">
            <tr>
                <td>
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h3>Use Following Fields To Filter Your Data:
                        </h3>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <table border="0" style="margin: 0 auto; width: 100%;">
                                            <tr>
                                                <td>
                                                    <label>Country:</label>
                                                </td>
                                                <td>
                                                    <select multiple=""
                                                        class="wdt chosen-select tag-input-style" id="selectCountry" data-placeholder="...">
                                                        <option value="2">BFA</option>
                                                        <option value="3">CMR</option>
                                                        <option value="4">TCD</option>
                                                        <option value="5">GMB</option>
                                                        <option value="6">MLI</option>
                                                        <option value="7">MRT</option>
                                                        <option value="8">NER</option>
                                                        <option value="9">NGA</option>
                                                        <option value="10">SEN</option>
                                                        <option value="567">SAH Reg</option>                                         
                                                    </select>
                                                </td>
                                                <td>
                                                    <label>Cluster:</label>
                                                </td>
                                                <td>
                                                    <select multiple="" class="wdt chosen-select tag-input-style" id="selectCluster" data-placeholder="...">
                                                        <option value="1">EDU</option>
                                                        <option value="2">Emg Shelter</option>
                                                        <option value="3">Health</option>
                                                        <option value="4">NUT</option>
                                                        <option value="5">PROT</option>
                                                        <option value="6">ER</option>
                                                        <option value="7">COOR</option>
                                                        <option value="8">FS</option>
                                                        <option value="9">Logistics</option>
                                                        <option value="10">WASH</option>
                                                        <option value="11">Emg Tel</option>
                                                        <option value="12">MS Ref</option>
                                                        <option value="13">CCCM</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <label>Year:</label>
                                                </td>
                                                <td>
                                                    <select id="selectYear" class="wdt">
                                                        <option value="2017">2017</option>
                                                        <option value="2016">2016</option>
                                                        <option value="2015">2015</option>
                                                    </select>
                                                </td>
                                                 <td>
                                                    <label>Lang:</label>
                                                </td>
                                                <td>
                                                    <select id="selectLang" class="wdt">
                                                        <option value="fr">French</option>
                                                        <option value="en">English</option>
                                                    </select>  
                                                    </td>  
                                            </tr>                                            
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <hr />
        <table border="0" style="margin: 0 auto; width: 100%;">
            <tr>
                <td>
                    <input type="hidden" id="hdURL" value="http://localhost:39770/api/v2/framework/indicatorsapi.ashx?" />
                    <input type="text" id="txtURL" value="http://localhost:39770/api/v2/framework/indicatorsapi.ashx?" class="width-100" readonly="readonly" />
                </td>
                <td>
                    <button type="button" id="btnURL" class="btn btn-small btn-sm">Copy Link</button>
                    <span class='orshelpicon' data-rel='popover' data-placement='bottom' data-original-title='API link' data-content='Click on Copy-Link button and use this link to import data in excel etc.'>?</span>

                </td>

            </tr>
            <tr>
                <td><span id="msg"></span></td>
            </tr>
        </table>
    </div>

    <script src="../../../assets/js/chosen.jquery.min.js"></script>

    <script type="text/javascript">
        jQuery(function ($) {
            $(".chosen-select").chosen();
        });
    </script>

</asp:Content>
