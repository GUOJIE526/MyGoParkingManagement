﻿//JQuery dataTable
$('#CarTable').dataTable({
    ajax: {
        url: '/MyCar/Car/IndexJson',
        type: 'GET',
        dataSrc: "",  // 如果你的 JSON 根是數組，否則用 dataSrc: "data"
        datatype: 'Json'
    },
    pagingType: "full_numbers",
    fixedHeader: {
        header: true
    },
    language: {
        url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json',
    },
    columns: [
        { data: 'userId' },
        { data: 'licensePlate' },
        { data:'registerDate'},
        { data:'isActive'},
        {
            data: 'carId',
            render: function (data) {
                return `
                <div>
                    <a data-id="${data}" class="EditBtn btn btn-info" data-bs-toggle="modal" data-bs-target="#EditModal">
                        <i class="fa-solid fa-pen-to-square fa-beat"></i>
                    </a>
                </div>
            `;
            }
        }
    ]
});
//動態加載Create內容
$('.CreateBtn').on('click', async function () {
    let response = await fetch('/MyCar/Car/CreatePartial');
    let partialView = await response.text();  // 讀取返回的 HTML 內容
    $('.CreateCar').html(partialView);  // 動態插入到指定區域

    // 綁定表單提交事件
    $(document).off('submit', '#CreateForm').on('submit', '#CreateForm', async function (e) {
        e.preventDefault(); // 防止默認提交行為

        // 將表單序列化
        let formData = new FormData(this);  // 使用 FormData 獲取表單數據

        // 使用 fetch 發送 POST 請求
        let fetchResponse = await fetch($(this).attr('action'), {
            method: 'POST',
            body: formData,
        });

        if (fetchResponse.ok) {
            let result = await fetchResponse.json(); // 解析為 JSON

            if (result.success) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "新增項目成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#CreateModal').modal('hide');  // 隱藏模態框
                $('#CarTable').DataTable().ajax.reload(null, false);  // 刷新表格資料
            } else {
                // 處理失敗，顯示錯誤
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: result.errors ? result.errors.join(", ") : result.message,
                });
            }
        } else {
            // 處理 AJAX 請求錯誤
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "請求發生錯誤，請稍後再試",
            });
        }
    });

    // 顯示模態框
    $('#CreateModal').modal('show');
});

//動態加載Edit內容
$(document).on('click', '.EditBtn', async function () {
    var id = $(this).data('id'); // 獲取該筆資料的 ID
    let response = await fetch(`/MyCar/Car/EditPartial/${id}`);
    let partialView = await response.text();  // 讀取返回的 HTML 內容
    $('.EditCar').html(partialView);  // 動態插入到指定區域
    console.log(id);
    // 綁定表單提交事件
    $(document).off('submit', '#EditForm').on('submit', '#EditForm', async function (e) {
        e.preventDefault(); // 防止默認提交行為

        // 將表單序列化
        let formData = new FormData(this);  // 使用 FormData 獲取表單數據

        // 使用 fetch 發送 POST 請求
        console.log($(this).attr('action'));

        let fetchResponse = await fetch($(this).attr('action'), {
            method: 'POST',
            body: formData,
        });

        if (fetchResponse.ok) {
            let result = await fetchResponse.json(); // 解析為 JSON

            if (result.success) {
                Swal.fire({
                    icon: "success",
                    title: "編輯成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#EditModal').modal('hide');  // 隱藏模態框
                $('#CarTable').DataTable().ajax.reload(null, false);  // 刷新表格資料
            } else {
                // 處理失敗，顯示錯誤
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: result.errors ? result.errors.join(", ") : result.message,
                });
            }
        } else {
            // 處理 AJAX 請求錯誤
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "請求發生錯誤，請稍後再試",
            });
        }
    });

    // 顯示模態框
    $('#EditModal').modal('show');
});
