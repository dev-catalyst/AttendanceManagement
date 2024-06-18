'use strict'

var handleResetForm = (dt) => {
    const resetButton = document.querySelector('[data-kt-docs-table-filter="reset"]');

    if (resetButton){
        resetButton.addEventListener('click', function () {
            dt.search('').draw();
            $("#TableSearch").val('');
        });
    }
}

var handleSearchDatatable = function (dt) {
    const filterSearch = document.querySelector('#TableSearch');
    if (filterSearch) {
        filterSearch.addEventListener('keyup', function (e) {
            dt.search(e.target.value).draw();
        });
    }
    const TableReload = document.querySelector('#TableReload');
    if (TableReload) {
        TableReload.addEventListener('click', function () {
            dt.search('').draw();
        });
    }
}

function GenerateActionButton(data){
    return `<a href="javascript:void(0);" id="${data}" class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                Actions
                                <span class="svg-icon svg-icon-5 m-0">
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                            <polygon points="0 0 24 0 24 24 0 24"></polygon>
                                            <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                                        </g>
                                    </svg>
                                </span>
                            </a>
                            <!--begin::Menu-->
                            <div class="mt-3 menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
                                <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="javascript:void(0);" onclick="ShowEdit('${data}')" class="menu-link px-3" data-kt-docs-table-filter="edit_row">
                                        Edit
                                    </a>
                                </div>
                                <!--end::Menu item-->

                                <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="javascript:void(0);" class="menu-link px-3" data-kt-docs-table-filter="delete_row">
                                        Delete
                                    </a>
                                </div>
                                <!--end::Menu item-->
                            </div>
                            <!--end::Menu-->`
}

function GenerateAdditionalEdit(data){
    return`<button onclick="ShowEdit1('${data}')" class="btn btn-sm btn-clean btn-icon btn-icon-md svg-icon-warning" title="View">
                                                                <span class="svg-icon-warning">
                                                                 <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" class="svg-icon-warning text-warning">
                                                                 <path opacity="0.3" fill-rule="evenodd" clip-rule="evenodd" d="M2 4.63158C2 3.1782 3.1782 2 4.63158 2H13.47C14.0155 2 14.278 2.66919 13.8778 3.04006L12.4556 4.35821C11.9009 4.87228 11.1726 5.15789 10.4163 5.15789H7.1579C6.05333 5.15789 5.15789 6.05333 5.15789 7.1579V16.8421C5.15789 17.9467 6.05333 18.8421 7.1579 18.8421H16.8421C17.9467 18.8421 18.8421 17.9467 18.8421 16.8421V13.7518C18.8421 12.927 19.1817 12.1387 19.7809 11.572L20.9878 10.4308C21.3703 10.0691 22 10.3403 22 10.8668V19.3684C22 20.8218 20.8218 22 19.3684 22H4.63158C3.1782 22 2 20.8218 2 19.3684V4.63158Z" fill="currentColor"/>
                                                                 <path d="M10.9256 11.1882C10.5351 10.7977 10.5351 10.1645 10.9256 9.77397L18.0669 2.6327C18.8479 1.85165 20.1143 1.85165 20.8953 2.6327L21.3665 3.10391C22.1476 3.88496 22.1476 5.15129 21.3665 5.93234L14.2252 13.0736C13.8347 13.4641 13.2016 13.4641 12.811 13.0736L10.9256 11.1882Z" fill="currentColor"/>
                                                                 <path d="M8.82343 12.0064L8.08852 14.3348C7.8655 15.0414 8.46151 15.7366 9.19388 15.6242L11.8974 15.2092C12.4642 15.1222 12.6916 14.4278 12.2861 14.0223L9.98595 11.7221C9.61452 11.3507 8.98154 11.5055 8.82343 12.0064Z" fill="currentColor"/>
                                                                 </svg>
                                                                 </span>
                                                             </button>`;
}

var exportButtons = (dt) => {
    const documentTitle = document.title;
    var buttons = new $.fn.dataTable.Buttons(dt, {
        buttons: [
            {
                extend: 'excelHtml5',
                title: documentTitle
            },
            {
                extend: 'csvHtml5',
                title: documentTitle
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    modifier: {
                        search: 'none',
                    }
                },
                title: documentTitle
            }
        ]
    }).container().appendTo($('#kt_datatable_example_buttons'));

    const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
    exportButtons.forEach(exportButton => {
        exportButton.addEventListener('click', e => {
            e.preventDefault();

            const exportValue = e.target.getAttribute('data-kt-export');
            const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

            target.click();
        });
    });
}

document.body.addEventListener('scroll', e => {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("kt_scrolltop").style.bottom = "20px";
    } else {
        document.getElementById("kt_scrolltop").style.bottom = "-80px";
    }
});


swup.animateHistoryBrowsing = true;
SwupRun()
swup.on('contentReplaced', SwupRun);

function SwupRun(){
    for (let i = 0; i < document.getElementsByClassName('menu-link').length; i++) {
        document.getElementsByClassName('menu-link')[i].classList.remove('active');
    }
    for (let i = 0; i < document.getElementsByClassName('menu-link').length; i++) {
        if (window.location.href.includes(document.getElementsByClassName('menu-link')[i].href)) {
            document.getElementsByClassName('menu-link')[i].classList.add('active');
        }
    }
    if(window.location.href[window.location.href.length - 1] !== '/') {
        document.getElementsByClassName('menu-link')[0].classList.remove('active');
    }else{
        document.getElementsByClassName('menu-link')[0].classList.add('active');
    }
    if (swup !== undefined || swup !== null)
        swup.cache.remove('/TimeTable');
}

$('#modal_edit_1').on('hidden.bs.modal', function () {
    $('#inner-body-display-edit-1').html('')
})

function GenerateDataTable(tableId, url, columns, columnsDefs){
    return $(tableId).DataTable({
        searchDelay: 200,
        responsive: true,
        processing: true,
        serverSide: true,
        ordering: false,
        lengthMenu: [10, 25, 50, 100, 500, 1000,5000],
        stateSave: true,
        ajax: {
            url: url,
            type: "POST",
        },
        columns: columns,
        columnDefs: columnsDefs,
    });
}

function CreateTable(){

}
