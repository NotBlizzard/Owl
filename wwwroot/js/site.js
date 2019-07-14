// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => {
    //const showdown_ = new showdown.Converter();
    let owl = new Vue({
        el: "#markdown",
        data: {
            markdownText: "",
            showdown: new showdown.Converter(),
        },
        methods: {
            updateMarkdown: (data) => {
                $("#mark").html(data);
            }
        }
    })

    $("i#delete-comment").click((e) => {
        let confirmDelete = confirm("Are you sure?");
        if (confirmDelete) {
            let id = $(e.target).attr("asp-route-id");
            let token = $("[name=__RequestVerificationToken]").val();
            $.ajax({
                url: `/Messages/Delete/${id}`,
                method: "POST",
                headers: {
                    RequestVerificationToken: token
                }
            }).done(() => {
                location.reload();
            })
        }
    })
})