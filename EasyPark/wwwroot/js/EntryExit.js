$(function () {
    /*原本的默認排序方式
        $.fn.dataTable.ext.order['boolean-asc'] = function (settings, data, dataIndex) {
            return data.map(function (value) {
                return value === 'true' ? 1 : 0;
            });
        };

        $.fn.dataTable.ext.order['boolean-desc'] = function (settings, data, dataIndex) {
            return data.map(function (value) {
                return value === 'true' ? 0 : 1;
            });
        };

        $('#table').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/2.1.6/i18n/zh-HANT.json',
            },
            order: [[7, 'asc'], [11, 'asc']], // 初始排序設置，根據你的需求進行調整
            columnDefs: [
                {
                    targets: 8, // 第一個布林值列的索引
                    type: 'boolean-asc' // 或 'boolean-desc' 根據需要選擇
                },
                {
                    targets: 12, // 第二個布林值列的索引
                    type: 'boolean-asc' // 或 'boolean-desc' 根據需要選擇
                }
            ]
        });
    */

    //modal視窗載入部分
    $('#mymodal').on('show.bs.modal', function (event) {
        //$.validator.unobtrusive.parse(".modal-body");//把驗證加載進modal中
        //console.log("我有被按到");                
        var button = $(event.relatedTarget); // 觸發modal的按鈕
        var id = button.data('id'); // 從按鈕中獲取資料ID
        var action = button.data('action'); // 從按鈕中獲取動作 (details/edit/create等)
        var title = button.data('title')

        // 清空 modal 的內容
        var modal = $(this);
        modal.find('.modal-body').empty(); // 清除現有內容

        if (action == "ShowPicturePartial") {
            $('#modalid').attr('class', 'modal-dialog modal-lg');
        }
        else {
            $('#modalid').attr('class', 'modal-dialog modal-xl');// 改為大尺寸
        }
        $('#exampleModalLabel').text(title)
        var modal = $(this);
        // 根據不同的動作，載入對應的 Partial View
        var url = 'EntryExitManagement/' + action + '/' + id;
        modal.find('.modal-body').load(url); // 動態加載Partial View
    });

    //圖片預覽部分
    $(document).on("change", "#Picture", function () {
        previewImage(this);
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

            // 假設伺服器返回一個狀態代碼來表示成功
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

        /*
        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: formData,
            processData: false, // 不序列化數據
            contentType: false, // 設定為 false 來讓 jQuery 設定 content-type
            success: function (response) {
                // 假設伺服器返回一個狀態代碼來表示成功
                if (response.success) {
                    Swal.fire({
                        title: '成功!',
                        text: '資料已成功更新!',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });

                    // 關閉 Modal 並且刷新 DataTable 或某個部分
                    $('#editModal').modal('hide');
                    $('#myDataTableId').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        title: '失敗!',
                        text: '更新失敗，必填欄位未填!',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                // 伺服器端錯誤處理
                Swal.fire({
                    title: '錯誤!',
                    text: '伺服器錯誤，請稍後再試!',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
        */

        /*這是改成如果驗證失敗會在partialview上顯示失敗的欄位
        $.ajax({
            url: form.attr('action'), // 取得表單 action 的 URL
            type: 'POST',
            data: formData,
            datatype: '',
            processData: false, // 不序列化數據
            contentType: false, // 設定為 false 來讓 jQuery 設定 content-type
            success: function (response) {

                try {
                    var jsonResponse = JSON.parse(response);
                    if (jsonResponse.success) {
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
                            text: jsonResponse.message,
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                }
                catch (e) {
                    $(".modal-body").html(response);
                    
                    Swal.fire({
                        title: '失敗!',
                        text: "請檢查欄位，並查看上方錯誤提示。",
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                    
                    console.log("Error parsing response: ", e);

                }
                // 假設伺服器返回一個狀態代碼來表示成功
                //if (response.success) {
                //    Swal.fire({
                //        title: '成功!',
                //        text: '資料已成功更新!',
                //        icon: 'success',
                //        confirmButtonText: 'OK'
                //    });

                //    // 關閉 Modal 並且刷新 DataTable 或某個部分
                //    $('#mymodal').modal('hide');
                //    $('#myDataTableId').DataTable().ajax.reload();
                //} else {
                //    Swal.fire({
                //        title: '失敗!',
                //        text: "請檢察欄位",
                //        icon: 'error',
                //        confirmButtonText: 'OK'
                //    });
                //    console.log(response)
                //    $(".modal-body").html(response);
                //    $.validator.unobtrusive.parse("#editForm");
                //}
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText); // 輸出錯誤信息
                // 錯誤提示
                Swal.fire('失敗', '資料更新失敗', 'error');
            }
        });
        */
    });

    //createform
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

    //deleteform 目前是刪掉這功能了
    $(document).on('submit', '#deleteForm', function (event) {
        event.preventDefault(); // 防止表單的默認提交

        var form = $(this);
        var actionUrl = form.attr('action'); // 獲取表單的提交 URL

        Swal.fire({
            title: '確認刪除',
            text: "您確定要刪除這條記錄嗎？",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: '刪除',
            cancelButtonText: '取消'
        }).then((result) => {
            if (result.isConfirmed) {
                var formElement = form[0];
                var formData = new FormData(formElement);

                $.ajax({
                    url: actionUrl, // 取得表單 action 的 URL
                    type: 'POST',
                    data: formData,
                    datatype: 'Json',
                    processData: false, // 不序列化數據
                    contentType: false, // 設定為 false 來讓 jQuery 設定 content-type
                    success: function (response) {
                        // 假設伺服器返回一個狀態代碼來表示成功
                        if (response.success) {
                            Swal.fire({
                                title: '成功!',
                                text: '資料已成功刪除!',
                                icon: 'success',
                                confirmButtonText: 'OK'
                            });

                            // 關閉 Modal 並且刷新 DataTable
                            $('#mymodal').modal('hide');
                            $('#myDataTableId').DataTable().ajax.reload();
                        } else {
                            Swal.fire({
                                title: '失敗!',
                                text: response.message,
                                icon: 'error',
                                confirmButtonText: 'OK'
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error(xhr.responseText); // 輸出錯誤信息
                        // 錯誤提示
                        Swal.fire('失敗', '資料更新失敗', 'error');
                    }
                });
            }
        });
    });
    

    //載入停車場數字的function
    async function getSlotNumber(lotId) {
        var response = await fetch(`EntryExitManagement/GetSlotNumber?lotId=${lotId}`);
        if (!response.ok) {
            throw new Error('網路有問題,請重新再試');
        }
        return await response.json();
    };

    var maskRect = document.getElementById('maskRect');    

    //改變車車圖案的function
    function updateMask(percentage) {
        var height = (percentage / 100) * 24;
        maskRect.setAttribute('height', height);
    }

    //抓平均停車時間的function
    async function GetAverageParkingTime(lotId){
        var response = await fetch(`EntryExitManagement/GetAverageParkingTime?lotId=${lotId}`);
        if (!response.ok) {
            throw new Error('網路有問題,請重新再試');
        }
        else {
            var avgparkingtime = await response.json();
            $('#GetAverageParkingTime').text(`${avgparkingtime.averageParkingTimeMinutes} `);
        }
    }

    //抓尖峰時段的function
    async function GetPeakHours(lotId) {
        var response = await fetch(`EntryExitManagement/GetPeakHours?lotId=${lotId}`);
        if (!response.ok) {
            throw new Error('網路有問題,請重新再試');
        }
        else {
            var peakhour = await response.json();

            if (peakhour.hour == "沒有停車紀錄") {
                $('#GetPeakHours').html(`<div class="fs-1">沒有停車紀錄</div>`);
            }
            else {
                while (peakhour.length < 3) {
                    peakhour.push({ hour: "從缺" }); // 補充 "從缺" 到 peakhour 陣列
                };
                $('#GetPeakHours').html(`第一名: ${peakhour[0].hour}<br>第二名: ${peakhour[1].hour}<br>第三名: ${peakhour[2].hour}`);
            }
        }
        //return await response.json();
    }

    //存目前選擇第幾頁的function
    function saveCurrentPage(page) {
        localStorage.setItem("currentPage", page);
    }

    //load之前記住的第幾頁的function
    function loadCurrentPage(dataTable) {
        const currentPage = localStorage.getItem("currentPage");
        if (currentPage) {
            //console.log('Loaded page:', currentPage);
            dataTable.page(parseInt(currentPage)).draw(false);
        }
    }

    //存目前選擇哪個停車場的function
    function saveSelection(value) {
        localStorage.setItem("selectedParkingLot", value);
    }

    //load選擇哪個停車場的function
    function loadSelection() {
        const selectedValue = localStorage.getItem("selectedParkingLot");
        if (selectedValue) {
            $("#LotId").val(selectedValue);
        }
    }

    //Lot選擇生成datatable
    $(document).on('change', '#LotId', async function () {
        var selectedLotId = $(this).val();

        // 刷新 DataTable
        $('#myDataTableId').DataTable().ajax.url('EntryExitManagement/GetParkingLotData?lotId=' + selectedLotId).load();

        // 清空搜尋框（可選）
        $('#myDataTableId').DataTable().search('').draw();

        //載入停車場
        var result = await getSlotNumber(selectedLotId);
        $('#counts').text(`剩餘車位: ${result.totalQty - result.counts} `);

        //更改車車圖案
        var percentage = (result.totalQty - result.counts) / result.totalQty * 100;
        updateMask(percentage);

        //平均停車
        GetAverageParkingTime(selectedLotId);

        //尖峰時段
        GetPeakHours(selectedLotId);

        //存選擇
        saveSelection($(this).val());
    });


    //初始化數字 舊方法, 原本是想說要直接trigger就好
    //但是這樣會導致datatable初始的時候有兩個, 但又不能刪掉原本的datable生成, 因為欄位那些都還沒被定義
    //$('#LotId').trigger('change');

    // 初始化時的function, 可以跟著selectlist變化生成的東西都在這先初始化
    async function initializeData() {
        // 獲取 LotId 的值
        var selectedLotId = $('#LotId').val();

        // 載入停車場
        var result = await getSlotNumber(selectedLotId);
        $('#counts').text(`剩餘車位: ${result.totalQty - result.counts} `);

        // 更改車車圖案
        var percentage = (result.totalQty - result.counts) / result.totalQty * 100;
        updateMask(percentage);

        //平均停車
        GetAverageParkingTime(selectedLotId);

        //尖峰時段
        GetPeakHours(selectedLotId);

        //datatable生成部分
        var selectedValue = localStorage.getItem("selectedParkingLot");
        var datatableurl = "";
        if (selectedValue) {            
            datatableurl = ('EntryExitManagement/GetParkingLotData?lotId=' + selectedValue);
        }
        else {
            datatableurl = ('EntryExitManagement/GetParkingLotData');
        }
        var dataTable = $('#myDataTableId').DataTable({
            ajax: {
                url: datatableurl,
                type: 'GET',
                dataSrc: '',
                datatype: 'json',
                complete: function () {
                    // 在數據加載完成後設置頁碼
                    loadCurrentPage(dataTable);
                }
            },
            scrollY: 500,
            language: {
                url: '//cdn.datatables.net/plug-ins/2.1.6/i18n/zh-HANT.json',
            },
            columns: [
                { data: 'parktype' },
                {
                    data: 'licensePlatePhoto',
                    render: function (data, type, row) {
                        return '<div class="text-center">' +
                            '<button class="btn btn-success preview" data-bs-toggle="modal" data-bs-target="#mymodal" data-id="' + row.entryexitId + '" data-action="ShowPicturePartial" data-title="預覽圖片">預覽</button>' +
                            '</div>';
                    }
                },
                {
                    data: null, // 因為我們將要自定義內容，所以設為 null
                    render: function (data, type, row) {
                        return row.carId + '<br>車主: ' + row.userName; // 同時顯示 carId 和 userName
                    }
                },
                { data: 'entryTime' },
                { data: 'licensePlateKeyinTime' },
                { data: 'amount' },
                { data: 'exitTime' },
                { data: 'paymentStatus' },
                { data: 'paymentTime' },
                { data: 'validTime' },
                { data: 'lotName' }, // 修改 LotId 為 lotName
                { data: 'isFinish' },
                {
                    data: 'entryexitId',
                    render: function (data) {
                        return '<button class="btn btn-warning preview" data-bs-toggle="modal" data-bs-target="#mymodal" data-id="' + data + '" data-action="EditPartial" data-title="編輯資料">編輯</button>';
                    }
                }/*,這段是加入刪除按鈕的 th要記得自己加
            {
                data: 'entryexitId',
                render: function (data) {
                    return '<button class="btn btn-danger preview" data-bs-toggle="modal" data-bs-target="#mymodal" data-id="' + data + '" data-action="DeletePartial" data-title="刪除資料">刪除</button>';
                }
            }*/
            ]
        });

        ///////////////////////////////////////////////////////
        //儲存使用者的選擇,用Local Storage
        //記住頁碼的部分
        dataTable.on('page.dt', function () {
            const pageInfo = dataTable.page.info();
            //console.log('Current page:', pageInfo.page);
            saveCurrentPage(pageInfo.page);
        });

        // 在頁面加載時加載用戶的選擇
        loadSelection();
    }
    //初始化
    initializeData();
});
