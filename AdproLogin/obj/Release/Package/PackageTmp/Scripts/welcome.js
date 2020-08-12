$(document).ready(function () {
    window.history.forward();
    var userid = GetSessionValue(appRoot, 'UserId');
    if (userid == null || userid == '' || userid == '0') {
        UserLogout();
    }
    $(document).keypress(function (e) {
        if (e.which == 13) {
            if ($('#lnkEbooking').closest('li').hasClass("disable"))
                return false;
            else
                document.getElementById("lnkEbooking").click();

            // new add by sarvesh for screening process.
            //if ($('#lnkScreening').closest('li').hasClass("disable"))
            //    return false;
            //else
            //    document.getElementById("lnkScreening").click();
        }
        if (e.which == 0) {
            $('#lnkEbooking').css('border', '');
            $('#lnkScreening').css('border', '');
        }
    });
    checkPageRight();
    ReadBuildDetailFile();
});
function checkPageRight() {
    var param = {};
    param.xmlData = '<parameter><userid>' + $('#hdnUserId').val() + '</userid><filtername>getright</filtername><centerid>' + $('#hdnCenterId').val() + '</centerid></parameter>';
    url = appRoot + 'home/GetRights';
    var result = getresult(url, param);
    for (var i = 0; i < result.length; i++) {
        if (result[4][1] == '0') {
            $('#lnkEbooking,#lnkScreening,#lnkDashboard,#lnkEReport,#lnkeMaterial,#lnkUpdateBoxAddress,#lnkEtrack').attr('href', '#');
            $('#lnkEbooking,#lnkScreening,#lnkDashboard,#lnkEReport,#lnkeMaterial,#lnkUpdateBoxAddress,#lnkEtrack').closest('li').addClass("disable");
            ShowMsg('User don`t have right for this center.', 0);
            return false;
        }
        switch (result[i][0]) {
            case "AllowBooking":
                if (result[i][1] == '0') {
				    $('#lnkEbooking').attr('href', '#').removeAttr('target');;
                    $('#lnkEbooking').closest('li').addClass("disable");
                    $('#lnkEbooking').css('border', '');
                }
                break;
				case "AllowDiscountApprovalSystam":
                if (result[i][1] == '0') {
                    $('#lnkScreening').attr('href', '#').removeAttr('target');
                    $('#lnkScreening').closest('li').addClass("disable");
                    $('#lnkScreening').css('border', '');
                }
                break;
            case "DashboardRight":
                if (result[i][1] == '0') {
                    $('#lnkDashboard').attr('href', '#').removeAttr('target');;
                    $('#lnkDashboard').closest('li').addClass("disable");
                }
                break;
            case "EReportRight":
                if (result[i][1] == '0') {
                    $('#lnkEReport').attr('href', '#').removeAttr('target');;
                    $('#lnkEReport').closest('li').addClass("disable");
                }
                break;
            case "eMaterialRight":
                if (result[i][1] == '0') {
                    $('#lnkeMaterial').attr('href', '#').removeAttr('target');;
                    $('#lnkeMaterial').closest('li').addClass("disable");
                }
                break;
            case "UpdateBoxAddress":
                if (result[i][1] == '0') {
                    $('#lnkUpdateBoxAddress').attr('href', '#').removeAttr('target');;
                    $('#lnkUpdateBoxAddress').closest('li').addClass("disable");
                }
                break;
            case "TrackingRights":
                if (result[i][1] == '0') {
                    $('#lnkEtrack').attr('href', '#').removeAttr('target');;
                    $('#lnkEtrack').closest('li').addClass("disable");
                }
                break;
            case "TypeSettingRights":
                if (result[i][1] == '0') {
                    $('#lnkeTypeSetting').attr('href', '#').removeAttr('target');;
                    $('#lnkeTypeSetting').closest('li').addClass("disable");
                }
                break;
            default:
                break;
        }

    }
}

function OpenBuildToggle() {
    if ($('#toggleNavigationLeft').hasClass('open')) {
        $('#toggleNavigationLeft').removeClass('open');
    }
    else {
        $('#toggleNavigationLeft').addClass('open');
    }
}

function ReadBuildDetailFile() {
    var url = appRoot + "Home/ReadBuildDetailFile";
    var param = {};
    var result = getresult(url, param);
    $("#detaildiv").html(result);
}