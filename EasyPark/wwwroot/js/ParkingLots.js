$(document).ready(function () {
    // 初始化 DataTable
    $('#ParkLotTable').dataTable({
        ajax: {
            type: "GET",
            url: "/MyParkingLot/ParkingLot/IndexJson",
            dataSrc: ""
        },
        columns: [
            { data: 'district', width:"10%" },
            { data: 'type', width: "10%" },
            { data: 'lotName', width: "10%" },
            { data: 'location', width: "10%" },
            { data: 'monRentalSpace', width: "10%" },
            { data: 'smallCarSpace', width: "10%" },
            { data: 'etcSpace', width: "10%" },
            { data: 'motherSpace', width: "10%" },
            { data: 'rateRules', width: "10%" },
            { data: 'weekdayRate', width: "10%" },
            { data: 'holidayRate', width: "10%" },
            { data: 'resDeposit', width: "10%" },
            { data: 'monRentalRate', width: "10%" },
            { data: 'opendoorTime', width: "10%" },
            { data: 'tel', width: "10%" },
            { data: 'latitude', width: "10%" },
            { data: 'longitude', width: "10%" },
            { data: 'validSpace', width: "10%" },
            {
                data: 'lotId',
                render: function (data) {
                    return `
                    <div class="text-nowrap">
                        <a class="btn" id="EditBtn" data-id="${data}" data-bs-toggle="modal" data-bs-target="#EditModal">
                            <i class="fs-5 fa-solid fa-pen-to-square" style="color:#090f3e"></i>
                        </a>
                    </div>`;
                }
            }
        ],
        pagingType: "full_numbers",
        fixedHeader: {
            header: true
        },
        scrollY: 550,
        language: {
            url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json'
        }
    })

    // 動態加載 Create 表單內容
    $('.CreateBtn').on('click', async function () {
        let response = await fetch('/MyParkingLot/ParkingLot/CreatePartial');
        let partialview = await response.text(); // 讀取返回的 HTML 內容
        $('.CreateParking').html(partialview); // 動態插入到指定區域

        // 綁定表單提交事件
        $(document).off('submit', '#CreateForm').on('submit', '#CreateForm', async function (e) {
            e.preventDefault(); // 防止默認提交行為
            let form = new FormData(this); // 使用 FormData 獲取表單數據
            let fetchresponse = await fetch($(this).attr('action'), {
                method: "POST",
                body: form
            });

            if (fetchresponse.ok) {
                let result = await fetchresponse.json();
                if (result.success) {
                    Swal.fire({
                        icon: "success",
                        title: "新增項目成功!",
                        showConfirmButton: false,
                        timer: 1500
                    });
                    $('#CreateModal').modal('hide');
                    $('#ParkLotTable').DataTable().ajax.reload(null, false); // 重新加載數據表格
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: result.errors ? result.errors.join(", ") : result.message,
                    });
                }
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: "請求發生錯誤，請稍後再試",
                });
            }
        });

        $('#CreateModal').modal('show');
    });

    // 動態加載 Edit 表單內容
    $(document).on('click', '#EditBtn', async function () {
        var id = $(this).data('id'); // 獲取該筆資料的 ID
        let res = await fetch(`/MyParkingLot/ParkingLot/EditPartial/${id}`);
        let partialview = await res.text();
        $('.EditParking').html(partialview);

        $(document).off('submit', '#EditForm').on('submit', '#EditForm', async function (e) {
            e.preventDefault();
            let form = new FormData(this);
            let fetchres = await fetch($(this).attr('action'), {
                method: 'POST',
                body: form
            });

            if (fetchres.ok) {
                let result = await fetchres.json();
                if (result.success) {
                    Swal.fire({
                        icon: "success",
                        title: "編輯成功!",
                        showConfirmButton: false,
                        timer: 1500
                    });
                    $('#EditModal').modal('hide');
                    $('#ParkLotTable').DataTable().ajax.reload(null, false); // 重新加載數據表格
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: result.errors ? result.errors.join(", ") : result.message,
                    });
                }
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: "請求發生錯誤，請稍後再試",
                });
            }
        });

        $('#EditModal').modal('show');
    });

});