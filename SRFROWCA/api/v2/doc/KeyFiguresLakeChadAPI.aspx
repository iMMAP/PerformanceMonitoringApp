<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KeyFiguresLakeChadAPI.aspx.cs" Inherits="SRFROWCA.api.v2.doc.KeyFiguresLakeChadAPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .wdt {
            width: 200px;
        }

        .wdt1 {
            width: 80%;
        }

        .wdt2 {
            width: 60px;
        }

        input[type="text"] {
            height: 24px;
        }
    </style>
    <link rel="stylesheet" href="../../../assets/css/chosen.css" />
    <script>
        $(function () {
            $('[data-rel=popover]').popover({ html: true });
            $("#txtFromDate").datepicker({
                dateFormat: "dd-mm-yy",
                defaultDate: Date.now(),
                onSelect: function (selected) {
                    //LoadData();
                }
            });

            $("#txtToDate").datepicker({
                dateFormat: "dd-mm-yy",
                defaultDate: Date.now(),
                onSelect: function (selected) {
                    //LoadData();
                }
            });

           
            $('#btnURL').click(function () {
                var queryString = "";

                var typeQS = MakeCountryQueryString();
                queryString = MakeURL(queryString, typeQS);

                var typeQS = MakeSubCatQueryString();
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeSingleValueQueryString("#txtFromDate", "datefrom");
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeSingleValueQueryString("#txtToDate", "dateto");
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeSingleValueQueryString("#selectIDs", "inclids");
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeCheckedValueQueryString("#cbLatest", "final");
                queryString = MakeURL(queryString, typeQS);

                typeQS = MakeSingleValueQueryString("#selectFormat", "format");
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

        function MakeSubCatQueryString() {
            var subCat = new Array();
            $('#selectedSubCat_chosen').find('.chosen-choices').each(function () {
                $(this).find('li.search-choice').each(function () {
                    var current = $(this);
                    var selectedSubCat = $(current).find('span').text();
                    var subCatId = GetSubCatId(selectedSubCat);
                    subCat.push(subCatId);
                });
            });

            var subCatQS = "";
            if (subCat.length > 0) {
                subCatQS = "subcat=" + subCat.join();
            }
            return subCatQS;
        }

        function MakeSingleValueQueryString(elemId, elemName) {
            var selectedVal = $(elemId).val();
            if (selectedVal != 'undefined' && selectedVal !== "") {
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

        function GetSubCatId(subcat) {
            var id = 0;
            switch (subcat) {
                case "Food Security - Severity level":
                    id = 6;
                    break;
                case "Health - Epidemiology":
                    id = 8;
                    break;
                case "Natural Disasters - Population affected by floods":
                    id = 13;
                    break;
                case "Natural Disasters - Population affected by Locust Infestation":
                    id = 14;
                    break;
                case "Nutrition - Malnutrition":
                    id = 5;
                    break;
                case "Population - Demographic Data":
                    id = 4;
                    break;
                case "Population - Inaccessible Area":
                    id = 17;
                    break;
                case "Population - Protection":
                    id = 15;
                    break;
                case "Population Movement - Asylum Seeker":
                    id = 16;
                    break;
                case "Population Movement - Host Community":
                    id = 18;
                    break;
                case "Population Movement - Internally Displaced Persons (IDPs)":
                    id = 10;
                    break;
                case "Population Movement - Refugees":
                    id = 9;
                    break;
                case "Population Movement - Returnees":
                    id = 11;
                    break;
                case "Population Movement - Third Country National":
                    id = 12;
                    break;
            }

            return id;
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
                                                    <label>Date From:</label>
                                                </td>
                                                <td>
                                                    <input type="text" id="txtFromDate" class="wdt" />
                                                    <label>dd-MM-yy</label>
                                                </td>
                                                <td>
                                                    <label>Date To:</label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="text" id="txtToDate" class="wdt" />
                                                    <label>dd-MM-yy</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>Include IDs:</label></td>
                                                <td>
                                                    <select id="selectIDs" class="wdt">
                                                        <option value="no">NO</option>
                                                        <option value="yes">YES</option>
                                                    </select>
                                                    <span class='orshelpicon' data-rel='popover' data-placement='bottom' data-original-title='Extra Columns' data-content='<b>Yes:</b>Include extra columns like IndicatorId, UnitId, MonthId etc.'>?</span>
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
                                                <td>
                                                    <label>Format:</label>
                                                </td>
                                                <td>
                                                    <select id="selectFormat" style="width:60px">
                                                        <option value="xml">XML</option>
                                                        <option value="json">Json</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <input type="checkbox" id="cbLatest" />
                                                    <label>Latest Reported</label>
                                                    <span class='orshelpicon' data-rel='popover' data-placement='bottom' data-original-title='Latest Reported' data-content='If selected, it will return only the latest data reported (for any date) for each Key-Figure.'>?</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="120px">
                                                    <label>Key Figure Category:</label>
                                                </td>
                                                <td colspan="6">
                                                    <select multiple=""
                                                        class="wdt1 chosen-select tag-input-style" id="selectedSubCat" data-placeholder="...">
                                                        <option value="6">Food Security - Severity level</option>
                                                        <option value="8">Health - Epidemiology</option>
                                                        <option value="13">Natural Disasters - Population affected by floods</option>
                                                        <option value="14">Natural Disasters - Population affected by Locust Infestation</option>
                                                        <option value="5">Nutrition - Malnutrition</option>
                                                        <option value="4">Population - Demographic Data</option>
                                                        <option value="17">Population - Inaccessible Area</option>
                                                        <option value="15">Population - Protection</option>
                                                        <option value="16">Population Movement - Asylum Seeker</option>
                                                        <option value="18">Population Movement - Host Community</option>
                                                        <option value="10">Population Movement - Internally Displaced Persons (IDPs)</option>
                                                        <option value="9">Population Movement - Refugees</option>
                                                        <option value="11">Population Movement - Returnees</option>
                                                        <option value="12">Population Movement - Third Country National</option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
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
                    <input type="hidden" id="hdURL" value="http://ors.ocharowca.info/api/v2/KeyFigures/KeyFiguresLakeChad.ashx?" />
                    <input type="text" id="txtURL" value="http://ors.ocharowca.info/api/v2/KeyFigures/KeyFiguresLakeChad.ashx?" class="width-100" readonly="readonly" />
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
