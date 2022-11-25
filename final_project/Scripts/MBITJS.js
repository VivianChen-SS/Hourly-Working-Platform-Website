
function showHideRequire(character) {

    switch (character) {

        case 'I':
            document.getElementById("Req_E").checked = false;
            document.getElementById("label_I").style.display = "block";
            document.getElementById("label_E").style.display = "none";
            break;
        case 'E':
            document.getElementById("Req_I").checked = false;
            document.getElementById("label_I").style.display = "none";
            document.getElementById("label_E").style.display = "block";
            break;
        case 'S':
            document.getElementById("Req_N").checked = false;
            document.getElementById("label_S").style.display = "block";
            document.getElementById("label_N").style.display = "none";
            break;
        case 'N':
            document.getElementById("Req_S").checked = false;
            document.getElementById("label_S").style.display = "none";
            document.getElementById("label_N").style.display = "block";
            break;
        case 'T':
            document.getElementById("Req_F").checked = false;
            document.getElementById("label_T").style.display = "block";
            document.getElementById("label_F").style.display = "none";
            break;
        case 'F':
            document.getElementById("Req_T").checked = false;
            document.getElementById("label_T").style.display = "none";
            document.getElementById("label_F").style.display = "block";
            break;
        case 'J':
            document.getElementById("Req_P").checked = false;
            document.getElementById("label_J").style.display = "block";
            document.getElementById("label_P").style.display = "none";
            break;
        case 'P':
            document.getElementById("Req_J").checked = false;
            document.getElementById("label_J").style.display = "none";
            document.getElementById("label_P").style.display = "block";
            break;

        case '1':
            document.getElementById("Req_I").checked = false;
            document.getElementById("Req_E").checked = false;
            document.getElementById("label_I").style.display = "none";
            document.getElementById("label_E").style.display = "none";
            break;
        case '2':
            document.getElementById("Req_S").checked = false;
            document.getElementById("Req_N").checked = false;
            document.getElementById("label_S").style.display = "none";
            document.getElementById("label_N").style.display = "none";
            break;
        case '3':
            document.getElementById("Req_T").checked = false;
            document.getElementById("Req_F").checked = false;
            document.getElementById("label_T").style.display = "none";
            document.getElementById("label_F").style.display = "none";
            break;
        case '4':
            document.getElementById("Req_J").checked = false;
            document.getElementById("Req_P").checked = false;
            document.getElementById("label_J").style.display = "none";
            document.getElementById("label_P").style.display = "none";
            break;

        default: break;
    }
}