var TableAdvanced = function () {

    var initTable1 = function() {

        /* Formatting function for row details */
        function fnFormatDetails (oTable, nTr )
        {
            var aData = oTable.fnGetData( nTr );
            var sOut = '<table>';
            sOut += '<tr><td>电子邮箱：</td><td>' + aData[6] + '</td></tr>';
            sOut += '<tr><td>联系电话：</td><td>' + aData[7] + '</td></tr>';
            sOut += '<tr><td>宿舍地址：</td><td>' + aData[8] + '</td></tr>';        
            sOut += '</table>';
             
            return sOut;
        }

        function restoreRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);

            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                oTable.fnUpdate(aData[i], nRow, i, false);
            }

            oTable.fnDraw();
        }

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);
            jqTds[1].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[1] + '">';
            jqTds[2].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[2] + '">';
            jqTds[3].innerHTML = '<select class="form-control input-small"><option>Male</option><option>Female</option></select>';
            jqTds[4].innerHTML = '<select class="form-control input-small"><option>Jishu</option><option>Shichang</option><option>Cehua</option><option>Xiaoshou</option><option>Xingzheng</option><option>Renshi</option></select>';
            jqTds[5].innerHTML = '<select class="form-control input-small"><option>Test01</option><option>Test01</option><option>Test02</option><option>Test03</option><option>Test04</option><option>Test05</option><option>Test06</option><option>Test07</option><option>Test08</option><option>Test09</option><option>Test10</option><option>Test11</option></select>';
            jqTds[6].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[6] + '">';
            jqTds[7].innerHTML = '<select class="form-control input-small"><option><span class="label label-sm label-success">Active</span></option><option><span class="label label-sm label-danger">Disable</span></option></select>';
            jqTds[8].innerHTML = '<a class="edit" href="">Save</a> | <a class="cancel" href="">Cancel</a>';
        }

        function saveRow(oTable, nRow) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 1, false);
            oTable.fnUpdate(jqInputs[1].value, nRow, 2, false);
            oTable.fnUpdate(jqSelects[1].value, nRow, 3, false);
            oTable.fnUpdate(jqSelects[2].value, nRow, 4, false);
            oTable.fnUpdate(jqSelects[3].value, nRow, 5, false);
            oTable.fnUpdate(jqInputs[2].value, nRow, 6, false);
            oTable.fnUpdate(jqSelects[4].value, nRow, 7, false);
            oTable.fnUpdate('<a class="edit" href="">Edit</a> | <a class="delete" href="">Delete</a>', nRow, 8, false);
            oTable.fnDraw();
        }

        function cancelEditRow(oTable, nRow) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[1].value, nRow, 1, false);
            oTable.fnUpdate(jqInputs[2].value, nRow, 2, false);
            oTable.fnUpdate(jqSelects[3].value, nRow, 3, false);
            oTable.fnUpdate(jqSelects[4].value, nRow, 4, false);
            oTable.fnUpdate(jqSelects[5].value, nRow, 5, false);
            oTable.fnUpdate(jqInputs[6].value, nRow, 6, false);
            oTable.fnUpdate(jqSelects[7].value, nRow, 7, false);
            oTable.fnUpdate('<a class="edit" href="">Edit</a> | <a class="delete" href="">Delete</a>', nRow, 8, false);
            oTable.fnDraw();
        }


        /*
         * Insert a 'details' column to the table
         */
        var nCloneTh = document.createElement( 'th' );
        var nCloneTd = document.createElement( 'td' );
        nCloneTd.innerHTML = '<span class="row-details row-details-close"></span>';
         
        $('#sample_1 thead tr').each( function () {
            this.insertBefore( nCloneTh, this.childNodes[0] );
        } );
         
        $('#sample_1 tbody tr').each( function () {
            this.insertBefore(  nCloneTd.cloneNode( true ), this.childNodes[0] );
        } );
         
        /*
         * Initialize DataTables, with no sorting on the 'details' column
         */
        var oTable = $('#sample_1').dataTable( {
            "aoColumnDefs": [
                {"bSortable": false, "aTargets": [ 0 ] }
            ],
            "aaSorting": [[1, 'asc']],
             "aLengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
             "iDisplayLength": 5,

             "sPaginationType": "bootstrap",
             "oLanguage": {
                 "sLengthMenu": "_MENU_ 条记录",
                 "oPaginate": {
                     "sPrevious": "Prev",
                     "sNext": "Next"
                 }
             },
             "aoColumnDefs": [{
                 'bSortable': false,
                 'aTargets': [0]
             }
             ]
        });

        jQuery('#sample_1_wrapper .dataTables_filter input').addClass("form-control input-small input-inline"); // modify table search input
        jQuery('#sample_1_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown
        jQuery('#sample_1_wrapper .dataTables_length select').select2(); // initialize select2 dropdown
         
        /* 为打开关闭详细信息添加事件监听
         * Note that the indicator for showing which row is open is not controlled by DataTables,
         * rather it is done here
         */
        $('#sample_1').on('click', ' tbody td .row-details', function () {
            var nTr = $(this).parents('tr')[0];
            if ( oTable.fnIsOpen(nTr) )
            {
                /* 该行已打开，点击事件为关闭 */
                $(this).addClass("row-details-close").removeClass("row-details-open");
                oTable.fnClose( nTr );
            }
            else
            {
                /* 打开该行的详细信息 */
                $(this).addClass("row-details-open").removeClass("row-details-close");
                oTable.fnOpen( nTr, fnFormatDetails(oTable, nTr), 'details' );
            }
        });

        //$('#sample_1_column_toggler input[type="checkbox"]').change(function () {
        //    /* Get the DataTables object again - this is not a recreation, just a get of the object */
        //    var iCol = parseInt($(this).attr("data-column"));
        //    var bVis = oTable.fnSettings().aoColumns[iCol].bVisible;
        //    oTable.fnSetColumnVis(iCol, (bVis ? false : true));
        //});


        //屏蔽显示第6、8列属性
        oTable.fnSetColumnVis(6, false);
        oTable.fnSetColumnVis(7, false);
        oTable.fnSetColumnVis(8, false);

        $('#sample_1_column_toggler input[type="checkbox"]').change(function () {
            /* 隐藏/显示某列信息 */
            var iCol = parseInt($(this).attr("data-column"));
            var flag = $(this).attr("checked");
            var bVis = oTable.fnSettings().aoColumns[iCol].bVisible;
            if (flag) {
                oTable.fnSetColumnVis(iCol, true);
            } else {
                oTable.fnSetColumnVis(iCol, false);
            }
        });




        var nEditing = null;

        $('#sample_1_new').click(function (e) {
            if (confirm("Are you sure to add this staff ?") == true) {
                document.getElementById("AddDiv").style.display = "none";

                e.preventDefault();
                var aiNew = oTable.fnAddData(['', '', '', '',
                        '<a class="edit" href="">Edit</a>', '<a class="cancel" data-mode="new" href="">Cancel</a>'
                ]);
                var nRow = oTable.fnGetNodes(aiNew[0]);
                SaveRow(oTable, nRow);
            }
        });

        $('#sample_1 a.delete').live('click', function (e) {
            e.preventDefault();
            var nRow = $(this).parents('tr')[0];

            var rowEmployeeNumber = $(this).parents('tr').children('td:nth(1)').text().trim();

            //alert($('td#' + rowEmployeeNumber).text());
            

            if (confirm("Are you sure to delete [" + rowEmployeeNumber + "] this employee ?") == false) {
                return;
            }
            var num = rowEmployeeNumber;
            $.ajax({
                type:"POST",
                url: "/Employee/EmployeeDelete",
                data: { empNum: num },
                success: function (msg) {
                    //msg==true 删除成功
                    
                    if (msg == "true") {
                        var sup = $('td#' + rowEmployeeNumber);
                        if (sup)
                        {
                            sup.text("");
                            sup.removeAttr("id");
                        }
                        alert("Delete[" + rowEmployeeNumber + "]Success");
                        oTable.fnDeleteRow(nRow);
                    }
                    else {
                        alert("Delete Error! ");
                    }
                },
                error: function (msg) {
                    
                    if (msg == "true") {
                        var sup = $('td#' + rowEmployeeNumber);
                        if (sup) {
                            sup.text("");
                            sup.removeAttr("id");
                        }
                        alert("Delete[" + rowEmployeeNumber + "]Success");
                        oTable.fnDeleteRow(nRow);
                    }
                    else {
                        alert("Delete Error!");
                    }
                }
            });
        });

        $('#sample_1 a.cancel').live('click', function (e) {
            e.preventDefault();
            if ($(this).attr("data-mode") == "new") {
                var nRow = $(this).parents('tr')[1];
                oTable.fnDeleteRow(nRow);
            } else {
                restoreRow(oTable, nEditing);
                nEditing = null;
            }
        });

        $('#sample_1 a.edit').live('click', function (e) {
            e.preventDefault();

            /* Get the row as a parent of the link that was clicked on */
            var nRow = $(this).parents('tr')[0];

            if (nEditing !== null && nEditing != nRow) {
                /* Currently editing - but not this row - restore the old before continuing to edit mode */
                restoreRow(oTable, nEditing);
                editRow(oTable, nRow);
                nEditing = nRow;
            } else if (nEditing == nRow && this.innerHTML == "Save") {
                /* Editing this row and want to save it */
                saveRow(oTable, nEditing);
                nEditing = null;
                alert("Updated! Do not forget to do some ajax to sync with backend :)");
            } else {
                /* No edit in progress - let's start one */
                editRow(oTable, nRow);
                nEditing = nRow;
            }
        });
    }


    return {

        //main function to initiate the module
        init: function () {
            
            if (!jQuery().dataTable) {
                return;
            }

            initTable1();
        }

    };

}();