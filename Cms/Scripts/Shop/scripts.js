$(document).ready(function (e) {
    //draft !!
    

    //draft do przepisania póżniej na oddzielne funkcje 
   // ************************************
    //add new Category
    var newCatA = $("a#newCatA");
    var newCatTextInput = $("#catName");
    var ajaxText = $("span.ajax-text");
    var table = $("table#categoryTable tbody");

    //get keyup "enter"
    newCatTextInput.keyup(function (e) {
        if (e.keyCode == 13) {
            newCatA.click();
        }

    });

    //onClick 
    newCatA.click(function (e) {
        e.preventDefault;
        var catName = newCatTextInput.val();
        //check length

        if (catName.length <= 3) {
            alert("Nazwa kategorii musi mieć co najmniej 3 znaki");
            return false;
        }

        ajaxText.show();
        var url = "/admin/shop/AddCategories";
        //post
        $.post(url, { catName: catName }, function (data) {
            //if false or error 
            var response = data.trim;

            if (response = "catexists") {
                ajaxText.html('<span class="alert alert-danger">Podana kategoria już istnieje</span>');
                setTimeout(function () {
                    ajaxText.fadeOut("slow", function () {
                        ajaxText.html(' <img src="~/Content/img/2.gif" />');
                    },2000);
                }); 
                return false;
            } else {
                //set success message 
                ajaxText.html('<span class="alert alert-success">Podana kategoria została dodana </span>');
                //success - add new item to table
                if (!$("#table#categoryTable").length) {
                    loaction.reload();
                } else {
                    newCatTextInput.val("");

                    var toAppend = $("table#categoryTable tbody tr:last").clone();
                    toAppend.attr("id", "id_" + data);
                    toAppend.find("#item_Name").val(catName);
                    toAppend.find("a.delete").attr('href', "/admin/shop/DeleteCategory/" + data);
                    table.append(toAppend);

                }
                //out
            }

        });



    });



    /***************'
     * 
     * $('.a-text').hide();
    $('#newCatName').keyup(function (e) {
        if (e.keyCode == 13) {
            addNewCategory();
        }
    });

    $('#newCatA').click(function (e) { e.preventDefault(); addNewCategory(); })

     * */

});


///add new category to shop
/**
 * 
 * @todo add alert message
 *              
 * 
 * 
**/
function addNewCategory() {

    var spinner = $('.a-text');

    var tableCatgories = "table#categoryTable tbody";

    var newCategoryValue = $('#catName').val();
    console.log(newCategoryValue);
    //prevent to post with less 2 
    if (newCategoryValue.length < 2) {
        return false;
    }
    spinner.show();
    tableRefresh(tableCatgories);
    $.post("/Admin/Shop/AddCategories", { catName: newCategoryValue } , function (data) {
        spinner.hide();

        var response = data.trim();
        console.log(data);
       
        if (response == 'catexists') {
            alert('Kategoria już istnieje!');
            return false;
        } else {
            //tableCategories refresh
            tableRefresh(tableCatgories);
            var toAdd = $(tableCatgories + 'tr:last').clone();
            
            console.log('dodaj clona');
            //add new row with new id and add new add category if 200
          //  toAdd.attr("id", "id_" + data);
         //   toAdd.find("#item_Name").val(newCategoryValue);
         //   toAdd.find("a.delete").attr("href", "/admin/shop/DeleteCategory/" + data);
            $(tableCatgories).append(toAdd);
         //   $(tableCatgories).sortable("refresh");
        }
      
    });


}

//table reload();'
function tableRefresh(tableName) {

    /**
     * @todo Update table refresh to async list
     * **/
    if (!$(tableName).length) {

        setTimeout(function () {
            $(tableName).fadeOut("slow", $(tableName).html("<h4>Odświeżam</h4>"), 2000);
        });

    } 


}

function sortable() { }