function getresult(url, param) {
    var result;
    $.ajax({
        url: url,
        async: false,
        type: 'POST',
        data: {
            format: 'json'
        },
        data: param,
        success: function (data) {
            result = data;
        },
        error: function (e) {
            alert('Some technical error');
        }
    });
    return result;
}

//var myinterval = setInterval(GetSessionValue, 3000);

function exportTableToCSV($table, filename) {
    var $rows = $table.find('tr:has(td),tr:has(th)'),
        tmpColDelim = String.fromCharCode(11),
        tmpRowDelim = String.fromCharCode(0),
        colDelim = '","',
        rowDelim = '"\r\n"',
        csv = '"' + $rows.map(function (i, row) {
            var $row = $(row),
                $cols = $row.find('td ,th');
            return $cols.map(function (j, col) {
                var $col = $(col),
                    text = $col.text();
                return text.replace(/"/g, '""'); // escape double quotes
            }).get().join(tmpColDelim);

        }).get().join(tmpRowDelim)
            .split(tmpRowDelim).join(rowDelim)
            .split(tmpColDelim).join(colDelim) + '"',
        csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);
    $(this)
        .attr({
            'download': filename,
            'href': csvData,
            'target': '_blank'
        });
}
function GetSessionValue(appRoot,key) {
    pageUrl = appRoot + "Account/GetSessionValue";
    var param = {};
    param.xmlParameter = key;
    return getresult(pageUrl, param);
}

function UserLogout() {
    pageUrl = appRoot + "Account/Logout";
    var param = {};
    getresult(pageUrl, param);
    window.location.href = appRoot;// + "Account/Login";
}
function ShowMsg(Msg, type, isAutoClose) { 
    //$('<div></div>').html(msg).dialog({
    //    show: 'blind',
    //    hide: 'fold',
    //    closeOnEscape: true,
    //    autoOpen: true,
    //    width: 380,
    //    modal: true,
    //    maxHeight:500,
    //    //position: { my: "middle top", at: "middle top", of: window },
    //    buttons: {
    //        "Ok": function () {
    //            $(this).dialog('close');
    //        }
    //    }
    //});
    
        //0-error,1-success,2-waring,3-info
        type = type == null ? 1 : type;
        isAutoClose = isAutoClose == null ? true : isAutoClose;
        switch (type) {
            case 0:
                msgString = '<div class="alert alert-danger" style="margin:0;">' + '<strong>Error! </strong>' + Msg + '</a>' + '</div>';
                break;
            case 1:
                msgString = '<div class="alert alert-success" style="margin:0;">' + '<strong>Success! </strong>' + Msg + '</a>' + '</div>';
                break;
            case 2:
                msgString = '<div class="alert alert-warning" style="margin:0;">' + '<strong>Warning! </strong>' + Msg + '</a>' + '</div>';
                break;
            case 3:
                msgString = '<div class="alert alert-info" style="margin:0;">' + '<strong>Info! </strong>' + Msg + '</a>' + '</div>';
                break;
            default:
                msgString = '<div class="alert alert-info" style="margin:0;">' + '<strong>Success! </strong>' + Msg + '</a>' + '</div>';
        }
        if (isAutoClose == false)
            msgString += "";
        var id = $('#showMSG');
        $(id).html(msgString);
        //  $(id).fadeIn(1000);
        //$(id).fadeTo("slow", 0.8);
        $(id).fadeIn(1000);
        $(id).delay(1000).fadeOut(1000);
        $(id).hover(function () {
            $(id).bind("fadeOut");;
        }, function () {
            $(id).bind("fadeOut");;
        });
        // $('#mask2').fadeIn(1000);
        //$('#mask2').fadeTo("slow", 0.8);
        //$(id).fadeIn(1000);
        //$('#mask2, .window').delay(1000).fadeOut(1000);

    }


function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}


function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}


function ChagePasswordPopup() {
    if ($("#loginpassword").val().trim() != "" && $("#loginusername").val().trim() != "") {
        if (CheckOldPassword()) {
            $("#ChangePasswordPopUp").dialog('open');
            $("#txtnewpassword").val('');
            $("#txtcnfrmpassword").val('');
            $("#txtnewpassword").focus();
        }
        else {
            return false;
        }
    }
}

function CheckOldPassword() {
    var param = {};
    url = appRoot + 'Account/CheckOldPassword';
    param.username = $('#loginusername').val();
    param.oldpassword = $("#loginpassword").val();
    var result = getresult(url, param);
    if (result.length > 0) {
        if (result[0].value == "1") {
            return true;
        }
        else {
            alert(result[0].key);
            return false;
        }
    }
    else {
        alert("Error in checking password");
        return false;
    }
}

function ChangePassword() {
    var loginpassword = $("#loginpassword").val();
    var newpassword = $("#txtnewpassword").val();
    var cnfpassword = $("#txtcnfrmpassword").val();
    if (newpassword != cnfpassword) {
        alert("Password mismatch.");
        $("#txtcnfrmpassword").focus();
        return false;
    }
    if (newpassword.length <= 7) {
        alert("Password Length Should be greater than 7.");
        $("#txtnewpassword").focus();
        return false;
    }
    var regularExpression = new RegExp();
    regularExpression = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/g;
    if (!regularExpression.test(newpassword)) {
        alert("Password should be a combination of alphanumeric and special characters.");
        $("#txtnewpassword").focus();
        return false;
    }
    regularExpression = /[a-z]/g;
    if (!regularExpression.test(newpassword)) {
        alert("Password should be a combination of alphanumeric and special characters.");
        $("#txtnewpassword").focus();
        return false;
    }
    regularExpression = /[0-9]/g;
    if (!regularExpression.test(newpassword)) {
        alert("Password should be a combination of alphanumeric and special characters.");
        $("#txtnewpassword").focus();
        return false;
    }
    var param = {};
    url = appRoot + 'Account/ChangePassword';
    param.username = $('#loginusername').val();
    param.oldpassword = loginpassword;
    param.newpassword = newpassword;
    var result = getresult(url, param);
    if (result.length > 0) {
        if (result[0].value == "1") {
            alert(result[0].key);
            $("#ChangePasswordPopUp").dialog('close');
        }
        else {
            alert(result[0].key);
            return false;
        }
    }
    else {
        alert("Error in password changing");
        $("#ChangePasswordPopUp").dialog('close');
    }
}

function ShowPassword() {
    var x = document.getElementById("txtnewpassword");
    var y = document.getElementById("txtcnfrmpassword");
    if (x.type === "password") {
        x.type = "text";
        y.type = "text";
    } else {
        x.type = "password";
        y.type = "password";
    }
}
