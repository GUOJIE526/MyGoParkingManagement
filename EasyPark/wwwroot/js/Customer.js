function loadCustomers() {
    $.ajax({
        url: '/MyCustomer/Customer/GetCustomers',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Load Customers Data:", data);
            var dataTable = $('#CustTable').DataTable(); // 確保 DataTable 已經初始化

            // 清除 DataTable 的現有數據
            dataTable.clear();

            if (data && data.length > 0) {
                var rows = data.map(function (customer) {
                    var isBlackDisplay = customer.isBlack ?
                        '<span class="badge bg-primary">是</span>' :
                        '<span class="badge bg-secondary">否</span>';
                    return [
                        `<div class="text-center">${customer.username}</div>`,
                        `<div class="text-center">${customer.email}</div>`,
                        `<div class="text-center">${customer.phone}</div>`,
                        `<div class="text-center">${customer.blackCount}</div>`,
                        `<div class="text-center">${isBlackDisplay}</div>`,
                        `<div class="text-center">
                                    <a href="#" class="customerEditBtn" data-id="${customer.userId}" data-bs-toggle="modal" data-bs-target="#customerEditModal">
                                        <i class="fa-solid fa-pen-to-square fa-xl"></i>
                                    </a>
                                </div>`
                    ];
                });

                // 添加新數據到 DataTable
                dataTable.rows.add(rows).draw();
            } else {
                $('#CustTable tbody').append('<tr><td colspan="6" class="text-center">目前沒有資料</td></tr>');
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX 請求錯誤：", error);
        }
    });
}

// 創建用戶後重新加載
$('#customerCreateBtn').on('click', function () {
    $('#Createbody').load('/MyCustomer/Customer/Create', function () {
        $('#customerCreateForm').on('submit', function (e) {
            e.preventDefault();
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        Swal.fire("成功", response.message, "success").then(() => {
                            $('#customerCreateModal').modal('hide');
                            loadCustomers();
                        });
                    } else {
                        Swal.fire("錯誤", response.message, "error");
                    }
                }
            });
        });
    });
});

// 編輯用戶後重新加載
$(document).on('click', '.customerEditBtn', function () {
    var id = $(this).data('id');
    $('#Editbody').load(`/MyCustomer/Customer/Edit/${id}`, function () {
        $('#customerEditForm').off('submit').on('submit', function (e) {
            e.preventDefault();
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        Swal.fire("成功", response.message, "success").then(() => {
                            $('#customerEditModal').modal('hide');
                            loadCustomers();
                        });
                    } else {
                        Swal.fire("錯誤", response.message, "error");
                    }
                },
                error: function (xhr, status, error) {
                    console.error("編輯 AJAX 請求錯誤：", error);
                }
            });
        });
    });
    $('#customerEditModal').modal('show');
});


$(document).ready(function () {
    // 初始化 DataTable（只執行一次）
    $('#CustTable').DataTable({
        pagingType: "full_numbers",
        fixedHeader: true,
        language: {
            url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json',
        }
    });

    // 加載用戶數據
    loadCustomers();
});