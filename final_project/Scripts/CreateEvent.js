
//【提醒】如果想在同一row顯示 shash 和 timeEnd，
//請把cshtml檔案裡，凡style帶有display:table或是table-cell的相關div，刪掉table、table-cell屬性，然後把以下程式碼換成block的那幾行(同時table-cell也要住解掉，否則會被覆蓋)
function showTimeEnd(checkbox) {
    if (document.getElementById(checkbox).checked) {
        //document.getElementById("timeEnd").style.display = "block";
        //document.getElementById("slash").style.display = "block";
        document.getElementById("timeEnd").style.display = "table-cell";
        document.getElementById("slash").style.display = "table-cell";
        document.getElementById("hideTimeEndCheckbox").checked = false;
    }
    else {
        document.getElementById("timeEnd").style.display = "none";
        document.getElementById("slash").style.display = "none";
        document.getElementById("hideTimeEndCheckbox").checked = true;
    }
}

function hideTimeEnd(checkbox) {
    if (document.getElementById(checkbox).checked) {
        document.getElementById("timeEnd").style.display = "none";
        document.getElementById("slash").style.display = "none";
        document.getElementById("showTimeEndCheckbox").checked = false;
    }
    else {
        //document.getElementById("timeEnd").style.display = "block";
        //document.getElementById("slash").style.display = "block";
        document.getElementById("timeEnd").style.display = "table-cell";
        document.getElementById("slash").style.display = "table-cell";
        document.getElementById("showTimeEndCheckbox").checked = true;
    }
}

function wageByHour(checkbox) {
    if (document.getElementById(checkbox).checked) {
        document.getElementById("byOthers").style.display = "none";
        document.getElementById("byHour").style.display = "block";
        document.getElementById("byOthersCheckbox").checked = false;
    }
    else {
        document.getElementById("byOthers").style.display = "block";
        document.getElementById("byHour").style.display = "none";
        document.getElementById("byOthersCheckbox").checked = true;
    }
}
function wageByOthers(checkbox) {
    if (document.getElementById(checkbox).checked) {
        document.getElementById("byOthers").style.display = "block";
        document.getElementById("byHour").style.display = "none";
        document.getElementById("byHourCheckbox").checked = false;
    }
    else {
        document.getElementById("byOthers").style.display = "none";
        document.getElementById("byHour").style.display = "block";
        document.getElementById("byHourCheckbox").checked = true;
    }
}

function onloadDisplayorNot(checkbox) {
    if (document.getElementById(checkbox).checked) {
        document.getElementById("byOthers").style.display = "none";
        document.getElementById("byHour").style.display = "block";
        document.getElementById("byOthersCheckbox").checked = false;
    }
    else {
        document.getElementById("byOthers").style.display = "block";
        document.getElementById("byHour").style.display = "none";
        document.getElementById("byHourCheckbox").checked = false;
    }
}

function showMap(map) {
    //var address = document.getElementById("Address").value;
    var address = $('#Address').val();
    document.getElementById("testing").innerText = address;
    if (address != null) {
        var map = document.getElementById(map);
        map.style.display = "table-cell";
        map.src = "https://www.google.com/maps/embed/v1/place?q=" + address + "&key=AIzaSyA-BLW-5csaWqjE4P0yLZdieY7DJ_0p19A";
    }
   

}