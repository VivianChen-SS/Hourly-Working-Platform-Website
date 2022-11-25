function hideCompanyForm(checkbox) {
    var form = document.getElementById("companyFormGroup");

    //【做法一】按下去就不能取消：
        form.style.display = "none";
    //[錯誤嘗試]用disable會變成「not checked」：document.getElementById(checkbox).disabled = true;
        document.getElementById(checkbox).checked = true 
    document.getElementById("CompanyName").value = "aaaaaaaa";
    document.getElementById("CompanyTel").value = "091345678";
    document.getElementById("COMPANY_Address").value = "aaaaaaaa";
    document.getElementById("COMPANY_Industry").value = "aaaaaaaa";
    


    ////【做法二】按下去還能夠取消：
    //if (document.getElementById(checkbox).checked) {
    //form.style.display = "none";
    //}
    //else {
    //    form.style.display = "block";
    //}
}


//如果js code修改了，你也肯定自己的code沒有錯，程式卻依照修改前舊版的code執行，那就請把這個js檔刪掉，重新開一個js file，複製貼上過去，因為vs當掉了
