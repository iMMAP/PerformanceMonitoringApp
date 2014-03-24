function showHideObj() {
    $(document).on('click', '.checkObj', function () {
        var selectedObjs = [];
        $("[id*=cblObjectives] input:checked").each(function () {
            selectedObjs.push($(this).val());
        });

        var selectedPr = [];
        $("[id*=cblPriorities] input:checked").each(function () {
            selectedPr.push($(this).val());
        });

        showAllObj();
        if (selectedObjs.length > 0 && selectedPr.length > 0) {
            hideAllObj();
            for (i = 0; i < selectedObjs.length; ++i) {
                for (j = 0; j < selectedPr.length; ++j) {
                    var objId = selectedObjs[i];
                    var prId = selectedPr[j];
                    var objpr = objId + '-' + prId;
                    showObjPriority(objpr);
                }
            }
        }
        else if (selectedObjs.length > 0) {
            hideAllObj();
            var i;
            for (i = 0; i < selectedObjs.length; ++i) {
                showObj(selectedObjs[i]);
            }
        }
        else if (selectedPr.length > 0) {
            hideAllObj();
            var i;
            for (i = 0; i < selectedPr.length; ++i) {
                showPriority(selectedPr[i]);
            }
        }
    });
}

function showHidePriority() {

    $(document).on('click', '.checkPr', function () {
        var selectedPr = [];
        $("[id*=cblPriorities] input:checked").each(function () {
            selectedPr.push($(this).val());
        });

        var selectedObjs = [];
        $("[id*=cblObjectives] input:checked").each(function () {
            selectedObjs.push($(this).val());
        });

        showAllObj();
        if (selectedObjs.length > 0 && selectedPr.length > 0) {
            hideAllObj();
            for (i = 0; i < selectedObjs.length; ++i) {
                for (j = 0; j < selectedPr.length; ++j) {
                    var objId = selectedObjs[i];
                    var prId = selectedPr[j];
                    var objpr = objId + '-' + prId;
                    showObjPriority(objpr);
                }
            }
        }
        else if (selectedObjs.length > 0) {
            hideAllObj();
            var i;
            for (i = 0; i < selectedObjs.length; ++i) {
                showObj(selectedObjs[i]);
            }
        }
        else if (selectedPr.length > 0) {
            hideAllObj();
            var i;
            for (i = 0; i < selectedPr.length; ++i) {
                showPriority(selectedPr[i]);
            }
        }
    });
}

function showAllObj() {

    $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
        $(this).parent().show();
    });
}

function hideAllObj() {

    $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
        $(this).parent().hide();
    });
}

function showObj(objId) {

    $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
        if ($(this).text() === objId || objId === '0') {
            $(this).parent().show();
        }
    });
}

function showAllPriority() {
    $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
        $(this).parent().show();
    });
}

function hideAllPriority() {
    $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
        $(this).parent().hide();
    });
}

function showPriority(priorityId) {
    $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
        if ($(this).text() === priorityId || priorityId === '0') {
            $(this).parent().show();
        }
    });
}

function showObjPriority(objPrId) {
    $('.istrow, .altcolor').find('td:nth-child(3)').each(function (i) {
        if ($(this).text() === objPrId || objPrId === '0') {
            $(this).parent().show();
        }
    });
}

function showProject(pId) {
    $('.istrow, .altcolor').find('td:nth-child(4)').each(function (i) {
        if ($(this).text() === pId || pId === '0') {
            $(this).parent().show();
        }
    });
}

function showObjPriorityAndProject(objPrPId) {
    $('.istrow, .altcolor').find('td:nth-child(5)').each(function (i) {
        if ($(this).text() === objPrPId || objPrPId === '0') {
            $(this).parent().show();
        }
    });
}

function showObjAndProject(objPId) {

    $('.istrow, .altcolor').find('td:nth-child(6)').each(function (i) {
        if ($(this).text() === objPId || objPId === '0') {
            $(this).parent().show();
        }
    });
}

function showPriorityAndProject(prPId) {
    $('.istrow, .altcolor').find('td:nth-child(7)').each(function (i) {
        if ($(this).text() === prPId || prPId === '0') {
            $(this).parent().show();
        }
    });
}