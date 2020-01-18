$(document).ready(function (e) {
    //draft !!
    /**
     * @todo: 
     * -sortowanie
     * -usuwanie wielu kategorii na raz
     * -komunikaty
     * */

    //draft do przepisania póżniej na oddzielne funkcje 
    // ************************************
    //add new Category
    var newCatA = $("a#newCatA");
    var newCatTextInput = $("#catName");
    var ajaxText = $(".message");
    var table = $("table#categoryTable tbody");

    //get keyup "enter"
    newCatTextInput.keyup(function (e) {
        if (e.keyCode == 13) {
            newCatA.click();
        }

    });
    /***
     * 
     * 
     * @todo:
     * -nie działa przeładowanie tabeli przy pierwszym nowym rekordzie !
     * -dodać komunikaty i wywalić loader (spinner)
     * 
     * 
     * ****/
    // ajaxText.hide();
    //onClick 
    newCatA.click(function (e) {
        e.preventDefault;
        //show message
        ajaxText.html('<span class="alert alert-danger">Podana kategoria już istnieje</span>');
        var catName = newCatTextInput.val();
        //check length

        if (catName.length <= 3) {
            alert("Nazwa kategorii musi mieć co najmniej 3 znaki");
            return false;
        }

        ///  ajaxText.show();
        var url = "/Admin/Shop/AddCategories";
        //post
        $.post(url, { catName: catName }, function (data) {
            //if false or error 
            var response = data.trim();
            //if cat exists
            if (response == "catexists") {

                ajaxText.html('<span class="alert alert-danger">Podana kategoria już istnieje</span>');
                setTimeout(function () {
                    ajaxText.fadeOut("slow", function () {
                        ajaxText.html(' <img src="~/Content/img/2.gif" />');
                    }, 2000);
                });
                return false;
            } else {

                //set success message 
                ajaxText.html('<span class="alert alert-success">Podana kategoria została dodana </span>');

                //success - add new item to table
                // if (!$("#table#categoryTable").length) {
                //     location.reload();
                //  } else {
                newCatTextInput.val("");
                ajaxText.fadeOut("slow", function () {
                    ajaxText.html('');
                }, 2000);
                var toAppend = $("table#categoryTable tbody tr:last").clone();
                toAppend.attr("id", "id_" + data);
                console.log(toAppend);
                toAppend.find("#item_Name").val(catName);

                toAppend.find("a.delete").attr('href', "/admin/shop/DeleteCategory/" + data);
                table.append(toAppend);
                // }
            }


            //out


        });



    });


    //sortowanie tabeli kategori! 

    $('table#categoryTable tbody').sortable({
        //items
        items: "tr:not(.home)",
        placeholder: "ui-state-hightlight",
        update: function () {
            var ids = $('table#categoryTable tbody').sortable("serialize");
            //method sortable
            var url = "/Admin/Shop/ReorderCategories";
            //post
            $.post(url, ids, function (data) { });
        }
    })



    //delete items

    $("body").on("click", "a.delete", function () {

        if (!confirm("Potwierdzasz usunięcie kategori")) return false;



    });

    /******************Edycja Kategorii*******************/

    var orginalTextValue;
    //get orginalTextValue;
    $('table#categoryTable input.text-box').dblclick(function () {
        orginalTextValue = $(this).val();
        $(this).attr("readonly", false);

    })
    $('table#categoryTable input.text-box').keyup(function (e) {
        if (e.keyCode == 13) {
            //e blur
            $(this).blur();

        }

    });
    //.aDivText
    $('table#categoryTable input.text-box').blur(function (e) {
        var $this = $(this);
        var aDivText = $this.parent().find('.aDivText');
        var newCatName = $this.val();
        //substring of id_
        var ids = $this.parent().parent().attr("id").substr('3');
        var url = "/Admin/Shop/RenameCategory";
        if (newCatName.length < 2) {
            //
            alert('Nazwa kategorii jest za krótka');
            $this.attr("readonly", true);
            return false;
        }

        $.post(url, { newCatName: newCatName, id: ids }, function (data) {
            var response = data.trim();
            if (response == "catexists") {
                alert('Nazwa kategorii już istnieje');

                $this.val(orginalTextValue);
                aDivText.html("<span class='alert alert-danger'>Nazwa kategorii już istnieje</span>");
                $this.attr("readonly", true);
                return false;
            } else {
                aDivText.html("<span class='alert alert-success'>Kategoria została zmieniona</span>");
                //set fadeOut for alert message
                setTimeout(function () {
                    aDivText.fadeOut("fast", function () {
                        $(this).html("");
                    });
                }, 2000);

            }
            //set if done
        }).done(function () {
            $this.attr("readonly", true);
        });


    });
    /********************Produkty***********************/


   //dodawanie zdjęć
    $('#imageUpload').change(function () {
        //preview image
        readUrl(this);
    })

    //filtrowanie produktów według kategorii 
    //przekazywanie selected Category

    $('select[name=SelectedCategory]').change(function () {
       
        var url = $(this).val();
        if (url) {
            window.location = '/admin/shop/products?catId=' + url;
        }
        return false;
    });


});

//czytaj plik do podglądu 
function readUrl(input) {
    //Podgld pliku
    if (input.files && input.files[0]) {
        //czytaj plik 
        var reader = new FileReader();
        //
        reader.onload = function (e) {
            //
            $('img#imagePrv').attr("src", e.target.result).width(200).height(200);

        }
        reader.readAsDataURL(input.files[0]);

    } 
        return false;
   
}