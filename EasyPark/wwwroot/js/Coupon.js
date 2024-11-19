$(() => {
    const datatableurl = ('Coupon/GetCouponAllData')

    //modal視窗載入部分
    $('#mymodal').on('show.bs.modal', function (event) {
        
        var button = $(event.relatedTarget); // 觸發modal的按鈕
        var id = button.data('id'); // 從按鈕中獲取資料ID
        var action = button.data('action'); // 從按鈕中獲取動作 (details/edit/create等)
        var title = button.data('title')

        // 清空 modal 的內容
        var modal = $(this);
        modal.find('.modal-body').empty(); // 清除現有內容

        $('#exampleModalLabel').text(title)
        var modal = $(this);
        // 根據不同的動作，載入對應的 Partial View
        var url = 'Coupon/' + action + '/' + id;
        modal.find('.modal-body').load(url); // 動態加載Partial View
    });

    $(document).on('submit', '#createForm', async function (e) {
        e.preventDefault(); // 防止表單的默認提交

        const form = this;
        const formData = new FormData(form);

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: formData
            });

            // 確保回應的格式是 JSON
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const result = await response.json();

            // 假設伺服器返回一個狀態代碼來表示成功
            if (result.success) {
                Swal.fire({
                    title: '成功!',
                    text: '資料已成功更新!',
                    icon: 'success',
                    confirmButtonText: 'OK'
                });

                // 關閉 Modal 並且刷新 DataTable 或某個部分
                $('#mymodal').modal('hide');
                $('#myDataTableId').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    title: '失敗!',
                    text: result.message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        } catch (error) {
            console.error(error); // 輸出錯誤信息
            Swal.fire('失敗', '資料更新失敗', 'error');
        }
    });


    //editform
    $(document).on('submit', '#editForm', async function (event) {
        event.preventDefault(); // 防止表單的默認提交

        var form = $(this);
        var actionUrl = form.attr('action'); // 獲取表單的提交 URL
        var formElement = form[0];
        var formData = new FormData(formElement);
        try {
            // 使用 fetch 進行 POST 請求
            const response = await fetch(actionUrl, {
                method: 'POST',
                body: formData,
            });

            // 檢查響應是否成功
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json(); // 解析 JSON 響應

            if (data.success) {
                Swal.fire({
                    title: '成功!',
                    text: '資料已成功更新!',
                    icon: 'success',
                    confirmButtonText: 'OK'
                });

                // 關閉 Modal 並且刷新 DataTable 或某個部分
                $('#mymodal').modal('hide');
                $('#myDataTableId').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    title: '失敗!',
                    text: '更新失敗，必填欄位未填!',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        } catch (error) {
            // 伺服器端錯誤處理
            Swal.fire({
                title: '錯誤!',
                text: '伺服器錯誤，請稍後再試!',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    });




    let dataTable = $('#myDataTableId').DataTable({
        ajax: {
            url: datatableurl,
            type: 'GET',
            datatype: 'json',
            dataSrc: '',
            complete: () => {

            }
        },
        language: {
            url: '//cdn.datatables.net/plug-ins/2.1.6/i18n/zh-HANT.json',
        },
        columns: [
            { data: 'userId' },
            { data: 'userName' },
            { data: 'couponCode' },
            { data: 'discountAmount' },
            { data: 'validFrom' },
            { data: 'validUntil' },
            { data: 'isUsed' },
            {
                data: 'couponId',
                render: function (data) {
                    return '<button class="btn btn-warning preview" data-bs-toggle="modal" data-bs-target="#mymodal" data-id="' + data + '" data-action="EditPartial" data-title="編輯資料">編輯</button>';
                }
            },
            //{ data: 'transactions'}
        ]
    });

});

